# Control Event Linker
*(If you have not yet already done so, go read the [event linkers][EventLinker] wiki page! This page only lists and explains the event trigger options on this event linker! It will not explain how to use the event linker.)*
[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/EventLinker.md %}

Control event linkers are used to activate events when the player presses certain buttons in-game. Using this, you could create a map where every time the player presses the jump button, a Mario jump sound is played. A list of all control triggers you can link to can be found below. All of the trigger options for this event linker are pretty self-explanatory
- **Jump controls:** You can link events to **jump press** and **jump release**. The actual button this corresponds to depends on user headset and controllers, and includes also the "joystick up" jump control.
- **Kick controls:** Much like jump controls, the actual button this is connected to depends on a user's device. You can link to **kick press** and **kick release**
- **Button controls:** All other triggers other than the four listed above are button controls. You can link events to button triggers on either hand (All options include **Right hand** or **Left hand** as part of their name), and your options are the "consistent" choices across all devices: **Use (press & release)** (Typically the trigger button), **Alt use (press & release)** (Typically the spell wheel button), and **Grip (press & release)** (This button is consistent across all devices).
