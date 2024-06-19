---
parent: ThunderRoad
---
# Hinge Drive

```note
Not to be confused with HingeJoint, however the Hinge Drive does generate a HingeJoint on playmode
```

The HingeJoint script is a component which creates a [HingeJoint] on play, and has many features in addition to help with stylised dynamic objects. It inclused features such as a motors, latches and [UnityEvents].

## Component Properties

| Field                                       | Description
| ---                                         | ---
| **World References**
| Hinges Holder                               | The reference inserted is the holder for the Hinge Joint, when it is generated at playtime.
| Frame                                       | The reference transform links the hinge joint to it.
| Hinge Targets                               | This list depicts the transforms that are used as targets for defining the hinge anchors in the scene.
| Colliders to Ignore                         | This list references colliders to which the object assigned to the Hinge Joint ignores.
| Handles                                     | This references the [Handle] attached to the object with the Hinge Joint. These handles are used for latches.
| **Angles config values**
| Rotation Axis                               | This defines the axis of which the hinge rotates
| Is Limited                                  | When disabled, this object ignores the minimum/maximum axis, and are able to move in full 360 degrees, and will not stop once the maximum angle is reached.
| Default Angle                               | This angle represents the angle of which the hinge starts/defaults at when in play.
| Min Angle                                   | This angle represents the minimum rotation for the hinge.
| Max Angle                                   | This angle represents the maximum rotation for the hinge.
| **Misc config values
| Enable Collision with Frame                 | When enabled, the hinge drive object can collide with the frame.
| **Latch config values**
| Use Latch                                   | When enabled, the Hinge Drive can be locked, and opened via the latch input.
| Allowed Inputs to Open Latch                | List of inputs that allow the player to unlock the latch.
| Latch Angles                                | List of angles of which the latch will lock on to
| Is Latch Brute Forceable?                   | When enabled, the latch can be brute forced open, when enough impulse is applied.
| Force Latch Opening Impulse Threshold       | Impulse threshold in which the latch is brute forced.
| Is Latch Breakable                          | When enabled, the latch can be broken
| Latch Health                                | The health of the latch. When this value reaches 0, the latch breaks.
| **Motor config values**
| Use Motor                                   | When enabled, motor becomes active. The motor rotates the hinge in perpetual motion. If the hinge is limited, it will instead rotate from minimum angle to maximum angle (unless motor force is negative), but when not limited, it will rotate in perpetual motion, like a wheel.
| Motor Target Velocity                       | This value determines the desired velocity that the motor rotates at.
| Motor Force                                 | This value determines the desired force that the motor rotates at.
| **Auto Close Config Values**                
| Angle Limits Bounceiness                    | Makes the hinge bounce (or not) when it is opened fully.
| Damper                                      | This damper prevents the hinge from moving very slowly for a long time.
| Auto Closes                                 | When enabled, the hinge will return to its resting angle.
| Resting angle                               | Angle of which the hinge rests at.
| Auto Close Spring                           | Force of the spring that pulls the hinge to its resting position.
| **Haptic config values**                     
| Enable Continuous Haptic on Move            | When enabled, when grabbed, the hinge handle will vibrate the player's controllers when the hinge moves/rotates.
| Use Speed Factor                            | When enabled, the speed affects the vibration when the hinge moves.
| Speed Factor (Curve)                        | This curve determines the intensity of the vibration based on the speed factor.
| Continuous Haptic Amplitude                 | Determines the intensity of the continuous vibration.
| Enable Haptic Angle Bump                    | When enabled, the grabbed handle will vibrate each time it reaches the threshold/desired angle.
| Angle Step for Bumps                        | Determines the angle of which the handle will vibrate. (In degrees)
| Angle Step Haptic Amplitude                 | Determines the intensity of the bump vibration.
| **Autio Config Values**                     | **These inputs require a component which utilises the [FxModule] component.**
| Soothing Samples                            | Determines how smooth the looping sound effects are. The higher the number, the smoother the sound effect.
| Effect Audio Hinge Moving Positive          | FX plays when the hinge moves from minimim to maximum angle.
| Effect Audio Hinge Moving Negative          | FX plays when the hinge moves from maximum to minimum angle.
| Effect Audio Slam                           | FX plays when the hinge is slammed with high velocity, in which the latch locks the hinge drive.
| Effect Audio Latch Lock                     | FX plays when the hinge latch is locked.
| Effect Audio Latch Unlock                   | FX plays when the hinge latch is unlocked.
| Effect Audio Latch Break                    | FX plays when the hinge latch is broken.
| Effect Audio Wiggle                         | FX plays when the hinge hits its minumum OR maximum angle.
| Effect Audio Latch Button Press             | FX plays when the hinge latch is grabbed and the Open Input is a button press.
| Effect Audio Latch Button Release           | FX plays when the hinge latch is grabbed and the Open Input is let go.
| **Auto Open Config Values                  
| Auto Open Fall Back Velocity                | Calling any automatic method without a velocity being given to the script will use this value as a fallback.
| Auto Open Fall Back Force                   | Calling any automatic method without a force being given to the script will use this value as a fallback.
| Auto Open Bypass Latch                      | Calling any automatic method without a given force will bypass the latch to open.

### Events
The HingeJoint component has a number of UnityEvents that are invoked when the conditions are met.   
For more about UnityEvents and how to use them, refer to the official [Unity Documentation][UnityEvents].

| Event                                       | Description
| On Hinge Move                               | Event will play when the hinge moves.
| On Latch Lock                               | Event will play when the hinge latch is locked.
| On Latch Unlock                             | Event will play when the hinge latch is unlocked.
| On Player Pressing Latch Button             | Event will play when the hinge latch is grabbed by the player, and the latch input is pressed.
| On Player Releasing Latch Button            | Event will play when the hinge latch, which is grabbed by the player, is released.
| On Latch Break                              | Event will play when the hinge latch is broken
| On Hinge Hit Threshold                      | Event will play when the hinge hits its velocity threshold (?)

![HingeDrive][HingeDrive]

### Gizmo

```Tip
This component has a gizmo which easily displays angles and axis, while also being configurable for easy assistance
```

![HingeDriveGizmo]

| This is the gizmo for the HingeDrive. 
| The Semi-circle depicts the Minimum and Maximum angles of the HingeDrive, with the blue part being the minimum and the orange part being the maximum angle. 
| The Red Line depicts the "Default Angle" of the HingeDrive, while the White arrow depicts the hinge origin point (zero)
| The light blue line in the middle of the hinge depicts the "Latch Angles". There can be multiple of these.


[HingeJoint]: https://docs.unity3d.com/Manual/class-HingeJoint.html
[UnityEvents]: https://docs.unity3d.com/Manual/UnityEvents.html
[Handle]: {{ site.baseurl }}{% link Components/ThunderRoad/Handle.md %}
[FxModule]: {{ site.baseurl }}{% link Components/ThunderRoad/FxModule.md %}

[HingeDrive]: {{ site.baseurl }}/assets/components/HingeDrive/HingeDrive.PNG
[HingeDriveGizmo]: {{ site.baseurl }}/assets/components/HingeDrive/HingeDriveGizmo.PNG