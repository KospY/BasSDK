# Damager Event Linker
*(If you have not yet already done so, go read the [event linkers][EventLinker] wiki page! This page only lists and explains the event trigger options on this event linker! It will not explain how to use the event linker.)*

The Damager Event Linker is attached to a specific damager, and will only detect damage and kills inflicted by that damager specifically. There are only two triggers for this event linker:
- **OnDamageDealt** triggers only when the damager this is linked to deals damage. This event can trigger even if a creature is dead!
- **OnKillDealt** triggers only when the damager kills a creature. This can trigger at the same time as OnDamageDealt!




[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/EventLinker.md %}