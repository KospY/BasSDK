# Item Spawner

Item Spawner is a script that allows you to spawn an item, whether it be to spawn a prop, shoot an item or any other means. This script allows you to spawn items through [Unity Events][UnityEvents], spawning items with motion, to allow you to create spawnable projectiles.

![ItemSpawner][ItemSpawner]

```note
"Spawn" Button is for internal use only. May be removed in the future.
```

## Components

| Field                 | Description
| ---                   | ---
| Item ID               | The Item ID of the [Item][Item] you want to spawn.
| Pooled                | Spawns items that are stored in pool (For items like Arrows, rocks).
| Spawn on Start        | Spawns the item on level load or on component enable.
| Disallow Despawn      | Disallows the item spawned to despawn.
| Spawn Count           | Depicts the amount of the item that spawn at one time.
| Random Radius         | Radius of the item spawn (Indicates items don't spawn at one point).
| Random Rotate         | When ticked, items spawned will be randomly rotated.
| Holder Object         | Reference the [Holder][Holder] that the item gets spawned in to.

## Spawning Objects in Motion

| Field                     | Description
| ---                       | ---
| Linear Velocity Mode      | The mode of which the items spawn. ``World Space`` spawns it at an `X/Y/Z` based on the world, ``Spawner Space`` spawns it based on the rotation of the spawner, and ``Item Space`` spawns the items based on the Item HolderPoint. 
| Linear Velocity Mode     s | Depicts the velocity of the item on the X/Y/Z Axis.
| Angular Velocity Mode     | The mode of which the items rotate when they spawn. See `Linear Velocity Mode` for info on modes.
| Angular Velocity          | Depicts the rotational speed of the item on the X/Y/Z Axis.
| Force Throw               | When ticked, items will spawn in thrown, enabling "OnFly" on the objects that have it enabled in the JSON. `An example is: Arrows will fly on their fly ref`.
| Ignore Collision Collider | Depicts collider that the items ignore when spawned/thrown.
| Ignore Collision Item     | Depicts items that the items ignore when spawned/thrown.
| Ignore Collision Ragdoll  | Depicts ragdolls that the items ignore when spawned/thrown.
| Set Player Last Handler   | Depicts that when spawned, the player will be seen as the last handler, and therefore counts as player kills if it kills NPCs.
| Ignored Player Parts      | Depicts parts that the item spawner ignores when spawned. Ones still enabled will deal damage/affect the player.

```note 
This is the gizmo of the ItemSpawner when Random Radius is above 0.
```

![SpawnerGizmo][SpawnerGizmo]


[UnityEvents]: https://docs.unity3d.com/Manual/UnityEvents.html
[ItemSpawner]: {{ site.baseurl }}/assets/components/ItemSpawner/ItemSpawner.PNG
[SpawnerGizmo]: {{ site.baseurl }}/assets/components/ItemSpawner/SpawnerGizmo.PNG
[Item]: {{ site.baseurl }}{% link Components/ThunderRoad/Item.md %}
[Holder]: {{ site.baseurl }}{% link Components/ThunderRoad/Holder.md %}