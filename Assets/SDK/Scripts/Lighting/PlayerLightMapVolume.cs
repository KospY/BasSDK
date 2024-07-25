using System;
using System.Collections.Generic;

using TriInspector;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

namespace ThunderRoad
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(BoxCollider))]
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/PlayerLightmapVolume.html")]
    [DrawWithTriInspector]
    public class PlayerLightMapVolume : MonoBehaviour
    {
#if UNITY_EDITOR        
        
        private static bool drawGizmos = false;
        [GUIColor("$GetColorGizmos")]
        [Button("PLV Gizmos")]
        private void _drawGizmos() => drawGizmos = !drawGizmos;
        private Color GetColorGizmos() => drawGizmos ? Color.green : Color.red;
        
        private static bool drawAllBakedLODGroups = false;
        [GUIColor("$GetColorBLG")]
        [Button("BakedLodGroup Gizmos")]
        private void _drawBLG() => drawAllBakedLODGroups = !drawAllBakedLODGroups;
        private Color GetColorBLG() => drawAllBakedLODGroups ? Color.green : Color.red;
        
        
        public static bool drawLinesToVolumes = false;
        [GUIColor("$GetColorLines")]
        [Button("Draw lines from BakedLODGroups to PLVs")]
        private void _drawLines() => drawLinesToVolumes = !drawLinesToVolumes;
        private Color GetColorLines() => drawLinesToVolumes ? Color.green : Color.red;
        
        public static bool drawLightmapScaleColors = false;
        [GUIColor("$GetColorScale")]
        [Button("Draw Lightmap Scale Colors")]
        private void _drawScales() => drawLightmapScaleColors = !drawLightmapScaleColors;
        private Color GetColorScale() => drawLightmapScaleColors ? Color.green : Color.red;
        
        public static bool updateSelectedLightmapScale = false;
        [GUIColor("$GetColorUpdateScale")]
        [Button("Update meshes lightmap scale when selecting a bakedlodgroup")]
        private void _updateScales() => updateSelectedLightmapScale = !updateSelectedLightmapScale;
        private Color GetColorUpdateScale() => updateSelectedLightmapScale ? Color.green : Color.red;
        

#endif
        public static List<PlayerLightMapVolume> playerLightMapVolumes = new List<PlayerLightMapVolume>();
        [Tooltip("Depicts what Player Light Map Volume has priority over others.")]
        [Range(0, 10)]
        public int priority = 1;
        [Range(0, 5)]
        [Tooltip("Adjusts the light map scale of Baked LOD Groups inside the box.")]
        public float lightMapScaleMultiplier = 1f;
        [Tooltip("The distance from the closest point of the bounding box of the volume where the lightmap scale multiplier will be 1. " +
                 "The lightmap scale multiplier will be interpolated between 1 and 0 from the bounding box to the falloff distance.")]
        public float lightMapFalloffDistance = 10f;
        [Tooltip("The box collider used to calculate Baked LOD groups.")]
        public BoxCollider boxCollider;
        
        private void OnEnable()
        {
	        //disable this gameobject if the game is playing
	        if (Application.isPlaying)
	        {
		        gameObject.SetActive(false);
		        return;
	        }	        

            boxCollider = GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            playerLightMapVolumes.Add(this);
            //set this gameobjects tag to EditorOnly
            gameObject.tag = "EditorOnly";
        }

        private void OnValidate()
        {
#if UNITY_EDITOR            
	        //only do this if the game is not playing
	        if (Application.isPlaying) return;
	        //only do this if the prefab is not active in the scene or prefab mode
	        if (gameObject.scene.name == null) return;
	        PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
	        bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
	        bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(this.gameObject) == PrefabInstanceStatus.Connected;
	        if (!isValidPrefabStage && prefabConnected)
	        {
		        //Variables you only want checked when in a Scene
		        //get all of the BakedLODGroups in the scene and update their lightmap scale
		        BakedLODGroup[] bakedLodGroups = FindObjectsOfType<BakedLODGroup>();
		        foreach (BakedLODGroup bakedLodGroup in bakedLodGroups)
		        {
			        bakedLodGroup.UpdateLightmapScale();
		        }
                //set the layer of this to the "Ignore Raycast" layer
                gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
	        }

#endif            
        }

        private void OnDisable()
        {
            playerLightMapVolumes.Remove(this);
        }
        private void OnDestroy()
        {
            playerLightMapVolumes.Remove(this);
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //draw gizmos for the box collider
            Gizmos.color = new Color(0.2f, 0.8f, 0.1f, 1);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
            //draw a label to show the priority and lightmap scale multiplier
            Handles.Label(transform.position, $"Priority: {priority}\nMultiplier: {lightMapScaleMultiplier}\nFalloff: {lightMapFalloffDistance}");
            
            
        }

        [NonSerialized]
        public static List<PlayerLightMapVolume> selectedPlayerLightmapVolumes;
        [NonSerialized]
        public static PlayerLightMapVolume primarySelectedPlayerLightmapVolume;
        [NonSerialized]
        bool stillToDraw = true;
        public void OnDrawGizmosSelected()
        {
            //Get the LightingGroup in the scene
            if (!drawGizmos) return;
            
            if (!selectedPlayerLightmapVolumes.IsNullOrEmpty())
            {
                if (selectedPlayerLightmapVolumes.Count != 1)
                {
                    //check if I am the last one still to draw so I can clean up
                    bool anyStillToDraw = false;
                    foreach (PlayerLightMapVolume volume in selectedPlayerLightmapVolumes)
                    {
                        if (volume == this) continue;
                        if (volume.stillToDraw) anyStillToDraw = true;
                        break;
                    }
                    if (!anyStillToDraw)
                    {
                        foreach (PlayerLightMapVolume volume in selectedPlayerLightmapVolumes)
                        {
                            volume.stillToDraw = true;
                        }
                        selectedPlayerLightmapVolumes.Clear();
                        primarySelectedPlayerLightmapVolume = null;
                        return;
                    }
                    this.stillToDraw = false;
                }
                
            }
            
            
            //update the selected PlayerLightMapVolumes
            GameObject[] selectedObjects = Selection.gameObjects;
            PlayerLightMapVolume.selectedPlayerLightmapVolumes ??= new List<PlayerLightMapVolume>();
            PlayerLightMapVolume.selectedPlayerLightmapVolumes.Clear();
        
            for (var i = 0; i < selectedObjects.Length; i++)
            {
                GameObject selectedObject = selectedObjects[i];
                if (selectedObject.TryGetComponent(out PlayerLightMapVolume selectedPlayerLightmapVolume))
                {
                    PlayerLightMapVolume.selectedPlayerLightmapVolumes.Add(selectedPlayerLightmapVolume);
                }
            }
            
            

            BakedLODGroup[] bakedLodGroups = FindObjectsOfType<BakedLODGroup>();
            for (var i = 0; i < bakedLodGroups.Length; i++)
            {
                BakedLODGroup bakedLodGroup = bakedLodGroups[i];
                if (!drawAllBakedLODGroups)
                {
                    if (bakedLodGroup.closestLightMapVolume == null) continue;
                    //see if the closestLightMapVolume is selected
                    bool selected = false;
                    if (bakedLodGroup.closestLightMapVolume == this) selected = true;
                    if (selectedPlayerLightmapVolumes.Contains(bakedLodGroup.closestLightMapVolume)) selected = true;
                    if (!selected) continue;
                }
                bakedLodGroup.OnDrawGizmosSelected();
            }
            this.stillToDraw = false;
        }
#endif
        
    }
}
