# Holder Event Linker
*(If you have not yet already done so, go read the [event linkers][EventLinker] wiki page! This page only lists and explains the event trigger options on this event linker! It will not explain how to use the event linker.)*
[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/EventLinker.md %}

Holder event linkers can be attached to Holder components of any type; rack holders, quivers, sheathes, or body holsters are all applicable holders to attach this linker to. There are only four events for this event linker:
- **OnSnap** triggers the event when an item is added to the holder *(But only if the holder is not filled!)*
- **OnSnapFull** triggers the event when an item is added to the holder and fills it entirely
- **OnUnsnap** triggers when any item is removed from the holder *(But only if the holder is not empty!)*
- **OnUnsnapEmpty** triggers when an item is removed and leaves the holder with no items remaining in it

For sheathes, holsters, and rack holders, generally you'll only be using **OnSnapFull** or **OnUnsnapEmpty**. Quivers are the only items where you may be able to utilize **OnSnap** and **OnUnsnap**.
