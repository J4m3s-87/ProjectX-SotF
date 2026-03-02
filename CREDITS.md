# Project X — Credits & Acknowledgements

Project X is a unified mod for Sons of the Forest, built from the ground up
with a unique architecture (static modules, manual Harmony patching, four-tier
compilation for Solo/Owner/Server/Client).

The following community mods were studied as reference material to understand
which game APIs to target and what player-facing behavior to implement.
No code was copied — all implementations were independently written.

## Mod Authors & Inspirations

| Feature Area                              | Inspired By                      | Original Author         |
| ----------------------------------------- | -------------------------------- | ----------------------- |
| Raid Customization                        | RaidCustomizer                   | codengine (skynet86)    |
| Scary Cross                               | ScaryCross                       | _(seeking author)_      |
| Dedicated Server Admin                    | DedicatedSuperuser               | _(seeking author)_      |
| Zipline Extension                         | ZiplineExtender                  | SmokyAce                |
| Stone Gate                                | StoneGate                        | SmokyAce                |
| Meat Dryer                                | RealisticMeatDryer               | SmokyAce                |
| Broadcast Message                         | BroadcastMessage                 | SmokyAce                |
| Log/Item Carry (BuilderStacks, Inventory) | LogCarryAmount / ItemCarryAmount | SmokyAce & ThirtyTwelve |
| Waterfall Audio                           | WaterfallSoundControl            | Toni Macaroni           |
| Ammo UI                                   | AmmoUi                           | ImAxel0                 |
| Open Sesame (Doors)                       | OpenSesame                       | Searica                 |
| Hotbar                                    | SonsHotbar                       | AEDEV                   |
| Stack Sizes                               | StackMod                         | Terroducky              |
| Loot Respawn                              | LootRespawnControl               | \_GLAD0S                |
| WaterCollectors                           | Realistic Water Collectors       | \_GLAD0S                |
| Structure Durability                      | StructureDurability              | _(seeking author)_      |
| Relocator                                 | Relocator                        | _(seeking author)_      |
| WeaponDamage                              | LessUselessGuns                  | _(seeking author)_      |
| Crafting Speed                            | FasterCrafting                   | _(seeking author)_      |
| Mod Compatibility                         | CompatibleModsSynchronizer       | _(seeking author)_      |
| Prefab Repair                             | PrefabRepair                     | _(seeking author)_      |
| Spawn Control                             | VailSpawnControl                 | _(seeking author)_      |

## Fully Original Features

These features have no original mod counterpart:

- Custom IMGUI Interface (ProjectXGUI)
- Four-Tier Architecture (Solo/Owner/Server/Client)
- Network Stack (Permissions, Integrity, ConfigSync, CommandBridge)
- Building Enhancements (Repair All, Durability System)
- Player Actions & Controls

## Frameworks & Tools

- **RedLoader** — Mod loader (LGPL-2.1) by Toni Macaroni & codengine
- **SonsSdk** — Game SDK (LGPL-2.1) by Toni Macaroni
- **HarmonyLib** — Runtime patching (MIT) by Andreas Pardeike
- **Il2CppInterop** — IL2CPP interop (LGPL-3.0) by BepInEx Team

See LICENSES.md for full license details.

---

_If you are an author listed above and would like to discuss attribution,
or if you recognise your mod in the "seeking author" entries, please reach out._
