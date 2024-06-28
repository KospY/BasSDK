---
parent: Levels
grand_parent: ThunderRoad
---
# Mirror

The mirror script adds a Mirror to Blade and Sorcery, letting you look at your character, items and allow you to change your clothing.

## Fields

| Field                             | Description
| ---                               | ---
| Use Occlusion Culling             | When ticked, the mirror will use occlusion culling, ensuring it doesn't render anything that is not in view
| Allow Armor Editing               | Allows the played to edit their armor when in the mirror zone. Can be turned on/off with a [Unity Event](https://docs.unity3d.com/Manual/UnityEvents.html).
| Reflection Direction              | Define the direction that the mirror is pointing in to reflection. The gizmo of the mirror should reflect this as a blue arrow.
| Width and Height                  | Defines the width/height of the mirror
| Quality                           | Defines the quality of the mirror reflection
| Intensity                         | Adjusts the intensity of the dirt/grain on the mirror reflection (?)
| Reflection Without GI             | When ticked, reflections will not have any Global Illumination/lighting.
| Anti Aliasing                     | Depicts how much anti-aliasing the reflection has. Higher the number, the smoother sharp edges will be.
| Filter Mode                       | Adjusts the anisotropic filtering of the reflection.
| Clear flags                       | Depicts flags that the mirror will avoid rendering in its' reflection
| Shadow                            | When enabled, the mirror reflection will render shadows.
| Enable Fog                        | When enabled, the mirror reflection will render fog.
| Show Wearable Highlight           | When "Allow Armor Editing" is enabled, if this is ticked, it will show a white highlight around the player part.
| Background Color                  | Color of the mirror background. (utilised if Skybox is disabled)
| Culling Mask                      | Defines the layers that the mirror will render.
| Mirror Mesh                       | Defines the mesh the mirror will be on.
| Mesh to Hide                      | In a scene, you are able to define in a list what meshes you are to hide in the reflection of the mirror.

## Setup

To see an example Mirror prefab setup, there is a proto-asset of the mirror in the SDK.