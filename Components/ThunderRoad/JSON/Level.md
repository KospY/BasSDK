---
parent: JSON
grand_parent: ThunderRoad
---

# Level

The Level JSON is used to be able to load in to a level. 

| Keys                          | Description |
| ---                           | --- |
| id                            | Unique Identifier of the level, make sure this is unique so it does not conflict with other levels. |
| version                       | Version of the json. MUST BE "3" |
| name                          | Name of the level. This appears on the map board as the "name" of the level. Does not have to be unique. |
| description                   | Description of the level, which will appear on the map board when you click on the level |
| descriptionLocalizationId     | Description of the level, using the ID to link to the localization files. |
| sceneAddress                  | Addressable Address of the level scene. |
| showOnlyDevMode               | When true, the level will only appear in Developer Mode (Recommended: Set to false) |
| showInLevelSelection          | When the "allowLevelSelection" is set to true in the GameMode, if this key is set to true, it will be visible in character selection after selecting your character. |
| worldmapPrefabAddress         | Addressable address of the world map. Default is `Bas.Image.Map.Default` |
| worldmapTextureAddress        | Addressable address of the world map texture. If you make a custom Worldmap page, you can specify what texture it uses for it. Default is `Bas.Worldmap.Eraden` |
| worldmapLabel                 | If you want your map to have a custom page dedicated for itself, you can specify a specific name here, and use the arrows at the top of the world map to switch to that page. Set to `null` to use the default Eraden page. |
| worldMapTravelAudioContainerAddress | Addressable address to the audio container that plays a sound when you enter a new level from the map board. |
| mapLocationIndex              | Select what "number" you want your map to be on the world map. You are able to see the numbers from the worldmap located in the SDK. If a number already exists, it will pick a number near it instead. |
| showOnMap                     | If set to true, it will appear on the world map. |
| hideOnAndroid                 | If set to true, this map will not appear on the android platform (Nomad) |
| mapLocationIconAddress        | The addressable address of the icon that appears on the circle selection the map. |
| mapLocationIconHoverAddress   | The addressable address of the icon that appears when you hover over the icon of the map |
| mapPreviewImageAddress        | The addressable address of the preview appearing on the map when you select the map. Often shown as a sketch for the other levels. |
| modePickCountPerRank          | ***depricated/obsolete*** |
| customCamera                  | Used for Arcade Mode. ***Obsolete/Not in Use*** |
| throwableRefType              | References the type of "Throwable" that the NPC will use if the player is both in sight but not on a navigation mesh for the NPCs to get to. <details>• *Item* - Specifies a specific item. <br>• *Table* - Specifies a table of items, used from a LootTable json. </details> |
| improvisedThrowableID         | Specifies the ID of the item/table that is used as a throwable by an enemy.

## Modes

"Modes" is game modes available to the level. For example, Survival is a game mode where you have three pillars which spawn items, and once an item is grabbed, a wave begins, allowing a progressive flow of waves with a reward at the end. 

| Keys                          | Description |
| ---                           | --- |
| name                          | Name of the game-mode |
| displayName                   | The name shown on the mapboard when you select the game-mode |
| description                   | Localisation ID linking to the language file which contains the description of the game-mode |
| allowGameModes                | Allows the mode to be available in a specific gamemode. So, for example, "Survival" would be available in "Sandbox". |
| mapOrder                      | 
| playerDeathAction             | If the player dies, what happens? <details>• *None* - Nothing happens when player dies.<br>•  *AskReload* - Asks the player if they want to reload the level.<br>• *LoadHome* - When player dies, Home is automatically loaded.<br>• *PermaDeath* - When player dies, character is deleted (NOT RECOMMENDED)</details> 

## Modules

<details markdown="block">

<summary>LevelModuleResetSpawners</summary>

The ResetSpawners level module is used on most Arenas in Blade and Sorcery. This module will respawn all items and containers that are spawned via ItemSpawners when a new wave has started.  

```json
{
    "$type": "LevelModuleResetSpawners, ThunderRoad"
}
```

</details>

<details markdown="block">

<summary> LevelModuleCleaner </summary>

The Cleaner module is an optimisation module which cleans up left over items and corpses. The "cleanerRate" determines the delay before the item/body despawns.

