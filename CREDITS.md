# Project X — Credits & Acknowledgements

Project X is a unified mod for Sons of the Forest, built from the ground up
with a unique architecture (static modules, manual Harmony patching, four-tier
compilation for Solo/Owner/Server/Client).

The following community mods were studied as reference material to understand
which game APIs to target and what player-facing behavior to implement.
No code was copied — all implementations were independently written.

## Mod Authors & Inspirations

| Feature Area                              | Inspired By                      | Original Author         | License  | Source                                                               |
| ----------------------------------------- | -------------------------------- | ----------------------- | -------- | -------------------------------------------------------------------- |
| Raid Customization                        | RaidCustomizer                   | codengine (skynet86)    | Unknown  | [sotf-mods](https://sotf-mods.com/mods/codengine/RaidCustomizer/)    |
| Scary Cross                               | ScaryCross                       | GLaD0S (laserman120)    | AGPL-3.0 | [GitHub](https://github.com/laserman120/ScaryCross)                  |
| Dedicated Server Admin                    | DedicatedSuperuser               | GLaD0S (laserman120)    | AGPL-3.0 | [GitHub](https://github.com/laserman120/DedicatedSuperuser)          |
| Zipline Extension                         | ZiplineExtender                  | SmokyAce (move123456789)| Unknown  | [sotf-mods](https://sotf-mods.com/mods/SmokyAce/)                    |
| Stone Gate (REMOVED from build)           | StoneGate                        | SmokyAce (move123456789)| Unknown  | [sotf-mods](https://sotf-mods.com/mods/SmokyAce/)                    |
| Meat Dryer                                | RealisticMeatDryer               | DarkAvatar7             | Unknown  | [sotf-mods](https://sotf-mods.com/mods/DarkAvatar7/)                 |
| Broadcast Message                         | BroadcastMessage                 | SmokyAce (move123456789) & ThirtyTwelve | Free for use | [GitHub](https://github.com/move123456789/SOTF-Mods)           |
| Log/Item Carry (BuilderStacks, Inventory) | LogCarryAmount / ItemCarryAmount | SmokyAce (move123456789)| Unknown  | [sotf-mods](https://sotf-mods.com/mods/SmokyAce/)                    |
| Waterfall Audio (FMOD volume control)     | WaterfallSoundControl            | Toni Macaroni           | Unknown  | [sotf-mods](https://sotf-mods.com/mods/ToniMacaroni/)                |
| Ammo UI                                   | AmmoUi                           | ImAxel0                 | MIT      | [GitHub](https://github.com/ImAxel0/AxelModMenu)                     |
| Open Sesame (Doors)                       | OpenSesame                       | tempbito                | Unknown  | [sotf-mods](https://sotf-mods.com/mods/tempbito/)                    |
| Hotbar                                    | SonsHotbar                       | AEDEV                   | Unknown  | [sotf-mods](https://sotf-mods.com/mods/AEDEV/)                       |
| Stack Sizes                               | StackMod                         | Terroducky              | Unknown  | [sotf-mods](https://sotf-mods.com/mods/Terroducky/)                  |
| Loot Respawn                              | LootRespawnControl               | GLaD0S (laserman120)    | AGPL-3.0 | [GitHub](https://github.com/laserman120/SOTF-Mod-LootRespawnControl) |
| WaterCollectors                           | Realistic Water Collectors       | GLaD0S (laserman120)    | GPL-3.0  | [GitHub](https://github.com/laserman120/Realistic-Water-Collectors)  |
| Structure Durability                      | StructureDurability              | tempbito                | Unknown  | [sotf-mods](https://sotf-mods.com/mods/tempbito/)                    |
| Relocator                                 | Relocator                        | Toni Macaroni           | None     | [GitHub](https://github.com/ToniMacaroni/Relocator)                  |
| WeaponDamage                              | LessUselessGuns                  | sknthelisper            | Unknown  | [sotf-mods](https://sotf-mods.com/mods/sknthelisper/)                |
| Crafting Speed                            | FasterCrafting                   | Laughingcat             | Unknown  | [sotf-mods](https://sotf-mods.com/mods/Laughingcat/)                 |
| Mod Compatibility                         | CompatibleModsSynchronizer       | tempbito                | Unknown  | [sotf-mods](https://sotf-mods.com/mods/tempbito/)                    |
| Prefab Repair                             | PrefabRepair                     | GLaD0S (laserman120)    | AGPL-3.0 | [GitHub](https://github.com/laserman120/PrefabRepair)                |
| Spawn Control (studied only)              | VailSpawnControl                 | GLaD0S (laserman120)    | AGPL-3.0 | [GitHub](https://github.com/laserman120/VailSpawnControl)            |

> **License note:** Mods marked "Unknown" were downloaded as compiled DLLs from
> sotf-mods.com. No license files were bundled with the downloads and no public
> source repositories were found at the time of writing. These mods were studied
> as reference material only — no code was copied.

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
