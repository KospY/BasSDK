---
parent: Creatures
grand_parent: ThunderRoad
---

# Ragdoll

The Ragdoll component is used to give [Creatures]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/Creature.md %}) physics-based rigs and the ability to be sliced.

![Inspector]

| Field | Description |
| :--- | :--- |
| meshRig | The root of your rig |
| meshRootBone | The root bone of rig |
| hipsAttached | If true, this pins the character's hips to a specific point in the world (like being sat in a chair or hanging from a ledge) while letting the limbs move freely. |
| allowSelfDamage | If true, parts from this ragdoll will be able to interact and damage other parts |
| grippable | If true, this ragdoll can be grabbed |
| springPositionForce | The strength of the spring used to pull ragdoll parts toward their animated positions |
| damperPositionForce | The amount of resistance applied to position springs to prevent them from bouncing |
| maxPositionForce | The maximum limit of force that can be applied to reach a target position |
| springRotationForce | The strength of the torque used to rotate ragdoll parts toward their animated orientations |
| damperRotationForce | The amount of resistance applied to rotation springs to prevent jittering |
| maxRotationForce | The maximum limit of torque that can be applied to reach a target rotation |
| destabilizedSpringRotationMultiplier | Multiplier for rotation strength when the creature is destabilized |
| destabilizedDamperRotationMultiplier | Multiplier for rotation damping when the creature is destabilized |
| destabilizedGroundSpringRotationMultiplier | Multiplier for rotation strength specifically when the destabilized creature is in contact with the ground |
| hipsAttachedSpringPositionMultiplier | Multiplier for position spring strength when `hipsAttached` is active |
| hipsAttachedDamperPositionMultiplier | Multiplier for position damping when `hipsAttached` is active |
| hipsAttachedSpringRotationMultiplier | Multiplier for rotation spring strength when `hipsAttached` is active |
| hipsAttachedDamperRotationMultiplier | Multiplier for rotation damping when `hipsAttached` is active |
| playerArmPositionSpring | The spring strength applied to the ragdoll's arms when they are controlled by a player |
| playerArmPositionDamper | The damping applied to the ragdoll's arms when controlled by a player |
| playerArmMaxPositionForce | The maximum position force for player-controlled arms |
| playerArmRotationSpring | The rotation spring strength for player-controlled arms |
| playerArmRotationDamper | The rotation damping for player-controlled arms |
| playerArmMaxRotationForce | The maximum rotation torque for player-controlled arms |

# State

| Field | Description |
| :--- | :--- |
| state | The default state this ragdoll will be in on load <details>- Inert<br>- Destabilized<br>- Frozen<br>- Standing<br>- Kinematic<br>- NoPhysic<br>- Disabled<br></details> |

# Physic Toggle

| Field | Description |
| :--- | :--- |
| physicToggle | Whether physics should be toggled for this ragdoll |
| physicTogglePlayerRadius | The radius at which physics are enabled & disabled relative to where the player is |
| physicToggleRagdollRadius | The radius at which physics are enabled & disabled relative to where other ragdolls are |
| physicEnabledDuration | The duration ragdolls are enabled for when toggled |

# [Parts]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/RagdollPart.md %})

| Field | Description |
| :--- | :--- |
| headPart | A reference to the head of this creature |
| leftUpperArmPart | A reference to the left upper arm of this creature |
| rightUpperArmPart | A reference to the right upper arm of this creature |
| targetPart | A reference to the torso of this creature |
| rootPart | A reference to the root [RagdollPart]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/RagdollPart.md %}) of this creature |
| parts | A list of every [RagdollPart]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/RagdollPart.md %}) on this creature |
| safeSliceParts | A list of every safely sliceable part, if one of your [RagdollParts]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/RagdollPart.md %}) is allowed to be dismembered, put it here |

# Stand Up

| Field | Description |
| :--- | :--- |
| standUpCurve | The animation curve evaluated as this creature stands up |
| standUpFromGrabDuration | The duration it takes for this creature to stand up after being grabbed |
| preStandUpDuration | The delay before the stand-up animation begins |
| preStandUpRatio | The percentage of the stand-up process required before this creature stands up |

# Collision

| Field | Description |
| :--- | :--- |
| collisionEffectMinDelay | The minimum delay between collision effects |
| collisionMinVelocity | The minimum velocity required to spawn an effect |

[Inspector]: {{ site.baseurl }}/assets/components/Ragdoll/Ragdoll.png