---
parent: Exclude
grand_parent: ThunderRoad
---
# Event Linkers

{% comment %}
This page is intended to explain the concept of Event Linkers and to be a quick reference for their usage. 
A seperate page will be created later in the Guides category for using event linkers, and will be linked to from this page.
{% endcomment %}

## Overview

Event linkers are a set of utility scripts that allow you to link together in-game events to activate functions on the components you add to items, maps, or creatures within Unity. All event linkers have a handful of [common functions](#common-functions) that you can activate which afford you even greater control over the control of your item(s).

Every event linker has the same standard "structure" with mild alterations: You define a list of events (For which you configure the **trigger** by selecting one from a dropdown selector) which give you a **Unity Event** to drag/drop GameObjects into in order to invoke **methods** from the components added to that GameObject. You can (and may even *need* to) make events with duplicate triggers: The execution order of these events matters!

Using event linkers, you can make items perform complex actions such as playing animations, particles, sounds, etc., introduce puzzle mechanics to your levels, or create custom humanoids with new additions which otherwise wouldn't be possible without writing code.

#### General Linkers
- [Control Event Linker][ControlEventLinker] for any player controller input events.
- [Game Event Linker][GameEventLinker] for any events related to hits, kills, creature, or item spawns. 
- [Handle Event Linker][HandleEventLinker] for any handle grab/ungrab triggers
- [Holder Event Linker][HolderEventLinker] for any holder snap/unsnap triggers
- [Spell Touch Event Linker][SpellTouchEventLinker] for triggering events when a spell touches an object 
- [Unity Event Grouper][UnityEventGrouper] for grouping events together to trigger from other places 
 
#### Item-Focused Linkers
- [Bow Event Linker][BowEventLinker] for triggers related to nocking arrows, firing bows, etc. 
- [Collision Event Linker][CollisionEventLinker] for any collision triggers
- [Damager Event Linker][DamagerEventLinker] for triggering events when a specific damager deals damage
- [Imbue Event Linker][ImbueEventLinker] for triggering events when imbue is added, filled, or used
- [Item Event Linker][ItemEventLinker] for any item-related triggers

#### Creature-Focused Linkers
- [Creature Event Linker][CreatureEventLinker] for triggering events when a creature is hit, grabbed, healed, or attacks 
- [Ragdoll Part Event Linker][RagdollPartEventLinker] for damage, grab, or touch based triggers 




{: .tip}
Event linkers are compatable with both Nomad and PCVR!


## Common Functions

The following functions can all be invoked by Unity Events, and provide additional functionality to Event Linkers.

| --- | 
| ![Common Functions Preview][CommonFunctions] |

### SetListen
This function can be used to toggle a linker's `Listening` property from an event.

If an event linker is not listening, its events will not trigger even when the associated in-game events occur. This allows you to temporarily disable an event linker. All event linkers are listening by default, but they can be toggled off by unchecking the `Listening` property in the Unity inspector.


### PrintDebug

Prints a message to the ingame console and player log. These may be useful to you if you're trying to figure out why your linker isn't behaving as intended.

{: .tip}
Adding exclamation marks (`!`) to the start of the message will change the output type of your message.  
A single exclamation mark will output a yellow warning message in the console.  
Two exclamation marks will output a red error message in the console.


### WaitFor...

These functions will add a time delay between the current event, and the next event in the list.

- WaitForFrames
  - Create a delay that will continue until the specified number of frames have occured.
- WaitForFixedFrames
  - Create a delay that will continue until the specified number of physics frames have occured.
- WaitForSeconds
  - Waits a number of in-game seconds. This time is scaled, and will be affected if slow-motion is active.
- WaitForSecondsRealtime
  - Waits a number of realtime seconds. This delay will not be affected when slow-motion is active.

{: .warning}
WaitFor... creates a delay between events, not within them. Once the delay has elapsed, the next event in the list will be invoked based on if that action **occurred in the frame the delay was started in**. 

This can be used to your advantage to create **timers**. By adding duplicate events to your events list and adding a delay to the first of these events, you can offset when your UnityEvent is invoked.

The following example will play a particle system when the alternate-use button is pressed while holding a handle, and after five realtime seconds, the particle system will be stopped. 

![WaitFor Example Usage][WaitForExample]

[WaitForExample]: {{ site.baseurl }}/assets/components/EventLinker/WaitFor.jpg






[Preview]:                {{ site.baseurl }}/assets/components/EventLinker/Preview.jpg
[CommonFunctions]:        {{ site.baseurl }}/assets/components/EventLinker/CommonFunctions.jpg


[ControlEventLinker]:     {{ site.baseurl }}{% link Components/ThunderRoad/ControlEventLinker.md %}
[GameEventLinker]:        {{ site.baseurl }}{% link Components/ThunderRoad/GameEventLinker.md %}
[HandleEventLinker]:      {{ site.baseurl }}{% link Components/ThunderRoad/Event-Linkers/HandleEventLinker.md %}
[HolderEventLinker]:      {{ site.baseurl }}{% link Components/ThunderRoad/Event-Linkers/HolderEventLinker.md %}
[SpellTouchEventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/Event-Linkers/SpellTouchEventLinker.md %}
[UnityEventGrouper]:      {{ site.baseurl }}{% link Components/ThunderRoad/UnityEventGrouper.md %}
[BowEventLinker]:         {{ site.baseurl }}{% link Components/ThunderRoad/BowEventLinker.md %}
[CollisionEventLinker]:   {{ site.baseurl }}{% link Components/ThunderRoad/CollisionEventLinker.md %}
[DamagerEventLinker]:     {{ site.baseurl }}{% link Components/ThunderRoad/Event-Linkers/DamagerEventLinker.md %}
[ImbueEventLinker]:       {{ site.baseurl }}{% link Components/ThunderRoad/ImbueEventLinker.md %}
[ItemEventLinker]:        {{ site.baseurl }}{% link Components/ThunderRoad/ItemEventLinker.md %}
[CreatureEventLinker]:    {{ site.baseurl }}{% link Components/ThunderRoad/CreatureEventLinker.md %}
[RagdollPartEventLinker]: {{ site.baseurl }}{% link Components/ThunderRoad/RagdollPartEventLinker.md %}