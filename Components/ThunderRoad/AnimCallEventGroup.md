# Anim Call Event Group
*This is an animation component, meaning it can only be added to **Animation States**. This component cannot be added to regular GameObjects.*

## Overview
This animation component allows you to invoke an [Event Group][UnityEventGrouper] when the state is entered or exited.

For this component to function, the Unity Event Group component must be attached to the same object your Animator is attached to.

## Component Fields

| Field         | Description
| ---           | ---
| Event Name    | The `Name` of the Unity Event Group to invoke.
| Transition    | Sets whether the event group is invoked when the state is entered, or exited.

[UnityEventGrouper]: {{ site.baseurl }}{% link Components/ThunderRoad/UnityEventGrouper.md %}
