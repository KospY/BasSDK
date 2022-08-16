# Level

The level script is required for a map to load properly. It references spawning the player, as well as allowing custom references for easy referencing in custom scripts. ]

![LevelScript][LevelScript]

## Components

| Field                       | Description
| ---                         | ---
| Spawn Player                | When ticked, allows player to spawn.
| Player Spawner ID           | Indicates ID of Player Spawner. Recommended to be set to "default"
| Custom References           | Allows you to reference GameObjects easily in code through custom references.

[LevelScript]: {{ site.baseurl }}/assets/components/Level/LevelScript.PNG