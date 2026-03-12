---
parent: Creatures
grandParent: ThunderRoad
---

# Feet Climber

{: .important}
This component should be on its own gameObject as a child of your [Creature]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/Creature.md %}) component as it is rotated based on `sweepAngle`!

The Feet Climber component is used to position a [Creature's]({{ site.baseurl }}{% link Components/ThunderRoad/Creatures/Creature.md %}) feet on terrain or ladder rungs while they climb.

![Inspector]

| Field | Description |
| :--- | :--- |
| footSpeed | The speed at which feet move to their new position |
| sweepAngle | The downward/forward tilt of the detection "fan" used to find surfaces |
| sweepMinDelay | The minimum time between sweeps |
| sweepMaxVerticalAngle | Defines the vertical field of view in which the component looks for potential footholds |
| sweepMaxHorizontalAngle | Defines the horizontal field of view in which the component looks for potential footholds |
| sphereCastRadius | The thickness of the sweep |
| moveOutWeight | The IK weight used when a foot is transitioning away from a surface |
| legLenghtMultiplier | Multiplies the creature's actual leg length to determine how far it can reach for a foothold |
| minFootSpacing | The minimum distance required between the left and right foot to prevent them from clipping into the same spot |
| legToHeadMaxAngle | A threshold used to determine if a leg pose is valid based on the angle between the leg and the head |
| footMaxAngle | The maximum allowable angle of a surface; prevents the feet from trying to "stick" to completely vertical walls or ceilings |
| showDebug | If true, debug lines will be drawn |

[Inspector]: {{ site.baseurl }}/assets/components/FeetClimber/FeetClimber.png