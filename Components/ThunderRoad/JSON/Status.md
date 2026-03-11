---
parent: JSON
grandParent: ThunderRoad
---


## Statuses

A Status JSON is used to create a customisable, moddable and performant status effect.

| Main | Secondary |
| :--- | :--- |
| id | The unique ID for the status |
| version | Version of the JSON. Must be **0** |

## Status

| Main | Secondary |
| :--- | :--- |
| type | The loaded and inflicted type, loaded by StatusData and created each time `Inflict()` is called |
| bypassImmunity Bypasses an entity's potential immunity \[Flags\] | <details>- Human<br>- Animal<br>- Golem<br>- Other<br></details> |
| stackType | Controls how the status stacks <details>- Refresh<br>- Stack<br>- None<br>- Infinite<br></details> |

## Conditions

| Main | Secondary |
| :--- | :--- |
| forceAllowOnCreatures | If true, this status is forcably applied to every creature |
| allowOnItems | If true, this status is forcably applied to every item |

## Effect

| Main | Secondary |
| :--- | :--- |
| creatureEffectId \[Dropdown\] | The ID of the [Effect]({{ site.baseurl }}{% link Components/ThunderRoad/JSON/Effect.md %}) this status spawns |
| itemEffectId \[Dropdown\] | The ID of the [Effect]({{ site.baseurl }}{% link Components/ThunderRoad/JSON/Effect.md %}) this status spawns |

