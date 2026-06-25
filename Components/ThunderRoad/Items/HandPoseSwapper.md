---
parent: Items
grand_parent: ThunderRoad
---
# Hand Pose Swapper

{: .important}
This component must be placed on a gameObject with a [HandlePose]({{ site.baseurl }}{% link Components/ThunderRoad/Items/HandlePose.md %})

The Hand Pose Swapper component can change the default and target pose of a [HandlePose]({{ site.baseurl }}{% link Components/ThunderRoad/Items/HandlePose.md %}).

![Inspector]

| Field | Description |
| :--- | :--- |
| alternateHandPoseIDs | A list of IDs to swap to when SetTargetPose is called |

## Changing Poses

As long as your Handle Pose Swapper has a single valid alternate pose you are able to call one of the following methods, either in an event linker or by code. It will then automatically update the attached [HandlePose]({{ site.baseurl }}{% link Components/ThunderRoad/Items/HandlePose.md %}).
![Poses]

[Inspector]: {{ site.baseurl }}/assets/components/HandPoseSwapper/HandPoseSwapper.png
[Poses]: {{ site.baseurl }}/assets/components/HandPoseSwapper/Poses.png