```json
{
    "$type": "ThunderRoad.LevelModuleCleaner, ThunderRoad",
    "cleanerRate": 5.0
}
```   

</details>

<details markdown="block">

<summary> LevelModuleSurvival </summary>

The survival module allows the gamemode to use survival elements, where a pillar will rise from the floor with weapons able to play with. Once the weapon is grabbed, waves will spawn. Once a wave ends, the pillars rise again.

| Keys                          | Description |
| ---                           | --- |
| canOnlyUseRewards             | When true, the player will only be able to use the weapons from the pillars. |
| rewardPillarAddress           | The addressable address for the pillar prefab. Our default is `Bas.LevelModule.RewardPillar` |
| pillarZone                    | The zone of which the boundries of the pillars are when the wave ends. If the player is not within the bounds, the player will be teleported to one of the pillars. |
| rewardsToSpawn                | This depicts the amount of pillars/weapons that can spawn as a reward |
| firstRewardContainerID        | ID of the LootTable container that spawns first, before any combat begins |
| startDelay                    | The delay before the waves start after grabbing the weapon reward |
| loop                          | Depicts if there is an end to the survival. <details><br>• *Loop* - Goes through all the waves, then loops to the first<br>• *NoLoop* - Once all waves are complete, finish and go back home.<br>• *RepeatLastWave* - Once waves are complete, repeat the last wave on loop.</details> |
| wavesNumberForReward          | The amount of waves you need to complete before it gives you the next reward. |
| waves                         | See [Waves](#wave) |
| textGroupId                   | Localisation of the "Survival" text |
| testNextWaveId                | Localisation of the text that displays the next wave text |
| textFightId                   | Localisation of the text that displasy "Fight" |
| textWaveId                    | Localisation of the text that displays what wave it is |

### Wave

To add a wave to the Survival "Waves" field, add this: 
```json
{
        "$type": "ThunderRoad.LevelModuleSurvival+Waves, ThunderRoad",
        "waveID": "Survival_Wave0",
        "containerID": "WeaponRewards1"
}
```
| Keys                          | Description |
| ---                           | --- |
| waveID                        | ID of the wave json used to spawn the enemies |
| containerID                   | Container/LootTable of the reward the spawns at the end of this wave |

</details>

<details markdown="block">

<summary> LevelModuleMusic </summary>

Adds dynamic music to your map during waves. Insert the music JSON ID here.

```json
{
    "$type": "ThunderRoad.LevelModuleMusic, ThunderRoad",
    "dynamicMusic": "MusicArena"
}
```

</details>

<details markdown="block">

<summary> LevelModuleWaveAssault </summary>

The Wave Assault module is very similar to survival, but is used for Crystal Hunt. Instead of spawning 3 pillars, of which weapons are rewards, there is only 1 reward, of which requires 1 to 2 waves to be completed beforehand.

| Keys                          | Description |
| ---                           | --- |
| rewardPillarAddress           | The addressable address for the pillar prefab. Our default is `Bas.LevelModule.RewardPillar` |
| defaultLength                 | The default amount of waves player has to complete before reward is given |
| textFightGroupID              | Localisation of the Fight Group (?)
| textFightID                   | Localisation of the "Get Ready" text
| textReturnHomeGroupId         | Localisation of the "Return Home" text (?)
| textReturnHomeId              | Localisation of the "Return Home" text (?)
| pillarZone                    | The zone of which the boundries of the pillar are when the wave ends. If the player is not within the bounds, the player will be teleported to the pillar. |
| returnHomeFadeInDuration      | When the reward is grabbed, this is the duration of the black fade in before it goes to the loading screen

```json
{
    "$type": "ThunderRoad.LevelModuleWaveAssault, ThunderRoad",
    "rewardPillarAddress": "Bas.LevelModule.RewardPillar",
    "defaultLength": 3,
    "textFightGroupId": "Survival",
    "textFightId": "GetReady",
    "textWaveId": "Wave",
    "textReturnHomeGroupId": "Survival",
    "textReturnHomeId": "RewardHome",
    "pillarZone": null,
    "returnHomeFadeInDuration": 2.0
}
```


</details>