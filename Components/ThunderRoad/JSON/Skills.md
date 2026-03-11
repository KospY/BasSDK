---
parent: JSON
grand_parent: ThunderRoad
---

# Skills

A Skill JSON defines the behaviour, look and order of a specific skill or spell within the skill tree.

| Keys | Description |
| --- | --- |
| id | The unique ID for the skill |
| version | Version of the JSON. Must be **0** |
| tier | The level requirement within the tree (e.g., **2**). This value is indexed, `0 = T1,` `1 = T2,` `2 = T3` etc |
| skillTreeDisplayName | A localized name for the skill |
| description | A localized description for the skill |
| costOverride | Manually sets the skill point cost. Set to **-1** to use the tree's default scaling |
| isTierBlocker | If true, this skill must be bought to progress through tier |
| orbIconAddress | The Addressable address for the icon shown on the skill tree orb |
| videoAddress | The Addressable address for the demonstration video in the popup |
| prefabAddress | The Addressable address for the skill orb prefab |
| shardId | The Item ID of the shard used to purchase this skill |
| primarySkillTreeId | The ID of the main Skill Tree this node belongs to (e.g., **Lightning**) |
| secondarySkillTreeId | The ID of the secondary Skill Tree this node belongs to (e.g., **Fire**) |

# Survival

| Keys | Description |
| --- | --- |
| canSpawnAsReward | If true, this skill will show up on its own during Survival |
| allowInRouletteMode | Controls whether this skill will show in Roulette |
| combatSkill | Defines if the skill is categorized for combat or utility |
| allowOnFirstWave | If true, this skill can show up during the initial reward stage of Survival |
| requiredSkills | A list of Skill IDs that must be unlocked before this skill can show in Survival |