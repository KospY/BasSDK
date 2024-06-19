---
parent: ThunderRoad
---
# Anim Random
*This is an animation component, meaning it can only be added to **Animation States**. This component cannot be added to regular GameObjects.*

## Overview

This animation component will set an integer property to a random value between zero and the given range when the state is **entered**.

This range is lower inclusive, upper exclusive. Meaning a range of 0 to 5 would have a uniform chance of picking any of the following values:  
`0, 1, 2, 3, 4`

## Component Fields

| Field         | Description
| ---           | ---
| Parameter     | The name of the **int** parameter to update.
| Range         | The upper range for the random value.