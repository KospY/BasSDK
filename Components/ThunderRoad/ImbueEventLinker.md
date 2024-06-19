---
parent: ThunderRoad
---
# Imbue Event Linker
*If you have not done so already, go read the [Event Linker][EventLinker]'s wiki page. This page only lists and explains the events available to this linker. It will not explain how to use the component itself.*

## Overview
The Imbue Event Linker listens for a number of imbuement-related events emitted from a specific collider group.

This linker can be combined with the [Spell Touch Event Linker][SpellTouchEventLinker] to only listen for imbuement events from a particular spell. This is done by toggling the listening state of this linker when a specific spell enters the area of effect.

## Events

| Event             | Description
| ---               | ---
| On Empty          | Invoked when the imbue fully runs out on the collider group.
| On New Spell      | Invoked when the collider group is empty and begins to be imbued.
| On Fill           | Invoked when the imbue reaches its max energy.
| On Try Use        | Invoked when the player attempts to (but cannot) "fire" a crystal type collider group.
| On Use Ability    | Invoked when the player successfully "fires" a crystal type collider group.
| On Hit            | Invoked when the imbue hits something but has no effect on it.
| On Hit Effect     | Invoked when the imbue hits something and has an effect on it ( Such as anti-gravity, or a staff slam).



[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/EventLinker.md %}
[SpellTouchEventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/SpellTouchEventLinker.md %}