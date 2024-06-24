---
parent: Levels
grand_parent: ThunderRoad
---
# Imbue Zone

{: .note}
Not to be confused with [Zone][Zone]

[Zone]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/Zone.md %}



Imbue zone is a zone that imbues items based on what Spell ID is indicated. Before this zone can be placed, a collider with "Trigger" enabled is required. This collider will be the zone of which the item gets imbued.

{: .tip}
This only imbues items when an item is put in to the zone first. Will not imbue a weapon if the zone is stored on the same item.


## Components

| Field                       | Description
| ---                         | ---
| Transfer Rate               | Indicates the speed of which the item is imbued.
| Transfer Max Percent        | Indicates the maximum percentage that the item can be imbued.
| Imbue Spell ID              | Specifies the Spell ID that this zone will imbue with. For base-game spells, this would be `Fire`, `Gravity` and `Lightning`.

{: .note}
This zone also accepts modded spells.


![ImbueZone][ImbueZone]


[ImbueZone]: {{ site.baseurl }}/assets/components/Imbue Zone/ImbueZone.PNG