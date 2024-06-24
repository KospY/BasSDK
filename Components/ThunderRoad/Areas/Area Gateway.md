---
parent: Areas
grand_parent: ThunderRoad
---
# Area Gateway

## Summary

Area Gateway is a component used in the Area system to connect rooms together. It is recommended that these doors have no other renderers past it, as these renderers can show up in the next room.

Please keep note of this Area Gateway Transform, as it is essential in connecting rooms together. These gateways are referenced in the "Area" JSON, which specifies what type of rooms it accepts (like cave or interior), as well as correctly connecting up two gateways.

Note that this

## Component Properties

| Field | Description |
| --- | --- |
| Fade |  |
| Fade Max Distance | Depicts the maximum distance of which the Area Gateway will fade the fake door view. |
| Light Blend Max Distance Ratio | --- |
| Fade Room Min Distance Ratio | Depicts the maximum distance of which the Area Gateway will fade the fake door view (Slider of the "Fade Max Distance") |
| Fade Audio Min Distance Ratio | Depicts the maximum distance of which the Audio will fade in/out when getting near the Gateway. |

![AreaGateway.PNG](Area%20Gateway%205f8c690b01c9452a9e8d247b7f564baa/AreaGateway.png)