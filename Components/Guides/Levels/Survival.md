---
parent: Levels
grand_parent: Guides
---

# Setting up Survival in your Modded Maps

Setting up survival is easy for your modded maps, there is just a list of components and parameters that you can place, and the rest is done at runtime. 

The Arena Pillars are not included in the SDK, however, the way they are coded makes it so they are now placed at run-time. 

## Setup

1. Create your Level, the [Level Component][Level] is required for this, especially for Survival setup
2. Create an Empty GameObject, and under it, place three gameobjects. You can name these "RewardPillar1/2/3", ensure that these pillars are far apart from eachother. In arena, they are placed like this:

![PillarPlacements][PillarPlacements]

3. Under the [Level Component][Level], under Custom references, apply these references. Ensure that the names match the image. Place the Wave Book under "WaveSelector", Racks under "Rack", the Item Spawner Book(s) under "WeaponSelector" and finally, reference all the reward pillar gameobjects you created under "RewardSpawnPosition".

{: .note}
The WeaponSelector/Rack/WaveSelector references disable these objects when in survival. If these objects are baked, these could cause baking issues like dark shadowed areas or culling issues. It is recommended to set these objects to not be static GI and Ocluders.

![CustomReferences][CustomReferences]

4. Set up waves like normal.
5. In the JSON, ensure that you have the Survival Module. This references all the waves, loot tablers for rewards, and the pillar address. You can copy the survival module from the Arena.json or other levels.

![JSON][JSON]

6. Now it should be done. Load the game, and at the Map Selection Board, use the arrows next to "Sandbox" to select "Survival".

### Wave Assault

Wave Assault is Crystal Hunt's survival wave on the map board. This will randomly appear on the map board every now and then, and contains coins when the wave has ended. You can add your own custom map here, by inserting this under the modes section of your Level.JSON. Copying the Arena.json file will also contain this method.

![WaveAssault][WaveAssault]



[PillarPlacements]: {{ site.baseurl }}/assets/components/Guides/Levels/PillarPlacements.png
[JSON]: {{ site.baseurl }}/assets/components/Guides/Levels/JSON.png
[CustomReferences]: {{ site.baseurl }}/assets/components/Guides/Levels/CustomReferences.png
[WaveAssault]: {{ site.baseurl }}/assets/components/Guides/Levels/WaveAssault.png

[Level]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/Level.md %}