---
parent: Items
grand_parent: ThunderRoad
---
# Bow String
###### This component has an [Event Linker][EventLinker].
[Unity Event Documentation](https://docs.unity3d.com/Manual/UnityEvents.html){: .btn .btn-purple }

# Bow String

The Bow String component is used to attach and configure the string of a bow to the bow itself. It only supports items, and can’t be used if there’s no item that it’s being attached to.

## Usage

1. Create your bow as an [Item][Item] as you normally would. 
2. Somewhere on your bow, you need to have an animation that plays when it pulls back. This **must** be an **Animation** (not Animator!) component, which means that your animation clip (in the below image this is the **LongBowDraw** clip) has to be marked as legacy! If it’s not, the BowString script will automatically update it when you change any values in the BowString script.
    
    ![BowAnimation][BowAnimation]
    
3. Set up your string handle with all of the components shown on the left; some of them will be added automatically for you! The rigidbody on your bow should have a low mass (Our default mass is 0.1)
    
    ![HandleScripts][HandleScripts]
    
4. Be sure to assign the Custom Rigid Body to the Rigidbody which is on the string handle! If you don’t, this script will not work!
    
    ![CustomRigidbody][CustomRigidbody]
    
5. Your capsule collider should have Is Trigger checked, and should cover much of the string; this is the zone that detects when your hand touches the string while holding an arrow.
    
    ![BowCapsule][BowCapsule]
    
6. Once your other components are set up, it’s time to work on configuring your bowstring to match what you need. A table with information about all of these fields can be found below. It’s possible you will only need to change a few of these values to suit your needs! You will need to drag and drop the **Animation**, **rest left**, **rest right**, **audio containers**, and **audio clip**.
    1. At this point, if you would like to use the auto configure bow wizard to set up the exact draw length and perfect pull curve, you can! Take a look in the editor tools section below to learn more
7. After configuring the BowString script, you can use the tools under the “Editor only” header to check the bow’s functionality before exporting! A table with information about the functionality of all of these editor tools can be found below.

## Gizmos

The BowString component shows useful information about your bow through its Gizmos. It has two primary gizmos it uses for this:

![BowGizmo][BowGizmo]

| Gizmo | Information |
| --- | --- |
| String draw visualizer | This is the line that goes from the string handle backwards in the direction the bow draws in. It shows you your bow’s string draw details.

The tall white line at the end indicates the furthest draw length the bow can reach.

The smaller pink/purple line indicates the current “test pull ratio”; if your bow is set up correctly, you should see that the pink line is always exactly where your bow string mesh is! If there’s some distance between the pink line and the bow string mesh, you may need to adjust you Pull Curve.

The color of the line indicates the “pull difficulty”, where green is “easy”, yellow is “tougher” and red is “impossible.” If you have issues with your arrow falling off of your bow as you draw it, you may need to adjust the Pull Difficult By Draw curve such that the bow can’t be drawn to that length. |
| Arrow length visualizer | This is the capsule which gets drawn from the test pull ratio line (the pink line described above) out to in front of the bow. It shows an “arrow” of the length defined by the Debug Arrow Length field.

When the arrow capsule color is green, this means that the arrow would not fall out of the bow. When it’s red, it indicates that the arrow would fall out of the bow. This won’t display if the Rest left and Rest right fields aren’t set. |

## Properties

Just about all of the below properties have tooltips that pop up when you hover over their names; if you ever need a quick clarification, give that a go!

![BowProperties][BowProperties]

| Property | Description | Default value |
| --- | --- | --- |
| Animation | The Animation component you use to animate your bow. | None |
| Pull curve | This curve defines the relationship between the “pull percentage” of the bow and the normalized time of the animation. Manually configuring this is tough! It’s recommended that you give the auto-configure tool a try, or simply leave it set to its default value. | Straight line from a key at 0, 0, to a key at 1, 1 |
| String draw length | How far your bow string can be pulled back. As you update this value, the configurable joint on the bow string will be updated as well! No more fussing with configurable joints in confusion. | 0.5 |
| Min fire velocity | The minimum required velocity for an arrow to be fired from a bow. If the arrow isn’t moving at this speed or higher, it will stay on the bow when you let go of the string. | 4 |
| Min pull | Defines the bare minimum percentage you need to pull the bow string back for the animation to update. Goes from 0 (0%) to 1.0 (100%) | 0.01 |
| Pull difficulty by draw | How hard the bow is to pull as you pull it back more. Along the curve, anything under a value of 0.5 is “easy to pull”, anything over 0.5 is “harder to pull”, and values of 1 and higher mean “the player can’t pull it past this point” | Default bezier curve from a key at 0, 0, to a key at 1, 1 |
| Rest left/Rest right | These transforms define the point on the bow the arrow passes through when it’s rested on the bow, depending on which hand the player is holding the bow with. If the arrow ever goes further back than this point, it falls out of the bow. | None |
| String always grabbable | Sets the string handle to always be grabbable. If set to true, you can grab the bow string even if the bow isn’t held in your other hand. | False |
| Nock only main handle | Define whether or not arrows can be nocked if you’re holding something other than their main (back) handle. If set to false, you can nock an arrow no matter how you’re holding it, otherwise you need to be holding it in the “archer pose” | True |
| Lose nock on ungrab | When the bow is ungrabbed, set whether or not the arrow should become unnocked (and potentially unrested). If set to true, when you drop the bow, if it has an arrow in it, the arrow falls out. Otherwise, the arrow remains in the bow. | True |
| Audio container shoot | An audio container which plays when the bow is fired. | None |
| Audio container draw | An audio container with clips which play as the bow is pulled back. | None |
| Audio clip string | An audio clip that plays on loop when the bow string is pulled or unpulled. | None |

## Editor tools

![EditorTools][EditorTools]

| Tool | Description | Default value |
| --- | --- | --- |
| Test pull ratio | Updates the position of the pink “test pull” line, as well as updates the animation of the bow string and the position of the debug “arrow” | 0 |
| Show debug arrow | Toggles the “debug arrow” off and on. | True |
| Debug arrow length | Adjusts how long the debug arrow is. The default value is the length of a vanilla arrow | 0.74557 |
| Bow mesh | If you want to test the pull ratio, or to use the auto configure tool, this value must be set! It has to be set to the mesh renderer that you use to animate your bow, otherwise those features will not work! | None |
| Vertex grab distance | Use this to fine-tune how the auto-configure tracks the draw length across your animation. If your draw length is close, but not quite right, try lowering this value a tiny bit. If your draw length is zero, try increasing this value until it works. | 0.01 |
| Auto configure button | Pressing this button will auto-configure your bow’s draw length and pull curve at the same time. As long as everything was set correctly before pressing, you should find that using the test pull ratio to visualize the bow pull animation, the pink line should perfectly sync up with your bow string, and the very end of the line should match the very end of the bow draw animation. | N/A |



[BowAnimations]: {{ site.baseurl }}/assets/components/BowString/BowAnimation.png
[BowCapsule]: {{ site.baseurl }}/assets/components/BowString/BowCapsule.png
[BowGizmo]: {{ site.baseurl }}/assets/components/BowString/BowGizmo.png
[BowProperties]: {{ site.baseurl }}/assets/components/BowString/BowProperties.png
[CustomRigidbody]: {{ site.baseurl }}/assets/components/BowString/CustomRigidbody.png
[EditorTools]: {{ site.baseurl }}/assets/components/BowString/EditorTools.png
[HandleScripts]: {{ site.baseurl }}/assets/components/BowString/HandleScripts.png

[EventLinker]: {{ site.baseurl }}{% link Components/ThunderRoad/Event-Linkers/BowEventLinker.md %}
[Item]: {{ site.baseurl }}{% link Components/ThunderRoad/Items/Item.md %}