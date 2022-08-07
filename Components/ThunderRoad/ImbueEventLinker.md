# Imbue Event Linker
*(If you have not yet already done so, go read the [event linkers](https://kospy.github.io/BasSDK/Components/ThunderRoad/EventLinker.html) wiki page! This page only lists and explains the event trigger options on this event linker! It will not explain how to use the event linker.)*

The imbue event linker attaches to a collider group, and its events are triggered by the imbue state of that collider group. With this event linker, you can't differentiate between imbue spells, but if you needed to, you could use the [spell touch event linker](https://kospy.github.io/BasSDK/Components/ThunderRoad/SpellTouchEventLinker.html) to enable/disable this event linker depending on the spell touching this collider group. The following events are available through the imbue event linker:
- **OnEmpty** triggers when the imbue fully runs out on the collider group
- **OnNewSpell** triggers when this collider group is empty and begins to be imbued
- **OnFill** triggers when the imbue reaches its max energy
- **OnTryUse** triggers when the player tries to (but cannot) "fire" a crystal-type collider group
- **OnUseAbility** triggers when the player successfully "fires" a crystal-type collider group
- **OnHit** triggers when the imbue hits something but has no effect on it
- **OnHitEffect** triggers when the imbue hits something and has an effect on it (Such as anti-gravity, or a staff slam)
