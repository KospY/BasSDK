# Damager

The damager script is essential in dealing damage with the weapons. This script utilises the [Collider Group](https://github.com/KospY/BasSDK/blob/gh-pages/Components/ThunderRoad/ColliderGroup.md) script/colliders to tell the game what part of the weapon is to deal this type of damage. 
The script will tell the user what the damager type it is set up for through an information box, also.

![Script Preview](/assets/u11-modder-update-guide/Damager/DamagerScript.png)

## Damager Components
### "Collider Group" and "Collider Only"
The [Collider Group](https://github.com/KospY/BasSDK/blob/gh-pages/Components/ThunderRoad/ColliderGroup.md) is another script used to reference a set of colliders. If there is one collider referenced, it would be best to reference this one collider in the "Collider Only" field. However, this is not required if it is referenced as a collider group. 
This component tells the game what collider deals this type of damage.

### Direction
The "Direction" parameter of the damager script is to make it so the damager deals damage if hit in this direction. For example, a "Pierce" damager has Direction set to "Forward" so that it stabs when thrust or thrown forwards.
Please note: Z Axis/Blue Arrow is Forwards, Y Axis/Green Arrow is Upwards.

![Direction Example](/assets/u11-modder-update-guide/Damager/PenDirection.png)

"Forward" only deals damage when thrown/thrusted/slashed on the positive Z axis / Pointing Blue Arrow
"Forward and Backward" only deals damage when thrown/thrusted/slashed on positive AND negative Z axis / Pointing Blue Arrow AND Other direction
"All" Deals damage in all directions

### Penetration Length
The Penetration Length extends to the length of the blade, and is made for slashing objects. For swords, this will extend the length of the whole blade, and for axes it shall show the height of the blade. This is mainly used for slashing, for where the depth must be the distance between the length and the edge of the blade, or, for axes, the damager should be set up with slashing and piercing combined.

![Length Example](/assets/u11-modder-update-guide/Damager/PenLength.png)

### Penetration Depth
The Penetration Depth is to depict how far a weapon can pierce an object. For example, a dagger has a Penetration Depth set to end at the handle, meaning that it will stop piercing once it reaches that point.

For Slash Damagers, the Penetration Depth must be from center to the edge of the blade, and for axe damagers, the depth goes from the edge of the blade to how far the axe is to pierce.

![Penetration Depth Example](/assets/u11-modder-update-guide/Damager/PenDepth.png)  ![Dagger and Axe Example](/assets/u11-modder-update-guide/Damager/DaggerAndAxe.PNG)

### Penetration Exit on Max Depth

When ticked, this item, when it reaches the end of the pierce, will stop piercing the object and will detatch.

## Damager Types

There are three types of damager types: Blunt, Pierce and Slashing. A weapon can utilise all of these damagers. For example, a dagger can use a blunt damager for the handle, a slash damager for slashing with it, and a pierce damager to stab with it.

### Pierce

The pierce damager utilises ONLY the penetration depth. The Penetration Length should be zero. This is used to stab objects, such as a dagger or sword. 
The Depth displays how far the weapon can go through the object. For pierces, the direction should be set to forward.

### Slash

The slash damager utilises both the depth and length.
For straight-bladed weapons, like swords and daggers, the penetration length must extend to the length of the blade, while the depth must be from the center to the edge of the blade. The direction for Slash on these weapons should be set to Forward and Backward.
For weapons like axes, the length must be the height of the blade and the depth must be how far it goes down the axe. The direction for Slash on axes should be set to forward

### Blunt

The Blunt damager is to deal blunt damage to an object, such as a mace or a non-bladed weapon. For a blunt damager, the length and depth must be zero, and the direction should be set to all.
