---
parent: Creatures
grand_parent: ThunderRoad
---
# Ragdoll Hand

{: .information}
This component is an extension of [RagdollPart]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/RagdollPart.md %}), please be sure to read the documentation available here to gain a full understanding of how to use this.

###### This component is derived from [RagdollPart]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/RagdollPart.md %})

The RagdollHand component is a specialised [RagdollPart]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/RagdollPart.md %}) used to create a hand that can grab items, pose, swim, interact with the world and in some cases, shake hands.

![Inspector]

| Field | Description |
| :--- | :--- |
| side | The side this hand is <details>- Right<br>- Left<br></details> |
| lowerArmPart | The lower arm [RagdollPart]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/RagdollPart.md %}) |
| upperArmPart | The upper arm [RagdollPart]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/RagdollPart.md %}) |
| meshFixedScale | Whether this mesh is at a fixed scale or not |
| meshGlobalScale | The original global scale of the mesh |
| axisThumb | The axis (direction) of the thumb on this hand |
| axisPalm | The axis (direction) of the palm on this hand |
| touchCollider | The collider used to know when the hand is touching something (.ie., [Interactable]({{ site.baseurl }}{% link Components/ThunderRoad/Misc/Interactable.md %})) |
| palmCollider | The collider used to know what part of the hand the palm covers |
| simplifiedCollider | The collider enabled when the finger colliders are disabled |
| wristStats | A reference to a [WristStats]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/WristStats.md %}) component which should be a child of the RagdollHand |
| poser | A reference to a [RagdollHandPoser]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/RagdollHandPoser.md %}) component which should be attached to the RagdollHand itself |
| fingers | A list of fingers on this hand. Use the `Setup Fingers` button to copy the Finger references to this collection |

# Fingers
The following references on RagdollHand are identical and should be set up with unique references to the proximal, intermediate and distral bones of the finger.
- fingerThumb 
- fingerIndex 
- fingerMiddle 
- fingerRing 
- fingerLittle 

![Finger]

[Inspector]: {{ site.baseurl }}/assets/components/RagdollPart/RagdollHand.png
[Finger]: {{ site.baseurl }}/assets/components/RagdollPart/Finger.png