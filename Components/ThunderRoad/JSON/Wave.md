---
parent: JSON
grand_parent: ThunderRoad
---

# Wave

The Wave JSON is used to spawn waves of enemies in maps in Blade and Sorcery. 

| Keys                          | Description |
| ---                           | --- |
| id                            | ID of the wave. Make sure it is unique so it does not overwrite/conflict with other mods |
| version                       | Version of JSON. MUST be `2` |
| category                      | What category the wave will come under. If the category doesn't already exist, it will create a new one on the Wave Spawner |
| localizationID                | ID of the section of the localisation that will change the "title" and "description depending on Language. |
| title                         | Name of the wave you are making. Will be under the Category set. |
| description                   | Text displayed when the wave is clicked on |
| loopBehaviour                 | Depicts how the wave loops. <details>• *NoLoop* - Will not loop the wave, the wave will end once all the enemies are defeated.<br>• *LoopSeamless* - Will loop the wave seamlessly, with infinite enemies spawning until you end the wave manually.<br>• *Loop* - Will loop the wave. When it gets to the bottom of the enemy list, once the last enemy is killed, it will restart the wave.</details> |
| totalMaxAlive                 | The total amount of enemies that can be on the map at a time. Higher the number, the more enemies that spawn, but the laggier it could get. |
| alwaysAvailable               | Wave is available on all maps, ignores "waveSelectors". If false, it will only be available on map IDs listed in "waveSelectors". These IDs are not the map ID, but the ID that is stated in the wave spawner. |
| waveSelectors                 | If "alwaysAvailable" is false, this wave can only appear on these maps. |
| factions                      | Stated what faction(s) this wave will contain. <details>• *-1 Passive* - NPCs will not attack.<br>• *0 None* - NPC will attack all, NPC and Player.<br>• *1 Ignore* - Will ignore player<br>• *2 Player* - Friendly. Will attack enemies but not player.<br>• *3 MixedEnemies* - Used for faction-less NPCs.<br>• *4 Bandits* - Bandit Faction, known in game as "Kingdom of Eraden".<br>• *5 Cult* - Cult Faction, known in game as "The Eye".<br>• *6 Mercenary* - Mercenary Faction, used with Bandits as "Wildfolk" or "Tribal".<br>• *7 Gladiator* - Gladiator faction, known in game as "Scavengers" or "Outlaws".<br>• *8 Soldier* - Soldier faction, used with "Bandits" or "Kingdom of Eraden".<br>• *9 Rogue* - Rogue faction, used with "Cult" faction or "The Eye".<br>Note: All *named* factions will combat other factions including the player. |

## Groups

Each "Group" is an enemy that spawns. If you want a seamlessly looping wave, you only need to specify one group. Otherwise, one group can be 1 enemy, or lots. Enemy count of that specific group can be adjusted in the "minMaxCount" field. 

| Keys                          | Description |
| ---                           | --- |
| reference                     | Whether you want to use "Table" which spawns the creature from a CreatureTable, or "Creature" to specify a specific creature. |
| referenceID                   | ID of the CreatureTable or Creature you want to use. |
| overrideFaction               | If true, this group will ignore the creature/table default faction and use the factionID field in this group. |
| factionID                     | Will use the faction if overrideFaction is true. The number represents the faction used, in order of [Factions](#wave) listed. |
| overrideContainer             | If true, this group will ignore containers stored in the creature/table. Will use overrideContainerID instead |
| overrideContainerID           | ID of the LootTable container used if overriden |
| overrideBrain                 | If true, this group will ignore NPC brains stored in the creature/table. Will use overrideBrainID instead |
| overrideBrainID               | ID of the Brain used if overriden |
| overrideMaxMelee              | If true, this group will ignore the set maximum amount of enemies that can be fighting the player at a time. <details>For example, if set to 1, only one enemy can fight at a time, and the other enemies will circle the player. If set to 3, 3 enemies will try and attack you at the same time. </details> |
| overrideMaxMeleeCount         | The amount of enemies that will fight the player at the same time, if overrideMaxMelee is true |
| groupHealthMultiplier         | The multiplier of the health enemies will have. "1.0" is their normal health amount. |
| minMaxCount                   | The minimum/maximum amount of enemies that can spawn from this group. |
| spawnPointIndex               | *Unused/Obsolete*, determines spawn point of an NPC.
| prereqGroupIndex              | Determines a group that needs to be "completed" before this group can spawn. <details> The first group is group -1, the second group is group 0, third group is group 1, it keeps going from there. |
| prereqMaxRemainingAlive       | Determines how many can be left alive from a previous group to be considered completed. <details> For example, setting group 0 to a MaxRemainingAlive to 0 means it will wait for group -1 to have no more enemies alive.<br>-1 means no prerequisite amount.|