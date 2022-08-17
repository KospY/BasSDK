# Event Linkers
Event linkers are a set of utility scripts that allow you to link together in-game events to activate functions on the components you add to items, maps, or creatures within Unity. All event linkers have a handful of [common functions](#common-methods) that you can activate which afford you even greater control over the control of your item(s).

Every event linker has the same standard "structure" with mild alterations: You define a list of events (For which you configure the **trigger** by selecting one from a dropdown selector) which give you a **Unity event** to drag/drop GameObjects into in order to invoke **methods** from the components added to that GameObject. You can (and may even *need* to) make events with duplicate triggers: The execution order of these events matters!

Using event linkers, you can make items perform complex actions such as playing animations, particles, sounds, etc., introduce puzzle mechanics to your levels, or create custom humanoids with new additions which otherwise wouldn't be possible without writing code. **Event linkers work on both PCVR and Nomad!**

For more specific information regarding each type of event linker and the triggers you can link to, please navigate to its associated wiki page. A full list of [specific event linker types can be found below](#specific-linker-types).

## How to use Event Linkers
(To be completed with screenshots)

## Common methods
The below methods can all be invoked by Unity events, and provide extra functionality that would otherwise be missing.
### SetListen(bool)
`SetListen(bool)` toggles whether or not the event linker is currently "listening". If an event linker is not listening (Set to false), its events will not trigger even when the associated in-game events occur. This allows you to change what behaviour is taken when in-game events occur, and may be useful depending on your use case. All event linkers are listening by default, but they can be toggled off by unchecking the `listening` boolean in the Unity inspector.
### PrintDebug(string)
`PrintDebug(string)` prints a message to the debug log, in a style dependent on how many `!`s you start the message with: Starting with no `!`s means it's a normal log, one `!` means it's a warning log, and two `!!` means it's an error log. These may be useful to you if you're trying to figure out why your item isn't behaving as intended.
### WaitFor\_\_\_\_\_\_(int or float)
`WaitForFixedFrames(int)`, `WaitForFrames(int)`, `WaitForSeconds(float)`, and `WaitForRealtimeSeconds(float)` all add some kind of delay to your event linker. **!! NOTE: these do not work within the same event! You need to have events with duplicate triggers for these to do anything!** With `WaitForFixedFrames`, you wait for that many "physics frames" (Which are different from render frames), with `WaitForFrames`, you're waiting for real rendered frames. Similar logic applies for `WaitForSeconds` and `WaitForRealtimeSeconds`; Realtime seconds means real time seconds, while normal `WaitForSeconds` is in-game time (Meaning it is affected by slow motion, while real seconds are not!)

## Specific linker types
- General
  - [Control Event Linker][ControlEventLinker] for any player controller input triggers
  - [Game Event Linker][GameEventLinker] for any triggers based off hits, kills, creature, or item spawns [^extras]
  - [Handle Event Linker][HandleEventLinker] for any handle grab/ungrab triggers [^varies]
  - [Holder Event Linker][HolderEventLinker] for any holder snap/unsnap triggers
  - [Spell Touch Event Linker][SpellTouchEventLinker] for triggering events when a spell touches an object [^unique]
  - [Unity Event Grouper][UnityEventGrouper] for grouping events together to trigger from other places [^unique]
- Item-focused
  - [Bow Event Linker][BowEventLinker] for triggers related to nocking arrows, firing bows, etc. [^extras]
  - [Collision Event Linker][CollisionEventLinker] for any collision triggers
  - [Damager Event Linker][DamagerEventLinker] for triggering events when a specific damager deals damage
  - [Imbue Event Linker][ImbueEventLinker] for triggering events when imbue is added, filled, or used
  - [Item Event Linker][ItemEventLinker] for any item-related triggers
- Creature-focused
  - [Creature Event Linker][CreatureEventLinker] for triggering events when a creature is hit, grabbed, healed, or attacks [^extras] 
  - [Ragdoll Part Event Linker][RagdollPartEventLinker] for damage, grab, or touch based triggers [^extras] 



----

[^extras]: These event linkers have additional parameters in addition to choosing the trigger
[^varies]: These event linkers have events which may or may not work depending on where the component reference is
[^unique]: These event linkers have entirely different trigger definition (No dropdown)





[ControlEventLinker]:     {{ site.baseurl }}{% link Components/ThunderRoad/ControlEventLinker.md %}
[GameEventLinker]:        {{ site.baseurl }}{% link Components/ThunderRoad/GameEventLinker.md %}
[HandleEventLinker]:      {{ site.baseurl }}{% link Components/ThunderRoad/HandleEventLinker.md %}
[HolderEventLinker]:      {{ site.baseurl }}{% link Components/ThunderRoad/HolderEventLinker.md %}
[SpellTouchEventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/SpellTouchEventLinker.md %}
[UnityEventGrouper]:      {{ site.baseurl }}{% link Components/ThunderRoad/UnityEventGrouper.md %}
[BowEventLinker]:         {{ site.baseurl }}{% link Components/ThunderRoad/BowEventLinker.md %}
[CollisionEventLinker]:   {{ site.baseurl }}{% link Components/ThunderRoad/CollisionEventLinker.md %}
[DamagerEventLinker]:     {{ site.baseurl }}{% link Components/ThunderRoad/DamagerEventLinker.md %}
[ImbueEventLinker]:       {{ site.baseurl }}{% link Components/ThunderRoad/ImbueEventLinker.md %}
[ItemEventLinker]:        {{ site.baseurl }}{% link Components/ThunderRoad/ItemEventLinker.md %}
[CreatureEventLinker]:    {{ site.baseurl }}{% link Components/ThunderRoad/CreatureEventLinker.md %}
[RagdollPartEventLinker]: {{ site.baseurl }}{% link Components/ThunderRoad/RagdollPartEventLinker.md %}