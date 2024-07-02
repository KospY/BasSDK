---
parent: Items
grand_parent: ThunderRoad
---
# Item

###### This component has an [Event Linker][EventLinker].

The `Item` script is the core component for items and weapons in Blade and Sorcery. This component is required for the item to work, and is a dependency on a lot of components listed on the weapon.

{: .warning}
Do not adjust Rigidbody weight, drag and angular drag, as this is overwritten in the json.

{: .tip}
You can press the "Setup Default Components" button to create the default components for an item. This includes a HolderPoint, ParryPoint and [Preview][Preview].

![Inspector][Inspector]

## Component Properties

| Field                             | Description
| ---                               | ---
| Item ID                           | The Item ID should match what you put as `ID` in the Item JSON. Ensure that these components match. In update 1.0.2, this field is no longer required to insert any ID for weapons you usually spawn. However, this is still required for world-attached items and items placed in levles, such as Lanterns, as well as items that may be children of items that are spawned in via an item book (such as a flail, where the ball would be a child of the handle).
| HolderPoint                       | The HolderPoint is a transform which depicts the rotation of the item when stored on a [Holder][Holder]. For example, the Z direction depicts downward direction when stored on the player sides or back.
| Additional HolderPoints           | Like `HolderPoint`, this allows adding more HolderPoints for specific holders. An example is the Weapon Rack in home, which utilises the `HolderRackTopAnchor` HolderPoint.
| ParryPoint                        | Shows the point that `AI` will try to block with when holding the weapon.
| Main Handle Left/Right            | Used for the handle that is grabbed by Default. See [Handle][Handle] to see handle setup.
| Fly Dir Ref                       | Used to point in this direction when thrown, so long as "Fly On Throw" is enabled in JSON. Z Axis / Blue arrow points forwards.
| Preview                           | Automatically added when `Item` script is added. See [Preview][Preview] for setup.
| World Attached                    | Used for Items that are connected to the world. An example is hanging items and doors.
| Creature Physic Toggle Radius     | Radius to depict how close this item needs to be to a creature before the creatures' collision is enabled.
| Use Custom Center of Mass         | Allows user to adjust the center of mass on object. If unticked, this is automatically adjusted. When ticked, adds a custom gizmo to adjust. Use this if weight on the item is acting strange.
| Custom Inertia Tensor             | Used for balance adjustment on a weapon. Use this if swinging weapons are strange. Adjust the Capsule collider to the width of the weapon.
| Custom References                 | Allows a custom reference to be able to reference specific gameobjects and scripts in External code.
| Force Mesh Layer                  | Forces layer of mesh when an item is spawned (Items will have their layer automatically applied when spawned, unless this is set)

## HolderPoint

![HolderPoint][HolderPoint]

### Weapon rack holder setup

Create a second `HolderPoint` (Transform) like in the pictures below.

Then add an additional `HolderPoint` on the `Item` component named **HolderRackTopAnchor** for the weapon rack, **HolderRackTopAnchorBow** for the bow rack and **HolderRackSideAnchor** for the shield rack.

#### Swords/Blades

![PointSetupSword][PointSetupSword]
![HolderPointRack][HolderPointRack]

#### Bows

![BowRack][BowRack]
![HolderPointRackBow][HolderPointRackBow]

#### Shields

![ShieldRack][ShieldRack]
![ShieldPoint][ShieldPoint]

## ParryPoint

![ParryPoint][ParryPoint]


[Handle]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Handle.md %}
[Preview]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Preview.md %}
[Holder]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Holder.md %}
[HolderPoint]: {{ site.baseurl }}/assets/components/Item/HolderPoint.png
[ParryPoint]: {{ site.baseurl }}/assets/components/Item/ParryPoint.PNG
[Inspector]: {{ site.baseurl }}/assets/components/Item/ItemScript.png
[EventLinker]: {{ site.baseurl }}{% link Components/ThunderRoad/Event-Linkers/ItemEventLinker.md %}
[PointSetupSword]: {{ site.baseurl }}/assets/components/Item/PointSetupSword.jpeg
[HolderPointRack]: {{ site.baseurl }}/assets/components/Item/HolderPointRack.png
[BowRack]: {{ site.baseurl }}/assets/components/Item/BowRack.jpeg
[HolderPointRackBow]: {{ site.baseurl }}/assets/components/Item/HolderPointRackBow.png
[ShieldRack]: {{ site.baseurl }}/assets/components/Item/ShieldRack.png
[ShieldPoint]: {{ site.baseurl }}/assets/components/Item/ShieldPoint.png