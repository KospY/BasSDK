---
parent: Items
grand_parent: ThunderRoad
---
# Handle Pose

The Handle Pose is dependent on the [Handle][Handle] component, for which this script shows what the hand pose looks like in game when that handle is grabbed. This script also manages handle orientations, so with all orientations from 0, 90, 180 and 270, and up-side down handles, some handles can have 8 to 16 handle pose scripts. 

The gizmo represents what pose the player hand will make when it grabs this handle, and can be adjusted for each handle pose. 

![HandlePose][HandlePose]

A normal weapon, with all normal orientations, looks something like this with all handposes.

![HandPoseExample][HandPoseExample]

## Components

| Field | Description |
| :--- | :--- |
| handle | References the handle this handpose is attached to. |
| side | Depicts which hand this handpose directs to.<details>- Right<br>- Left<br></details> |
| defaultHandPoseId | ID of the handpose that is set to default if the target weight is zero. |
| spellOrbTarget | A per-HandlePose override for the Handle's SpellOrbTarget |
| targetWeight | Blends the "Default" handpose and the "Target" handpose, allowing you to create more unique and fitting handposes without needing to create new ones. |
| targetHandPoseId | ID of the handpose that is used to blend against the default handpose. Handpose that is used if the target weight is one. |
| creature | Allows you to select a creature to test the handpose on. |
| creatureName | Uses the ID of the creature to ensure that the hand bones are correct. |

## Hand Pose Weighting

The hand pose system allows the user to select from a variety of different handposes contained inside the catelog. When the `Catelog Picker` button is enabled, the drop down will be available, allowing the user to click between the `Default Hand Pose ID` and the `Target Hand Pose ID`. 

The `Target Weight` is a slider which can blend between the two handposes in the `Default Hand Pose ID` and the `Target Hand Pose ID`. This will allow a more accurate Hand pose for your item, without creating a new hand pose from scratch. 

![HandPoseWeight][HandPoseWeight]

![HandPoseList][HandPoseList]


[Handle]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Handle.md %}
[HandlePose]: {{ site.baseurl }}/assets/components/Handle/Handposes/HandlePose.PNG
[HandPoseList]: {{ site.baseurl }}/assets/components/Handle/Handposes/HandPoseList.PNG
[HandPoseExample]: {{ site.baseurl }}/assets/components/Handle/Handposes/HandPoseExample.PNG
[HandPoseWeight]: {{ site.baseurl }}/assets/components/Handle/Handposes/HandPoseWeight.gif