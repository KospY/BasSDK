# Ragdoll Part Event Linker
*If you have not done so already, go read the [Event Linker][EventLinker]'s wiki page. This page only lists and explains the events available to this linker. It will not explain how to use the component itself.*

## Overview
The Ragdoll Part Event Linker listens for events emitted from a specific item.

A reference to a [Ragdoll Part][RagdollPart] component is required in order for this Event Linker to function.

## Events

| Event                             | Description
| ---                               | ---
| On Damage                         | Invoked when this ragdoll part takes damage.
| On Kill                           | Invoked when the ragdoll part's creature dies.
| On Touch (Start/End)              | Invoked when a collision that deals damage acts on this part.
| On Touch No Damage (Start/End)    | Invoked when a collision that deals no damage acts on this part.
| On Grab(/Ungrab)                  | Invoked when this part has been (grabbed/released) by another creature.
| On Tele Grab(/Ungrab)             | Invoked when this part has been (grabbed/released) using telekinesis.
| On Tele Repel (Start/End)         | Invoked when this part (starts/stops) being pushed away using telekinesis.
| On Tele Pull (Start/End)          | Invoked when this part (starts/stops) being pulled closer using telekinesis.
| On (Pre/Post) Slice               | Invoked (before/after) this ragdoll part has been severed.




[EventLinker]: {{ site.baseurl }}{% link Components/ThunderRoad/EventLinker.md %}
[RagdollPart]: {{ site.baseurl }}{% link Components/ThunderRoad/RagdollPart.md %}