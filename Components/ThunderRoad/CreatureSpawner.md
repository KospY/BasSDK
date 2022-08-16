# Creature Spawner

Creature Spawner, or CreatureTable Spawner is a script used to spawn creatures within a level. 

> Note: With an Event, Spawn() can be used to spawn the creature on demand.

![CreatureSpawner][CreatureSpawner]

## Components

| Field                       | Description
| ---                         | ---
| Creature Table ID           | Reference the Creature Table ID (CreatureTable JSONS)
| Pooled                      | Spawns the creature from the Creature Pool. `Recommended for optimisation, using the pool generated at runtime.`
| Async Spawn                 | Spawns creatures via Async. This spawns them immediatly, but can break the creature. If creatures spawned this way is broken, uncheck this.
| Spawn on Start              | Spawns the creature when the level is loaded, or when this GameOject containing this script is enabled.
| Block Load                  | If `Spawn on Start` is enabled, it will instead delay the spawning of the creature until after the level loading has been completed.
| Spawn on Nav Mesh           | If the creature has not spawned on an area where there is a navmesh, will move to spawn the creature on the nearest navmesh. 
| Ignore Room Max NPC         | Used for Dungeon. Ignores Room Max NPC limit per room, and spawns the creature anyway.
| Spawn at Random Waypoint    | Spawns NPC at a random [WayPoint][WayPoint] on the map.
| Waypoints Root              | Allows you to select what [WayPoint][WayPoint] the NPC spawns on.

## Events

> For Events, see [UnityEvents][UnityEvents]

| Field                       | Description
| ---                         | ---
| On Start                    | Plays Event when the creature has Spawned/Creature Spawner has started.
| On Kill                     | Plays Event when the creature is killed
| On Despawn                  | Plays Event when the creature has despawned

[CreatureSpawner]: {{ site.baseurl }}/assets/components/CreatureSpawner/CreatureSpawner.PNG
[WayPoint]: {{ site.baseurl }}{% link Components/ThunderRoad/WayPoint.md %}
[UnityEvents]: https://docs.unity3d.com/Manual/UnityEvents.html