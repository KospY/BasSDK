---
parent: Items
grand_parent: ThunderRoad
---
# Handle

#### This component has an [Event Linker][EventLinker].

{: .note }

This component inherits from [Interactable][Interactable]

{: .note }
> Not to be confused with HandleRagdoll, though most of its components are alike.

The Handle is a dependacy of an [Item][Item], and is used to be able to grab an item with correct handling. When created, it creates [HandlePose][Handlepose], which are used to adjust hand poses when handling the item.

## Componentss

| Field                       | Description
| ---                         | ---
| Interactable ID             | ID of the Interactable JSON. Automatically assigned via json if not placed in scene (e.g. spawned only via item spawner).
| Allowed Hand Slide          | Choice between `Both`, `Left` and `Right`. Allows handle to only be able to be grabbed by specified hands.
| Axis Length                 | Axis Length changes the length of the handle, allowing you to slide/grab it further up the handle. When >0, a button appears which allows point-to-point editing of the length.
| Touch Radius                | Adjusts the `radius` to grab the handle.
| Artificial Distance         | Creates a distance which shows UI that can be grabbed, however is not in touch radius, and will not grab `[Needs Confirmation]`.
| Touch Center                | Allows you to offset the touch center from the middle of the handle. If set to `zero`, cannot be held with two hands.
| Default Grab Axis Ratio     | Allows you to change the position on the `axis length` which is grabbed by default (e.g. when grabbed via telekinesis).
| Ik Anchor Offset            | Offsets the IkAnchor transform. The X value is inverted when grabbed by the left hand.
| Orientation Default         | Depicts the default [HandlePose][HandlePose] used when grabbing the weapon.
| Release Handle              | When assigned with another handle, will ungrab this handle when the Release handle is grabbed.
| Silent Grab                 | When ticked, no sound plays when the handle is grabbed.
| Force Auto Drop when Grounded | When ticked, ungrabs the handle when the player is grounded (touching the ground).
| Reach                       |   Lets AI know how far weapon is away from the player. `Should extend from handle to furthest part of the weapon (aka the tip of the weapon, or bottom of handle).` A button exists which calculates the range depending on the colliders of the weapon.
| Hand Overlap Colliders      | Disables specified colliders when the handle is grabbed.
| Custom Rigidbody            | Allows you to add a custom `rigidbody` to the handle (Do not refernece Item, you can leave this blank).
| Slide to Up Handle          | Allows you to switch to another handle when reaching the `top` of the handle length.
| Slide to Bottom Handle      | Allows you to switch to another handle when reaching the `bottom` of the handle length.
| Slide to Handle Offset      | Offset where the "bottom" and "top" is indicated in the handle. Can switch handle when reaching 0.2 meters away from the bottom, for example.
| Slide Behaviour             | Allows you to enable/disable handle sliding.
| Move to Handle              | When you slide, and axis length is `zero`, will snap to this handle instead.
| Move to Handle Axis Pos     | Axis position for the `Move to Handle` handle.

{: .tip}
 > The "Update to New Orientations" button upgrades the obsolete "Allowed Orientations" list. Using this button will automatically > spawn a [HandlePose][HandlePose].
 
> [HandlePose]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/HandlePose.md %}


## Picture

![HandleLength0][HandleLength0]
This Depicts the gizmo of the handle. The Outer Ring gizmo is the reach, which goes from center to tip of the blade. The Inner White gizmo depicts the Touch radius, and the yellow circle depicts the Touch center.

![HandleWithLength][HandleWithLength]
This depicts the handle with a length. The yellow gizmo depicts where the hand would automatically grab when grabbed via telekinesis. The capsule gizmo depicts the length of the handle.




[HandleWithLength]: {{ site.baseurl }}/assets/components/Handle/HandleWithLength.PNG
[HandleLength0]: {{ site.baseurl }}/assets/components/Handle/HandleLength0.PNG
[Item]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Item.md %}
[EventLinker]: {{ site.baseurl }}{% link Components/ThunderRoad/Event-Linkers/HandleEventLinker.md %}
[HandlePose]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/HandlePose.md %}
[Holder]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Holder.md %}
[Interactable]: {{ site.baseurl }}{% link Components/ThunderRoad/Misc/Interactable.md %}