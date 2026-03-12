---
parent: Creatures
grand_parent: ThunderRoad
---
# Wearable

The Wearable component is used to create places to equip armour on a creature's body. This component is generally parented to a [RagdollPart]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/RagdollPart.md %}).

![Inspector]

| Field | Description |
| :--- | :--- |
| wardrobeChannel \[Dropdown\] | The channel this wardrobe supports |
| wardrobeLayers \[Dropdown\] | A list of layers for this channel |

# Data

| Field | Description |
| :--- | :--- |
| interactableId \[Dropdown\] | (Only needed for non-json handles)<br>Insert Interactable ID here. |
| highlighterTransform | The position the highlighter will show at |

# Touch

| Field | Description |
| :--- | :--- |
| allowedHandSide | What hand is allowed to grab the handle.<details>- Both<br>- Right<br>- Left<br></details> |
| axisLength | The length of which the player can grab along.<br>If >0, a button will appear and allow you to adjust the length along its points |
| touchRadius | The radius of which the player can grab the handle |
| artificialDistance | When the player's hand is within the range of multiple interactables, the closest one is prioritized. <br> <br>Artifical distance is a fake distance added to the player's hand while checking which interactable is the nearest.<br>Setting this to a high value gives it a low priority when working out which interactable to use, while a low (or negative) value will give it a high priority compared to other interactables. <br> <br> Generally, you can leave this value at 0. |
| touchCenter | Determines the center of the touchRadius |

[Inspector]: {{ site.baseurl }}/assets/components/Wearable/Wearable.png