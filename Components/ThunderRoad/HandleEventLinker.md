# Handle Event Linker
*If you have not done so already, go read the [Event Linker][EventLinker]'s wiki page. This page only lists and explains the events available to this linker. It will not explain how to use the component itself.*

## Overview
The Handle Event Linker listens for events emitted from a specific [Handle][Handle] component.

A reference to a [Handle][Handle] component is required in order for this Event Linker to function.  

## Events

| Event                                         | Description
| ---                                           | ---
| On Grab(/Release)                             | Invoked when the handle has been (grabbed/dropped) by a creature
| On Throw                                      | Invoked when the handle is released with substantial velocity.
| On Slide (Start/End)                          | Invoked when the holder (starts/stops) sliding along the handle axis.
| On Slide To (Up/Bottom) Handle                | Invoked when the holder reaches the (top/bottom) of the handle while sliding.
| On Grabbed Use (Press/Release)                | Invoked when the spell-cast/trigger button has been (pressed/released) while the handle is held. 
| On Grabbed Alternate Use (Press/Release)      | Invoked when the alternate-use[^1] button has been (pressed/released) while the handle is held.
| On Non Grabbed Use (Press/Release)            | Invoked when the spell-cast/trigger button has been (pressed/released) while the handle is not held.
| On Non Grabbed Alternate Use (Press/Release)  | Invoked when the alternate-use[^1] button has been (pressed/released) while the handle is not held.
| On Tele Grab(/Release)                        | Invoked when the handle is (grabbed/released) using telekinesis.
| On Tele Throw                                 | Invoked when the handle has been launched using telekinesis.
| On Tele Spin Start (Success/Fail)             | Invoked when the telekinesis spin action (succeeds/fails) to be started on the held handle.
| On Tele Spin End                              | Invoked when the handle stops being spun using telekinesis.
| On Tele Repel (Start/End)                     | Invoked when the handle (starts/stops) being pushed away using telekinesis.
| On Tele Pull (Start/End)                      | Invoked when the handle (starts/stops) being pulled closer using telekinesis.

-----

[^1]: `Alternate-Use` refers to the button for opening the spell wheel, however this input varies platform to platform.

[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/EventLinker.md %}
[Handle]:  {{ site.baseurl }}{% link Components/ThunderRoad/Handle.md %}