# Items and Weapons

## Introduction

The SDK contains tools to allow users to create props and weapons to be able to be used in Blade and Sorcery. This document will contain what components are available through the SDK, including information on the weapon component and how to use it. Please note that this document will not contain information on components that are part of Unity, such as Mesh renderers and materials.

## Item Definition

The Item Definition is the core component of items. It is necessary for items and weapons to be able to spawn and use and defines all components necessary for the item to work. Inserting an Item definition will automatically add a rigidbody component, and the script is called "Item".

![](https://i.imgur.com/XCchfj6.png)

* The Item ID will have to match the Item JSON ID for the item to work.
* HolderPoint is a component that is automatically added when the Item Definition is added. The HolderPoint is the point of the rotation the weapon will be on the hip/back of the player and the way it is held on the item rack. The Blue Arrow of the HolderPoint points DOWN, meaning if the blue arrow points towards the tip of the blade, it means that the weapon will be held pointing the tip of the blade to the floor. The position of this defines where the center snap is. The Red arrow points sideways of the center of the blade.

![](https://i.imgur.com/RWUqJI9.png)

* ParryPoint is also a component added when the ItemDefinition is created. This component is mainly used when NPCs hold the weapon you made, in where it is the main point that the NPCs hold the weapon.
* Main Handle is the main handle used when grabbing the weapon. This will not update when you make the handle definition, however if you edit something (like change the Item id by one letter then change it back to the original) it will automatically update it for you. This will also be the case of what handle you will grab when un-holstering your weapon from your hip or back.
* FlyRef is the direction the weapon goes when it is thrown or flying. Blue arrow points forward. (this is an empty game object you create yourself)(this component is optional)
* Preview is what the item looks like inside the Item Spawner. For more details on this, it will be mentioned below
* Custom Center of Mass is an edit to define where most of the mass will be when you hold the weapon and for the rigidbody at all times. Usually, this will not need to be edited or ticked, however if you make a weapon which may have a strange default center of mass, this will need changing.
* Custom Inertia Tensor Collider is a Capsule Collider that makes the balance of the weapon more refined and will assist in throwing the item. This is not needed usually, but can be necessary for weapons like Spears, Katanas and Arrows.
* Custom References will be used for Code, and lets coders define parts of the weapon itself.

## The Weapon Mesh
When I defined making it so Unity components will not be mentioned, this is an exception. There are scripts and additions added to the meshes of the items that let it work well in game. 
### Paintable - After Update 9.1, this script should be removed and replaced with Reveal (info below). After Update 10, this script will be removed.
The Paintable script let's blood and decals be added to the weapon mesh. 

![](https://i.imgur.com/LLWieQy.png)

![](https://i.imgur.com/sZ2MvzP.png)

This script does not need configuring, just put it on the mesh at hand. However, if you use a shader like Audodesk Interactive, you will need to add to material properties and replace _BaseMap with _MainTex

### Reveal Decal

![](https://i.imgur.com/K7JgmQW.png)

Reveal is the replacement to Paintable for weapons. It is more performant than Paint, and can create a better looking blood effect. 

INFO: Reveal WILL ONLY WORK for the new ThunderRoad/Lit Shader (which uses Basemap, Normal Map and MSAOE Map) and URP/Lit. Make sure that these shaders are assigned before adding Reveal Decal. Do not change the material that the Reveal Decal generates, as this will change the looks of your weapon when it hits an enemy.

### Emission/Imbue
For imbue, things have to be a little different. The ImbueMap must be created for the mesh (or you can use another texture, like the weapon's metallic map) and put it in the "Emission" part of the material. For the emission colour, it MUST be set to 1,1,1 otherwise the imbue will not work.

![](https://i.imgur.com/GeEKEO0.png)

## Preview Script
The preview script is a component already added to the weapon when the Item Definition is created. 

![](https://i.imgur.com/jAxQPoo.png)

The blue arrow will point towards you when you have it in the book. When you have correctly scaled and rotated the preview, you can tick "Generate Icon" and it will create a PNG next to where the prefab is saved. You can only generate the icon when the prefab is saved. You then export this through addressables and reference it in the json.

## Collider Groups

Collider Groups apply colliders to the weapon and allow the game to interact with them via damage. For a normal sword, it is recommended to use as many collider groups as there are areas of which the item will do damage. For example, a blade on a sword will use pierce and slash, which will require one damager definition, and then the handle, which will deal blunt damage, will require another damager definition. 

![](https://i.imgur.com/gdM8W1w.png)

* Imbue Effect Renderer is the areas of which it defines where the effect will spawn. The "Generate Imbue Mesh" button will create this for you, which is a mesh that will be created in the shape of the colliders.
* Imbue Emission Renderer links to the "Emission" part of this document. For this, you must move the mesh that wants to be imbued to that field.
* Imbue shoot is a gameObject you add for magic staves. You can ignore this if your weapon is not a stave and you do not want to shoot fireballs from the weapon. The gameObject that must be applied to this field must be set where the blue arrow is pointing in the direction it is to shoot.
* Whoosh Point (?)

![](https://i.imgur.com/eKKmdWA.png)

## Damagers

There are three type of damagers: Slash, Pierce and Blunt. Each type of weapon, whether it be a mace, axe and sword, these all use different methods to work with the damager definition. You can have a lot of different damagers per weapon, like having two different types of blunt damagers (like for BluntHead and BluntHandle). 

Note: Multiple Pierce Damagers close to each other is currently not supported and may cause buggy interactions.

### Blunt

Blunt is the easiest way for a weapon to do damage. For blunt damage, you must assign the blunt damage collidergroup, for this being the handle of the sword, and then set both the penetration depth and length to 0. Direction MUST be set to all.

![](https://i.imgur.com/8tkzI7t.png)

### Pierce

Pierce is when the player stabs the enemy (note: this does not count axe piercing). For this, the length of the penetration must be 0, and the depth must go down to the bottom of the stabbed part. Where the penetration depth ends is where the pierce will end when stabbed in to an NPC or material. The Direction must be set to forward, and the part you want to stab with, it's collidergroup must be assigned to it. 

![](https://i.imgur.com/ysgilqg.png)

### Slash

Slash damager is part of damagers of which creates cuts and slashes, and has the ability to dismember body parts. For this to work correctly, the depth MUST be set to an incredibly small number (0.01, for example). Then, the "Length" must fit the size of the weapon. All directions (All, Forward, Backward, Forward/Backward) will work with slashes. This is for blades. For weapons like axes this is different. The length must fit the blade of the axe, however the depth is then used for how far the axe blade can penetrate in to ragdolls and materials.

Sword Slash

![](https://i.imgur.com/8eQsykg.png)

Axe Slash

![](https://i.imgur.com/OtTfwK9.png)

## Handle

Handles are how the player hold the weapon. The handle size must be set via the "Axis Length"

![](https://i.imgur.com/jvhGuSb.png)

* Interactable ID references the Interactables JSON. This can be mentioned in the JSON too.
* Side references which hand can grab the handle
* Touch Radius is how far away the player hand can be to pick up the item
* The Default Grab Axis Ratio is where the default position the hand will grab the item when un-holstered or using telekinesis grab.
* Release Handle references another handle to make it so if the referenced handle is grabbed, it will force the hand holding this handle to ungrab. It can reference itself, meaning that it forces you to grab it with one hand. 
* Slide Up/Bottom handle will reference other handles, in where reaching the top/bottom of this handle will automatically move the players hand to the referenced handle
* Allowed orientations is very important. It allows the player to hold the weapon in a certain way. For best rotation, follow the same orientations as the image above. Then once completed, click the "Update to New Orientations" button.
* Reach is how the NPC knows how far away to stay from the player, and the NPC knows the distance they need to be because of this. Calculate Reach should help easily for this.

![](https://i.imgur.com/P4g6aUu.png)

## Parry

The Parry Definition lets the NPC know the size of the weapon, and the overall area that the NPC knows what to block. Fit the Parry definition to the size of the weapon.

![](https://i.imgur.com/c1osEfq.png)

## Whoosh

The whoosh is a script which displays where the "whooshing" sound plays when you swing the weapon.

![](https://i.imgur.com/APx54Gq.png)