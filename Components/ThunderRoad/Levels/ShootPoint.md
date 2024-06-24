---
parent: Levels
grand_parent: ThunderRoad
---
# Shoot Point

Shoot Point is a component that AI will use to move to and shoot from. This point is where archers (usually) will move to in order to shoot at you. 

![ShootPoint][ShootPoint]

Inside the script, there is a slider which allows you to adjust the angle that the archer can shoot from. If you face the opposide direction, or alternatively are not in the angle of the ShootPoint, then the archers will not go to this position. This slider will allow you to change this angle.

![ShootPointGizmo][ShootPointGizmo]

Similar to [WavePoint][WayPoint], this component detects if the NavMesh is placed on to it. The Gizmo will appear gray if it detects the navmesh, but will appear Red if it does not. If there is no NavMesh underneath this gizmo, but has a NavMesh nearby, there will be a gizmo pointing to it, where the NPC will go to instead to shoot from.

![ShootPointNavmesh][ShootPointNavmesh]

[ShootPoint]: {{ site.baseurl }}/assets/components/ShootPoint/ShootPoint.PNG
[ShootPointGizmo]: {{ site.baseurl }}/assets/components/ShootPoint/ShootPointGizmo.gif
[ShootPointNavmesh]: {{ site.baseurl }}/assets/components/ShootPoint/ShootPointNavmesh.png
[WayPoint]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/WayPoint.md %}