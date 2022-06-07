# JSONs: An Overview

##  Introduction
_This WIKI is a work in progress by Drags. Content on this page is subject to change over the course of updates, and changes to these JSONs will not be documented here._

JSON files are the foundation for modding, these configuration files are read by the game at runtime and act as definition files for game parameters. It can also reference asset bundles (meshes, textures, audio, etc...) and custom class type (plugins, custom code).

These JSON files are stored in the Default folder, found in ``B:\Steam Games\steamapps\common\Blade & Sorcery\BladeAndSorcery_Data\StreamingAssets\Default\Bas``. You can copy the configuration files from this default folder in to your mod, however make sure that you do not overwrite these default files as this can cause issues that require a file verification if done incorrectly.
To edit contents inside the vanilla game JSONs, you can copy the file to a separate folder inside the "Mods" folder in StreamingAssets. Once done, you put a [manifest.json](https://github.com/KospY/BasSDK/blob/master/_ModsExamples/ModFolder/WitchBroom/manifest.json) inside that folder, then you can edit the contents of the JSON without overwriting the Default folder files. The folder you created in mods will be read first, so this means that the files you edit that overwrite the Default JSON files will load instead of the JSON files stored in Default
##  JSON types
Currently, the game use 21 types of JSON:
* **BehaviorTrees** - Currently a Work in Progress
* **Brains** - AI parameters (aim precision, strafe distance, attack speed...)
* **ColliderGroups** - Parameters of a specific item part (Blade, Handle, Crystal...)
* **Containers** - Equipment definition for NPC, and other containers (chests, inventories, merchants...)
* **Creatures** - NPC and player parameters (health, mana, look...)
* **CreatureTable** - Pick creature(s) given random distributions. This is used to randomly spawn enemies and give diversity to wave spawning
* **Damager** - Damage definition for an item (damage type, penetration force, impulse force needed...)
* **Effects** - Definition of all Sounds, Particles, VFX graph, Decals, Painting, Shader and Mesh effects used
* **Expressions** - Character face expression
* **HandPoses** - Array of positions and rotations to define a hand pose (grabbed, pointing, etc...)
* **Interactables** - Parameters for interactable objects (handles, holsters, ropes...) 
* **Items** - Parameters for all existing items (weapons, apparels, potions...)
* **Levels** - Parameters for all existing levels (Arena, Ruins, etc...)
* **LootTables** - Same as CreatureTable but for items. Used to randomly fill containers with equipment and enemy wave item randomizer.
* **MaterialDamages** - Damage parameters relative to materials
* **MaterialEffects** - Effects parameters relative to materials
* **Materials** - Physical surface parameters (toughness, velocity absorption...)
* **Menus** - Book menu page parameters
* **Ragdolls** - Colliders, joints and rigid bodies parameters for creatures
* **Spells** - Contains parameters for all existing spells. (Fire, Gravity, Lightning... etc)
* **Texts** - Work in Progress! This is used to define text used in game to assist in different languages
* **Liquids** - Defines the JSON for potions! Allows custom edits of potion parameters, and allows the use for modders to make their own.
* **Waves** - Wave definition (creature spawned, order, min/max count...) 
* **Game.json** - Game parameters

_Please Keep Note that there are also .bundle files, .hash files and a catelog json stored in this folder. DO NOT DELETE these, as these can cause many problems in game._ 

Keep in mind that each data type can be referenced by others (like container -> lootTable -> items), so try to avoid changing ID as it could break some references.