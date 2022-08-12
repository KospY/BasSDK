# Holder
###### This component inherits from [Interactable][Interactable]. <br> This component has an [Event Linker][EventLinker].

## Overview
The holder component allows you to create a slot for items to be placed in. 

The behaviour of holders are determined by the `Interactable Data` linked by the `Interactable ID` property.   
Holders can be used to create quivers, weapon racks and weapon holsters on creatures.

Holder slots have a visual indicator for how held items will be oriented. This orientation is relative to the [holder point][HolderPoint] rather than the item itself.  
The Z axis (blue arrow) of the holder point will align with the purple arrow on the gizmo, while the Y axis (green arrow) will align with the green arrow.

### Setup
1. Create an empty object.
2. Select `Add Component` in the inspector and add the Holder component.

## Component Properties

| Field                 | Description
| ---                   | ---
| Draw Slot             | The position of this holder, assuming it has been placed on a creature.
| Use Anchor            | If true, this holder will only align items to the first slot.
| Slots                 | A list of physical positions that held items will appear in.
| Start Objects         | A list of items this holder will start with. These items must already exist in the scene.
| Ignored Colliders     | Items placed in this holder will not have physics interactions with any colliders in this list.
| Editor Target Anchor  | This value is used to define the target anchor **in editor only**. This value will not be used ingame.
| Linked Container      | A [Container][Container] that the contents of this holder will be saved and loaded from.



[Interactable]: {{ site.baseurl }}{% link Components/ThunderRoad/Interactable.md %}
[EventLinker]: {{ site.baseurl }}{% link Components/ThunderRoad/HolderEventLinker.md %}
[HolderPoint]: {{ site.baseurl }}{% link Components/ThunderRoad/Item.md %}#holderpoint
[Container]: {{ site.baseurl }}{% link Components/ThunderRoad/Container.md %}