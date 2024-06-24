---
parent: Levels
grand_parent: ThunderRoad
---
# Toggle Objects

Toggle Objects is a script which makes it an easy function to disable a mass amount of GameObjects and Renderers stored in one script, to allow for one reference to disable many game objects in an [UnityEvents][UnityEvents] Call. 

![ToggleObjects][ToggleObjects]

## Components

| Field                       | Description
| ---                         | ---
| Renderers                   | Reference Mesh and Skinned Mesh Renderes here to Toggle. Will not toggle GameObject, just the renderer itself.
| Behaviours                  | References scripts and behaviors. Will toggle scripts and behaviours but not the GameObject.
| Game Objects                | References GameObjects. Will completly toggle GameObjects.
| Update on Android Export    | Used Internally [Should Be Removed].

Using a [Unity Event][UnityEvents], such as Event Linkers, you can use the ToggleObjects(bool) to enable/disable the referenced GameObjects

![ToggleObjectsUnityEvent][ToggleObjectsUnityEvent]

[ToggleObjects]: {{ site.baseurl }}/assets/components/ToggleObjects/ToggleObjects.PNG
[ToggleObjectsUnityEvent]: {{ site.baseurl }}/assets/components/ToggleObjects/UnityEvent.PNG
[UnityEvents]: https://docs.unity3d.com/Manual/UnityEvents.html