---
parent: Creatures
grand_parent: ThunderRoad
---
# Footstep

{: .important}
This component should be on its own gameObject as a child of your [Creature]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/Creature.md %}) component!<br><br>Alternately this component should be a child of one of your foot [RagdollParts]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/RagdollPart.md %}) with a trigger Collider attached.

The Footstep component is used to create sound and visual effects when either the left or right foot comes into contact with terrain.

![Inspector]

| Field | Description |
| :--- | :--- |
| materialId | Id of the material to retrieve the sounds from (in catalog). |
| usePerFootRaycastCheck | Enables on each step a ray sampling above and under the foot. Plays only the effects of the highest collider. (Needs to be true for water planes) |
| minMaxStandingVelocity | Velocity thresholds, used in an inverse lerp when walking and running. |
| minMaxFallingVelocity | Velocity thresholds, used in an inverse lerp when falling. |
| fallingIntensityFactor | Factor used to tweak the intensity of the footsteps when falling. |
| crouchingIntensityFactor | Factor used to tweak the intensity of the footsteps when crouching. |
| stepMinDelay | Cool-down delay in second between two steps. Different timers are used for each foot. |
| fallMinDelay | Cool-down delay in second between two falls. |
| footstepDetectionHeightThreshold | Height used to detect footsteps, it's added to the locomotion ground point (in meters). |
| footstepDetectionRunningHeightThreshold | Height used to detect footsteps while running, it's added to the locomotion ground point (in meters). |

[Inspector]: {{ site.baseurl }}/assets/components/Footstep/Footstep.png