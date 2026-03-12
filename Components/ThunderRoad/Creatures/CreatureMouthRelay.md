---
parent: Creatures
grand_parent: ThunderRoad
---

# Creature Mouth Relay

{: .important}
This component needs a trigger Collider and kinematic Rigidbody on the same gameObject to function!

The Creature Mouth Relay is a component that detects when food items and liquids come into contact with it, this is used so that you and other creatures can drink potions, eat apples and burn people with Sentari concoctions.

![Inspector]

| Field | Description |
| :--- | :--- |
| mouthRadius | How big is the detection radius? |
| isMouthActive | Can this mouth receive food/liquid? |
| playerOnly | If enabled this relay will only be active if the current creature is the player. |

[Inspector]: {{ site.baseurl }}/assets/components/CreatureMouthRelay/CreatureMouthRelay.png