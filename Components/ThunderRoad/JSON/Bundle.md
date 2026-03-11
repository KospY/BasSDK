---
parent: JSON
grand_parent: ThunderRoad
---

# Bundles

A Bundle JSON is used to group specific items, skills, and loot tables together into a single type for the Survival level mode.

| Keys | Description |
| --- | --- |
| id | The unique ID for the bundle |
| version | Version of the JSON. Must be **0** |
| localizationGroup | The ID of the localisation group |
| displayName | A localized name for the bundle |
| description | A localized description of the bundle contents |
| isArmorBundle | If true, the bundle is treated specifically as a set of wearable armor and will be equipped |
| bundleItemId | The base Item ID used to represent the bundle object itself |
| items | A list of Item IDs included in this bundle (e.g., **BowOld**, **QuiverSmall**) |
| skills | A list of Skill IDs granted  with this bundle |
| lootTables | A list of LootTable IDs used to generate random contents |
| spriteAddress | The Addressable address for the icon used in the UI popup |