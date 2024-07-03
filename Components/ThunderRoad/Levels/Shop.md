---
parent: Levels
grand_parent: ThunderRoad
---

# Shop

The "Shop" script allows you to create your own shop, indicating both the "buy" and "Sell" zone, as well as creature components like Shopkeeper waypoints and price tags.

![Component][Component]

## Fields

| Field                                 | Description
| ---                                   | ---
| Shop Data ID                          | This indicates the Shop JSON ID
| Default Shopkeeper Spawn Point        | The default spawn point for the shopkeeper
| Tutorial Shopkeeper Spawn Point       | The spawn point for the shopkeeper during the tutorial
| Shop Contents                         | Contains the [Container][Container] for the contents of the shop
| Price Tag Prefab                      | Places this prefab on the items inside the shop for the price tag.
| Tutorial Over There Animation ID      | Plays this animation for the "over there" section of the tutorial.
| Shop Keeper Kill Reload Duration      | The duration it takes for reload when the shopkeeper is killed
| Shopkeeper Buy Waypoint               | Waypoint for the shopkeeper when the player approaches the "Buy" table
| Shopkeeper Sell Waypoint              | Waypoint for the shopkeeper when the player approaches the "Sell" table
| Shopkeeper Store Waypoint             | A [Zone][Zone] that encapsulates the shopkeeper area.
| Idle Waypoints                        | Waypoints for the shopkeeper to idle to.
| Player Buy Zone                       | The "[Zone][Zone]" for buying items in the shop
| Player Sell Zone                      | The "[Zone][Zone]" for selling items in the shop
| Player Store Zone                     | A [Zone][Zone] that encapsulates the whole shop

[ShootPointNavmesh]: {{ site.baseurl }}/assets/components/Shop/Component.png
[Zone]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/Zone.md %}
[Container]: {{ site.baseurl }}{% link Components/ThunderRoad/Creatures/Container.md %}