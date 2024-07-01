---
parent: Items
grand_parent: ThunderRoad
---

# Item Magnet

The Item Magnet script moves an item to its center point. The item can be released/pulled out of this area, and you can filter what type of items can be dragged to this area.

{: .important}
This script requires a collider before the script is added. This zone depicts the "magnetic" area.

![Component][Component]

## Fields

| Field                             | Description
| ---                               | ---
| Tag Filter                        | The filter of which is either "Any Except" or "None Except" when referring to the slots.
| Slots                             | Define the item "slots" which can be affected by the magnet. See Below.
| Kinematic Lock                    | When ticked, the item is locked in to a kinematic state and cannot be moved/removed.
| Release on Grab or TK Only        | When enabled, the magnetized item can only be removed with grab or telekinesis.
| Enabled Collission With Joint Rigidbody | When ticked, it enables the collission with the joint rigidbody.
| Catched Item Ignore Gravity Push  | When enabled, the magnetized item cannot be removed from the magnet via a Gravity push.
| Auto Ungrab                       | When the item is magnetized, the item ungrabs from your hand.
| Magnet Reactivate Duration on Release | When the magnetized item is released, it's duration is reactivated.
| Gravity Multiplier                | Defines the gravity multiplier of the item when it is held by the magnet.
| Mass Multiplier                   | Multiplies the mass of the item when magnetized by the magnet.
| Sleep Threshold Ratio             | Defines the Sleep Threshold when held by the magnet (when the item is classified as "asleep")
| Max Count                         | Maximum amount of items that can be held by the magnet at one time
| Progressive Force Radius          | The radius of the force movement between the item and the magnet center
| Stabilized Max Distance           | The max distance between the center of the magnet before the item is stabilized.
| Stabilised Max Angle              | The maximum angle of the item when it is stabilized
| Stabilized Max Up Angle           | The maximum "up angle" of the item when it is stabilized
| Stabilized Max Velocity           | The maximum velocity of the item before it is stabilized.
| Position Spring                   | The spring of the item when it is stabilized in the magnetized zone.
| Position Damper                   | The damper of the item when stabilized in the magnetized zone.
| Position Max Force                | The maximum force of the item when stabilized in the magnetized zone.
| Rotation Spring                   | The spring rotation of the item when stabilized in the magnetized zone
| Rotation Damper                   | The damper of the item when stabilized in the magnetized zone.
| Rotation Max Force                | The force of the rotation when stabilized in the magnetized zone.

## Events
[Unity Event Documentation](https://docs.unity3d.com/Manual/UnityEvents.html){: .btn .btn-purple }

| Event                             | Description
| ---                               | ---
| On Item Catched                   | Event plays when the item is caught by the magnetized zone.
| On item Released                  | Event plays when the item is released from the magnetized zone.
| On Item Stabilization             | Event plays when the item is Stabilized. 

## Slots

For this item magnet to pull the correct items, you must reference the slots that are/are not magnetized. Below are listed slots that are vanilla to Blade and Sorcery:

    - Small (Small items, daggers, etc)
    - Medium (Medium sized items, swords, axes)
    - Large (Large items, greatswords, spears)
    - Potion
    - Head (Head slot, Baron's Hat)
    - Shield 
    - ShieldSmall (small shield, buckler)
    - Bow
    - Quiver
    - Arrow
    - Throwables (throwing items, throwing daggers, rocks)
    - Bolt (Unused)
    - Cork (Potion Cork)
    - Torch
    - SkillTreeReceptacle (Skill Crystals)
    - InventoryBag
    - Gigantic (Giant Statue sword)

[Component]: {{ site.baseurl }}/assets/components/ItemMagnet/Component.png
