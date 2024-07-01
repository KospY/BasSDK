---
parent: Event-Linkers
grand_parent: ThunderRoad
---
# Event Load Level

Event Load Level is a script used to load different levels, via a scriptable field or event.

![Component][Component]

## Fields

| Field                       | Description
| ---                         | ---
| level Id                    | Insert here what level you want to load.
| Mode Name                   | When loading your level, indicate what gamemode you want to load (CrystalHunt/Sandbox/Survival)
| Level Options               | Allows you to define certain level options when loading, such as Tutorial.
| Fade in Duration            | The duration (in seconds) as to how long it takes for the player screen to fade to the loading screen.
| Loading UI State            | Depicts what loading screen the loader uses. This could depict, for example, the loading screen showing no tips on load.


## Loading the Level

To load the level via events, see [Event Linkers][EventLinker] and [Unity Event Documentation](https://docs.unity3d.com/Manual/UnityEvents.html)

To utilise level loading, you can reference the EventLoadLevel, and use the "LoadLevel" event.


[EventLinker]:  {{ site.baseurl }}{% link Components/ThunderRoad/Event-Linkers/index.md %}
[Component]: {{ site.baseurl }}/assets/components/EventLoadLevel/Component.png
[Event]: {{ site.baseurl }}/assets/components/EventLoadLevel/Event.png