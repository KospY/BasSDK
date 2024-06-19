---
parent: ThunderRoad
---
# Damager Event Linker
*If you have not done so already, go read the [Event Linker][EventLinker]'s wiki page. This page only lists and explains the events available to this linker. It will not explain how to use the component itself.*


## Overview
The Damager Event Linker listens to relevant events emitted from a specified [Damager][Damager] component. 

A reference to a [Damager][Damager] is required in order for this Event Linker to function.  

## Events 

| Event             | Description
| ---               | ---
| On Damage Dealt   | Triggered when the linked damager inflicts damage on any **living or non-living** creature or player. 
| On Kill Dealt     | Triggered when the linked damager kills a creature.

<br>

```note
`On Damage Dealt` will still be invoked when the linked damager kills a creature.
```




[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/EventLinker.md %}
[Damager]:      {{ site.baseurl }}{% link Components/ThunderRoad/Damager.md %}