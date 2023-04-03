using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Manikin
{
    public static class ManikinUtilities
    {
        #region Asset Menu
        #endregion

        #region GameObject Menu
        [MenuItem("GameObject/Manikin/Create New Part")]
        static void CreateNewManikinPart(MenuCommand command)
        {
            GameObject[] objects = Selection.gameObjects;

            string path = EditorUtility.SaveFolderPanel("Save prefabs in folder", "", "");
            path = path.Replace(Application.dataPath, "Assets");

            if (string.IsNullOrEmpty(path))
                return;

            Dictionary<string, List<GameObject>> LODs = new Dictionary<string, List<GameObject>>();

            for(int i = 0; i < objects.Length; i++)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(objects[i]))
                {
                    if (EditorUtility.DisplayDialog("Prefab", "This object is part of a prefab. Unpack it?", "OK", "Cancel"))
                    {
                        GameObject root = PrefabUtility.GetOutermostPrefabInstanceRoot(objects[i]);
                        PrefabUtility.UnpackPrefabInstance(root, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                    }
                    else
                    {
                        return;
                    }
                }

                EditorUtility.DisplayProgressBar("Creating Part", objects[i].name, (i / objects.Length));

                //First check if this is an LOD
                //If so, we need to nest it under a ManikinGroupPart
                if (objects[i].name.Contains("_LOD"))
                {
                    int startIndex = objects[i].name.IndexOf("_LOD");
                    string lodName = objects[i].name.Remove(startIndex);
                    
                    if(LODs.TryGetValue(lodName, out List<GameObject> lods))
                    {
                        lods.Add(objects[i]);
                    }
                    else
                    {
                        List<GameObject> newLods = new List<GameObject>();
                        newLods.Add(objects[i]);
                        LODs.Add(lodName, newLods);
                    }
                }
                else
                {
                    if (objects[i].GetComponent<MeshRenderer>() != null)
                    {
                        objects[i].AddComponent<MaterialInstance>();
                        objects[i].AddComponent<ManikinMeshPart>().Initialize();
                    }
                    else
                    {
                        if (objects[i].GetComponent<SkinnedMeshRenderer>() != null)
                        {
                            objects[i].AddComponent<MaterialInstance>();
                            objects[i].AddComponent<ManikinSmrPart>().Initialize();
                        }
                    }
                    string filename = path + "/" + objects[i].transform.root.name + "_" + objects[i].name + ".prefab";
                    PrefabUtility.SaveAsPrefabAssetAndConnect(objects[i], filename, InteractionMode.UserAction);
                }
            }

            foreach(KeyValuePair<string, List<GameObject>> pair in LODs)
            {
                GameObject obj = new GameObject(pair.Key, typeof(ManikinGroupPart));
                obj.transform.parent = pair.Value[0].transform.parent;

                foreach(GameObject child in pair.Value)
                {
                    child.transform.parent = obj.transform;

                    if (child.GetComponent<MeshRenderer>() != null)
                    {
                        child.AddComponent<MaterialInstance>();
                        child.AddComponent<ManikinMeshPart>();
                    }
                    else
                    {
                        if (child.GetComponent<SkinnedMeshRenderer>() != null)
                        {
                            child.AddComponent<MaterialInstance>();
                            child.AddComponent<ManikinSmrPart>();
                        }
                    }
                }

                obj.GetComponent<ManikinGroupPart>().Initialize();
                string filename = path + "/" + obj.transform.root.name + "_" + obj.name + ".prefab";
                PrefabUtility.SaveAsPrefabAssetAndConnect(obj, filename, InteractionMode.UserAction);
            }

            EditorUtility.ClearProgressBar();
        }

        [MenuItem("GameObject/Manikin/Create New Part", true, 10)]
        static bool ValidateCreateNewManikinPart()
        {
            if (Selection.activeGameObject != null)
            {
                return (Selection.activeGameObject.GetComponent<MeshRenderer>() != null ||
                    Selection.activeGameObject.GetComponent<SkinnedMeshRenderer>() != null ||
                    Selection.activeGameObject.GetComponent<LODGroup>() != null);
            }

            return false;
        }
#endregion
    }
}