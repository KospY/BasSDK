---
parent: Levels
grand_parent: ThunderRoad
---

# Baked LOD Group

Baked LOD Group is a script that reduces lightmap space and allows you to use high poly models for lightmap baking.

This component goes well in correlation with a Player Lightmap Volume
[Player Lightmap Volume Documentation][PlayerLightmapVolume]{: .btn .btn-purple }

## Lightmap Scaling

In The component, you will see a "Min" and "Max" lightmap scale. The "min" is the minumum lightmap scale, and the "max" is the maximum lightmap scale that the mesh renderer can be. 
The value is determined by the player play area, determined by a Player Lightmap Volume.

The "Scale by Object Scale" tickbox makes it so the min/max values are altered differently depending on the object's scale, increasing it so that there is no lightmap artifacts/errors.

## Apply Source Lightmap on Target

This section of the Baked LOD Group applies the lightmap to its LODs. When ticked, and the LODs are referenced, all lightmaps from the LOD0 will be applied to the lower LODs, and so the models' UVs are placed in the same position in the lightmap. 
This will only be effective if the LODs have the same Lightmap UV between then, and it will likely not work properly if the model is using Unity Generated UVs.

For them to take the LOD0 lightmap, the LOD0 must be referenced in the "Mesh Renderer" under "Source".

If the Baked LOD Group is on the Mesh, you can click the "Get Mesh Renderer" button to automatically apply the settings.

## High Poly

This section allows you to bake your model using a high poly varient of the model. To prevent high poly models being exported and increasing memory, you must instead reference the Mesh name and Mesh GUID in to the high poly field.

[PlayerLightmapVolume]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/PlayerLightmapVolume.md %}