---
parent: Levels
grand_parent: ThunderRoad
---
# Player Lightmap Volume

This component goes well in correlation with assets containing a Baked LOD Group

[Baked LOD Group Documentation][BakedLODGroup]{: .btn .btn-purple }

![PLV][PLV]

The Player Lightmap volume is a component used to help the Baked LOD Group determine the minimum/maximum lightmap scale. 

The component will create a box collider which you can use. It is recommended for the box to cover the main play area of the map, of which the height will be the height of the player. 

You can use the "Light Map Scale Multiplier" to adjust the lightmap scale on the components that use the Baked LOD Group, as well as the falloff distance to determine how far away mesh rendereres that have a Baked LOD Group need to be before they reach their minimum lightmap scale.  

[BakedLODGroup]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/BakedLodGroup.md %}
[PLV]: {{ site.baseurl }}/assets/components/PLV/PLV.png