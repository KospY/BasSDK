---
parent: Levels
grand_parent: ThunderRoad
---
# Way Point

The Waypoint system is used for creature movements out of combat. They will move from waypoint to waypoint or stay in that waypoint, depending on how it is set up. This component should be a child of a gameobject utilising the [CreatureSpawner][CreatureSpawner] script.

![Component][Component]

## Fields

| Field                             | Description
| ---                               | ---
| Turn to Direction                 | When ticked, the creature will turn to the blue arrow/Z axis when they get to the waypoint.
| Turn Speed Ratio                  | Indicates the speed of which the enemy will turn to that direction. Defaulted to 1.
| Wait Min Max Duration             | When an creature reaches this waypoint, they will wait X/Y time at this position.
| Animation                         |
| Play animation                    | Will play an animation when they reach this waypoint.
| Animation Id                      | Will play this animation ID.
| Animation Turn Min Angle          | The minimum angle of which the creature will turn when playing the animation.
| Animation Random Min Max Delay    | Specify the delay (X to Y) between reaching the waypoint and playing the animation.
| Action Subtree                    |
| Action Behaviour Tree ID          | Sets a Behaviour tree the creature tries to complete when arriving at this waypoint.
| Target                            | Sets an object the creature targets if the behaviour tree needs one.
| Required Tree Success Count       | How many times the creature needs to complete the behaviour tree to proceed
| Failures to Skip                  | How many times the behaviour tree can fail before the creature moves on.

## Setup

To correctly setup the waypoint system, it requires a [CreatureSpawner][CreatureSpawner] of which you will reference itself in the "Waypoints Root". Once done, a sphere gizmo will be placed on the waypoint.

{: .note}
For the gizmo to work properly, ensure that the waypoint is on a valid Navmesh.

![WayPointAlone][WayPointAlone]

Once complete, waypoints should be easy to set up. You should have a hierarchy of "Creature Spawner > Waypoint0 (etc)".

![Hierarchy][Hierarchy]

 You can copy the Waypoint0 and it will create WayPoint1, 2 (etc), and they will give you a guide of the patrol between the waypoints. With each waypoint, you can specify unique interactions and animations to play, including wait times.

![WayPointMultiple][WayPointMultiple]

[WayPointAlone]: {{ site.baseurl }}/assets/components/Waypoint/WayPointAlone.png
[WayPointMultiple]: {{ site.baseurl }}/assets/components/Waypoint/WayPointMultiple.png
[Component]: {{ site.baseurl }}/assets/components/Waypoint/Component.png
[Hierarchy]: {{ site.baseurl }}/assets/components/Waypoint/Hierarchy.png

[CreatureSpawner]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/CreatureSpawner.md %}