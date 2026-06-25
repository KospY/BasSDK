---
parent: Creatures
grand_parent: ThunderRoad
---

# Wrist Relaxer

The Wrist Relaxer component is used to visually relax and tighten an arm's wrist based on the hand's rotation, creating a neat and realistic forearm.

![Inspector]

| Field | Description |
| :--- | :--- |
| armTwistBone | A reference to the upper arm bone that twists, controlling the mesh inbetween the hand and elbow |
| upperArmBone | A reference to the upper arm bone |
| lowerArmBone | A reference to the lower arm bone |
| handBone | A reference to the hand bone |
| weight | The weight of relaxing the twist |
| weightArm | The weight of relaxing the arm of this Transform (UMA) |
| parentChildCrossfade | If 0.5, will be twisted half way from parent to child. If 1, the twist angle will be locked to the child and will rotate with along with it. |
| twistAngleOffset | Rotation offset around the twist axis. |

[Inspector]: {{ site.baseurl }}/assets/components/WristRelaxer/WristRelaxer.png