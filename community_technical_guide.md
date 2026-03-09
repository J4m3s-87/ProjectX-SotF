# IL2CPP Modding Technical Guide — Sons of the Forest

> A comprehensive reference for modding Sons of the Forest using **RedLoader** and **IL2CPP interop**. Covers recurring pitfalls, proven patterns, safe harbors, and hard-won lessons from building a large-scale mod over 300+ development phases.
>
> **Target audience:** Intermediate-to-advanced modders working with Harmony, IL2CPP reflection, and dedicated server deployment.

---

## Table of Contents

- [Recurring Technical Issues & Solutions](#recurring-technical-issues--solutions)
- [Proven IL2CPP Patterns ("Safe Harbors")](#proven-il2cpp-patterns-safe-harbors)
  - [1. Method Access Pivot](#1-the-method-access-pivot)
  - [2. SendCommand Pivot](#2-the-sendcommand-pivot)
  - [3. Deployment Hybrid Structure](#3-the-deployment-hybrid-structure)
  - [4. Server-Universal Manifest](#4-the-server-universal-manifest)
  - [5. Polling Over Patching](#5-polling-over-patching)
  - [6. Piggyback Pattern (MonoBehaviour Callback Workaround)](#6-the-piggyback-pattern-monobehaviour-callback-workaround)
  - [7. Harmony Awake Interception (Reactive Object Discovery)](#7-harmony-awake-interception-reactive-object-discovery)
  - [8. Transform Traversal (GetComponentsInChildren Replacement)](#8-transform-traversal-getcomponentsinchildren-replacement)
  - [9. Direct IL2CPP Memory Offset Access](#9-direct-il2cpp-memory-offset-access)
  - [10. IL2CPP Field Access Decision Tree](#10-il2cpp-field-access-decision-tree)
  - [11. Harmony OnEnable Tracking (Physics.OverlapSphere Replacement)](#11-harmony-onenable-tracking-physicsoverlapsphere-replacement)
  - [12. Double-Cast + Reflection (DummyDll Workaround)](#12-double-cast--reflection-dummydll-workaround)
  - [13. String-Based Harmony Patching (Assembly Chain Bypass)](#13-string-based-harmony-patching-assembly-chain-bypass)
  - [14. IL2CPP Type Check (is/as Operator Replacement)](#14-il2cpp-type-check-isas-operator-replacement)
  - [15. ChatBox.SendLine for Client→Server Commands](#15-chatboxsendline-for-clientserver-commands)
  - [16. Harmony PREFIX Blocker + Typed API Trigger](#16-harmony-prefix-blocker--typed-api-trigger)
  - [17. Discord Relay Bounce](#17-discord-relay-bounce)
  - [18. Direct Memory String Read](#18-direct-memory-string-read)
  - [19. Server-Safe Tick Driver](#19-server-safe-tick-driver)
  - [20. Anti-Cheat Hybrid Protocol](#20-anti-cheat-hybrid-protocol)
  - [21. Direct VailActor API Bypass (Stimuli Workaround)](#21-direct-vailactor-api-bypass-stimuli-workaround)
  - [22. Persistent Game State Override via PREFIX + Marshal](#22-persistent-game-state-override-via-prefix--marshal)
  - [23. Follower HP Persistence via Stat Forensics + Delayed Timer](#23-follower-hp-persistence-via-stat-forensics--delayed-timer)
- [Additional Standards](#additional-standards)
- [Architecture Evolution](#architecture-evolution)
- [Stat Override Architecture](#stat-override-architecture)
- [Custom Structure Patterns](#custom-structure-patterns)

---

## Recurring Technical Issues & Solutions

### IL2CPP / Reflection

| Issue                                                                                                                                             | Root Cause                                                                                                                                      | Resolution                                                                                                                                                                                      |
| :------------------------------------------------------------------------------------------------------------------------------------------------ | :---------------------------------------------------------------------------------------------------------------------------------------------- | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **The Reflection Paradox** — Fields visible in `dump.cs` return `null` via `AccessTools.Field`                                                    | IL2CPP strips metadata for private fields, even if the binary offset exists                                                                     | **Method Access Pivot**: Prefer getter methods or properties (`AccessTools.Property`) over direct field access. **Field Flood Diagnostic**: Iterate all fields at runtime to verify visibility. |
| **Stripped Engine Methods** — `MissingMethodException` on `FindObjectsOfType<T>` or `Scene.GetRootGameObjects`                                    | IL2CPP strips generic methods and rarely-used Unity API calls                                                                                   | **Harmony Awake Interception**: Patch `Awake()` on the target type — components come to you. `GameObject.Find(name)` works for known names. `GetComponent<T>()` is never stripped.              |
| **Total Object-Finding API Stripping** — ALL variants stripped: `FindObjectsOfType<T>()`, `FindObjectsOfType(Type)`, `Scene.GetRootGameObjects()` | Only singular `FindObjectOfType<T>()` survives for types the game itself queries                                                                | **Reactive over Polling**: Use Harmony Postfix on the target type's `Awake()`. Zero API calls needed — objects register themselves via your hook.                                               |
| **FindObjectsOfType Generic Stripping** — `MissingMethodException` for ALL types                                                                  | IL2CPP strips ALL plural `FindObjectsOfType` generic instantiations — only singular `FindObjectOfType<T>()` survives for select types           | **Harmony Awake Interception**: Patch the target type's `Awake()` with a Postfix.                                                                                                               |
| **GetComponentsInChildren Stripping** — `MissingMethodException` on `GetComponentsInChildren<T>()`                                                | IL2CPP strips plural generic component queries                                                                                                  | **Transform Traversal Pattern**: Recursively walk `Transform.GetChild(i)` with `GetComponent<T>()` on each child. See Pattern #8.                                                               |
| **GetFields() Returns Only IL2CPP Base Fields** — Returns 2 fields: `isWrapped`, `pooledPtr`                                                      | `System.Reflection.GetFields()` on IL2CPP types only sees the managed wrapper fields                                                            | Use `AccessTools.Field` (~60% of types) or **direct memory offset access** via `Marshal.Copy` (100%). See Pattern #9.                                                                           |
| **AccessTools.Field Type-Specific Failure** — Returns `null` for some types but works for others                                                  | Stripping is **per-type**, not per-field or per-visibility. No reliable way to predict which types are affected.                                | First try `AccessTools.Field`. If null, fall back to **direct memory offset access** using offsets from `dump.cs`. See Pattern #9.                                                              |
| **Non-Generic Method MissingMethodException** — `StatsManager.GetStat(System.Type)` throws despite being in `dump.cs`                             | The `Type` parameter maps to `Il2CppSystem.Type` at runtime, not `System.Type`. Generic version also stripped.                                  | **Bypass the API entirely**: Use unsafe IntPtr + offset to read/write data directly in IL2CPP memory (Pattern #9).                                                                              |
| **Physics.OverlapSphere Overload Stripping** — 3-param overload throws `MissingMethodException`, 2-param works                                    | IL2CPP strips the `(Vector3, float, int)` overload. 2-param `(Vector3, float)` is preserved.                                                    | **Use 2-param + code filtering**: Call without layerMask, filter results with `GetComponentInParent<VailActor>()` or use Harmony OnEnable Tracking (Pattern #11).                               |
| **DummyDll Type Hierarchy Gap** — `.Cast<T>()` and `.TryCast<T>()` fail to compile                                                                | DummyDll stubs lack `Il2CppObjectBase` in the type hierarchy at compile time                                                                    | **Double-Cast + Reflection Pattern** (#12): Cast via `(Il2CppObjectBase)(object)obj`, then `MakeGenericMethod`.                                                                                 |
| **Assembly Dependency Chain Trap** — `CS0012` when referencing types with deep inheritance                                                        | The type's **entire inheritance chain** must be present. IL2CPP hierarchies are fragmented across DLLs.                                         | **String-Based Harmony Pattern** (#13): Use `AccessTools.TypeByName()` + `object __instance`.                                                                                                   |
| **MakeGenericMethod on Stripped Generics** — Returns null or crashes                                                                              | Generic instantiation doesn't exist in the IL2CPP binary. Reflection cannot resurrect stripped code.                                            | **Harmony Interception**: Patch the target type's methods directly — the component comes to you via `__instance`.                                                                               |
| **Follower HP Stat Misidentification** — Writing to HealthStat has no effect despite `_max=5000`                                                  | VailActor has 13 stats; multiple have `_max=100`. Matching by `_max` picks wrong stat (behavioral, not health). Also, `Revive()` resets timing. | Match by `_baseValue` (0x14) not `_max`. Use 1-second delayed timer after Revive postfix. See Pattern #23.                                                                                      |
| **ScaryObject Stimuli Ineffective for Burn** — Scare/flee works but fire stimulus fails to ignite                                                 | Stimuli grid requires specific source actor context not replicable from modded components                                                       | **Direct Actor API Bypass** (Pattern #21): Use `VailActor.IgniteSelf(burnTime)` directly. Keep stimuli for scare/flee only.                                                                     |
| **Cross-Type Invoke on Interface-Typed Objects** — "Object does not match target type"                                                            | IL2CPP wraps interface-typed objects with a different managed proxy than concrete-typed objects                                                 | **Direct Memory String Read** (Pattern #18): `Marshal.ReadIntPtr` at dump.cs offset + `Il2CppStringToManaged`.                                                                                  |
| **MonoBehaviour Callback Trap** — `Update()`, `OnGUI()` never fire on injected MonoBehaviours                                                     | IL2CPP-injected MonoBehaviours don't receive lifecycle callbacks. Only `Awake()` fires reliably.                                                | **Piggyback Pattern**: Create static methods, call from an existing working callback. See Pattern #6.                                                                                           |

### Harmony Patching

| Issue                                                                            | Root Cause                                                              | Resolution                                                                                        |
| :------------------------------------------------------------------------------- | :---------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------ |
| **VTable Corruption (Fatal CLR Error)** — Crash when accessing patched component | Patching high-frequency native interop types corrupts the IL2CPP vtable | **Avoid Native Hooks**: Use polling (Pattern #5) or `SendCommand` (Pattern #2) instead.           |
| **Generic Type Crashing** — `AccessViolationException` on generic classes        | Harmony cannot safely patch open generic types in IL2CPP                | **Standalone Isolation**: Keep unstable patches in separate DLLs or use non-generic base classes. |

### Deployment

| Issue                                                                                | Root Cause                                                                   | Resolution                                                                                       |
| :----------------------------------------------------------------------------------- | :--------------------------------------------------------------------------- | :----------------------------------------------------------------------------------------------- |
| **Shadowing / Ghost DLLs** — Changes not appearing despite successful build          | RedLoader scans both `Mods/` and `_Redloader/Mods/`. Old DLLs mask new ones. | **Clean-Build Protocol**: Delete target DLLs before deployment. Audit both directories.          |
| **Manifest Rejection** — "0 Plugins to Load" errors                                  | RedLoader enforces strict manifest schema (lowercase keys, `type: "Mod"`)    | **Hybrid Structure**: Place `.dll` in `Mods/` root and `manifest.json` + `Assets/` in subfolder. |
| **Server SFTP Failure** — Upload failing for `.dll` files                            | Some hosts block executable extensions via SFTP                              | **Zip Workaround**: Upload a `.zip` archive and extract via control panel.                       |
| **DLL File Lock During Hot Deploy** — `Copy-Item` reports success but file unchanged | Game process locks loaded DLLs                                               | **Always verify deploy timestamp** after copy. Close game first if stale.                        |

### Other

| Issue                                                                           | Root Cause                                                                         | Resolution                                                                                           |
| :------------------------------------------------------------------------------ | :--------------------------------------------------------------------------------- | :--------------------------------------------------------------------------------------------------- |
| **Ghost Registry / Phantom Toggles** — Settings visible but not functional      | `SettingsRegistry` creating default stubs, or UI not wired to logic                | **Action-on-Change**: Use `OnValueChanged.Subscribe`. Use unique IDs for registry calls.             |
| **Side-Effect Crashes** — Mass entity operations causing delayed crashes        | Rapidly invoking native `ForceDeath`/`Destroy` overwhelms the event queue          | **Throttled Execution**: Use `try-catch` per entity and/or throttle over multiple frames.            |
| **Fake Console Command Pattern** — `SendCommand` fails silently                 | Command names fabricated without verifying against `dump.cs`                       | **API Verification**: Always verify commands exist in `dump.cs` under `DebugConsole`.                |
| **Hot-Path Allocation Trap** — Game stutter during world load                   | Crypto hashing + allocations in hot loops (e.g., `MD5.Create()` firing 600+ times) | **Integer Hash Combining**: Replace crypto with `GetHashCode()` + quantized position. ~100x faster.  |
| **Reflection Invoke in Hot Loops** — Lag during winter with complex systems     | `MethodInfo.Invoke` called every tick even when state hasn't changed               | **State Caching**: Track processed instances with `HashSet<int>`. Only invoke on state transitions.  |
| **ChatBox.SendLine Visibility Bug** — Server-side messages invisible to clients | `SendLine()` adds to history but doesn't trigger client popup                      | **Discord Relay Bounce** (Pattern #17): Route through Discord for visible delivery.                  |
| **Packets.NetEvent Client→Server Drop** — Ad-hoc packets silently dropped       | Bolt doesn't deliver ad-hoc `NetEvent` client→server on dedicated servers          | **Scope-based NetEvent** (LootSyncEvent pattern) works. Fallback: **Hybrid Protocol** (Pattern #20). |
| **SdkEvents.OnInWorldUpdate Dead on Headless** — Fires once then stops          | Not designed for headless/batch mode. No exception, no log.                        | **Server-Safe Tick Driver** (Pattern #19): Use `SeasonsManager.LateUpdate` Harmony Postfix.          |

---

## Proven IL2CPP Patterns ("Safe Harbors")

### 1. The Method Access Pivot

_When a private field is inaccessible, use a getter method instead._

When a private field `_data` is inaccessible via reflection, check `dump.cs` for a private getter `GetData()`. Invoking the method is 99% more reliable than reflecting the field.

### 2. The SendCommand Pivot

_Dispatch game features through `DebugConsole` to avoid VTable corruption._

- **Unsafe:** Direct `MethodInfo.Invoke` on game singletons (Risk of VTable corruption)
- **Safe:** `DebugConsole.Instance.SendCommand("stonehack on")` (Native parser handles logic)

> **⚠️ Only use for commands that actually exist in `DebugConsole`** (verify in `dump.cs`). Some commands may appear to work but are actually fabricated. For features without console commands, use direct static APIs on game manager classes.

### 3. The Deployment Hybrid Structure

_Satisfy RedLoader's requirements while maintaining asset organization._

```
Mods/
├── YourMod.dll                      [CODE / ROOT]
└── YourMod/                         [METADATA / SUB]
    ├── manifest.json                [Must have "IsUniversal": true for servers]
    └── Assets/                      [AssetBundles, Textures]
```

### 4. The "Server-Universal" Manifest

_Load your mod on both Client and Dedicated Server._

- **Manifest:** Must include `"IsUniversal": true`.
- **Code:** Must guard client-only logic (UI, Input) with `#if !SERVER`.

### 5. Polling Over Patching

_Prefer throttled update loops over Harmony hooks on unstable types._

For "read-only" state checks or low-frequency updates, prefer a 1-second throttle loop in `OnUpdate` over hooking `Update` or `FixedUpdate` via Harmony. This eliminates the risk of destabilizing the native render loop.

### 6. The Piggyback Pattern (MonoBehaviour Callback Workaround)

_When injected MonoBehaviours don't receive callbacks, ride an existing one._

IL2CPP-injected MonoBehaviours often fail to receive `Update()`, `OnGUI()`, `FixedUpdate()`. Only `Awake()` fires reliably. Instead of fighting this, add a static tick method and call it from an already-working callback:

```csharp
// In your module (static class, no MonoBehaviour):
public static void OnGuiTick()
{
    if (!_activated) return;
    if (Time.time - _lastScan < 2f) return;
    _lastScan = Time.time;
    ScanForTargets();
}

// In a working MonoBehaviour's OnGUI():
MyModule.OnGuiTick();
```

### 7. Harmony Awake Interception (Reactive Object Discovery)

_Stop searching for objects — let them come to you._

When ALL object-finding APIs are stripped, patch the target type's `Awake()` to intercept objects at spawn time:

```csharp
// In Init():
var awakeMethod = AccessTools.Method(typeof(TargetType), "Awake");
var postfix = AccessTools.Method(typeof(MyModule), nameof(OnTargetAwake));
_harmony.Patch(awakeMethod, postfix: new HarmonyMethod(postfix));

// Harmony Postfix — fires automatically when any instance spawns:
private static void OnTargetAwake(TargetType __instance)
{
    __instance.gameObject.AddComponent<MyController>();
}
```

This is the most reliable IL2CPP pattern: `Awake()` is always present on MonoBehaviours, Harmony Postfix on non-generic concrete types is stable, and no object-finding API is needed.

> **⚠️ `Scene.GetRootGameObjects()` is also stripped** — scene traversal as a discovery approach is not viable.

### 8. Transform Traversal (GetComponentsInChildren Replacement)

_Recursively walk the transform hierarchy when plural component queries are stripped._

`GetComponentsInChildren<T>()` is stripped. Use recursive `Transform.GetChild()` + `GetComponent<T>()` (singular):

```csharp
private static void CollectComponents<T>(Transform parent, List<T> result) where T : Component
{
    var comp = parent.GetComponent<T>();
    if (comp != null) result.Add(comp);

    for (int i = 0; i < parent.childCount; i++)
    {
        var child = parent.GetChild(i);
        if (child != null) CollectComponents(child, result);
    }
}
```

**Key insight**: `GetComponent<T>()` (singular, on a specific GameObject) is NEVER stripped. Only the plural/children variants are.

### 9. Direct IL2CPP Memory Offset Access

_The nuclear option — read/write fields directly in memory when all reflection fails._

When ALL reflection fails (`AccessTools.Field` returns null, `GetFields()` returns only wrapper fields), use `Il2CppDumper`'s `dump.cs` offsets:

```csharp
using System.Runtime.InteropServices;

// Offsets from dump.cs comments (e.g., "private float _myField; // 0x74")
private const int OFFSET_MY_FIELD = 0x74;

private static IntPtr GetPointer(MyType obj)
{
    var baseObj = (Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase)(object)obj;
    return baseObj.Pointer;
}

private static float ReadFloat(IntPtr ptr, int offset)
{
    var bytes = new byte[4];
    Marshal.Copy(ptr + offset, bytes, 0, 4);
    return BitConverter.ToSingle(bytes, 0);
}

private static void WriteFloat(IntPtr ptr, int offset, float value)
{
    var bytes = BitConverter.GetBytes(value);
    Marshal.Copy(bytes, 0, ptr + offset, 4);
}
```

> **⚠️ VERIFY OFFSETS AT RUNTIME** — always read known fields first and log the values. If you get garbage (`NaN`, `1.4E-45`), the offsets are wrong. The `dump.cs` offsets should be accurate, but verify before writing.

> **⚠️ GAME VERSION SENSITIVITY** — offsets may shift between game updates. Re-run `Il2CppDumper` and compare offsets after patches.

### 10. IL2CPP Field Access Decision Tree

_A systematic approach to accessing private fields on game types._

```
Need to access a private field on a game type?
│
├─ Does the type have a public getter/setter method or property?
│  YES → Use the public API (safest, always works)
│
├─ Does AccessTools.Field(typeof(X), "_field") return non-null?
│  YES → Use AccessTools.Field (works for ~60% of types)
│
├─ Does the type have a public method that accepts the value?
│  YES → Use the public method
│
└─ All reflection failed?
   → Use Direct Memory Offset Access (Pattern #9)
   → Get offset from dump.cs ("// 0xNN" comment on the field)
   → Cast to Il2CppObjectBase via (object) to get .Pointer
   → Marshal.Copy to read/write at pointer + offset
```

### 11. Harmony OnEnable Tracking (Physics.OverlapSphere Replacement)

_Track instances via Harmony when physics queries are stripped._

When `Physics.OverlapSphere` is stripped (fails silently — no exception, just empty results), use Harmony to track instances:

```csharp
private static List<TemperatureModifierVolume> _heatSources = new();

// In Init() — patch OnEnable to track instances:
var onEnable = AccessTools.Method(typeof(TemperatureModifierVolume), "OnEnable");
_harmony.Patch(onEnable, postfix: new HarmonyMethod(typeof(Module), nameof(OnSourceEnabled)));

private static void OnSourceEnabled(TemperatureModifierVolume __instance)
{
    if (__instance != null && !_heatSources.Contains(__instance))
        _heatSources.Add(__instance);
}

// Replace Physics.OverlapSphere with distance checks:
private static bool IsSourceNearby(Transform origin, float radius)
{
    Vector3 pos = origin.position;
    foreach (var source in _heatSources)
    {
        if (source == null) continue;
        if (Vector3.Distance(pos, source.transform.position) <= radius)
            return true;
    }
    return false;
}
```

> **⚠️ Cache state transitions** — avoid calling reflection methods every tick. Use a `HashSet<int>` of instance IDs to track processed objects. Only invoke on state changes.

### 12. Double-Cast + Reflection (DummyDll Workaround)

_Bridge the compile-time type hierarchy gap when using DummyDll types._

When DummyDll types don't inherit from `Il2CppObjectBase` at compile time, `.Cast<T>()` fails in two ways: (1) the extension method doesn't bind, and (2) the target type doesn't satisfy the generic constraint. At runtime, both types DO inherit from `Il2CppObjectBase`.

```csharp
using Il2CppInterop.Runtime;

// Problem: icon is a Texture (DummyDll), Texture2D is also DummyDll.
// icon.Cast<Texture2D>()  → CS1061 (no Cast on Texture)
// il2cppBase.Cast<Texture2D>() → CS0311 (constraint violation)

// Solution: double-cast to bridge compile-time gap, then reflection:
var icon = itemData.UiData._icon;
var il2cppBase = (Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase)(object)icon;
var castMethod = il2cppBase.GetType().GetMethod("Cast");
var genericCast = castMethod.MakeGenericMethod(typeof(Texture2D));
var texture = (Texture2D)genericCast.Invoke(il2cppBase, null);
```

**Why this works**: The `(object)` cast erases the compile-time type, allowing the downcast to `Il2CppObjectBase` (which the runtime type IS). `MakeGenericMethod` bypasses the compile-time constraint because it's only enforced at compile time, not at runtime via reflection.

> **⚠️ This affects ALL DummyDll types**, not just `Texture`. Any time you need `.Cast<T>()` on a type from `Il2CppDumper/DummyDll/`, use this pattern. Types from `_Redloader/Game/` already have the correct hierarchy.

> **⚠️ Cache the result** — `MakeGenericMethod` + `Invoke` is expensive. Use a dictionary cache to avoid calling this per-frame.

### 13. String-Based Harmony Patching (Assembly Chain Bypass)

_Resolve types at runtime when assembly dependency chains are too deep._

When a target type's assembly chain is too fragmented for compile-time references, use string-based resolution:

```csharp
public static void ApplyHarmonyPatch(Harmony harmony)
{
    var targetType = AccessTools.TypeByName("Sons.Weapon.RangedWeapon");
    if (targetType == null) { RLog.Error("Type not found"); return; }

    var target = AccessTools.Method(targetType, "LateUpdate");
    if (target == null) target = AccessTools.Method(targetType, "Update");

    var prefix = AccessTools.Method(typeof(MyModule), nameof(MyPrefix));
    harmony.Patch(target, prefix: new HarmonyMethod(prefix));
}

// Use 'object __instance' — no typed parameter needed:
public static void MyPrefix(object __instance)
{
    // Cache MethodInfo/PropertyInfo on first call:
    if (_getDataMethod == null)
        _getDataMethod = __instance.GetType().GetMethod("GetData");

    var data = _getDataMethod?.Invoke(__instance, null);
}
```

**Key insights:**

- `AccessTools.TypeByName` resolves types without adding assembly references
- `object __instance` avoids compile-time dependency — Harmony passes the reference untyped
- Cache all `MethodInfo`/`PropertyInfo` lookups — reflection is expensive per-frame
- **IL2CPP property vs field**: Many `dump.cs` "fields" are actually **properties** at runtime. Always check `GetProperties()` if `GetField()` returns null

> **⚠️ Enum marshaling**: When IL2CPP returns an `Int32` that represents an enum, you can pass it directly to `MethodInfo.Invoke` — IL2CPP auto-marshals the integer.

### 14. IL2CPP Type Check (is/as Operator Replacement)

_C# `is`/`as` operators DO NOT WORK on IL2CPP proxy types._

They always return false/null because the .NET type hierarchy doesn't match the IL2CPP object's actual type.

```csharp
// BROKEN — always returns false in IL2CPP:
if (obj is VailWorldEventData.SearchPartyEvent) { ... }

// CORRECT — IL2CPP-native type check:
var targetType = Il2CppType.Of<VailWorldEventData.SearchPartyEvent>();
if (obj.GetIl2CppType().Equals(targetType))
{
    var typed = obj.Cast<VailWorldEventData.SearchPartyEvent>();
    // ... use typed ...
}
```

> **⚠️ This is silent.** `is` returns `false` without exception. Code using `is`/`as` on IL2CPP types will simply skip branches, making features appear broken with no error output.

### 15. ChatBox.SendLine for Client→Server Commands

_Use the game's own chat system when custom network events fail._

When `DebugConsole.SendCommand()` is local-only and custom Bolt events fail:

```csharp
// Client side — send command via chat:
var chatBox = UnityEngine.Object.FindObjectOfType<ChatBox>();
chatBox.SendLine("/mymod command args");

// Server side — hook chat events:
private static void OnChatEvent(ChatEvent evnt)
{
    if (!BoltNetwork.isServer) return;
    var msg = (string)_messageProperty.GetValue(evnt);
    if (msg != null && msg.StartsWith("/mymod "))
        ProcessCommand(msg);
}
```

> **⚠️ What DOES NOT work for client→server:**
>
> 1. `DebugConsole.SendCommand()` — local only
> 2. `Packets.NetEvent` — silently dropped on dedicated servers for _ad-hoc_ events (but scope-based `NetEvent` with `ReadMessageServer`/`ReadMessageClient` works — see LootSyncEvent pattern)
> 3. `AdminCommand.Create(GlobalTargets.OnlyServer)` — blocked by Bolt security
> 4. `ChatEvent.Create()` + `.Send()` via reflection — server `OnEvent` never fires
> 5. ✅ `ChatBox.SendLine("/mymod ...")` — game's own proven bidirectional path

### 16. Harmony PREFIX Blocker + Typed API Trigger

_Set game state with typed APIs, then block the game from reverting it._

When client-side visual systems (weather, seasons) are controlled by periodic update methods that override your mod-set state:

| Component             | Role                             | Implementation                                                                  |
| --------------------- | -------------------------------- | ------------------------------------------------------------------------------- |
| **Typed API Trigger** | Initiates the visual change      | `WeatherSystem.Instance.ForceRain(3)` or `SeasonsManager.Instance.SetSeason(n)` |
| **PREFIX Blocker**    | Prevents the game from reverting | Harmony PREFIX that returns `false` to skip execution                           |

```csharp
// Part 1 — Typed API (called once on button press):
public static void ToggleForceRain(bool on)
{
    var ws = WeatherSystem.Instance;
    if (ws == null) return;
    if (on) ws.ForceRain(3);
    else    ws.StopRaining();
}

// Part 2 — PREFIX Blocker (runs every frame):
[HarmonyPrefix]
static bool CheckForRain_Prefix()
{
    if (_forcedRain.HasValue) return false; // Skip original
    return true;
}
```

> **⚠️ Never call methods via reflection inside a Harmony PREFIX/POSTFIX in IL2CPP.** Even when `MethodInfo` resolves successfully, `Invoke()` may silently drop the call.

### 17. Discord Relay Bounce

_Route server messages through Discord for visible client delivery._

When `ChatBox.SendLine()` adds messages to the server's history buffer but doesn't trigger client popups, bounce messages through Discord:

1. Post the message as tagged text to the Discord chat channel
2. The polling relay picks it up and relays through the proven Discord→Game `ChatEvent` path
3. Modify the bot filter to allow tagged bot messages through

### 18. Direct Memory String Read

_Read IL2CPP string fields on interface-typed objects where all reflection fails._

When an IL2CPP object is typed as an interface and you need a field from the concrete type, **all reflection approaches fail**. This extends Pattern #9 for string fields:

```csharp
var il2cppObj = (Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase)token;
var ptr = il2cppObj.Pointer;
IntPtr strPtr = Marshal.ReadIntPtr(ptr + 0x10); // offset from dump.cs
string name = Il2CppInterop.Runtime.IL2CPP.Il2CppStringToManaged(strPtr);
```

> **⚠️ Why reflection fails**: IL2CPP wraps interface-typed objects with a different managed proxy than concrete-typed objects. `AccessTools.TypeByName` finds the concrete type, but its `PropertyInfo`/`MethodInfo` cannot be applied to the interface proxy.

### 19. Server-Safe Tick Driver

_Use `SeasonsManager.LateUpdate` instead of `SdkEvents` on headless servers._

`SdkEvents.OnInWorldUpdate` does **NOT** fire on headless dedicated servers. It fires at most once then stops forever — silently.

```csharp
// In server module init:
var lateUpdate = AccessTools.Method(typeof(Sons.Atmosphere.SeasonsManager), "LateUpdate");
var tickPostfix = AccessTools.Method(typeof(MyPlugin), nameof(ServerTickPostfix));
harmony.Patch(lateUpdate, postfix: new HarmonyMethod(tickPostfix));

// Postfix — runs every frame on all tiers including headless:
private static void ServerTickPostfix()
{
    try { MyModule.Poll(); } catch { }
}
```

> **⚠️ Wrap every module call in individual try-catch.** An unhandled exception in `SeasonsManager.LateUpdate` may corrupt the Harmony patch chain.

> **⚠️ This affects ALL `SdkEvents` callbacks.** Do not use SdkEvents for any server-side polling on dedicated servers.

### 20. Anti-Cheat Hybrid Protocol

_Combine NetEvent and ChatBox for server-side mod verification._

`Packets.NetEvent` is unidirectional on dedicated servers:

| Direction       | Protocol                                   | Status                 |
| --------------- | ------------------------------------------ | ---------------------- |
| Server → Client | `Packets.NetEvent` (REQUEST_CHECK)         | ✅ Works               |
| Client → Server | `ChatBox.SendLine("/mymod integrity ...")` | ✅ Works (Pattern #15) |
| Server → Client | `Packets.NetEvent` (CHECK_RESULT)          | ✅ Works               |

**Key design decisions:**

- **Timeout = Allow**: Vanilla players (no mods) can't respond. Timeout is "likely vanilla" — allowed. Only explicit whitelist violations trigger disconnection.
- **Background→Main Thread Deferred Queue**: Player connection events fire on Bolt background threads, but `BoltNetwork` operations require the main thread. Queue via `lock` and process in the tick driver (Pattern #19).
- **IL2CPP Cast for BoltConnection**: Use try-catch direct cast, not `is`/`as` (Pattern #14).

### 21. Direct VailActor API Bypass (Stimuli Workaround)

_Skip the stimuli system — call the actor's ignition API directly._

When `ScaryObject.Fire()` stimuli broadcasts fail to produce burn damage (common when the source `VailActor` is null or improperly initialized), bypass the stimuli system entirely:

```csharp
// Proven API — same as PlayerActions.BurnAllEnemies()
foreach (var actor in detectedActors)
{
    if (actor == null) continue;
    int id = actor.GetInstanceID();
    if (_ignitedActors.Contains(id)) continue;

    actor.IgniteSelf(burnTimeSeconds);  // Direct burn — no stimuli needed
    _ignitedActors.Add(id);
}

// Clear _ignitedActors periodically (e.g., every 2s) to allow re-ignition
```

**Key insight**: `VailActor.IgniteSelf(float)` handles all burn state (visual effects, damage, audio) internally. The stimuli system is only needed for area-of-effect scare/flee behavior. Use `HashSet<int>` of instance IDs to prevent per-frame spam.

### 22. Persistent Game State Override via PREFIX + Marshal

_Force game state every frame via Harmony PREFIX + direct memory writes._

When `AccessTools.Field()` returns `null` and high-level APIs have no effect on dedicated servers, use a Harmony PREFIX combined with `Marshal.WriteByte`/`Marshal.WriteInt32`:

```csharp
private const int AI_PAUSED_OFFSET = 0x100;  // from dump.cs
private static PropertyInfo _pointerProp;

// Setup: cache Pointer property, patch Update()
_pointerProp = instance.GetType().GetProperty("Pointer",
    BindingFlags.Instance | BindingFlags.Public |
    BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

_harmony.Patch(AccessTools.Method(simType, "Update"),
    prefix: new HarmonyMethod(typeof(MyModule), nameof(UpdatePrefix)));

// PREFIX: runs every frame, enforces target value
private static void UpdatePrefix(object __instance)
{
    if (_pointerProp == null) return;
    try
    {
        IntPtr ptr = (IntPtr)_pointerProp.GetValue(__instance);
        if (ptr == IntPtr.Zero) return;
        byte target = Config.FreezeAI.Value ? (byte)1 : (byte)0;
        Marshal.WriteByte(ptr + AI_PAUSED_OFFSET, target);
    }
    catch { } // Silent — runs every frame
}
```

**Key rules:**

1. Get the IL2CPP memory offset from `dump.cs` (the `// 0xNN` comment on each field)
2. Access the native pointer via the `Pointer` property (inherited from `Il2CppObjectBase`)
3. Use `Marshal.WriteByte` for bools, `Marshal.WriteInt32` for ints/enums
4. PREFIX on the class's `Update()`/`LateUpdate()` ensures the value is enforced every frame
5. Config value controls the PREFIX behavior — toggling is instant without re-patching

### 23. Follower HP Persistence via Stat Forensics + Delayed Timer

_Correctly identify and persist follower HP through stat matching and timed writes._

When modifying follower (Kelvin/Virginia) HP at runtime, two problems stack:

**Problem 1 — Wrong Stat Matching:**

VailActor's `StatsManager._stats` contains **13 entries**. Multiple stats have `_max=100`. Matching by `_max` selects stat[0] (a behavioral stat with `_baseValue=0.0`), NOT stat[12] (the real HealthStat with `_baseValue=100.0`).

```csharp
// WRONG — matches behavioral stat[0]:
if (Math.Abs(statMax - baseHealth) < 1f) { ... }

// CORRECT — matches real HealthStat (stat[12]):
float statBaseVal = *(float*)((byte*)statObj.ToPointer() + 0x14); // _baseValue
if (Math.Abs(statBaseVal - baseHealth) < 1f)
{
    *(float*)((byte*)statObj.ToPointer() + 0x24) = targetHealth; // _max
    *(float*)((byte*)statObj.ToPointer() + 0x10) = targetHealth; // _currentValue
    *(float*)((byte*)statObj.ToPointer() + 0x14) = targetHealth; // _baseValue
}
```

**Problem 2 — Post-Revive HP Reset:**

`VailActor.Revive()` fires when helping a downed companion. The game's recovery logic runs AFTER the Harmony postfix and resets `_currentValue`.

```csharp
// Schedule HP re-application 1 second later:
System.Threading.Timer delayTimer = null;
delayTimer = new System.Threading.Timer(_ =>
{
    try { ApplyFollowerHP(capturedActorPtr, capturedTarget); }
    catch { }
    finally { delayTimer?.Dispose(); }
}, null, 1000, System.Threading.Timeout.Infinite);
```

> **⚠️ The stat forensics technique** (dump all stats, compare across state transitions, match by `_baseValue`) is reusable for any stat system modification. Never assume stat ordering — always verify by dumping and comparing.

> **⚠️ `_baseValue` serves dual purpose** — it identifies the stat AND is used by the recovery system to determine target HP after revival. Writing `_baseValue = targetHealth` ensures recovery aims for the multiplied value.

---

## Additional Standards

### HarmonyPatchAll Default Trap

Any DLL inheriting from `SonsMod` defaults to `HarmonyPatchAll = true`. This causes Harmony to scan ALL types — including open generic types — corrupting IL2CPP vtables. **Always** set `HarmonyPatchAll = false` in your constructor and apply patches manually.

### ClassInjector Type Registration Persistence

`ClassInjector.RegisterTypeInIl2Cpp<T>()` registrations persist for the entire session. Once registered, types cannot be unregistered. Prefer static classes with the Piggyback Pattern over injected MonoBehaviours. Never register types that interact with serialization. Full game restart required after removing modules with registered types.

### Unpatchable Types Registry

| Type                                       | Patch Attempted       | Result                                     |
| ------------------------------------------ | --------------------- | ------------------------------------------ |
| `ScrewStructureBase<T>` (generic)          | Various methods       | VTable corruption → crash on Tab/Inventory |
| `ScrewStructure.Awake`                     | Postfix               | Delayed crash on structure interaction     |
| `CraftingCog` inner methods                | Multiple (9 attempts) | Assembly-level crash, worked standalone    |
| `HeldOnlyItemController` (property setter) | `set_InfiniteHack`    | Console spam (hidden `SendCommand`)        |

For these types, use polling (Pattern #5), SendCommand (Pattern #2), direct memory (Pattern #9), or Piggyback (Pattern #6).

### IMGUI Unicode & Emoji Crash Prevention

Unity IMGUI cannot render emoji characters (`☀`, `🌧`, `❄`, `🔥`). Including them in any `GUI.*` string causes an **unrecoverable native render-thread crash** with no managed exception and no log entry. Use ASCII alternatives: `[Sun]`, `[Rain]`, `[Snow]`, `[Fire]`. Basic Unicode (`±`, `°`, `×`) is safe.

### Season Transition Pipeline

`DebugConsole._season("summer")` changes the enum but does NOT trigger visual updates. Use `SeasonsManager.Instance.SetSeasonPostDeserialize(seasonEnum, offset)` — a typed IL2CPP call that fires `UpdateReceiversSeason()`. Hold the season with PREFIXes on `LateUpdate`, `UpdateTime`, and `SetSeasonPostDeserialize`. Set `_seasonIsLocked=false` (not true) to allow native recovery paths.

### Dedicated Server Raid Queuing Gotchas

Key findings from implementing raid control on dedicated servers:

- **Harmony patches on `VailWorldEventData.EventsForTime` and `VailWorldEvents.ChooseEventsForDay` are stable** — no vtable corruption.
- **`ChooseEventsForDay` runs every frame** via `VailWorldEvents.Update()` — manually-added `_queuedEvents` are overwritten within seconds. Must use `_lastQueuedDay = -1` to trigger the game's own requeue.
- **The Dedicated Server Anger Trap**: `cannibalAnger` is always 0 on dedicated servers (no player exploration triggers anger). Most events have `minMaxAnger > 0`, so ALL events fail `IsValid()`. Fix: Force `IgnoreMinMaxAnger=true` and `IgnoreMinMaxDay=true`.
- **The Event Cooldown Trap**: After `ChooseEventsForDay` selects events, their run statistics persist. On subsequent requeues, `InCooldown()` returns `true` for everything. Fix: Clear `_eventRunStats` before resetting `_lastQueuedDay`.
- **IL2CPP `is`/`as` operators on event types** always return false (Pattern #14). Must use `Il2CppType.Of<T>()` + `GetIl2CppType().Equals()` — otherwise all event operations silently fail.

### Stat Override Limitation — Dedicated Server

IL2CPP metadata stripping on dedicated servers means:

- **Health multipliers work** — applied via Harmony Postfix on `VailActor.OnEnable`
- **Damage/Aggression multipliers fail** — reflection returns `null` for all field names

**Lesson:** On dedicated servers, always prefer Harmony method patching over field reflection. If a gameplay value is calculated by a method, patch the method.

---

## Stat Override Architecture

The RaidCustomizer stat multiplier system required **four iterations** before finding a working approach:

| Attempt | Approach                                                    | Result                   | Why                                                             |
| ------- | ----------------------------------------------------------- | ------------------------ | --------------------------------------------------------------- |
| 1       | `AccessTools.Field(typeof(VailActor), "_damageMultiplier")` | `null`                   | IL2CPP strips private field metadata                            |
| 2       | Runtime `GetField()` with `BindingFlags.NonPublic`          | `null`                   | IL2CPP exposes private fields as **properties**, not fields     |
| 3       | `StatsManager.GetStat(typeof(HealthStat))` (non-generic)    | `MissingMethodException` | `Type` parameter maps to `Il2CppSystem.Type`, not `System.Type` |
| 4       | **Unsafe IntPtr + offset** (Pattern #9b)                    | ✅ **Works**             | Direct memory write at known IL2CPP offsets                     |

**Working solution** (VailActor.OnActorEnabled postfix, unsafe pointer arithmetic):

- `_gameSettingsDamageMultiplier` at offset 0x4CC (float)
- `_gameSettingsAngerMultiplier` at offset 0x4D0 (float)
- `_healthSettings` at offset 0x218 (ptr) → `_health` at offset 0x18 (float)
- `.Pointer` property cached via `GetProperty("Pointer")` at runtime (DummyDll gap workaround)

**Key lesson:** When both reflection AND typed API calls fail, unsafe pointer arithmetic at known IL2CPP offsets is the final reliable approach (Pattern #9b).

---

## Architecture Evolution

### Early Phase — Fragmentation

20+ individual mods, frequent conflicts, disparate config files. Each mod was a standalone DLL.

### Mid Phase — Consolidation ("Great Merge")

All mods merged into a single project. Discovery of IL2CPP limitations led to abandoning many patterns that worked in Mono modding.

### Stabilization

Shift from SUI (unstable) to IMGUI (native). Retirement of `HarmonyPatchAll=true`. Learned which Unity APIs are stripped and which survive.

### Server Authority

Introduction of 4-Tier Architecture (Solo, Owner, Client, Server) and permission sync. First dedicated server deployment.

### Module Hardening

Production deployment of complex modules that required **six rewrites** across every object-finding API before discovering ALL are stripped. Final solutions: Harmony Awake Interception and the Piggyback Pattern as canonical IL2CPP-safe patterns. Performance optimization pass (MD5 → integer hashing).

### Deep IL2CPP Mastery

Full depth of IL2CPP field stripping exposed. `AccessTools.Field` fails per-type (not per-field). `GetFields()` returns only managed wrapper fields. Established the IL2CPP Field Access Decision Tree and Direct Memory Offset Access pattern.

### API Verification & Physics Stripping

Discovered `Physics.OverlapSphere` is stripped (silent failure). Exposed the **Fake Console Command** antipattern — multiple `SendCommand` calls were fabricated commands that never existed. Replaced with direct game APIs. Added state-caching optimization for reflection-heavy hot loops.

### DummyDll Type Hierarchy Gap

Icon/texture loading exposed that DummyDll types lack `Il2CppObjectBase` inheritance at compile time. Established Double-Cast + Reflection Pattern as the canonical workaround. Root cause: DummyDlls are stubs from `Il2CppDumper`, NOT the interop-generated assemblies from RedLoader.

### Server Infrastructure

Client→Server command routing via ChatBox. Server-safe tick drivers. Anti-cheat hybrid protocol. Weather/season PREFIX blockers. Discord bidirectional relay with player name resolution via direct memory reads.

---

## Custom Structure Patterns

Based on analysis of a decompiled custom structure mod (RedLoader, ~6.6 MB, 6 custom building structures).

### Blueprint Book Injection

Custom pages can be added to the in-game Blueprint Book (item #552) at runtime:

1. Create a `BlueprintBookPageData` with `_topRecipe` and `_bottomRecipe`
2. Set `_pageTitleLocalizationId` (register via `LocalizationTools.ItemsTable.AddEntry()`)
3. Get the `BlueprintBookController` from `ItemTools.GetHeldPrefab(552)`
4. Append data to `component._pages._pages`

### Bolt Prefab Registration for Multiplayer

Custom structure prefabs must be registered with Bolt's `PrefabDatabase` for multiplayer replication:

```csharp
if (BoltNetwork.isRunning && go.TryGetComponent<BoltEntity>(out var entity))
{
    if (!PrefabDatabase.Instance.Prefabs.Contains(go))
    {
        PrefabDatabase.Instance.Prefabs = PrefabDatabase.Instance.Prefabs.AddItem(go).ToArray();
        PrefabDatabase._lookup[entity.prefabId] = go;
    }
}
```

**Critical for multiplayer:** Both Server and Client editions must register the same prefabs. Failure causes silent sync failures.

### Structure Ghost Preview System

The game's `StructureGhostSwapper` handles the translucent placement preview. All renderers in a custom structure need this component:

- Iterate all `Renderer` components in the hierarchy
- Skip engine objects: `TerrainBumpProjector`, `Shadows`, `GrassPusher` (destroy these)
- Add `StructureGhostSwapper` to remaining renderers

### Crafting Node Template Pattern

Create new structure crafting nodes by cloning existing vanilla nodes (e.g., Recipe #26, LeanTo):

1. Get vanilla prefab from `ConstructionTools.GetRecipe(26)._structureNodePrefab`
2. Instantiate, strip children, mark `DontDestroyOnLoad`
3. Attach custom structure transform as child
4. Auto-detect `StructureCraftingNodeIngredient` components
5. `ShowGhost(true)` to initialize ghost state

This avoids constructing the complex `StructureCraftingNode` hierarchy from scratch.

### Asset Bundle Loading: Embedded vs External

| Approach     | Method                                                                                   | Pros                                | Cons                          |
| ------------ | ---------------------------------------------------------------------------------------- | ----------------------------------- | ----------------------------- |
| **Embedded** | `AssetLoaders.LoadBundleFromAssembly(Assembly.GetExecutingAssembly(), "Resources.name")` | Single DLL, no external files       | Bloats DLL size               |
| **External** | `AssetBundle.LoadFromFile(path)` from `Mods/YourMod/Assets/`                             | Lean DLL, assets managed separately | Requires correct deploy paths |

---

## Contributing

If you've encountered other IL2CPP patterns or pitfalls not covered here, consider sharing them with the modding community. Every pattern documented saves another modder days of debugging.

---

_This guide was compiled from 400+ development phases of iterative IL2CPP modding. Many patterns were discovered through systematic failure — if something seems overly specific, it's because someone hit that exact wall._
