# FleePoint

Flee Points are positions enemies will attempt to reach when fleeing. Upon reaching a flee point, the enemy will despawn from the level. These are often placed near doors or exits that the player cannot interact with.

Enemies will flee when the wave has ended early, or if they are disarmed and are unable to find a new weapon.

Flee points can be automatically generated from the [Wave Spawner][WaveSpawner] component by enabling the "Add As Fleepoint On Start" toggle. Doing so will create a flee point at each of the wave's spawn points.

The gameobject this component is attached to must be placed near to the ground.

```note
There are no fields on this component.
```

[WaveSpawner]: {{ site.baseurl }}{% link Components/ThunderRoad/WaveSpawner.md %}