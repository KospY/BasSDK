---
parent: Items
grand_parent: ThunderRoad
---
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

{: .tip}
The broken items can use the same Item ID as the broken mesh, however they can also contain different item IDs, even their own, to ensure that they have the same weight and item properties.


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
| **Parent Breakable**
| Parent Breakable                            | The parent of a breakable item if applicable.                                          
| **Object Holders**
| Unbroken Objects Holder					  | Parent GameObject containing the unbroken/full item.
| Broken Objects Holder						  | Parent GameObject containing the broken meshes/items.
| **Break Points**
| Break Points								  | Here, you can edit a specific point of which the item will break. You can create a sphere to make it so the item can only break if hit in this certain area.
| **Damage**
| Can Break                                   | When enabled, item can break.
| Break on Envuro                             | When set to "Always", the item can break on static environment. 
| Contact Break Only                          | When enabled, item can only be broken by something that isn't a collission.
| Use Health    							  | When ticked, this item should use the "momentum health" field. If disabled, item will break in one sufficient hit
| Momentum Health           				  | How much "Health" the item has. This amount is decreased proportional to the strength/velocity of the hit.
| Minimum Damage Momentum       			  | The mimimum momentum needed for a hit to count for damage (to damage Momentum Health)
| Clamp Damage                                | Applies a maximum amount of "momentum health" damage that can be dealt in a single hit. This can ensure that the item can't be broken in one single big hit.
| Max Collission Momentum                     | This is the maximum momentum damage that can be taken in a single hit. Will only work if "Clamp Damage" is enabled.
| Momentum Mass Factor Clamp                  | The Minimum/Maximum values for the mass factor to calculate momentum. Only change this if you find that heavy/light objects aren't properly affecting the breakable item.
| Ignore Objects Under Certain Mass           | When enabled, items with a mass under the "Mimimal Mass Threshold" will not damage the breakable health
| Mimumal Mass Threshold                      | Minimum mass of an item that can deal damage to the breakable item. Any item with a mass under this threshold will not do damage.
| Can Instantaneously Break					  | When ticked, the item can ignore "Hits Until Break" if "Instantaneously Break Velocity Threshold" is met
| Instantantaneously Break Damage             | Momentum damage required to instantly break the item.
| **Time**
| Hit Cooldown Time							  | The cooldown of how many times items can be "Hit".
| Despawn Linked Item						  | When ticked, broken items/meshes will despawn.
| Despawn Linked Item Delay					  | Delay of which the broken items/meshes will despawn when enabled.
| **Handle**									  
| Handle Links								  | Will link [Handle][Handle] between the unbroken and broken mesh. Using the button at the top, this will automatically link handles that are in the same place between the two states.
| **Explosion Forces**
| Ignore Collision on Break				  | When ticked, broken items will ignore collision with eachother for a short amount of time when the item is broken.
| Use Explosion Force						  | When ticked, when an Item breaks, broken items/meshes will "explode" in an outward direction, forcing a backward velocity for the object.
| Explosion Force Factor					  | The force given to the broken objects when they "explode".

### Events
The Breakables component has a number of UnityEvents that are invoked when a Breakable item is Hit or Broken.  
 
[Unity Event Documentation](https://docs.unity3d.com/Manual/UnityEvents.html){: .btn .btn-purple }

| Event										  | Description
| ---										  | ---
| On Take Damage							  | Event will invoke when the item is "Hit" under the "Hits Until Break" Field.
| On Non Break Hit                            | Event will invoke when the breakable item is hit by an item that will not break the breakable.
| On Break									  | Event will invoke when the item is broken




[Item]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Item.md %}
[Handle]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Handle.md %}
[UnityEvents]: https://docs.unity3d.com/Manual/UnityEvents.html
[BreakableScript]: {{ site.baseurl }}/assets/components/Breakables/BreakableScript.PNG
[BreakableOrganisation]: {{ site.baseurl }}/assets/components/Breakables/BreakableOrganisation.PNG
[Gizmo]: {{ site.baseurl }}/assets/components/Breakables/Gizmo.png
