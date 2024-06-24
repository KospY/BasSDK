---
parent: Levels
grand_parent: ThunderRoad
---
# Water Zone

Water Zone is similar to [ImbueZone][ImbueZone], except instead, it depletes the imbue on that item. 

{: .tip}
This GameObject requires a collider with "Trigger" enabled to work. This will be the region the zone takes effect in. 


![WaterZone][WaterZone]

## Components

| Field                       | Description
| ---                         | ---
| Depletion Rate              | Defines the rate of which the imbue will deplete.
| Platform                    | Select platform to Exclude/Only include on. Choices are Windows (PCVR) and Android (Quest 2).
| Allow Strip                 | Toggle to Strip or Not, without removing script.

[ImbueZone]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/ImbueZone.md %}
[WaterZone]: {{ site.baseurl }}/assets/components/WaterZone/WaterZone.PNG