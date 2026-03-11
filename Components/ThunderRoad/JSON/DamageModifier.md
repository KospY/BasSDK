---
parent: JSON
grandParent: ThunderRoad
---

# Damage Modifiers

A DamageModifier JSON is used to control how [Damagers]({{ site.baseurl }}{% link Components/ThunderRoad/JSON/Damager.md %}) react to different [Material]({{ site.baseurl }}{% link Components/ThunderRoad/JSON/Material.md %}). For instance, one damager could penetrate wood, another would only be able to penetrate stone.

| Keys | Description |
| :--- | :--- |
| id | The unique ID for the damage modifier |
| version | Version of the JSON. Must be **0** |
| damageType | The type of damage this modifier deals to materials, items and enemies <details>- Unknown<br>- Pierce<br>- Slash<br>- Blunt<br>- Energy<br>- Fire<br>- Lightning<br>- UnBlockable<br></details> |

# Collision(s)

| Keys | Description |
| :--- | :--- |
| collisions | A list of every material-based modifier, each collision has a list of modifiers for further modification |

