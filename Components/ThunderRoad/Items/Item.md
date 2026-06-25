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

| Field | Description |
| :--- | :--- |
| lightVolumeReceiver | A reference to a [LightVolumeReceiver]({{ site.baseurl }}{% link Components/ThunderRoad/Areas/LightProbeVolumeReceiver.md %}) component for accurate lighting |
| itemVolume | The real geometric volume of the item used for water submersion calculations. Calculated from the prefabs Oriented Bounding Box |

# General

| Field | Description |
| :--- | :--- |
| itemId \[Dropdown\] | The Item ID of the item specified in the Catalog |
| spawnPoint | Specifies the spawn point of the item. This specifies the position and rotation of the item when spawned, it's mostly used for itemSpawner and spawning the item via an item book |
| parryPoint | Shows the point that AI will try to block with when they are holding the item. |
| preview | States the Preview for the item. |
| audioSource | A reference to an AudioSource for sound effects this item uses. Added to avoid creating at runtime |
| snapPitchRange | A random range of pitches the snap AudioContainer will be played at |
| forceMeshLayer | Forces layer of mesh when an item is spawned.<br><br>(Items will have their layer automatically applied when spawned, unless this is set) |
| customReferences | Allows a custom reference to be able to reference specific gameobjects and scripts in External code. |

# Handles

| Field | Description |
| :--- | :--- |
| mainHandleLeft | Specifies what handle is grabbed by default for the Left Hand |
| mainHandleRight | Specifies what handle is grabbed by default for the Right Hand |

# World Parenting

| Field | Description |
| :--- | :--- |
| DisallowDespawn | If ticked, this item won't despawn. Tick at your own risk! |
| worldAttached | Tick if the item is attached to the world and not spawned via item spawner or item book. |
| keepParent | Tick if the item should keep its parent when it loads into the scene. |

# Storage

| Field | Description |
| :--- | :--- |
| holderPoint | Specifies the Holder Point of the item. This specifies the position and rotation of the item when held in a holder, such as on player hips and back. The Z axis/blue arrow specifies towards the floor. |
| additionalHolderPoints | Can add additional holderpoints for different interactables.<br> <br>For Items on the Item Rack, the anchor must be named HolderRackTopAnchor, or alternatively for the bow rack, HolderRackTopAnchorBow, and HolderRackSideAnchor for Shield rack |
| customSnaps | A list of custom snap positions per holder. If an item is misaligned on your left back slot you can use this to change the rotation and position of that holder |
| audioContainerSnap | A reference to the snap audio container played when you holster this item |
| audioContainerInventory |  A reference to the inventory audio container played when you put this item in your inventory |

# Physics

| Field | Description |
| :--- | :--- |
| creaturePhysicToggleRadius | Radius to depict how close this item needs to be to a creature before the creatures' collision is enabled. |
| obbPoint | Oriented bounding box |
| orientedBoundsPoints | World Oriented transforms for a bounding box around the item. |

# Custom CoM

| Field | Description |
| :--- | :--- |
| useCustomCenterOfMass | Allows user to adjust the center of mass on an object.<br>If unticked, this is automatically adjusted. When ticked, adds a custom gizmo to adjust.<br> <br>Use this if weight on the item is acting strange. |
| customCenterOfMass | Position of Center of Mass (if ticked) |

# Custom Inertia

| Field | Description |
| :--- | :--- |
| customInertiaTensor | Used for balance adjustment on a weapon.<br> <br>Use this if swinging weapons are strange. Adjust the Capsule collider to the width of the weapon. |
| customInertiaTensorCollider | Collider of the Custom Inertia Tensor |

# Handling

| Field | Description |
| :--- | :--- |
| flyDirRef | Used to point in direction when thrown.<br>Z-Axis/Blue Arrow points forwards. |
| distantGrabSafeDistance | The safe distance this item can be TK grabbed |
| distantGrabSpinEnabled | Whether this item can be spun |
| distantGrabThrowRatio | The distance to throw ratio of this item |

# Fly Bools

| Field | Description |
| :--- | :--- |
| forceThrown | When ticked, item is automatically set as "Thrown" when spawned. |
| flyFromThrow | When ticked, item is automatically set as "Thrown" when spawned. |

# Fly Settings

| Field | Description |
| :--- | :--- |
| flyRotationSpeed | Speed of which the item rotates when thrown |
| flyThrowAngle | Angle offset of the z-axis arrow when thrown. |

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

## SpawnPoint

The spawnpoint should be positioned in the middle of the item, rotated like so:

![SpawnPoint][SpawnPoint]


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
[SpawnPoint]: {{ site.baseurl }}/assets/components/Item/SpawnPoint.png