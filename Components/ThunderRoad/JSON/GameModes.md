---
parent: JSON
grand_parent: ThunderRoad
title: GameModes
---

# GameModes

The "GameModes" JSON is used to create game modes in Blade and Sorcery. It allows you to change specific components of the game, as well as allow you to adjust difficulty settings and the Home Level.

{: .note}
If you want to change any points regarding already-existing game modes, it is recommended to only reference the ID and the field you have edited, to prevent any file override conflicts.

| Keys                          | Description |
| ---                           | --- |
| id                            | ID of the gamemode. Must be unique to avoid conflict of already existing gamemodes. |
| name                          | Name of the gamemode that will appear on GameMode Selection |
| nameLocalizationId            | ID of the localization section for the "name". |
| warning                       | The text used before you start the game. For example, this would be used if you have Mods instlaled for CrystalHunt |
| warningId                     | Localisation ID for the warning text. |
| iconAddress                   | Addressable address of the "Icon" that is used for the main menu game-mode selection |
| order                         | Order of which the gamemode will appear on the selection list |
| state                         | Determines the visibility of the gamemode. <details>• *Show* - Shown and able to be played.<br>• *Disabled* Unable to be shown or played.<br>• *Hidden* - Not shown but able to be played. </details> |
| refreshMapOnlyWhenPlayerNear  | Will refresh the map selection board only when the player gets near it. |
| saveGameOnSkillUnlock         | Will save the player progress when they unlock a skill |
| allowLevelSelection           | Allows the player to select the levels they want to load in to. |
| allowRefundCoreSkills         | Allows the player to refund the "core" skills. |
| defaultPlayerInvincibility    | When enabled, player will be invincible by default. |
| defaultPlayerFallDamage       | When enabled, player will have fall damage by default |
| defaultPlayerInfiniteFocus    | When enabled, player will have infinite focus by default |
| defaultPlayerFastCast         | When enabled, player will have instant spell casting by default |
| defaultInfiniteImbue          | When enabled, player imbued weapons will have its imbue last forever |
| defaultInfiniteSupply         | When enabled, player-owned quivers and weapon holders will be infinite |
| defaultClimbFree              | When enabled, climbing will be easier |
| defaultEasyDismemberment      | When enabled, dismemberment will be easier |
| defaultInfiniteArrows         | When enabled, player's arrows will be infinite, and will spawn on the bow when pulled back |
| defaultArmorDetection         | When enabled, enemy's armor will have detection. When disabled, enemy armor is ineffective and won't block/negate weapon attacks |
| levelHome                     | Level ID of the default "Home" map. |
| levelHomeModeName             | ID of the gamemode used in the home for in-game events |
| levelHomeTravelSpawnerId      | Name of the PlayerSpawner transform of the spawner that spawns the player after returning from a level |
| hasTutorial                   | When true, the gamemode will have a tutorial option |
| tutorialPlayerSpawnerId       | ID of the PlayerSpawner that spawns the player when the tutorial starts. |
| playerInventorStart           | Default Inventory for player options. See [Inventory Start](#inventory-start) |
| mainCurrency                  | The main currency used for monetary transactions. |
| otherCurrencies               | You can list alternative currencies here. |
| characterStatPrefabAddress    | Addressable address of the player stats UI |
| difficultyOptions             | Shows potential difficulty adjustments. See [Difficulty](#difficulty) |

## Inventory Start

The inventory start section allows you to have default starting options for the player in the game-mode. Currently this is used for selecting what type of magic you want to start with, but can be used for starting gold, inventory contents and specific armor/weapons.

| Keys                          | Description |
| ---                           | --- |
| containerID                   | ID of Container json which contains the contents of the player inventory at default |
| textGroupID                   | ID of the text group the localisation of the title is located |
| titleTextID                   | ID of the text used for the title used for the inventory start |
| descriptionTextID             | ID of the text used for the start description |
| descriptionText               | Text used for the Description if it is not edited by the Text ID |
| titleImageAddress             | Addressable address of the image used for the inventory start |
| currencies                    | Allow you to start with a basic amount of currency. You can add "Gold" and "CrystalShard" by default.

## Difficulty

The Difficulty options is used to create difficulty settings, from allowing cheats to adjusting player damage.

### Potential Difficulty Options


| Difficulty Option             | Description |
| ---                           | --- |
| DealthPenalty                 | Allows you to have a penalty to death, like losing gold on death or delete your character when you die. |
| EnemyDamageOption             | Allows you to adjust how much damage enemies deal to the player |
| PlayerDamageOption            | Allows you to adjust how much damage the player deals to enemies |
| AutoTranslateOption           | When set, Dalgarian notes will automatically translate to english. |
| CheatsMenu                    | When set, the cheats option will be available in the gamemode

### Difficulty Presets

With the difficulty presets, you can automatically set difficulty options with the specific difficulty, under the "Drop Levels".

## Modules

<details markdown="block">
<summary>Crystal Hunt Level Instances Module</summary>

The Crystal Hunt level module determines all settings related to Crystal Hunt, from the tutorial to the dungeon map board.

| Keys                          | Description |
| ---                           | --- |
| mapInfoPrefabAddress          | Addressable address of the map information | 
| difficultyIcon                | Addressable address of the level difficulty. Default is `Bas.Icon.Skull` |
| difficultyIconColor           | Color picker of the difficulty icon |
| minPinsToGeneratePerDay       | The minimum amount of dungeon levels that can appear on a map board every time the player returns home |
| maxPinsToGeneratePerDay       | The maximum amount of dungeon levels that can appear on a map board every time the player returns home |
| arenasLevelInfo               | References the "Arenas" that can appear on the map board, with their designated game mode |
| minNumberOfArenasAllowedPerDay | The minimum amount of arenas that can appear on the map board after the player returns home |
| maxNumberOfArenasAllowedPerDay | The maximum amount of arenas that can appear on the map board after the player returns home |
| dalgarianDungeonLevelInfo     | All the information regarding the enemy configurations and loot configurations for Dalgarian Dungeons |
| dalgarianMapLocationRandomNearest | The nearest map point that the Dalgarian Dungeon map icon will spawn near |
| tutorialDungeonLevelInfo      | Information regarding the tutorial dungeon, including the level and mode id |
| outpostDungeonLevelInfo       | All the information regarding the enemy configurations and loot configurations for the Outpost dungeon | 
| outpostMapLocationRandomNearest | The nearest map point that the Outpost Dungeon map icon will spawn near |
| shopID                        | ID of the level ID used for the shop |
| endRewardBalanceAddress       | Addressable address relating to the end of dungeon reward configuration
| dungeonLengthBalanceAddress   | Addressable address relating to the dungeon length balance configuration |
| dungeonLootMultiplierBalanceAddress   | Addressable address relating to the dungeon loot multiplier configuration |
| dungeonTypeBalanceAddress     | The addressable address relating to the dungeon type balance configuration |
| outpostFactionTierBalanceAddress | The addressable address relating to the outpost faction tiers balance configuration |
| dalgarianFactionTierBalanceAddress | The addressable address relating to the dalgarian faction tiers balance configuration |

<details markdown="block">
<summary>JSON block</summary>

    {
      "$type": "ThunderRoad.Modules.CrystalHuntLevelInstancesModule, ThunderRoad",
      "mapInfoPrefabAddress": "Bas.Map.CrystalHuntMapInfos",
      "difficultyIcon": "Bas.Icon.Skull",
      "difficultyIconColor": {
        "r": 0.215686277,
        "g": 0.1764706,
        "b": 0.1764706,
        "a": 1.0
      },
      "minPinsToGeneratePerDay": 3,
      "maxPinsToGeneratePerDay": 5,
      "arenasLevelInfo": [
        {
          "$type": "ThunderRoad.Modules.CrystalHuntLevelInstancesModule+LevelInfo, ThunderRoad",
          "levelId": "Arena",
          "modId": "WaveAssault"
        },
        {
          "$type": "ThunderRoad.Modules.CrystalHuntLevelInstancesModule+LevelInfo, ThunderRoad",
          "levelId": "Sanctuary",
          "modId": "WaveAssault"
        },
        {
          "$type": "ThunderRoad.Modules.CrystalHuntLevelInstancesModule+LevelInfo, ThunderRoad",
          "levelId": "Canyon",
          "modId": "WaveAssault"
        },
        {
          "$type": "ThunderRoad.Modules.CrystalHuntLevelInstancesModule+LevelInfo, ThunderRoad",
          "levelId": "Citadel",
          "modId": "WaveAssault"
        },
        {
          "$type": "ThunderRoad.Modules.CrystalHuntLevelInstancesModule+LevelInfo, ThunderRoad",
          "levelId": "Market",
          "modId": "WaveAssault"
        }
      ],
      "minNumberOfArenasAllowedPerDay": 1,
      "maxNumberOfArenasAllowedPerDay": 2,
      "dalgarianDungeonLevelInfo": {
        "$type": "ThunderRoad.Modules.CrystalHuntLevelInstancesModule+LevelInfo, ThunderRoad",
        "levelId": "DungeonDalgarian",
        "modId": "CrystalHunt"
      },
      "dalgarianEnemyConfigs": [
        "DalgarianEnemyConfigT1",
        "DalgarianEnemyConfigT1",
        "DalgarianEnemyConfigT2",
        "DalgarianEnemyConfigT3"
      ],
      "dalgarianLootConfigs": [
        "DalgarianLootConfigT0",
        "DalgarianLootConfigT1",
        "DalgarianLootConfigT2",
        "DalgarianLootConfigT3"
      ],
      "dalgarianMapLocationRandomNearest": 3,
      "tutorialDungeonLevelInfo": {
        "$type": "ThunderRoad.Modules.CrystalHuntLevelInstancesModule+LevelInfo, ThunderRoad",
        "levelId": "DungeonTutorial",
        "modId": "CrystalHunt"
      },
      "tutorialEnemyConfig": "OutpostEnemyConfigT0",
      "tutorialLootConfig": "OutpostLootConfigT0",
      "outpostDungeonLevelInfo": {
        "$type": "ThunderRoad.Modules.CrystalHuntLevelInstancesModule+LevelInfo, ThunderRoad",
        "levelId": "DungeonOutpost",
        "modId": "CrystalHunt"
      },
      "outpostEnemyConfigs": [
        "OutpostEnemyConfigT0",
        "OutpostEnemyConfigT1",
        "OutpostEnemyConfigT2",
        "OutpostEnemyConfigT3"
      ],
      "outpostLootConfigs": [
        "OutpostLootConfigT0",
        "OutpostLootConfigT1",
        "OutpostLootConfigT2",
        "OutpostLootConfigT3"
      ],
      "outpostMapLocationRandomNearest": 5,
      "shopID": "Shop",
      "endRewardBalanceAddress": "Bas.Config.CrystalHunt.DungeonEndReward",
      "dungeonLengthBalanceAddress": "Bas.Config.CrystalHunt.DungeonLength",
      "dungeonLootMultiplierBalanceAddress": "Bas.Config.CrystalHunt.DungeonLootMultiplier",
      "dungeonTypeBalanceAddress": "Bas.Config.CrystalHunt.DungeonType",
      "outpostFactionTierBalanceAddress": "Bas.Config.CrystalHunt.DungeonOutpostFactionTiers",
      "dalgarianFactionTierBalanceAddress": "Bas.Config.CrystalHunt.DungeonDalgarianFactionTiers"
    }

  </details>
</details>


