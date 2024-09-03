---
parent: Levels
grand_parent: Guides
---

# Level Creation Best Practices

This page contains helpful pointers on creating performant and well designed levels.

---


# General Design

#### What makes a good level feel great.

### Object Scale 

Unlike on other platforms, the incorrect scaling of objects is much more noticable in VR. A barrel that is just a little too big, or a doorway with too low a headway is going to immediately feel incorrect to a player, and can result in their immersion being broken. It is important to avoid this wherever possible.

If you are modelling a level yourself, it is recommended to try and match the objects in your scene with real-world proportions. Approaching the creation of your level this way early on will save you the pain of carefully fixing things down the line.

### Accessibility

Not all players are fond of some of the more stomach-churning aspects of VR. It is recommended that you provide alternative routes in your level design that do not require players to perform complex movement such as swimming or climbing.

### Interface Placements

Be careful to ensure gameplay-critical elements like the item book and wave spawner are placed in clear and intuitive positions, preferably close to where the player enters the level. For maps with a survival mode, aim to place the reward pillars in a central area where the player is most likely to be frequently engaged in combat.

### Intuitive Navigation

The visual design of your level has an important role in the way the player moves through it. Try to ensure navigable areas are indicated clearly. One way to implement this is by ensuring the play area is decently lit, while dead ends are darker and cluttered with props.  

---

# Optimization

#### Making sure your level runs effectively and performant.

### Mesh Complexity

The amount of triangles used in your levels contains can highly impact the performance of the game. When creating or using assets for the level, be careful not to add meshes with an unnecessarily high triangle count.

Try to aim for the following tri-count budgets when developing your level for Blade and Sorcery. Keep in mind, these budgets are for the combined total of every mesh present in your level.

| Platform| Triangle Budget |
| :---- | :--------: |
| PCVR   | 2.5 mil    |
| Nomad  | 750k       |

### LOD

For larger levels, you can make use of LOD (Level of Detail) components to avoid rendering unnecessary model details. Usually, our LODs are at least 50% in tri count from the upper LOD. Overuse of multiple LODs on one model can go over the amount of performance than having no LODs.

These work by switching the rendered models out for a less detailed variant when the player camera is far away. You can read more about LOD components and how to use them [here][UDLOD].

### Occlusion Culling

By default, Unity will render everything the player can see, even if that object is obscured behind something. We can prevent this by making use of Occlusion Culling.

Read more about Occlusion Culling and how to use it [here][UDOcclusionCulling].

### Static Batching

Always make sure that any gameobjects present in your level that never move, become disabled, or get destroyed (walls, floor, windows, etc) are correctly marked as static. This will allow Unity to optimize the way in which these objects are rendered.

Assets that have a material with different keywords will often break static batching. Try to avoid using different keywords if possible (for example, a mix of objects with moss enabled/diabled).

Read more about how the static toggle affects objects [here][UDStaticBatching].

### Lighting and Effects

When adding lighting to your level, try to avoid using a lot of realtime lights wherever possible. These types of lights can be incredibly expensive for rendering, and often aren't neccesary. Always aim to use baked lights when applicable.


--- 


# AI 

#### Careful use of level-specific components is key to ensuring the enemy AI behaves correctly. 

### Enemy Spawn Points

Enemy spawn locations can be configured in the [Wave Spawner][WaveSpawner] component. 

The placement and amount of enemy spawn points is important to ensuring the AI spawns without issue.
- It is recommended to include at least 3 enemy spawn locations in your level, to avoid enemies spawning inside each other and becoming stuck. 
- Spawn points should have at least 0.5 meters of space around them to avoid enemies getting stuck inside of walls on spawn.
- Any rotation on a spawn point's X or Z axis can potentially cause issues when spawning enemies.

It is important to note that the rotation of a spawn point will dictate the rotation of the spawned enemy. Enemies will always face the direction the blue arrow (Z axis forward) is pointing when spawning.

### Shoot Point

Shoot points indicate positions in which an enemy archer can move to when shooting at the player. These are an important component in making ranged enemies feel more intelligent in their behaviour, and more challenging for the player.

It is recommended to place these points on elevated areas, or positions with a good line of sight with central areas of your map.

Be careful to make sure these points are placed on the map's navmesh, in areas that are accessibile by enemy AI. Read more about Shoot Points [here][ShootPoint].

### Navigation

Enemies may have trouble navigating some parts of your level's terrain, we can make use of a few methods to improve the way enemies move through the level.

If you notice AI getting stuck at certain points in your level, you can place a [Nav Mesh Obstacle][UDNavMeshObstacle] at that point to encourage the enemies to navigate around it.

Neither the player nor enemies can navigate up stairs without some assistance. The recommended way to add stairs to your level is to use a rotated cube collider as a ramp that aligns with the stairs.
You can also combine this with the expected collider that matches each step, so that props and weapons can still collide with the stairs correctly. Add both the ramp and step-accurate colliders and place the ramp gameobject on the `LocomotionOnly` layer, and the other collider on the `NoLocomotion` layer.





[UDStaticBatching]: https://docs.unity3d.com/Manual/static-batching.html
[UDLOD]: https://docs.unity3d.com/Manual/LevelOfDetail.html
[UDOcclusionCulling]: https://docs.unity3d.com/Manual/OcclusionCulling.html
[UDNavMeshObstacle]: https://docs.unity3d.com/2021.3/Documentation/Manual/class-NavMeshObstacle.html
[WaveSpawner]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/WaveSpawner.md %}
[ShootPoint]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/ShootPoint.md %}