---
parent: Event Linkers
grand_parent: ThunderRoad
---
# Unity Event Grouper
*If you have not done so already, go read the [Event Linker][EventLinker]'s wiki page. This page only lists and explains the events available to this linker. It will not explain how to use the component itself.*

## Overview
The Unity Event Grouper component allows you to create a collection of events that can be invoked later by other linkers.

This can help you to clean and organize your events, and avoid repeating yourself across many different linkers.

## Usage
All groups of events have a `Name` property, which will be used later to invoke the events inside that group.  
The Unity Event Grouper has an `ActivateNamedEvent` function that can be called by other linkers. This function will find the group with the specified name and invoke its events.


Consider the following example where you have multiple events that invoke the same behaviour:

![Without Event Groups][DisorganizedEvents]

If at any point you decided to change any element of this behaviour, you would have to go through and update each event.  
This can be tedious work, but by using an event group, you can create your behaviour once and invoke it from any number of events:

![With Event Groups][OrganizedEvents]

[EventLinker]:          {{ site.baseurl }}{% link Components/ThunderRoad/Event-Linkers/index.md %}
[DisorganizedEvents]:   {{ site.baseurl }}/assets/components/UnityEventGrouper/Disorganized.jpg
[OrganizedEvents]:   {{ site.baseurl }}/assets/components/UnityEventGrouper/Organized.jpg
