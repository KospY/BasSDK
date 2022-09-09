# Bow Event Linker
*If you have not done so already, go read the [Event Linker][EventLinker]'s wiki page. This page only lists and explains the events available to this linker. It will not explain how to use the component itself.*

## Overview
The Bow Event Linker listens to relevant events emitted from a specified [Bow String][BowString] component. 

A reference to a [Bow String][BowString] is required in order for this Event Linker to function.  

## Events


| Event                     | Description
| ---                       | ---
| On Arrow Add              | Invoked when an arrow is nocked.
| On Arrow Removed          | Invoked when an arrow is un-nocked.
| On Arrow Fired            | Invoked when the arrow is fired.
| On String Pull Start      | Invoked when the bow string is grabbed. [^1]
| On String Release Start   | Invoked when the bow string is released. [^1]
| On String Release End     | Invoked when the bow string returns to it's default position. [^1] [^2]

-----

[^1]: This event is invoked regardless of if an arrow is currently nocked in the bow.
[^2]: This event is invoked regardless of if the string is still being held.



[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/EventLinker.md %}
[BowString]:    {{ site.baseurl }}{% link Components/ThunderRoad/BowString.md %}