---
parent: Areas
grand_parent: Guides
---
# How to create an Area

- Create a folder for the area in your project.
- Create an empty prefab in this folder for your area and make it addressable. (you may want to put all your area in the same addressable bundle)
- In the Addressables Groups tab, find the area addressable and add its Labels: Windows, Android, and Area.
- Open the area prefab, make a child GameObject, and start creating the area (models and assets, etc). You can also add child GameObjects for creature spawners, items spawners, etc. 
Remember that the gate you want to make on the side of the area needs to be the same size as the AreaConnectionType you want to assign.

{: .important}
Don’t forget to add a PlayerSpawner (in Start Area, or for debug/teleport in other areas).  See [Player Spawner][PlayerSpawner].

{: .note}
Don’t forget to add a way to load other levels in End Area so the player can leave when they end the dungeon. You can do this utilising an Event Level Loader and any sort of event linker.

- **Area Catalog Data**
    
    Set Up the AreaData. You can copy and edit one from the proto sample (...\BasSDK\BuildStaging\Catalogs\default\proto\Areas).
    
    Edit these fields :
    
    - ID: the Catalog ID for your area
    - isUnique: set to true if you want the area to spawn only once in the dungeon
    - areaPrefabAddress : Put the addressables address of the area prefab
    - allowedRotation : List of allowed rotations, you should let the 4 rotation allowed
    
    *Leave the rest for later.*
    
- Don’t forget to add the Data in SDK, then click on ThunderRoad (SDK) / load All Json. Every time you change the data reload all JSON to avoid log errors.
- Return to the Area Prefab view
- On the root/parent, add the Area component and set the field DataId with the ID you chose in the catalog Data. See [Area][Area].
- Create a box collider as a separate GameObject of the root, make sure its position is 0,0,0.
- Edit the box collider so it contains all the colliders in the prefab (wall etc).
- Make sure this box stopped at connection position.
- Copy the value of this box to the AreaData file in the fields name Bounds. (center and size)
- Delete the box collider from the prefab.
- Save the prefab and go back to the AreaData file to edit connections.
For each connection your Area has :
    - Add an areaConnection to the connections list (copy-paste from the sample)
    - Edit the fields :
        - connectionTypeIdContainerList :  the list of the different AreaConnectionTypeData that this connection can connect to. See [Creating Connections][CreatingConnections] on AreaConnections.
        - overrideBlockerTableAdress : same as blocker in the AreaConnectionType, if there is something in the Drop list then those blocker will be used for this specific area instead of the one from AreaConnectionType. 
        Leave empty unless you want something specific for this area connection.
        - position : the coordinate of the connection from the center of the area (when area root is at 0,0,0).
        It should be in the bottom middle of the doorway for horizontal area(Front, Back, Left, Right) and in the middle for vertical (Up and Down).
        - face : face of the area when looking prefab with z Axis (blue arrow in unity) to the front.
        
        Leave the fakeViewAddress empty for now.
        
- Open the area prefab.
- On the root, add the component ItemSpawnerLimiter and setup its value.
- On the root, add the LightingGroupComponent.
- On the root, add the NavMeshSurface Component.
- Create an empty game object name Gateways under the root, and place it at the origin (0,0,0).
- For each connection, in the same order as in the AreaData :
    
    Create an empty game object under Gateways 
    
    - change its coordinate so it is at the connection position, and rotate it so the Z axis face the outside of the area.
    - Add the AreaGateway Component to it. See [Area Gateway][AreaGateway]
    - Setup the LocalBound of this component (value or with gizmos).
    When the player enters this box it will fade the fake view and set active the adjacent Area.
    - Click on the SetupFakeView button.
    - A GameObject called reflection sorcery should appear as a child.
    Select it and click on the ToggleEditMode Button.
        
        ![ToggleEditMode][ToggleEditMode]
        
    
    A GameObject named room volume appear, it's a box on the prefab scene, edit it so it fits to the near wall. Then Click on ToggleEditMode button on reflection sorcery again.
    
- Save and close the prefab.
- Open the AreaTest Scene.
- Drag and drop the area prefab in it. the name should appear in red on the hierarchy.
- Place the prefab to the 0,0,0 position coordinate.
- find the connections you just created on the prefab. For each AreaGateway Component :
    - Click on the BakeFakeViewData
    - In the area prefab folder, it should have created a FakeViewData scriptableObject.
    Make it an addressable. Find it in the Addressables Groups tab and add labels Windows and Android. (you can change the addressable name)
    - Copy the address of this scriptable object and paste it into the fakeViewAddress in the connection of the AreaData.
- Remove the prefab from the scene and open the scene again so it's clean.
- Drag and drop the area prefab on the scene at the coordinates 0,0,0.
- The prefab appears in red on the hierarchy. Next to its name, there should be a grey square, click on it. A popup should open and choose to unlock. (this is a security because importing the prefab changes it and we don’t want to apply those automatic changes on the real prefab)

{: .note}
When a prefab is red, it is locked, and cannot be edited. It may also lag if you try to edit any components while it is locked. To edit the prefab, click the “Unlock” button on the prefab in the hierarchy, and unlock it. When the prefab is saved, it will lock again.

- Once unlocked, the prefab should appear normal on the scene. Select the root of the Area and click “Bake” for the NavMeshSurface.
- Then apply all the overrides on the prefab. The prefab should re-appear red again (locked)
- on the NavMeshSurface Component, click on the NavMeshData to find it in the project and move it to the area folder.
- Clean the scene again and add the prefab (same as before)
- Unlock the area prefab.
- Create the Lighting Preset :
On the folder of the prefab, Right click create lighting Data.
- Add the lighting data to the area lighting group in Area Prefab.
- Change the lighting data so it match your needs.
- Open the lightmap window (Window/Rendering/Lighting). Open BakedLightmap tab and bake the lightmap (generate lighting).
- After the baking, select the area in the scene. On the component LightingGroup, right-click and select “UpdateReferencesAndSaveAlll”.

![UpdateReferencesAndSaveAll][UpdateReferences]

- Apply all the changes to the prefab. The Area should be locked again.
- The Area should be ready to use 🙂
- You can now add the AreaData to Area Tables, and be able to spawn it inside Dungeons.

[AreaGateway]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/AreaGateway.md %}
[Area]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/Area.md %}
[PlayerSpawner]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/PlayerSpawner.md %}
[CreatingConnections]: {{ site.baseurl }}{% link Components/Guides/Areas/Connections.md %}
[UpdateReferences]: {{ site.baseurl }}/assets/components/Guides/Areas/UpdateReferences.png
[ToggleEditMode]: {{ site.baseurl }}/assets/components/Guides/Areas/ToggleEditMode.png