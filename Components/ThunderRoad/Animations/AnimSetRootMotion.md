---
parent: Animations
grand_parent: ThunderRoad
---
# Anim Set Root Motion
*This is an animation component, meaning it can only be added to **Animation States**. This component cannot be added to regular GameObjects.*

## Overview

This animation component will set the ["Apply Root Motion"][ApplyRootMotion] property on the animator component to the given value.

Unlike the [Anim Clip Root Motion][AnimClipRootMotion] component, this value will persist even after the controller moves to a new state.

## Component Fields

| Field         | Description
| ---           | ---
| State         | Determines if the `Apply Root Motion` property should be toggled on or off.
| Transition    | Sets whether the change is applied when the state is entered, or exited.


[ApplyRootMotion]: https://docs.unity3d.com/ScriptReference/Animator-applyRootMotion.html
[AnimClipRootMotion]: {{ site.baseurl }}{% link Components/ThunderRoad/Animations/AnimClipRootMotion.md %}