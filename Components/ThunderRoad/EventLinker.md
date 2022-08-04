# Event Linkers
Event linkers are a set of utility scripts that allow you to link together in-game events to activate functions on the components you add to items, maps, or creatures within Unity. All event linkers have a handful of common functions that you can activate which afford you even greater control over the control of your item(s).

Every event linker has the same standard "structure" with mild alterations: You define a list of events (For which you configure the trigger) which give you a **Unity event** to drag/drop GameObjects into in order to invoke **methods** from the components added to that GameObject. You can (and may even *need* to) make events with duplicate triggers: The execution order of these events matters!

Using event linkers, you can make items perform complex actions such as playing animations, particles, sounds, etc., introduce puzzle mechanics to your levels, or create custom humanoids with new additions which otherwise wouldn't be possible without writing code. **Event linkers work on both PCVR and Nomad!**

For more specific information regarding each type of event linker and the triggers you can link to, please navigate to its associated wiki page.

## Common methods
The below methods can all be invoked by Unity events, and provide extra functionality that would otherwise be missing.
### SetListen(bool)
`SetListen(bool)` toggles whether or not the event linker is currently "listening". If an event linker is not listening, its events will not trigger even when the associated in-game events occur. This allows you to change what behaviour is taken when in-game events occur, and may be useful depending on your use case.
### PrintDebug(string)
`PrintDebug(string)` prints a message to the debug log, in a style dependent on how many `!`s you start the log with: Starting with no `!`s means it's a normal log, one `!` means it's a warning log, and two `!!` means it's an error log. These may be useful to you if you're trying to figure out why your item isn't behaving as intended.
### WaitFor\_\_\_\_\_\_(int or float)
`WaitForFixedFrames(int)`, `WaitForFrames(int)`, `WaitForSeconds(float)`, and `WaitForRealtimeSeconds(float)` all add some kind of delay to your event linker. **!! NOTE: these do not work within the same event! You need to have events with duplicate triggers for these to do anything!** With `WaitForFixedFrames`, you wait for that many "physics frames" (Which are different from render frames), with `WaitForFrames`, you're waiting for real rendered frames. Similar logic applies for `WaitForSeconds` and `WaitForRealtimeSeconds`; Realtime seconds means real time seconds, while normal `WaitForSeconds` is in-game time (Meaning it is affected by slow motion, while real seconds are not!)
