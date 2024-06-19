---
parent: ThunderRoad
---
# Spell Touch Event Linker
*If you have not done so already, go read the [Event Linker][EventLinker]'s wiki page. This page only lists and explains the events available to this linker. It will not explain how to use the component itself.*

## Overview
The Spell Touch Event Linker detects when a spell enters its area of effect. 

For this event linker to work, a 3D collider component must first be added to the object. Events are activated when a spell enters the area defined by this collider.

```danger
The attached collider **must** have its `Is Trigger` field enabled to be used by this event linker.
```

## Event Fields

| Field         | Description
| ---           | ---
| Spell ID      | If the detected spell does not match this ID, it will be ignored.
| Step          | This event will only listen to spells (entering/exiting) the area.
| Min Charge    | The event will ignore spells with a charge lower than this value (0 -> 1)



[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/EventLinker.md %}