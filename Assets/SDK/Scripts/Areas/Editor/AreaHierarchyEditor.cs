using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ThunderRoad
{
    [InitializeOnLoad]
    public class AreaHierarchyEditor
    {
        private static Texture buttonTexture;
        static AreaHierarchyEditor()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
            buttonTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SDK/Scripts/Areas/Editor/Lock.png", typeof(Texture));
        }

        private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            if (Application.isPlaying) return;
            Color fontColor = Color.red;
            Color backgroundColor = new Color(56.0f / 255.0f, 56.0f / 255.0f, 56.0f / 255.0f);

            Object obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj != null)
            {
                if (obj is GameObject go)
                {
                    Area area = go.GetComponent<Area>();
                    bool isChild = false;
                    if (area == null)
                    {
                        area = go.GetComponentInParent<Area>();
                        isChild = true;
                    }

                    if (area != null
                        && area.modifiedInImport)
                    {
                        if (Selection.instanceIDs.Contains(instanceID))
                        {
                            backgroundColor = new Color(44 / 255.0f, 93 / 255.0f, 135 / 255.0f);
                        }

                        Rect offsetRect = new Rect(selectionRect.position, selectionRect.size);
                        EditorGUI.DrawRect(selectionRect, backgroundColor);
                        if (!isChild)
                        {
                            offsetRect.x += selectionRect.height + 2;
                            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
                            buttonStyle.padding = new RectOffset(2, 2, 2, 2);
                            if (GUI.Button(new Rect(selectionRect.x, selectionRect.y, selectionRect.height, selectionRect.height),
                                                    new GUIContent(buttonTexture),
                                                    buttonStyle))
                            {
                                if (EditorUtility.DisplayDialog("Unlock area prefab instance?",
                                                                "This will reimport the prefab asset without the import modification.\n Don't forget to reimport prefab after your changed."
                                                                , "Yes Unlock area", "Do Not Unlock"))
                                {
                                    GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(go);
                                    string prefabAdress = AssetDatabase.GetAssetPath(prefab);
                                    ChangeAreaImportAndReimportPrefab(prefabAdress);
                                }
                            }
                        }

                        EditorGUI.LabelField(offsetRect, obj.name, new GUIStyle()
                        {
                            normal = new GUIStyleState() { textColor = fontColor },
                        }
                        );
                    }
                }
            }
        }

        private static void ChangeAreaImportAndReimportPrefab(string prefabPath)
        {
            AreaAssetPostProcessor.SetUpAreaOnImport = false;
            AssetDatabase.ImportAsset(prefabPath);
            AreaAssetPostProcessor.SetUpAreaOnImport = true;
        }
    }

}