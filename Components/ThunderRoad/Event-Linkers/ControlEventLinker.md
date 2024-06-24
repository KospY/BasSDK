---
parent: Event Linkers
grand_parent: ThunderRoad
---
# Player Control Event Linker
*If you have not done so already, go read the [Event Linker][EventLinker]'s wiki page. This page only lists and explains the events available to this linker. It will not explain how to use the component itself.*

## Overview
Control event linkers are used to activate events when the player presses certain buttons in-game. 

This event linker can be used in conjunction with other event-based components to create more complex behaviour. For example, toggling this linker using a [Zone][Zone] component would allow you to only listen for inputs when a player is within a certain area.  

## Events
Some of the below events have variants for if they were invoked by the left or right hand. They have been combined on this wiki page to avoid confusion.

| Event                             | Description
| ---                               | ---
| On Jump (Press/Release)           | Invoked when the jump button is pressed/released. [^1]
| On Kick (Press/Release)           | Invoked when the kick button is pressed/released.
| On Use (Press/Release)            | Invoked when the spell-cast button is pressed/released.
| On Grip (Press/Release)           | Invoked when the grab-object button is pressed/released.
| On Alternate-Use (Press/Release)  | Invoked when the alternate-use button is pressed/released. [^2]

-----

[^1]: `On Jump` is also triggered when jumping by pushing the thumbstick up. 
[^2]: `Alternate-Use` refers to the button for opening the spell wheel, however this input varies platform to platform.



[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/Event-Linkers/index.md %}
[Zone]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/Zone.md %}