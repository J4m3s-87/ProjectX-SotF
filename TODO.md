# Project X — TODO

## LootRespawn — ⚠ Performance Fix Needed

- [x] Fix crash — manual `harmony.PatchAll(typeof(...))` instead of `HarmonyPatchAll`
- [x] Verified: Awake Postfix fires (612+ items), Collect Postfix fires on pickup
- [x] Tracking via MD5 hash (position + rotation + name), 6 items confirmed tracked
- [x] Production build deployed (211KB) — diagnostic logging removed
- [x] Verify respawn suppression on save/reload (collect items → reload → confirm gone)
- [ ] **BUG**: Game stutters on item pickup — MD5 hash + `RLog.Msg` on every Collect
- [ ] Fix: Remove `RLog.Msg` from `OnPickUpCollected`, replace MD5 with fast hash

## Cleanup — ✅ Complete

- [x] Remove StoneGate keybinds from `Config.cs` (Use Tool, Change Mode, Finish/Open)
- [x] Remove Open Sesame toggle from `Config.cs`
- [x] Remove Prefab Repair toggle from `Config.cs`
- [x] Source files already excluded from csproj (`<Compile Remove>`)
- [ ] Delete StoneGate/OpenSesame/PrefabRepair source dirs (optional — already excluded from build)

## Config Housekeeping — ✅ Complete

- [x] Move Rope Bridge config from Zipline → Features category

## FasterCrafting — ✅ Complete

- [x] Rewrote dead stub → manual Harmony patch on `CraftingCog.OnCraftBeginEvent`
- [x] Test in-game: confirmed crafting speed works at 10x
- [x] No Tab/backpack crash (10th approach succeeded where 9 previous failed)

## MeatDryer — ⚠ v6 Ready (Fixing Scanner)

- [x] v5 (Harmony patch) deployed but untested
- [x] v6 (ProjectXUI pattern) ready to deploy — cleaner architecture
- [ ] Verify scanner loop runs in-game

## LootRespawn — ⚠ Optimized

- [x] Optimization: Replaced slow MD5 hashing with fast integer hash to fix lag
- [ ] Verify no lag on load
- [ ] Verify items still respawn correctly

## Deployment

- [ ] Upload Server DLL to Eugamehost
- [ ] Solo regression test
- [ ] Server + Owner test
- [ ] Server + Client test
- [ ] Rebuild server pack (V1.7+)
- [ ] Rebuild Friend Pack for distribution

## Backlog

- [ ] Build book accessible in no-clip mode
- [ ] `addallitems` safe toggle button
- [ ] Burn wood on fires module
- [ ] Remove gold from structures module
- [ ] Server admin: Freeze all players toggle
- [ ] Consolidate Inventory + Stack modules
- [ ] IMGUI scroll indicator for long panels
