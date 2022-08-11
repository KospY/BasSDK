# Imbue Zone

> Not to be confused with [Zone][Zone]


Imbue zone is a zone that imbues items based on what Spell ID is indicated. Before this zone can be placed, a collider with "Trigger" enabled is required. This collider will be the zone of which the item gets imbued.

> Note: This only imbues items when an item is put in to the zone first. Will not imbue a weapon if the zone is stored on the same item.

## Components

| Field                       | Description
| ---                         | ---
| Transfer Rate               | Indicates the speed of which the item is imbued.
| Transfer Max Percent        | Indicates the maximum percentage that the item can be imbued.
| Imbue Spell ID              | Specifies the Spell ID that this zone will imbue with. For base-game spells, this would be `Fire`, `Gravity` and `Lightning`.

> Note: This zone also accepts modded spells. 

![ImbueZone][ImbueZone]


[Zone]: {{ site.baseurl }}{% link Components/ThunderRoad/Zone.md %}
[ImbueZone]: {{ site.baseurl }}/assets/components/Imbue Zone/ImbueZone.PNG