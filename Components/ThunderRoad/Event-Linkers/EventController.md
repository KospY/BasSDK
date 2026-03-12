---
parent: Event-Linkers
grand_parent: ThunderRoad
---
# Event Controller

An Event Controller component is designed to delay and control a Unity event's timing. This is perfect for non-linearly looping environment effects or weapons with custom abilities.

![Inspector]

| Field | Description |
| :--- | :--- |
| beginOnStart | Whether to invoke on start |
| maxInvoke | The maximum amount of times this event controller can be invoked |
| concurentInvokeBehaviour | If this event controller is invoked twice this controls how it behaves <details>- IgnoreIfRunning<br>- StopAndReplace<br>- RunParallel<br></details> |
| loopCount | The amount of times an invoke should loop |
| minDelay | The min delay before invoking |
| maxDelay | The max delay before invoking |
| invokeMaxIndex | Number of different index in case of multiple conditions |
| timedEvent | The event to invoke |

[Inspector]: {{ site.baseurl }}/assets/components/EventController/EventController.png