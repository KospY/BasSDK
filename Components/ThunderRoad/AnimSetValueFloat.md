---
parent: ThunderRoad
---
# Anim Set Value Float
*This is an animation component, meaning it can only be added to **Animation States**. This component cannot be added to regular GameObjects.*

## Overview
This animation component allows you to modify any [Animation Parameter][AnimationParameter] when the state is entered, or exited.

There is a seperate component for each parameter data type. You can find these here:

| Type      | Component
| ---       | ---
| Boolean   | [Anim Set Value Bool][AnimSetValueBool]       
| Float     | [Anim Set Value Float][AnimSetValueFloat]     
| Integer   | [Anim Set Value Integer][AnimSetValueInteger]


## Component Fields

| Field         | Description
| ---           | ---
| Parameter     | The name of the parameter to update.
| Value         | The new value for the parameter.
| Transition    | Determines whether the value should be set when the state is entered, or exited.


[AnimationParameter]: https://docs.unity3d.com/Manual/AnimationParameters.html
[AnimSetValueBool]: {{ site.baseurl }}{% link Components/ThunderRoad/AnimSetValueBool.md %}
[AnimSetValueFloat]: {{ site.baseurl }}{% link Components/ThunderRoad/AnimSetValueFloat.md %}
[AnimSetValueInteger]: {{ site.baseurl }}{% link Components/ThunderRoad/AnimSetValueInteger.md %} 