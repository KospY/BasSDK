---
parent: ThunderRoad
---
# Collision Event Linker
*If you have not done so already, go read the [Event Linker][EventLinker]'s wiki page. This page only lists and explains the events available to this linker. It will not explain how to use the component itself.*

## Overview
The Collision Event Linker listens for events emitted from a specific [Collider Group][ColliderGroup].

This event linker will only respond to collisions with the specified collider group. See one of the following pages if you wish to detect **all** collisions for:
- **Items:** [Item Event Linker][ItemEventLinker]
- **Creatures:** [Creature Event Linker][CreatureEventLinker], [Ragdoll Event Linker][RagdollEventLinker]

A reference to a [Collider Group][ColliderGroup] component is required in order for this Event Linker to function.  

{: .danger}
The linked collider group must have a rigidbody component on itself or one of its parent objects. Otherwise, this event linker will not function.


## Events

| Event | Description
| --- | ---
| On Collision (Enter/Exit) | Triggers when somethings (starts/stops) touching the collider group, regardless of if damage is dealt.
| On Harmless Collision | Triggers when the collider group experiences a collision which deals no damage.
| On Damage Dealt | Triggers when the collider group is involved in a collision which deals damage.
| On Kill Dealt | Triggers when the collider group is involved in a collision which deals fatal damage to a creature.




[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/EventLinker.md %}
[ColliderGroup]: {{ site.baseurl }}{% link Components/ThunderRoad/ColliderGroup.md %}
[ItemEventLinker]: {{ site.baseurl }}{% link Components/ThunderRoad/ItemEventLinker.md %}
[RagdollEventLinker]: {{ site.baseurl }}{% link Components/ThunderRoad/RagdollPartEventLinker.md %}
[CreatureEventLinker]: {{ site.baseurl }}{% link Components/ThunderRoad/CreatureEventLinker.md %}