---
parent: Levels
grand_parent: ThunderRoad
---
# Player Lock

{: .danger}
This component's GameObject should be disabled by default, and only enabled via an event. If not done so, player will be locked no matter what. You can also disable the lock via the event "PlayerLock.Unlock" and lock it with "PlayerLock.Lock".

The Player Lock script is a component that will lock player actions depending on what you enable. This is used for camera scenes like Cutscenes, but can also be used to remove the ability for the player to move or disable spell casting for the player.

{: .information}
This Script utilises Unity Events.
[Unity Event Documentation](https://docs.unity3d.com/Manual/UnityEvents.html){: .btn .btn-purple }

![Component][Component]

## Fields

| Field                       | Description
| ---                         | ---
| Disable Locomotion          | Disables player locomotion when locked.
| Disable Move                | Disables player-controlled movement when locked.
| Disable Turn                | Disables player turning when locked.
| Disable Jump                | Disables player being able to jump when locked.
| Disable Slow Motion         | Disables slow-motion and Hyperfocus when locked.
| Disable Casting             | Disables spell-casting when locked.
| Invincible                  | When locked, player is invincible.
| Disable Player Camera       | Disables player camera when locked. This is not recommended unless you plan to use other cameras (e.g. Cutscenes).
| Disable Options Menu        | Disables opening the Options Menu when locked.

## Events

| Event                       | Description
| ---                         | ---
| Trigger Button              | This field will allow the events to play, playing the event when the button set is pressed
| On Button Press ()          | Plays the event when the button on "Trigger Button" is pressed when the player is locked.

[Component]:{{ site.baseurl }}/assets/components/PlayerLock/Component.png