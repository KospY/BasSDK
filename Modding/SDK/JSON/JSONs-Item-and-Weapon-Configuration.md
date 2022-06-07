# JSONs: Item and Weapon Configuration

_For the SDK item/prop making, please visit this [page.](https://github.com/KospY/BasSDK/wiki/The-SDK:-Items-and-Weapons)_

_This page will be updated if there are any changes to the JSON. However, this will not be instant, and may take some time to update._

## Introduction

Item JSONs are used for weapons and props for Blade and Sorcery. These are necessary to reference items and weapons and add them to the game. This document will contain how to add weapons to the game through this JSON, as well as possible debug steps if the JSON has errors. Please note that this JSON will reference other JSONs, which you may want to create yourself, however the page might not exist yet and will be made in the future. This document will also contain some script components, such as Quiver definitions. 

For modding, you can copy the "default" Item JSONs and you can edit them accordingly, making sure you change the ID, name and prefab address.

_Please note that if the author does not understand a particular definition inside the JSON, it will be left out until the author finds out. These will be marked with <?>_

## Components

_Note: The JSON used for this is the SwordShortCommon_

**"$type"** : This is to reference the Item data. Do not change this, it should be **_"ThunderRoad.ItemData, Assembly-CSharp,"_**

**"ID"** : This is the Unique Identifier for the weapon. This must be unique to ensure that the ID does not conflict with other mods or the default game. For errors, the Console will be able to define what weapon is at fault.

**"saveFolder"** : This is for internal use, and does not need to be changed. This can be left to **Bas**

**"version"** : This must match the in-game JSON version for the Item Definition. At the current moment (Update 9) this should remain at "**4**". Do not change this.

**"localizationId"** : This is to reference another ID, and is recommended to change to a Unique Identifier. Perhaps you can use a duplicate ID to the "ID" of this JSON.

**"displayName"** : This is how the item name is displayed inside the book, as well as what text displays when you hover your hand over the item.

**"description"** : This is the text that displays in the description inside the book. This can be left blank, however its good to spice up the lore on your item

**"author"** : This defines what displays as the "blacksmith" inside the book. It is recommended to put the creator of the model in here as a reference, however you can reference yourself also.

**"price"** : This is for future use, and does not need to be changed.

**"purchasable"** : This is to define if this item is available inside the item spawner. If set to false, it will not be inside the book.

**"tier"** : Tiers will be more influential in the future, however right now it is currently utilised in the colliderGroup. Currently, tiers are set 0-4.

**"levelRequired"** : This is for future use. It is best to keep this at 0 for the time being.

**"isStackable"** : <?>

**"categoryPaths** This is to define where to find the items in the book. The categories are:
* Unknown
* DamagerBlunt
* DamagerPierce
* DamagerSlash
* Daggers
* Swords
* Axes
* Spears
* Blunt
* Exotics
* Bows
* Crossbow
* Firearms
* Staves
* Shields
* Potions
* Misc
* Traps
* Funny
Custom categories can be added by mods as well.

**"iconEffectId"** : This is used for spells on the spell wheel.

**"iconAddress"** : This is to reference what the Icon looks like in the book. This can be left Null, which then the game will generate an icon at runtime, however this will impact load time. For addressables, click [here.](https://github.com/KospY/BasSDK/wiki/The-SDK:-Addressables) For learning about generating icons and previews [here.](https://github.com/KospY/BasSDK/wiki/The-SDK:-Items-and-Weapons)

**"prefabAddress"** : This is to reference the Item in Unity using addressables. To find out how to reference Addressables, check [here.](https://github.com/KospY/BasSDK/wiki/The-SDK:-Addressables) The Item MUST be the same reference used in the addressables group.

**"pooledCount"** : This is the maximum count for item pooling. Item pooling is a system in which items are "stored" behind the scenes when despawning instead of being removed. This helps with performance for things that get spawned often, like for example arrows.

**"type"** : This is to reference what type of item is used. For example, "Weapon" and "Prop".

**"slot"** : This is to reference how items are held on the player body, as well as other slots/Interactables available. Slots used are below:
* Potion: Used for Potions only
* Small: Hip Slot Only
* Medium: Hip and Back Slots only
* Large: Back Slots only
* Head: Used for Hats and Head slots
* Shield: Used for NPCs to equip weapons.
* Arrow: Used to allow to be inserted in base-game quiver
* Bolt: Used for future usage if Crossbows are added to the base game
* Cork: Used for corks of potions
* Torch: Used for the "Torch" slot on Citadel
Custom slots can be added by mods as well.

**"snapAudioContainerAddress"** : Audio used for the sound of putting the weapon into a slot and taking it out.
The audio group used for this **must** contain 2 sounds. The first one will be used for placing the item, the second one for removing it.

**"mass", "drag" and "angularDrag** : This is used for handling of the weapon. To learn about mass and drag, check out [Mass](https://docs.unity3d.com/ScriptReference/Rigidbody-mass.html) and [Drag](https://docs.unity3d.com/ScriptReference/Rigidbody-drag.html)
NOTE: These values overwrite Unity's Rigidbody component values.

**"manaRegenMultiplier"** : This is used for mana regeneration on Spells Only!

**"spellChargeSpeedPlayerMultiplayer"** : This is used for Charge Speed on Spells only!

**spellChargeSpeedNPCMultiplayer"** : This is used for Charge Speed on Spells only! Charge Speed for NPCs.

**"collisionMaxOverride"** : <?>

**"collisionEnterOnly"** : <?>

**"collisionNoMinVelocityCheck"** : <?>

**"forceLayer"** : This is used for Unity internals. For information about this, check [here.](https://docs.unity3d.com/Manual/Layers.html)

**"flyFromThrow"** : This is used for the weapon to rotate in the direction of the FlyRef. The FlyRef is referenced [here.](https://github.com/KospY/BasSDK/wiki/The-SDK:-Items-and-Weapons)

**"flyRotationSpeed"** : This is the speed of weapon spinning using telekinesis.

**"flyThrowAngle"** : This is the angle of which the item will fly at (if FlyRef isnt referenced<?>)

**"telekinesisSafeDistance"** : When the item is spinning, it is the distance the item stays away from the player.

**"telekinesisSpinEnabled"** : Dictates if the player can telekinesis spin the item.

**"telekinesisThrowRatio"** : <?>

**"grippable"** : If the item can be gripped on any spot (different from grabbing by handle definition)

**"modules"** : Used for referencing scripts. For more information on modules and how to implement them click [here](https://github.com/KospY/BasSDK/wiki/Item-modules)
(ItemModuleAI does not need changing if you copy the correct base default weapon).

**"colliderGroups"** : This is to define imbuement. The "transformName" must be the colliderGroup name used in the prefab. The colliderGroup ID references the colliderGroup JSON, and will reference colliderGroups which are indifferent to types of weapon.

**"damagers"** : This is to allow weapons to deal damage. The "transformName" references the name of the damager, and "damagerID" references the damager JSON. Damagers will be defined differently to the type of damage. 

**"Interactables"** : This is to define handles. "InteractableId" references the Interactable JSONs, and the HandPose definition in this references the Handpose JSON. Note: This also applies to quivers, and will need to be made for custom quivers or holders.

**whooshs"** : This is to define the sounds made when the item is swung. "transformName" references the name of the GameObject, as well as the "effectId" references the Effect JSON, which parameters define the sounds made. A Whoosh component will have to be made on the weapon for this to take effect, along with the "Whoosh" script added to it.