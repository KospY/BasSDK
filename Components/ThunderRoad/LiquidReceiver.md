# Liquid Receiver

## Overview
This component is intended to be placed on creatures, and is required for creatures to receive the effects of potions.

The object this component is attached to must be rotated such that it's y-axis (green arrow) is pointing **outwards** from the creatures mouth.

```warning
This component requires a [Creature Mouth Relay][CreatureMouthRelay] component on one of its parents.

[CreatureMouthRelay]: {{ site.baseurl }}{% link Components/ThunderRoad/CreatureMouthRelay.md %}
```


## Component Properties

| Field             | Description
| ---               | ---
| Drink Effect ID   | An effect played when liquid is received.
| Max Angle         | If the degrees difference between the y-axis (green arrow) and an axis pointing directly upwards is greater than this value, liquid will be ignored. 
| Stop Delay        | How long to wait in seconds after the last gulp before assuming the creature is no longer recieving liquid.
| Effect Rate       | The time in seconds before the drink effect can play again.
