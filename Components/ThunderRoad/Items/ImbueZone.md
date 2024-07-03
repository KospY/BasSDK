---
parent: Items
grand_parent: ThunderRoad
---

# Imbue Zone

{: .important}
This component requires a collider to be put on the gameObject before adding the script.

Imbue Zone is a component which can imbue items in its' zone. 

![Component][Component]

## Fields

| Field                             | Description
| ---                               | ---
| Transfer Rate                     | How fast the item will imbue in the zone
| Transfer Max Percent              | You can change this to limit the amount of imbue this zone can provide. 100 is "100%" of imbue, while 25 would be 25% of the imbue, before it stops imbuing the item.
| Imbue Spell Id                    | Specify what spell imbue is imbuing this weapon. For Blade and Sorcery, the vanilla IDs are "Fire", "Lightning" and "Gravity".

[Component]: {{ site.baseurl }}/assets/components/ImbueZone/Component.png