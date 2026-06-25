---
parent: Creatures
grandParent: ThunderRoad
---

# Locomotion

The Locomotion component controls how a [Creature]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/Creature.md %}) moves throughout a level, including jumping, walking, crouching, strafing, running and falling.

![Inspector]

# Ground movement

| Field | Description |
| :--- | :--- |
| allowMove | Toggles movement |
| allowTurn | Toggles rotation |
| allowJump | Toggles jumping |
| allowCrouch | Toggles crouching |
| moveForceMultiplierByAngleCurve | Adjusts force based on ground slope. This acts like a slower for steep hills or inclines |
| forwardSpeed | Walking speed (forward) |
| backwardSpeed | Walking speed (backward) |
| strafeSpeed | Walking speed (sideways) |
| runSpeedAdd | Bonus speed added when sprinting |
| crouchSpeed | Movement speed while crouched |
| globalMoveSpeedMultiplier | Global multiplier for all speeds |
| forwardAngle | Degree threshold to define "forward" movement. If the move direction fits within this angle the creature is considered "moving forward" |
| backwardAngle | Degree threshold to define "backward" movement. If the move direction fits within this angle the creature is considered "moving backward" |
| runDot | Dot product threshold for running. If the move direction is within this value you will be able to run. (0.6 == 53°) |
| runEnabled | Toggles sprinting |
| crouchHeightRatio | Animator height threshold for the creature to be considered crouching (0.0 - 1.0) |
| turnSpeed | Rotation rate of the creature |

# Jump / Fall

| Field | Description |
| :--- | :--- |
| horizontalAirSpeed | Horizontal control speed while falling |
| verticalAirSpeed | Manual vertical speed (flight/swimming) |
| waterSpeed | Movement speed while standing in water |
| airForceMode | Force type for aerial movement <details>- Force<br>- Impulse<br>- VelocityChange<br>- Acceleration<br></details> |
| jumpGroundForce | Upward force applied during a jump |
| jumpClimbVerticalMultiplier | Vertical boost when jumping while climbing |
| jumpClimbVerticalMaxVelocityRatio | Cap for velocity gained from climb-jumps |
| jumpClimbHorizontalMultiplier | Horizontal boost when jumping while climbing |
| jumpMaxDuration | Max time jump force is sustained |

# Turn

| Field | Description |
| :--- | :--- |
| turnSmoothDirection | Smoothed rotation delta |
| turnSnapDirection | Instant rotation value (30 degrees per snap turn) |
| turnSmoothSnapDirection | Smoothing for snap rotation |
| verticalForceMode | Force type for vertical movement <details>- Force<br>- Impulse<br>- VelocityChange<br>- Acceleration<br></details> |

# Colliders

| Field | Description |
| :--- | :--- |
| colliderRadius | Thickness of the capsule collider |
| colliderShrinkMinRadius | Minimum thickness when squeezing through gaps |
| colliderGrowDuration | Time to return to full size after shrinking |
| colliderHeight | Total height of the capsule collider |
| collideWithPlayer | Toggles collision against the player character, don't enable if this creature *is* the player |

# Ground detection

| Field | Description |
| :--- | :--- |
| groundDetectionDistance | Distance to check for ground below collider |
| colliderGroundMaterial | PhysicMaterial used while grounded |
| groundDrag | Rigidbody drag while grounded |
| colliderFlyMaterial | PhysicMaterial used while airborne |
| flyDrag | Rigidbody drag while airborne |
| customGravity | Multiplier for gravity |

[Inspector]: {{ site.baseurl }}/assets/components/Locomotion/Locomotion.png