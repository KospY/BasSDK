---
parent: Creatures
grand_parent: ThunderRoad
---
# Container

The Container script is used to load specific items/armors on creatures and the Player. This can load a specific container for the player when they load in to an area, and also use [ContainerSaver][ContainerSaver] to save their current container so it persists through levels.

![Component][Component]

## Fields

| Field                             | Description
| ---                               | ---
| Load Content                      | Will load the container. "Container ID" uses the Container ID, and "Player Inventory" loads the player's inventory.
| Load Player Container ID          | Will load the Container ID for the player
| Container ID                      | Uses the Container ID for this component
| Load on Start                     | Loads the container on Start
| Spawn Owner                       | Will spawn the container for the Owner.
| Linked Holders                    | Can spawn the container items in to the listed [Holder][Holder]

[Holder]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Holder.md %}
[ContainerSaver]: {{ site.baseurl }}{% link Components/ThunderRoad/Creatures/ContainerSaver.md %}
[Component]: {{ site.baseurl }}/assets/components/Container/Component.PNG