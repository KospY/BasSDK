---
parent: Armors
grand_parent: Guides
---
# Creating custom armor

<aside>
⚠️ Custom armors is for advanced modders, as it require a good knowledge of rigging, skinning and 3D modeling characters in different softwares.
The guide is a first draft about how to do armors, it may lack some important tips, in this case, feel free to contact a Warpfrog team member for more information.

</aside>

Since U12, the SDK now contain a special version of **Manikin**, the system we use to manage modular characters. 

In the example folder in the SDK, you will find a custom helmet already configured, that serve as example of how to do a custom armor part.

![Untitled](Creating%20custom%20armor%206f51b810fafa46ca8ced4ea7df7d6851/Untitled.png)

## Source fbx

---

![Untitled](Creating%20custom%20armor%206f51b810fafa46ca8ced4ea7df7d6851/Untitled%201.png)

The example folder contain a male and female fbx, that can be used as an example source to create new armor parts. 

The guide will not cover how to create a compatible character model using a 3d software, so you will have to figure out how to do this.

The fbx can be used as a cut guide, to help you to avoid clipping and adjust a bitmask used for vertex occlusion (an armor part can hide a part of the torso for example), and as reference for the current rig used in the game.

![Untitled](Creating%20custom%20armor%206f51b810fafa46ca8ced4ea7df7d6851/Untitled%202.png)

![The guide can’t focus on using a 3d modeling software, but we can at least share the export settings for Maya that is used by our character artist :p](Creating%20custom%20armor%206f51b810fafa46ca8ced4ea7df7d6851/Untitled%203.png)

The guide can’t focus on using a 3d modeling software, but we can at least share the export settings for Maya that is used by our character artist :p

![Untitled](Creating%20custom%20armor%206f51b810fafa46ca8ced4ea7df7d6851/Untitled%204.png)

<aside>
🗒️ When we export the Rig cannot be different from a scale of 1 before being exported from the modeling software.

</aside>

## Creating parts

---

<aside>
🗒️ A part is a prefab that contain all skinned mesh renderers and components that will be added on the character in game

</aside>

To create a new armor part, drag and drop the armor fbx into the scene, select one (or multiple) skinned mesh renderer(s), go to `Gameobject → Manikin → Create new part` then set the path where the part(s) should be saved.

Once the part is created, drag and drop the asset in an addressable group of your choice, and define an address to it.

Optionally, you can open the prefab and do the following:

Assign a rig on the manikin smr part to be able to view the mesh in prefab mode:

![Untitled](Creating%20custom%20armor%206f51b810fafa46ca8ced4ea7df7d6851/Untitled%205.png)

Add the reveal decal component on all skinned mesh renderers (including all lods) so reveal (decals) works

Character material should use the Shader ThunderRoad LIT, it’s necessary for reveal (decal) and other systems.

![Try to optimize mask resolution depending on part size and LOD. For example sandals may need a lower resolution while a chestplate need an higher one, and LOD3 probably could even have lower resolution, if even no reveal at all.](Creating%20custom%20armor%206f51b810fafa46ca8ced4ea7df7d6851/Untitled%206.png)

Try to optimize mask resolution depending on part size and LOD. For example sandals may need a lower resolution while a chestplate need an higher one, and LOD3 probably could even have lower resolution, if even no reveal at all.

### Physic material / IDMAP

Like items and environments colliders, armors can also use a physic material. To set a different material than flesh (default on characters), you need to add the mesh part component to the root of the part prefab.

![Untitled](Creating%20custom%20armor%206f51b810fafa46ca8ced4ea7df7d6851/Untitled%207.png)

As character only use simple colliders, the game use a custom system to determine the physic material on the collision point. This work by ray casting the low poly mesh to get an UV coordinate that can be then used to retrieve a specific color on an IDMap.

![ID map of the chest part of the gladiator armor](Creating%20custom%20armor%206f51b810fafa46ca8ced4ea7df7d6851/Untitled%208.png)

ID map of the chest part of the gladiator armor

