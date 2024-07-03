---
parent: Creatures
grand_parent: ThunderRoad
---
# Mesh Part

Mesh Part is a component used for armor detection on Manikin Armor. 

{: .important }
For more information on making Armors through Manikin, see the [Armor Documentation][ArmorGuide].

## Fields

| Field                             | Description
| ---                               | ---
| Skinned Mesh Renderer             | Mesh used for mesh detection. If your armor has LODs, it is recommended to use your lowest LOD here for better performance. The ID map must fit this LOD.
| Default Physic Material           | If the ID map has an invalid/unsupported color, or has no ID map at all, all of this armor part will have this physics material.
| Id Map                            | ID Map Texture. Ensure once you put it in the component, you convert it to an Id Map Array with the button. For more information on the Id Map, check the [Armor Documentation][ArmorGuide].
| Id Map Array                      | This is an optimisation to an Id Map, and is best to be converted from the Id Map. Ensure once the part is complete, the "Id Map" texture is removed.
| Scale                             | The factor to scale the ID map down by. Default '4'.


[ArmorGuide]:  {{ site.baseurl }}{% link Components/Guides/Armors/index.md %}