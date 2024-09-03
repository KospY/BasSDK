---
parent: SDK-HowTo
grand_parent: Guides
---

# Layers

{: .important }
> Do not modify the layers provided in the SDK. You cannot create new layers or edit existing ones.

Layers are used to dictate when or if an object should be visible to the player, as well as what objects it should be allowed to collide with.

Below is a matrix displaying which layers will collide with each other. If a box is checked, these two layers will physically collide with each other on contact.

![Layers][Layers]

### Layer Descriptions

{: .note }
Some of these layers are automatically managed by scripts in the SDK and in-game. 

| Layer                  | Description 
| :----:                 | :-------- 
| Default                | The default layer for all objects. All static objects that you want everything to collide with should be default. | 
| TransparentFX          | Used for transparent VFX
| Ignore Raycast         | Will be ignored for raycast events (e.g. raycast rope, UI pointer etc)    
| Reflections            | Unused. Will be invisible in player view
| Water                  | Used for in-game water (ocean, river etc)
| UI                     | Used for UI interactions
| PhysicsObject          | Potentially unused(?)
| Mirror                 | Used for the Mirror
| LightProbeVolume       | Used for the Light Probe Volume script.
| Touch                  | Used for colliders related to interactions such as item and creature handles.
| DroppedItem            | Automatically used for items when they are dropped or placed in a holster.
| MovingItem             | Used for items in motion (held weapons, props etc)
| PlayerLocomotionObject | Layer used to be able to push player (?)
| Ragdoll                | Used for creature ragdoll
| LiquidFlow             | Used for the flow collider on Potions
| LocomotionOnly         | Will allow collision only for player/AI movements.
| SpectatorHide          | Will hide these objects in spectator view
| NoLocomotion           | Will allow collision only for non-player/AI collisions
| Highlighter            | Used for highlighter actions (e.g ItemSpawner, WaveSpawner etc)
| LoadingCamera          | Used for the loading camera
| SkyDome                | Used for internal tools
| MovingObjectOnly       | Used for objects that have continuous movement (e.g. Waterwheels, portcullis etc)
| PlayerLocomotion       | Used for the player locomotion collider
| BodyLocomotion         | Used for the NPC body locomotion collider
| ItemAndRagdollOnly     | Used for collisions to Items and Ragdolls only.
| TouchObject            | Automatically applied as the active layer on an object when touched by the player.
| Avatar                 | Used for the player's ragdoll body.
| NPC                    | Used for the enemy's ragdoll body.
| FPVHide                | Objects under this layer will not be visible in first person view
| Zone                   | Used for Zone colliders
| ObjectViewer           | Automated layer used for generating preview icons for items.
| PlayerHandAndFoot      | Used for player hands and feet









[Layers]: {{ site.baseurl }}/assets/components/Guides/Layers/Layers.png