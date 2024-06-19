---
parent: ThunderRoad
---
# Disable on Platform

Disable on Platform, also known as `Disable or Strip GameObject relative to platform` is an internal script which is used to strip the GameObject when a tool is used to create an android varient.

![StripOnPlatform][StripOnPlatform]

| Field                       | Description
| ---                         | ---
| Platform Filter             | Depicts whether the gameobject is "`Only on` Platform" or "`Exclude from` Platform.
| Platform                    | Select platform to Exclude/Only include on. Choices are Windows (PCVR) and Android (Quest 2).
| Allow Strip                 | Toggle to Strip or Not, without removing script.

```danger
This is an internal tool, and cannot be used by modders without their own export tooling.
```

[StripOnPlatform]: {{ site.baseurl }}/assets/components/DisableOnCondition/DisableOnPlatform.PNG