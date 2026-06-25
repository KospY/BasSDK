---
parent: Creatures
grand_parent: ThunderRoad
---
# Creature Eye

The Creature Eye component can be used to make eyes blink, close and look around. 

![Inspector]

| Field | Description |
| :--- | :--- |
| closeAmount | The amount the eye should close, this is evaluated by each close curve in every Eye Part |
| eyeTag | The tag for the eye (e.g., **Left**, **Right**) |
| eyeParts | A list of Eye Parts referencing the eye lid of a rig |

# Eye Part

| Field | Description |
| :--- | :--- |
| name | The name of this part of the eye (e.g., **UpperLid**, **"LowerLid"**) |
| transform | The bone this eye part controls |

# Rotation Curves

| Field | Description |
| :--- | :--- |
| closeCurveX |  |
| closeCurveY |  |
| closeCurveZ |  |
| closeCurveW |  |



[Inspector]: {{ site.baseurl }}/assets/components/CreatureEye/CreatureEye.png