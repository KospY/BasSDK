using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ThunderRoad
{
    [CustomEditor(typeof(AreaGateway))]
    public class AreaGatewayEditor : Editor
    {
        private BoxBoundsHandle boxBoundsHandle = new BoxBoundsHandle();
        private AreaGateway areaGateway;

        void OnEnable()
        {
            areaGateway = (AreaGateway)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("SetupFakeView"))
            {
                Area area = areaGateway.GetComponentInParent<Area>();
                if (area == null)
                {
                    Debug.LogError("Can not find Area component in parent");
                    return;
                }

                Catalog.EditorLoadAllJson();
                AreaData areaData = area.areaData.Data;
                if (areaData != null)
                {
                    AreaGateway[] gateways = area.GetComponentsInChildren<AreaGateway>();
                    int indexGateway = -1;
                    for (int i = 0; i < gateways.Length; i++)
                    {
                        if (gateways[i] == areaGateway)
                        {
                            indexGateway = i;
                            break;
                        }
                    }

                    if (indexGateway >= 0
                        && indexGateway < areaData.connections.Count)
                    {

                        GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource<GameObject>(area.gameObject);
                        string prefabAdress;
                        if (prefab == null)
                        {
                            // Check prefab preview Mode
                            prefabAdress = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage()?.assetPath;
                        }
                        else
                        {
                            prefabAdress = AssetDatabase.GetAssetPath(prefab);
                        }

                        areaGateway.SetupFakeView(areaData, indexGateway, prefabAdress);
                    }
                    else
                    {
                        Debug.LogError("Can not find index for this gateway");
                        return;
                    }
                }
                else
                {
                    Debug.LogError("AreaData is not set for area : " + area.name);
                    return;
                }
            }
            
            if (GUILayout.Button("BakeFakeViewData"))
            {
                Area area = areaGateway.GetComponentInParent<Area>();
                if (area == null) return;
                Catalog.EditorLoadAllJson();
                AreaData areaData = area.areaData.Data;
                if (areaData != null)
                {
                    AreaGateway[] gateways = area.GetComponentsInChildren<AreaGateway>();
                    int indexGateway = -1;
                    for (int i = 0; i < gateways.Length; i++)
                    {
                        if (gateways[i] == areaGateway)
                        {
                            indexGateway = i;
                            break;
                        }
                    }

                    if (indexGateway >= 0
                        && indexGateway < areaData.connections.Count)
                    {

                        GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource<GameObject>(area.gameObject);
                        string prefabAdress;
                        if (prefab == null)
                        {
                            // Check prefab preview Mode
                            prefabAdress = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage()?.assetPath;
                        }
                        else
                        {
                            prefabAdress = AssetDatabase.GetAssetPath(prefab);
                        }

                        areaGateway.BakeFakeViewData(areaData, indexGateway, prefabAdress);
                    }
                    else
                    {
                        Debug.LogError("Can not find index for this gateway");
                        return;
                    }
                }
                else
                {
                    Debug.LogError("AreaData is not set for area : " + area.name);
                    return;
                }
            }
        }

        protected virtual void OnSceneGUI()
        {
            // Trigger bounds
            boxBoundsHandle.center = areaGateway.localBounds.center;
            boxBoundsHandle.size = areaGateway.localBounds.size;
            // draw the handle
            Handles.matrix = areaGateway.transform.localToWorldMatrix;
            EditorGUI.BeginChangeCheck();
            boxBoundsHandle.DrawHandle();

            if (EditorGUI.EndChangeCheck())
            {
                // record the target object before setting new values so changes can be undone/redone
                Undo.RecordObject(areaGateway, "Change Bounds");
                // copy the handle's updated data back to the target object
                Bounds newBounds = new Bounds();
                newBounds.center = boxBoundsHandle.center;
                newBounds.size = boxBoundsHandle.size;
                areaGateway.localBounds = newBounds;
            }
        }
    }
}