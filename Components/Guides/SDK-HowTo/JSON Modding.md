---
parent: SDK-HowTo
grand_parent: Guides
---
# JSON Modding

## Introduction

---

JSON files are the foundation for modding, these configuration files are read by the game at runtime and act as definition files for game parameters, like a database or catalog. It can also reference assets (meshes, textures, audio, etc...) and custom class type from DLL plugins. 

The default JSON files used by the base game are packed in a`.jsondb` archive file (zip), in `[Blade & Sorcery folder]\BladeAndSorcery_Data\StreamingAssets\Default`or as loose files in the BasSDK Unity project: `[BasSdk folder]\BuildStaging\Catalogs\Default`

To create a mod, most of the time you have to create a json that will act as a new item or an override of an existing game json that use the same ID. To do so, you can copy the configuration files from the SDK or the game, modify it, then place it in a dedicated mod folder in `[Blade & Sorcery folder]\BladeAndSorcery_Data\StreamingAssets\Mods` for the PCVR version of the game or in  `[Device storage]\Android\data\com.Warpfrog.BladeAndSorcery\files\Mods`for the Nomad version of the game.

<aside>
🗒️ It's highly recommended to keep in the customized JSON only the fields you want to change, this way, if another mods do changes to another field in the same JSON it will not conflict.

</aside>

Each customized JSON should contain at least the fields `$type`, `id` and `version`. It is recommended that you do not edit these, and that if there is a new update that changes the JSON files you edit, it is recommended to start with a fresh new json parameters to prevent any issues.

```json
{
  "$type": "ThunderRoad.CreatureData, ThunderRoad",
  "id": "PlayerDefaultMale",
  "version": 6,
  "health": 1000,
  "locomotionSpeed": 5.0
}
```

## Manifest file

---

A `manifest.json` file is required in the mod folder, without it, the folder/mod will be ignored by the game. This file contain some data about the mod, like the mod version, the description, the author, etc...

```json
{
    "Name": "MyMod",
    "Description": "This is my cool mod",
    "Author": "Someone",
    "ModVersion": "1.0.0",
    "GameVersion": "0.12.0.0",
		"Thumbnail": "thumbnail.png"
}
```

<aside>
🗒️ Thumbnail path is relative to the mod folder path, and it’s also possible to use an http address like `"https://www.some.image.link/123.png"`

</aside>

<aside>
🗒️ GameVersion is the game mod version compatible with the mod

</aside>

## JSON types

---

As for the U12 version, the game use around 30 types of JSON:

| Animations  | Dynamic animations reference that can be used on NPC |
| --- | --- |
| AreaCollectionDungeons  | Dungeon definition |
| AreaCollectionFixLayouts  |  |
| AreaConnectionTypes  | Type of dungeon room gateway |
| Areas  | Dungeon rooms |
| AreaTables  | Pick dungeon room given random distribution. |
| BehaviorTrees  | Behaviour trees used by AI |
| Brains  | AI parameters (aim precision, strafe distance, attack speed...) |
| ColliderGroups  | Parameters of a specific item part (Blade, Handle, Crystal...) |
| Containers | Equipment definition for Player / NPC and other containers (chests, inventories, merchants...) |
| Creatures | NPC and player parameters (health, mana, look, etc…) |
| CreatureTables | Pick creature(s) given random distributions. This is used to randomly spawn enemies and give diversity to wave spawning |
| Damagers | Damage definition for an item (damage type, penetration force, impulse force needed...) |
| DamageModifiers | Damage parameters relative to materials |
| Effects | Definition of all Sounds, Particles, VFX graph, Decals, Painting, Shader and Mesh effects used |
| EffectGroups | Group of effects |
| Expressions | Character face expression (not used currently) |
| HandPoses | Array of positions and rotations to define a hand pose (grabbed, pointing, etc...) |
| Interactables | Parameters for interactable objects (handles, holsters, ropes...) |
| Items | Parameters for all existing items (weapons, apparels, potions...) |
| Keyboards | Defines a keyboard config, mapping transform names to key labels and actions |
| Levels | Parameters for all existing levels (Arena, Ruins, etc...) |
| Liquids | Liquid contained in potions |
| LootTables | Pick item(s) given random distributions. Used to randomly fill containers and NPC with equipment |
| Materials  | Physical surface parameters (toughness, velocity absorption...) |
| MaterialEffects  | Effects parameters relative to materials |
| Menus | Option menu page parameters |
| MusicGroups |  |
| Musics |  |
| Spells | Contains parameters for all existing spells. (Fire, Gravity, Lightning... etc) |
| Texts | Define text used in game to assist in different languages |
| Voices | Voice definition used by NPC |
| Waves | Wave definition (creature spawned, order, min/max count...) |