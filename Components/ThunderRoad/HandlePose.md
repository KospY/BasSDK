# Handle Pose

The Handle Pose is dependant on the [Handle][Handle], for which this script shows what the handpose looks like in game when that handle is grabbed. This script also manages handle orientations, so with all orientations from 0, 90, 180 and 270, and up-side down handles, some handles can have 8 to 16 handle pose scripts. 

The gizmo represents what pose the player hand will make when it grabs this handle, and can be adjusted for each handle pose. 

![HandlePose][HandlePose]

A normal weapon, with all normal orientations, looks something like this with all handposes.

![HandPoseExample][HandPoseExample]

## Components

| Field                       | Description
|---                          |---
| Handle                      | Depicts the handle that the handpose applies to. Is automatically assigned if the handpose was created by that handle.
| Side                        | Picks which hand this handpose applies to.
| Hand Pose Weighting         | `See Below`
| [Editor Only Creature]      | Allows you to select a creature to test this handpose on, if handposes are set up correctly for that creature.

## Hand Pose Weighting

The hand pose system allows the user to select from a variety of different handposes contained inside the catelog. When the `Catelog Picker` button is enabled, the drop down will be available, allowing the user to click between the `Default Hand Pose ID` and the `Target Hand Pose ID`. 

The `Target Weight` is a slider which can blend between the two handposes in the `Default Hand Pose ID` and the `Target Hand Pose ID`. This will allow a more accurate Hand pose for your item, without creating a new hand pose from scratch. 

![HandPoseWeight][HandPoseWeight]

![HandPoseList][HandPoseList]


[Handle]: {{ site.baseurl }}{% link Components/ThunderRoad/Handle.md %}
[HandlePose]: {{ site.baseurl }}/assets/components/Handle/Handposes/HandlePose.PNG
[HandPoseList]: {{ site.baseurl }}/assets/components/Handle/Handposes/HandPoseList.PNG
[HandPoseExample]: {{ site.baseurl }}/assets/components/Handle/Handposes/HandPoseExample.PNG
[HandPoseWeight]: {{ site.baseurl }}/assets/components/Handle/Handposes/HandPoseWeight.gif