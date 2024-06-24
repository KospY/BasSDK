---
parent: Areas
grand_parent: Guides
---
# Creating Connections

### **Create AreaConnectionTypeData**:

AreaConnectionTypeData is the catalog data that defines how areas can be connected to each other.
Each Area will have a different connection, each can have a different ConnectionData associated with it. Two areas can be connected through a connection only if they both contain a connectionData.

**AreaConnectionTypeData fields** :

- size : The size of the connection (gate).
- randomBlockerAdress : List of «blocker». The blocker is the prefab that will spawn to the area connection location if it's not connected to another. Update Drops List :
    - dropItem : the address of a **blocker** that can spawn
    - probabilityWeight : the weight to change the probability this item is chosen over the other.
- randomGateTableAddress : List of « gate » addresses. The Gate is the prefab that will spawn in the middle of the 2 areas at the connection location. Update Drops List:
    - dropItem : the address of a blocker that can spawn
    - probabilityWeight : the weight to change the probability this item is chosen over another.

If you want the area to be able to connect to other area in Greenland Dungeon you should import the AreaConnectionTypeData from the game catalog to the SDK. If you want to create your own dungeon you can create your own ConnectionData using the proto template.