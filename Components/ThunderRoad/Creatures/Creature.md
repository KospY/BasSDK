---
parent: Creatures
grand_parent: ThunderRoad
---
# Creature

{: .important}
This component should have a [LightVolumeReceiver]({{ site.baseurl }}{% link Components/ThunderRoad/Areas/LightProbeVolumeReceiver.md %}) on the same gameObject!

The Creature component is the core of any NPC or enemy you want to make and must be placed on the root of your custom enemy. 

| Field | Description |
| :--- | :--- |
| creatureId \[Dropdown\] | The Creature ID of the creature. |
| animator | The animator the creature will use for animations |
| lodGroup | The LOD Group for the creature meshes, if it has one. |
| container | The container the creature uses for its parts. |
| vfxRenderer | If the creature use a VFX renderer, put it here. |
| armorSFX | References the class for armor SFX |
| factionId \[Dropdown\] | Set the faction of this creature. Will generally be overridden by creature data. |

# Head

| Field | Description |
| :--- | :--- |
| centerEyes | The transform used for eye rotation |
| eyeCameraOffset | The offset used for the eye camera |
| autoEyeClipsActive | When enabled, the creature blinks |
| allEyes | Reference the eyes for blinking |
| eyeClips | Reference the eye animation clips |
| meshesToHideForFPV | Reference the meshes to hide when in first person. |
| jaw | Reference the jaw bone for creature speaking |
| jawMaxRotation | Max rotation of the jaw when it speaks. |

# Movement

| stepEnabled | Whether this creature steps while being moved |
| stepThreshold | The distance this creature needs to move to step |
| headMinAngle | The minimum angle this creature's head can turn |
| headMaxAngle | The maximum angle this creature's head can turn |
| turnSpeed | The speed this creature can turn |

# Move Animation

| Field | Description |
| :--- | :--- |
| animationDampTime | Defines how quickly the creature blends into and out of locomotion animations. |
| verticalDampTime | Defines how quickly the creature blends into and out of locomotion animations while moving in the air. |
| stationaryVelocityThreshold | Defines the speed threshold for the creature to be considered moving. |
| turnAnimSpeed | Define the speed for turn animations. |

# Falling

| Field | Description |
| :--- | :--- |
| fallAliveAnimationHeight | The height off the ground the creature needs to be before they play the fall animation |
| fallAliveDestabilizeHeight | The height the creautre needs to fall before their ragdoll distabilizes |
| groundStabilizationMaxVelocity | The maximum velocity for the creatures body before it can stand up |
| groundStabilizationMinDuration | The minimum duration a creature is on the ground before getting up. |
| swimFallAnimationRatio | How submerged the creature needs to be before they can start the swimming animations. |
| fallState | <details>- None<br>- Falling<br>- NearGround<br>- Stabilizing<br>- StabilizedOnGround<br></details> |