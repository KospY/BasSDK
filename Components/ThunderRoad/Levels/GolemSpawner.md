---
parent: Levels
grand_parent: ThunderRoad
---
# Golem Spawner

## Overview
This component allows you to spawn in a golem, either with [Unity Events](https://docs.unity3d.com/Manual/UnityEvents.html), or when the level loads.

![Component][Component]

| Field | Description |
| :--- | :--- |
| golemAddress | The prefab address of the golem, the vanilla game uses `Bas.Boss.Golem`|
| arenaCrystalRandomizer | The GameObject that contains your ArenaCrystalRandomizer, can be empty |
| spawnOnStart | If the golem spawns when the level loads |
| actionOnSpawn | <details>- The golem does nothing on spawn None<br>- The golem waits for the player to get close before activating Disable<br>- The golem starts attacking immediatly f Wake<br></details> |
| onStartWakeFull |  |
| onStartWakeShortA |  |
| onStartWakeShortB |  |
| onGolemAwaken | When the golem wakes up |
| onGolemDefeat | When the golem falls over |
| onGolemKill | When the golems crystal is pulled |
| onGolemStun | When the golem gets stunned by the player |
| onCrystalGrabbed | When the golems crystal is grabbed |
| onCrystalUnGrabbed | When the golems crystal is let go of |

[Component]: {{ site.baseurl }}/assets/components/GolemSpawner/GolemSpawnerComponent.png