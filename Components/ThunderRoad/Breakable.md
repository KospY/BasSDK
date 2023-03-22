# Breakables

This item is the core component to allow an item to break in Blade and Sorcery. This component supports GameObjects that contain the [Item][Item] Script, as well as supporting components that only contain rigidbody.

![BreakableScript][BreakableScript]

## Usage

### Basic Setup
1. Create an Empty GameObject, add the [Item][Item] script. 
2. Crate another empty GameObject under the item, recommended to name "Unbroken. Move the parent "HolderPoint, "ParryPoint" and "Preview" under this object.
3. Set up the "Unbroken" GameObject in to an Item, with all damagers and colliders, as well as the unbroken mesh. (See [Item][Item] on how to set up items)
4. Now, set up a new GameObject under the main Parent, recommended to name "Broken".
5. Put all broken meshes under this gameobject. If you want to make them in to an item, put the mesh under a parent, and set the broken parts as [Items][Item]. All non-item broken meshes MUST contain a RigidBody.
6. Once completed, add the "Breakables" script at the top parent, where the main Item is.
7. Set the "Unbroken" Gameobject in the "Unbroken Objects Holder" and the "Broken" Gameobject in the "Broken Objects Holder".
8. The Breakables Item is now setup, now you can adjust the fields to your liking.

```tip
The broken items can use the same Item ID as the broken mesh, however they can also contain different item IDs, even their own, to ensure that they have the same weight and item properties.
```

![BreakableOrganisation][BreakableOrganisation]

### Buttons
This script has five buttons which assist you in setting up the breakable items automatically. 

| Button									  | Description
| ---										  | ---
| Set up handles							  | Automatically sets up handles between the unbroken and broken items. For the handles to link, they must be in the same rotation and position.
| Check for collider intersection			  | Four buttons check for this, in which it will check the item intersections between the broken items, to prevent unwanted explosion. You can check between a 10cm threshold, 5cm threshold, 1cm threshold and 0cm threshold.

### Gizmo
This component contains a lot of Gizmo to help you check visually if the item is set up correctly.

![Gizmo][Gizmo]

## Component Properties

| Field										  | Description
| ---										  | ---
| **Object Holders**
| Unbroken Objects Holder					  | Parent GameObject containing the unbroken/full item.
| Broken Objects Holder						  | Parent GameObject containing the broken meshes/items.
| **Break Points**
| Break Points								  | Here, you can edit a specific point of which the item will break. You can create a sphere to make it so the item can only break if hit in this certain area.
| **Damage**
| Hits Until Break							  | This number determines how many hits it will take before the item is broken. 
| Needed Impact Force to Damage				  | This determines the force required to the object to count as a "Hit" for the item to break.
| Ignore Object Under Certain Mass			  | This determines the the minimum mass an item needs to be to count as a "Hit" for the item to break.
| Can Instantaneously Break					  | When ticked, the item can ignore "Hits Until Break" if "Instantaneously Break Velocity Threshold" is met
| Instantantaneously Break Velocity Threshold | Velocity of which the item will break if "Can Instantaneously Break" is ticked.
| **Time**
| Hit Cooldown Time							  | The cooldown of how many times items can be "Hit".
| Despawn Linked Item						  | When ticked, broken items/meshes will despawn.
| Despawn Linked Item Delay					  | Delay of which the broken items/meshes will despawn when enabled.
| **Handle**									  
| Handle Links								  | Will link [Handle][Handle] between the unbroken and broken mesh. Using the button at the top, this will automatically link handles that are in the same place between the two states.
| Explosion Forces
| **Ignore Collision on Break**				  | When ticked, broken items will ignore collision with eachother for a short amount of time when the item is broken.
| Use Explosion Force						  | When ticked, when an Item breaks, broken items/meshes will "explode" in an outward direction, forcing a backward velocity for the object.
| Explosion Force Factor					  | The force given to the broken objects when they "explode".

### Events
The Breakables component has a number of UnityEvents that are invoked when a Breakable item is Hit or Broken.   
For more about UnityEvents and how to use them, refer to the official [Unity Documentation][UnityEvents].

| Event										  | Description
| ---										  | ---
| On Break									  | Event will invoke when the item is broken
| On Take Damage							  | Event will invoke when the item is "Hit" under the "Hits Until Break" Field.




[Item]: {{ site.baseurl }}{% link Components/ThunderRoad/Item.md %}
[Handle]: {{ site.baseurl }}{% link Components/ThunderRoad/Handle.md %}
[UnityEvents]: https://docs.unity3d.com/Manual/UnityEvents.html
[BreakableScript]: {{ site.baseurl }}/assets/components/Breakables/BreakableScript.PNG
[BreakableOrganisation]: {{ site.baseurl }}/assets/components/Breakables/BreakableOrganisation.PNG
[Gizmo]: {{ site.baseurl }}/assets/components/Breakables/Gizmo.png
