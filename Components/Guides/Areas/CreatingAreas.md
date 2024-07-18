---
parent: Areas
grand_parent: Guides
---
# How To Create an Area

1. Create a folder for your area. This folder is primarily for your area, and will host all the lightmap data, and you can put any area-specific assets in here as well.
2. Create your area in your scene or as a new prefab. You can place models, scripts (like ropes and CreatureSpawners) and other normal level assets. 
3. Add [AreaGateways][AreaGateway] so that your rooms can connect eachother. It is recommended that no models go past this gate, as they will be visible in other rooms, or the collider can block progress in other rooms also. For each connection, in the same order as in the AreaData :
  
    - change its coordinate so it is at the connection position, and rotate it so the Z axis face the outside of the area.
    - Add the AreaGateway Component to it. See [Area Gateway][AreaGateway]
    - Setup the LocalBound of this component (value or with gizmos).
    When the player enters this box it will fade the fake view and set active the adjacent Area.
    - Click on the SetupFakeView button.
    - A GameObject called reflection sorcery should appear as a child.
    Select it and click on the ToggleEditMode Button.
        
        ![ToggleEditMode][ToggleEditMode]
        
    
    A GameObject named Room Volume appears, it's a box on the prefab scene, edit it so it fits to the near wall. Then Click on ToggleEditMode button on reflection sorcery again.
4. Add the [Area][Area] component to the root of your room. Create a JSON for your Area, and insert the ID in to the "Area ID". Once done, add a [LightingGroup][LightingGroup], [AreaLightingGroupLiteMemoryToggle][AreaLightingGroupLiteMemoryToggle], NavMeshSurface and [ItemSpawnerLimiter][ItemSpawnerLimiter].
5. Crate a box collider as a separate GameObject of the root, and ensure its position in the scnee is 0,0,0.
6. Adjust the Box collider so it fills the bounderies of the room. The bounderies must not go past the AreaGateways you have created, and if models are past the AreaGateways, ignore them, and ensure that the box collider rests on the AreaGateway.
7. In your Area JSON, copy the scale size of the box and use that to put it in the "Area Bounds" of the Area JSON.
8. You can now delete that box collider, as otherwise it could block the player apon entering the room, or any raycasts that will be cast.
9. In the Area JSON, you must reference every AreaGateway, including each of their positions within the room. 
10. Now, make your Area a Prefab in the Area folder you created earlier. This room will now lock. Every edit in the scene (not prefab) will not work on the room unless you unlock the room (unlock button in the hierarchy) and override prefab once your change was complete.
11. Create an Addressable Asset (see [Addressables Documentation][AddressableAssets]) and place your Area on to your new Addressable. Name it something that fits, add the "Windows" (or Android for Nomad) and "Areas" Labels to it.
12. Next, go to the "AreaTest" scene and place your room. Ensure that the room is placed at the origin (0,0,0). 
13.  Select each AreaGateway again. Under it, on the AreaGateway component, click on the "BakeFakeViewData", it should create a FakeViewData file in your Area folder. Once complete, make that FakeViewData Addressable by placing it under either a new addressable packed assets, or the same one for the area you made. Name it something appropriate with the Windows/Android tag, and reference it in the Area JSON. Make sure that the fake view matches the doors you place them under, so that they show the correct fake view in play. After this, the room MAY lock itself, unlock it to keep editing.
14. Under your parent gameObject, under the NavMeshSurface, change the "Include Layers" to "Default" and "LocomotionOnly", and for "Use Geometry", set it to "Physics Colliders". Then, press Bake. The room MAY lock itself, unlock it to keep editing.
15. Once done, go to the top left of your Inspector while on the parent, and apply all changes. The room will lock itself.
16. Set up [Light Probe Volumes][LightProbeVolume] and make sure that they go to each Area Gateway or past it so it can affect the Door Blockers.
17. Unlock your room to edit once more, and create a "Lighting Data" (Right Click > Create > ThunderRoad > Levels > Lighting Data)
18. Add the lighting data to this component, such as the directional light direction, fog, light color, etc. Place this lighting data in the "Lighting Group" script you placed on the parent earlier.
19. Open the Lighting window (Window > Rendering > Lighting). You now will bake your room.
20. Once the baking is complete, select the Area in your scene again. On the LightingGroup, right click it and select "UpdateReferencesAndSaveAll".

![UpdateReferencesAndSaveAll][UpdateReferences]

21. Apply all the changes to the prefab. The Area should be locked again.
22. Go to the Area JSON, ensure that all of the references are correct and positions/scales of the room and its' gateways are correct. "IsUnique" ensures thatr your room can only spawn once in a run, while "allowedRotation" allows different rotations of the room. You should set it to allow 4 rotations (Front, Back, Left, Right).
23. You can now add your Area to the AreaTables, to allow your room to spawn

{: .important}
Don’t forget to add a PlayerSpawner in your Room. This is a requirement for if your room is a "Start" room, or if you want to debug your Area.  See [Player Spawner][PlayerSpawner].

{: .note}
If your room is an "End" room, you'll want a way to get back home. You can do this utilising an [EventLoadLevel][EventLoadLevel] and any sort of Event (e.g. [Zone][Zone])

[AreaGateway]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/AreaGateway.md %}
[AreaLightingGroupLiteMemoryToggle]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/AreaLightingGroupLiteMemoryToggle.md %}
[ItemSpawnerLimiter]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/ItemSpawnerLimiter.md %}
[LightingGroup]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/LightingGroup.md %}
[LightProbeVolume]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/LightProbeVolume.md %}
[Area]: {{ site.baseurl }}{% link Components/ThunderRoad/Areas/Area.md %}
[Zone]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/Zone.md %}
[EventLoadLevel]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/EventLoadLevel.md %}
[AddressableAssets]: {{ site.baseurl }}{% link Components/Guides/SDK-HowTo/CreatingAssetBundles.md %}
[PlayerSpawner]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/PlayerSpawner.md %}
[CreatingConnections]: {{ site.baseurl }}{% link Components/Guides/Areas/Connections.md %}
[UpdateReferences]: {{ site.baseurl }}/assets/components/Guides/Areas/UpdateReferences.png
[ToggleEditMode]: {{ site.baseurl }}/assets/components/Guides/Areas/ToggleEditMode.png