# Collision Event Linker
The Collision event linker is attached to a collider group, and will *only* function if that collider group is part of something which has a rigidbody (Such as items, ragdoll parts, etc.) Any collisions encountered by that object will check if they involve the collider group associated with this collision event linker. If you wish to detect any collisions on an item, ragdoll part, creature, etc., the Item Event Linker, Ragdoll Part Event Linker, and Creature Event Linkers have triggers you can use to activate events on any collision for any part of them.

- **OnCollisionEnter** triggers when the collider group this is attached to enters *any* collision. This one always activates whether or not the collision deals damage!
- **OnHarmlessCollision** triggers only when the collider group enters a collision which deals no damage.
- **OnCollisionExit** triggers when the collider group exits any collision. No damage information is available for this event.
- **OnDamageDealt** triggers when the collider group is involved in a collision which deals damage.
- **OnKillDealt** triggers when the collider group is involved in a collision which deals fatal damage to a creature.
