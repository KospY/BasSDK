# Tube Builder

## Overview
This component is used to dynamically generate a simple cylinder mesh between two points. Using the `Continous Update` property, the created tube will constantly update itself to fit between two points.

The object this component is added to acts as the starting point of the tube.


### Setup
1. Create an empty object.
2. Select `Add Component` in the inspector window and select the TubeBuilder component.
3. Create a new gameobject to act as your target. Assign this object to the `Target` field. 


## Component Properties

| Field              | Description
| ---                | ---
| Target             | A transform representing the end point of the tube.
| Radius             | The [radius](#-radius) of the generated tube mesh.
| Tiling Offset      | The vertical tiling of the MainTex on the generated mesh.
| Material           | The material set here will be assigned to the generated mesh.
| Layer              | The layer to place the generated mesh on.
| Use Collider       | If enabled, the generated mesh will have a physics collider.
| Physic Material    | The physics material applied to the generated mesh's collider.
| Pre Generate       | Setting this to true will immediately generate the tube mesh, and will not regenerate the mesh ingame.
| Continuous Update  | If enabled, the tube mesh will constantly be updated to fit between the start and end point.

## Notes

### • Radius
The radius of the tube mesh will be **halved** when generated.