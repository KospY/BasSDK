---
parent: Event-Linkers
grand_parent: ThunderRoad
---
# Collision Events Bridge

{: .note}
This component requires a `Collider` and a `Rigidbody` somewhere in the hierarchy!

The Collision Events Bridge is a component that allows you to detect when a collider comes into contact with a surface. As long as the gameObject has a `Rigidbody` and `Collider` on the same gameObject (or a parent), the events will be invoked.

![Inspector]

| Event | Description |
| :--- | :--- |
| onCollisionEnter | Invoked when this collider comes into contact with another |
| onCollisionExit | Invoked when this collider stops contacting another |
| onCollisionStay | Invoked as long as this collider is in contact with another |

[Inspector]: {{ site.baseurl }}/assets/components/CollisionEventsBridge/CollisionEventsBridge.png