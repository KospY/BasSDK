---
parent: ThunderRoad
---
# Player Spawner

Player Spawner is a script required to spawn the player in a map. Multiple of these can be used on a map to randomise spawn locations.

{: .tip}
Ensure that this GameObject is placed on the floor or near the floor, as if it is placed too high, player can die of fall damage.


![PlayerSpawner][PlayerSpawner]

| Field                       | Description
| ---                         | ---
| ID                          | ID of the player spawn. `Recommended to be set to "default"`
| Spawn Weight                | `-1` means that it will use the default spawning chances. `Note: If there is other player spawners, -1 defaults to 50% chance to spawn here`
| Spawn Body                  | Spawns the player body
| Force Spawn Here if New Player | Used for Tutorial. This is used to spawn in this location if the player character is new.
| Player Spawn Event ()       | Unity Event occurs when player spawns.





[PlayerSpawner]:{{ site.baseurl }}/assets/components/PlayerSpawner/PlayerSpawner.PNG