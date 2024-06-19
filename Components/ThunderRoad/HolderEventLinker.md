---
parent: ThunderRoad
---
# Holder Event Linker
*If you have not done so already, go read the [Event Linker][EventLinker]'s wiki page. This page only lists and explains the events available to this linker. It will not explain how to use the component itself.*

## Overview
The Holder Event Linker listens for events emitted from a specific [Holder][Holder] component.


## Events
For sheaths, holsters, and rack holders, you'll likely only be using `On Snap Full` or `On Unsnap Empty`.   
Quiver holders are the only holders where you may be able to utilize `OnSnap` and `On Unsnap`.

| Event             | Description
| ---               | ---
| On Snap           | Triggers the event when an item is added to the holder *(But only if the holder is not filled!)*
| On Snap Full      | Triggers the event when an item is added to the holder and fills it entirely
| On Unsnap         | Triggers when any item is removed from the holder *(But only if the holder is not empty!)*
| On Unsnap Empty   | Triggers when an item is removed and leaves the holder with no items remaining in it





[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/EventLinker.md %}
[Holder]:  {{ site.baseurl }}{% link Components/ThunderRoad/Holder.md %}