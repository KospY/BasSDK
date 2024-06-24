---
parent: Levels
grand_parent: ThunderRoad
---
# Ladder Builder

## Overview
This component allows you to easily create a ladder of any length by repeating a single rung object along the Y axis. 

This component has an example within the SDK examples folder.

### Setup
1. Create an empty gameobject.
2. Select `Add Component` in the inspector window and add the LadderBuilder component.
3. Place either the LadderRung prefab from the SDK examples folder, or your own rung prefab in the `Rung Prefab` field.
4. Set the `Rung Count` property to the amount of rungs your ladder should have, and press `Create Rungs`.

## Component Properties

| Field              | Description
| ---                | ---
| Rung Count         | How many rungs should be generated.
| Rung Height        | How far apart each rung should be.
| Disable Rung Mesh  | If enabled, all mesh renderers in the rung prefab will be disabled on generation.
| Rung Prefab        | The prefab that will be repeated vertically to form the ladder.