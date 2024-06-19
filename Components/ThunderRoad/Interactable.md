---
parent: ThunderRoad
---
# Interactable

```danger
This component is not intended to be used directly. Instead, use one of the components that inherit from this one.
```
#### Components That Inherit From Interactable:
- [Handle][Handle]
  - [Handle Ragdoll][HandleRagdoll]
  - [Rope][Rope]
- [Holder][Holder]
- [Wearable][Wearable]


## Overview
This component handles detecting interactions with the player's hand. When a hand is brought within it's radius, a tooltip will appear.

> *Example of the tooltip that appears*  
![tooltip][Tooltip]

## Component Fields

| Field               | Description
| ---                 | ---
| Interactable ID     | The catalog ID that this interactable will use to define it's behaviour.
| Allowed Hand Side   | Which hands the interactable will acknowledge. 
| Axis Length         | Extends the touch radius along the y-axis (green arrow) by the length provided.
| Touch Radius        | How far a hand must be before the player can interact. 
| Artificial Distance | See the section [below](#-artifical-distance).
| Touch Center        | Determines the center of the touch radius. 

## Notes

### • Artifical Distance

When the player's hand is within the range of multiple interactables, the closest one is prioritized.  

Artifical distance is a fake distance added to the player's hand while checking which interactable is the nearest.  
Setting this to a high value gives it a **low priority** when working out which interactable to use, while a low (or negative) value will give it a **high priority** compared to other interactables.

Generally, you can leave this value at `0`.


[Tooltip]:        {{ site.baseurl }}/assets/components/Interactable/tooltip.jpg

[Handle]:         {{ site.baseurl }}{% link Components/ThunderRoad/Handle.md %}
[HandleRagdoll]:  {{ site.baseurl }}{% link Components/ThunderRoad/HandleRagdoll.md %}
[Rope]:           {{ site.baseurl }}{% link Components/ThunderRoad/Rope.md %}
[Holder]:         {{ site.baseurl }}{% link Components/ThunderRoad/Holder.md %}
[Wearable]:       {{ site.baseurl }}{% link Components/ThunderRoad/Wearable.md %}