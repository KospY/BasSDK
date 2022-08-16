# Waves Spawner

```note
The Waves Spawner is a requirement for `UI Wave Spawner` 
```

The Wave Spawner is a script which allows AI to spawn in the map. Through the use of spawn points, as well as music handling and Events, this script allows you to adjust the way AI spawn.

![WaveSpawner][WaveSpawner]

## Components

| Field                       | Description
| ---                         | ---
| Spawns                      | Spawns are the spawnpoints that AI will spawn at at the start of a wave. These GameObjects can be a child of the wavespawner, with no script required except to be referenced here. `It is recommended to place these gameobjects as low to the ground as possible. The blue arrow / Z Axis points forward, as well as green arrow pointing upwards. Ensure that these gameobjects are not tilted on the Y or X axis as this can cause issues.`
| Add as FleePoint on Start   | This entails that when a wave ends or an enemy has been disarmed (with no available weapon), enemies will flee to these points to despawn. 
| Begin Wave on Start         | When ticked, a wave will start as soon as the level is loaded.
| Begin Wave on Start Delay   | Inputs the delay before the wave starts when loading in to the level.
| Start Wave ID               | ID of the wave that the spawner is to start with. Can be found in `Default/Waves`.
| Pooled                      | When ticked, uses creatures from the pool of characters. `Warning: Unticking this can cause lag on NPC spawns`.
| Clean Bodies and Items on Wave Start | Despawns dropped items and dead bodies when a new wave has started.
| Ignored Faction ID          | Faction that NPCs are forced to is the spawner is disabled.
| Update Rate                 | Determines how often the spawner checks to see if it can spawn a new creature.
| Same Spawn Delay            | Determines the delay between spawning creatures at the same spawn point.
| Spawn Delay                 | Delay between AI spawns.
| **Music**
| Music Wave Address          | The addressable of what music plays when a wave is in progress.
| Music Audio Volume          | The audio of the playing music when wave is in progress. Separate from in game audio settings.
| Step Audio Group Address    | [Needs Info]
| Step Audio Volume           | Audio volume of `Step Audio Group Address`.

## Events

```note
When the event is met, its output is played. For information on Unity Events, see [Unity Events][UnityEvents].

[UnityEvents]: https://docs.unity3d.com/Manual/UnityEvents.html
```

| Event                       | Description
| ---                         | ---
| On Wave Begin Event         | Completes event when a wave has started.
| On Wave Any End Event       | Completes event when a wave is ended in any way.
| On Wave Win Event           | Completes event when a wave has bee won (NPC list in wave has ended).
| On Wave Loss Event          | Completes event when a wave has been lost (Player Dying).
| On Wave Loop  Event         | Completes event when a wave has finished the NPC list and looped.

[WaveSpawner]: {{ site.baseurl }}/assets/components/WaveSpawner/WaveSpawnerScript.PNG