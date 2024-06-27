---
parent: Areas
grand_parent: ThunderRoad
---
# Item Spawner Limiter

The Item Spawner Limiter is a script used for Areas to limit the amount of items that spawn in the room.

This script changes how ItemSpawners spawn their items, and will spawn then randomly depending on their priority. 

[Item Spawner Documentation][ItemSpawner]{: .btn .btn-purple }

## Component Properties

| Field | Description |
| --- | --- |
| Max Spawn | This is the max amount of items that spawn in the room. |
| Max Child Spawn | This is the amount of children item spawners (spawners that have a parent spawner referenced) that spawn items. |
| Android Max Spawn | This is the max amount of items that spawn in the room. (for android) |
| Android Max Child Spawn | This is the amount of children item spawners (spawners that have a parent spawner referenced) that spawn items. (for android) |
| Spawn on Start | Spawns the items on start (should be disabled) |
| Spawn on Level Load | Spawns the items on Level Load (should be disabled) |

[ItemSpawner]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/ItemSpawner.md %}