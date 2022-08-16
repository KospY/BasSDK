# Container Spawner

```note
Not to be confused with [ItemSpawner][ItemSpawner] and [CreatureSpawner][CreatureSpawner].

[ItemSpawner]: {{ site.baseurl }}{% link Components/ThunderRoad/ItemSpawner.md %}
[CreatureSpawner]: {{ site.baseurl }}{% link Components/ThunderRoad/CreatureSpawner.md %}
```

The Container Spawner is a script used to spawn items contained within a Container json. It will spawn the item from the list, which could make spawning of an item random.

![ContainerSpawner][ContainerSpawner]

## Componants

| Field                       | Description
| ---                         | ---
| Container ID                | ID of the container that items will spawn from.
| Tier                        | Choice Between `None`, `Common` and `Rare` [Not Currently Used]
| Pooled                      | Gets the Items from the Item Pool if ticked. 
| Spawn All                   | Will Spawn ALL items from this pool, instead of one random item.
| Allow Duplicates            | If items are spawned multiple times, or has multiple spawn points, having this ticked allows for the same item to appear multiple times during the spawn container. Unticked, the item(s) will only ever appear once.
| Disallow Despawn            | Disallows the items spawned to despawn
| Holder is Pivot             | The pivot of this gameobject is the GameObject containing the Container Spawner. When enabled, the HolderPoint of the item is the Pivot instead.
| Spawn Points                | With this, you point where the objects will spawn. Can spawn many items at a time.


[ContainerSpawner]: {{ site.baseurl }}/assets/components/ContainerSpawner/ContainerSpawner.PNG