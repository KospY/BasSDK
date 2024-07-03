---
parent: Creatures
grand_parent: ThunderRoad
---

# Container Saver

{: .important}
This saves to the listed [Container][Container] and the Player Container.

Container Saver is a component that saves containers for the player. This can be placed in levels to always save the container whenever it is changed.

![Component][Component]

## Fields

| Field                             | Description
| ---                               | ---
| Save Player Inventory             | Saves the player's inventory
| Save on Level Unload              | Saves the container when the level is unloaded (e.g. loading another level)
| Save if Player Alive Only         | Will only save the container if the player is alive.
| Save on App Quit                  | Will save the container when the game is closed.
| Transfer Un Owned Item to Player on Grab  | If the item is marked as "stolen" or is now owned by the player, it will become owned if the player grabs the item, if this is ticked.
| Transfer include item from spawner | Items spawned via a spawner will become owned by the player
| Save Containers                   | Saves listed containers that may be placed in a scene

## Events

[Unity Event Documentation](https://docs.unity3d.com/Manual/UnityEvents.html){: .btn .btn-purple }

| Event                             | Description
| ---                               | ---
| On Pre Save                       | Will play the event before the container is saved.

[Container]: {{ site.baseurl }}{% link Components/ThunderRoad/Creatures/Container.md %}
[Component]: {{ site.baseurl }}/assets/components/ContainerSaver/Component.png