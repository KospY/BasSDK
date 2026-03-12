---
parent: Creatures
grand_parent: ThunderRoad
---

# Ragdoll Hand Poser

The Ragdoll Hand Poser component controls the hand poses of the linked [RagdollHand]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/RagdollHand.md %}) using [HandlePoses]({{ site.baseurl }}{% link Components/ThunderRoad/Items/HandlePose.md %})

![Inspector]

| Field | Description |
| :--- | :--- |
| ragdollHand | A reference to a hand this component will pose |
| defaultHandPoseId | The default pose for this hand |
| targetWeight | The target weight, this blends between `default` and `target` |
| targetHandPoseId | The target pose for this hand |
| globalRatio | Is the weight ratio global? |
| thumbCloseWeight | The closedness of this hand's thumb |
| indexCloseWeight | The closedness of this hand's index finger |
| middleCloseWeight | The closedness of this hand's middle finger |
| ringCloseWeight | The closedness of this hand's ring finger |
| littleCloseWeight | The closedness of this hand's pinky finger |
| allowThumbTracking | Whether to enable thumb tracking |
| allowIndexTracking | Whether to enable index finger tracking |
| allowMiddleTracking | Whether to enable middle finger tracking |
| allowRingTracking | Whether to enable ring finger tracking |
| allowLittleTracking | Whether to enable pinky finger tracking |
| mirrorParams | A dropdown of parameters to mirror on specific axis, used for making hand poses |
| saveHandPoseId | The ID of the pose that has been saved |
| poseComplete | Whether the current pose transition is complete |

[Inspector]: {{ site.baseurl }}/assets/components/RagdollHandPoser/RagdollHandPoser.png