---
parent: Items
grand_parent: ThunderRoad
---
# Handle

##### This component has an [Event Linker][EventLinker].

{: .note }
This component inherits from [Interactable][Interactable]

{: .note }
> Not to be confused with HandleRagdoll, though most of its components are alike.

The Handle is a dependacy of an [Item][Item], and is used to be able to grab an item with correct handling. When created, it creates [HandlePose][Handlepose], which are used to adjust hand poses when handling the item.

![Component]

# Grabbing

| Field | Description |
| :--- | :--- |
| defaultGrabAxisRatio | Define where the handle is automatically grabbed along the axis length |
| ikAnchorOffset |  |
| silentGrab | When ticked, no sound will play when the handle is grabbed |
| forceAutoDropWhenGrounded | When ticked, the player will ungrab the handle when grounded (touching the ground) |
| ignoreClimbingForceOverride | The default handpose to be grabbed (Right Hand) |
| spellOrbTarget | If the player can cast spells while holding this handle, this transform defines where the orb will appear. |
| reach | Lets AI know how far the item is away from the player. You can use the button to calculate this automatically, so long as a ColliderGroup is set up with sufficient colliders. |
| handOverlapColliders | (Optional)Disables listed colliders once the handle is grabbed. |
| customRigidBody | (Optional) Allows you to add a custom rigidbody to the handle. (Do not reference item!) |
| twoHandedRequireSamePhysicbody | If set to true, requires that both handlers be grabbing onto the same physicbody to count as two handed grip |
| slideToUpHandle | (Optional) When player hand reaches the top of the handle via slide, it will switch to listed handle once the top is reached. |
| slideToBottomHandle | (Optional) When player hand reaches the bottom of the handle via slide, it will switch to listed handle once the bottom is reached. |
| slideToHandleOffset | (Optional) Offset of the bottom and top slide up handles. Can switch handle when reaching 0.2 meters away from the top/bottom, for example. |
| slideBehavior | Allows you to enable/disable handle sliding (Only works on handles with a length greater than 0!)<details>- CanSlide<br>- KeepSlide<br>- DisallowSlide<br></details> |
| moveToHandle | (Optional) When you slide up the handle, and the axis length is 0, sliding will instead snap to the referenced handle. |
| moveToHandleAxisPos | Axis Position for the "Move To Handle" handle |
| updatePosesAutomatically | When this box is checked, hand poses will update whenever the target weight changes or whenever the pose data changes. |

# Horiz

| Field | Description |
| :--- | :--- |
| orientationDefaultLeft | The default handpose to be grabbed (Left Hand) |
| orientationDefaultRight | The default handpose to be grabbed (Right Hand) |

# Orientations

| Field | Description |
| :--- | :--- |
| allowedOrientations | Legacy method for setting hand poses. This is still functional but is unnecessary to fill.<br><br>You can still use this list to set hand poses: once you're finished, use the "Try Make Default Hand Poses" button above to generate proper hand pose objects. |

# Other Handles

| Field | Description |
| :--- | :--- |
| releaseHandle | When linked handle is grabbed, ungrip this handle. |
| activateHandle | Handle will only activate when the linked handle is grabbed. |
| AIGrabHandle | NPCs will try to grab this handle if their brain logic tells them to. |

# Data

| Field | Description |
| :--- | :--- |
| interactableId \[Dropdown\] | (Only needed for non-json handles)<br>Insert Interactable ID here. |
| highlighterTransform | The position a highlighter will appear |

# Touch

| Field | Description |
| :--- | :--- |
| allowedHandSide | What hand is allowed to grab the handle.<details>- Both<br>- Right<br>- Left<br></details> |
| axisLength | The length of which the player can grab along.<br>If >0, a button will appear and allow you to adjust the length along its points |
| touchRadius | The radius of which the player can grab the handle |
| artificialDistance | When the player's hand is within the range of multiple interactables, the closest one is prioritized. <br> <br>Artifical distance is a fake distance added to the player's hand while checking which interactable is the nearest.<br>Setting this to a high value gives it a low priority when working out which interactable to use, while a low (or negative) value will give it a high priority compared to other interactables. <br> <br> Generally, you can leave this value at 0. |
| touchCenter | Determines the center of the touchRadius |

{: .tip}
 > The "Update to New Orientations" button upgrades the obsolete "Allowed Orientations" list. Using this button will automatically > spawn a [HandlePose][HandlePose].
 
> [HandlePose]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/HandlePose.md %}

![HandleLength0][HandleLength0]
<br>This Depicts the gizmo of the handle. The Outer Ring gizmo is the reach, which goes from center to tip of the blade. The Inner White gizmo depicts the Touch radius, and the yellow circle depicts the Touch center.

![HandleWithLength][HandleWithLength]
<br>This depicts the handle with a length. The yellow gizmo depicts where the hand would automatically grab when grabbed via telekinesis. The capsule gizmo depicts the length of the handle.



[Component]: {{ site.baseurl }}/assets/components/Handle/Handle.png
[HandleWithLength]: {{ site.baseurl }}/assets/components/Handle/HandleWithLength.PNG
[HandleLength0]: {{ site.baseurl }}/assets/components/Handle/HandleLength0.PNG
[Item]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Item.md %}
[EventLinker]: {{ site.baseurl }}{% link Components/ThunderRoad/Event-Linkers/HandleEventLinker.md %}
[HandlePose]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/HandlePose.md %}
[Holder]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Holder.md %}
[Interactable]: {{ site.baseurl }}{% link Components/ThunderRoad/Misc/Interactable.md %}