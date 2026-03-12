---
parent: Creatures
grand_parent: ThunderRoad
---

# Wrist Stats

The Wrist Stats component is used to add a visual display to a point on a creature, most commonly used in combination with the [RagdollHand]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/RagdollHand.md %}) component, as the hand requires a reference to this component.

![Inspector]

| Field | Description |
| :--- | :--- |
| showDistance | The maximum distance at which the stats are shown. If the display is further they will be hidden |
| showAngle | The maximum angle the stats forward direction can be from your eyes before they are hidden |
| distanceFromEyes | The distance these stats will be from your eyes after moving through armour that may hide it |
| minComfortableDistance | The minimum distance away from its original position |
| healthEffectId \[Dropdown\] | The effect ID for the health display |
| focusEffectId \[Dropdown\] | The effect ID for the focus display |
| manaEffectId \[Dropdown\] | The effect ID for the mana display (obsolete) |

[Inspector]: {{ site.baseurl }}/assets/components/WristStats/WristStats.png