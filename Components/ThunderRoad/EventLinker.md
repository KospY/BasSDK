# Event Linkers
Event linkers are a set of utility scripts that allow you to link together in-game events to activate functions on the components you add to items, maps, or creatures within Unity. All event linkers have a handful of common functions that you can activate which afford you even greater control over the control of your item(s).

Every event linker has the same standard "structure" with mild alterations: You define a list of events (For which you configure the **trigger** by selecting one from a dropdown selector) which give you a **Unity event** to drag/drop GameObjects into in order to invoke **methods** from the components added to that GameObject. You can (and may even *need* to) make events with duplicate triggers: The execution order of these events matters!

Using event linkers, you can make items perform complex actions such as playing animations, particles, sounds, etc., introduce puzzle mechanics to your levels, or create custom humanoids with new additions which otherwise wouldn't be possible without writing code. **Event linkers work on both PCVR and Nomad!**

For more specific information regarding each type of event linker and the triggers you can link to, please navigate to its associated wiki page. A full list of specific event linker types can be found below.

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
  - [Control Event Linker](https://kospy.github.io/BasSDK/Components/ThunderRoad/ControlEventLinker.html) for any player controller input triggers
  - [Game Event Linker](https://kospy.github.io/BasSDK/Components/ThunderRoad/GameEventLinker.html)[^1] for any triggers based off hits, kills, creature, or item spawns
  - [Handle Event Linker](https://kospy.github.io/BasSDK/Components/ThunderRoad/HandleEventLinker.html) for any handle grab/ungrab triggers
  - [Holder Event Linker](https://kospy.github.io/BasSDK/Components/ThunderRoad/HolderEventLinker.html) for any holder snap/unsnap triggers
  - [Spell Touch Event Linker](https://kospy.github.io/BasSDK/Components/ThunderRoad/SpellTouchEventLinker.html)[^2] for triggering events when a spell touches an object
  - [Unity Event Grouper](https://kospy.github.io/BasSDK/Components/ThunderRoad/UnityEventGrouper.html)[^2] for grouping events together to trigger from other places
- Item-focused
  - [Bow Event Linker](https://kospy.github.io/BasSDK/Components/ThunderRoad/BowEventLinker.html)[^1] for triggers related to nocking arrows, firing bows, etc.
  - [Collision Event Linker](https://kospy.github.io/BasSDK/Components/ThunderRoad/CollisionEventLinker.html) for any collision triggers
  - [Damager Event Linker](https://kospy.github.io/BasSDK/Components/ThunderRoad/DamagerEventLinker.html) for triggering events when a specific damager deals damage
  - [Imbue Event Linker](https://kospy.github.io/BasSDK/Components/ThunderRoad/ImbueEventLinker.html) for triggering events when imbue is added, filled, or used
  - [Item Event Linker](https://kospy.github.io/BasSDK/Components/ThunderRoad/ImbueEventLinker.html) for any item-related triggers
- Creature-focused
  - [Creature Event Linker](https://kospy.github.io/BasSDK/Components/ThunderRoad/CreatureEventLinker.html) for triggering events when a creature is hit, grabbed, healed, or attacks
  - [Ragdoll Part Event Linker](https://kospy.github.io/BasSDK/Components/ThunderRoad/RagdollPartEventLinker.html) for damage, grab, or touch based triggers

[^1]: These event linkers have additional parameters in addition to choosing the trigger
[^2]: These event linkers have entirely different trigger definition (No dropdown)