Mesh part component:

<aside>
🗒️ `Skinned mesh renderer` reference the mesh that will be used to retrieve the ID map color. For better performance, we recommend to use an higher lod so the raycast will be faster on a low poly mesh

</aside>

<aside>
🗒️ `Default physic material` is the default material that will be used on the whole mesh. If the part is only one physic material, setting this should be enough.

</aside>

<aside>
🗒️ `Id Map` is the texture to use in case the armor have multiple physic material

</aside>

![SDK contain textures of each material type and an IdMapTest shader to visualize your idmap on your armor](Creating%20custom%20armor%206f51b810fafa46ca8ced4ea7df7d6851/Untitled%209.png)

SDK contain textures of each material type and an IdMapTest shader to visualize your idmap on your armor

## Creating wardrobe

---

<aside>
🗒️ A wardrobe is a scriptable object that contain the data about how to render the part on the character (slot, channel, layers, etc…)

</aside>

To create a new wardrobe, right click on the project folder you want and select `Create → Manikin → Wardrobe Data`

Once the wardrobe is created, drag and drop the asset in an addressable group of your choice, and define an address to it.

Then reference the part you created in `asset Prefab`, set the `editor location label`, and define the slot (channel), and layer that will be used for this part.

![Untitled](Creating%20custom%20armor%206f51b810fafa46ca8ced4ea7df7d6851/Untitled%2010.png)

You can optionally fully or partially occlude a layer (in the above example, we hide the hair as it’s an helmet)

### How to create a part and a wardrobe, full video below:

[Unity_Iwoc7QSSwf.mp4](Creating%20custom%20armor%206f51b810fafa46ca8ced4ea7df7d6851/Unity_Iwoc7QSSwf.mp4)

## Creating a world item and configuring json

---

Armor need a physical item, so armor can be manipulated and dropped anywhere. For this, you need to create a new item. The creation of an item is not covered on this tutorial, so we will focus on the specifics for an armor. For an example of an already configured armor item, you can check `ArmorItems_ProtoHelmet` prefab in the SDK.

In the case the male and female armor have a different look, you will need to add on the prefab the component: `Clothing Gender Switcher`

This component will switch the mesh on the item depending on the player gender. The component should be pretty self explanatory, you only have to reference you different meshes and optionally reference the handles.

![Untitled](Creating%20custom%20armor%206f51b810fafa46ca8ced4ea7df7d6851/Untitled%2011.png)

![Untitled](Creating%20custom%20armor%206f51b810fafa46ca8ced4ea7df7d6851/Untitled%2012.png)

On the item json, add the module `ItemModuleWardrobe`, and define two wardrobes (male and female), with the address you set for your wardrobe:

```json
"modules": [
{
	"$type": "ThunderRoad.ItemModuleWardrobe, ThunderRoad",
	"category": "Apparel",
	"castShadows": "PlayerAndNPC",
	"wardrobes": [
	{
		"$type": "ThunderRoad.ItemModuleWardrobe+CreatureWardrobe, ThunderRoad",
		"creatureName": "HumanMale",
		"wardrobeDataAddress": "Creature.Wardrobe.ProtoMaleHelmet"
	},
	{
	"$type": "ThunderRoad.ItemModuleWardrobe+CreatureWardrobe, ThunderRoad",
	"creatureName": "HumanFemale",
	"wardrobeDataAddress": "Creature.Wardrobe.ProtoFemaleHelmet"
	}
	],
	"isMetal": false
}
],
```

If needed, a json example is available in the SDK in the proto json folder: `[SDK Path]\BuildStaging\Catalogs\default\proto\Items\Item_Wardrobe_ApparelProtoHelmet.json`

The latest stage then will be to create the asset bundles (see [Using SDK to create asset bundles](Using%20SDK%20to%20create%20asset%20bundles%20daf5ec5a890945c9a514be6985eedcd3.md)), and copy over the armor item json in your mod folder. Once in-game, the new armor part(s) should automatically show on the item spawners.