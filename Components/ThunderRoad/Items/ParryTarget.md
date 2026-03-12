---
parent: Items
grand_parent: ThunderRoad
---
# Parry Target

The Parry Target script is a script needed on an [Item][Item] for the AI to detect your weapon and be able to parry it. When created, it creates a gizmo line, which you are to make extend the entire length of the weapon.

![ParryTargetScript][ParryTargetScript]

| Field | Description |
| :--- | :--- |
| length | Depicts the length of the ParryTarget. With this, AI will know how long your weapon is, and be able to parry it.<br><br>Can be adjusted via button to adjust the edge-to-edge gizmo. |

There is a button which allows you to move each end to fit the weapon, to allow an easy adjustment to the length of the Parry Target from point to point. An item can have more than one ParryTarget.

![ParryTarget][ParryTarget]


[ParryTarget]: {{ site.baseurl }}/assets/components/ParryTarget/ParryTargetPreview.PNG
[ParryTargetScript]: {{ site.baseurl }}/assets/components/ParryTarget/ParryTargetScript.PNG
[Item]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Item.md %}