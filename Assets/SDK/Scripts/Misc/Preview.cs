using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Preview")]
    [AddComponentMenu("ThunderRoad/Items/Preview")]
    public class Preview : MonoBehaviour
    {
        public bool closeUpPreview;
        public float size = 1;
        public int iconResolution = 512;
        public int tempLayer = 2;
        public List<Renderer> renderers;
        public Texture2D generatedIcon;

        protected virtual void OnDrawGizmosSelected()
        {
            size = transform.lossyScale.x;
            Gizmos.color = Common.HueColourValue(HueColorName.Green);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(size / this.transform.lossyScale.x, size / this.transform.lossyScale.y, 0));
            Common.DrawGizmoArrow(Vector3.zero, (Vector3.back * 0.3f) / this.transform.lossyScale.z, Color.blue, 0.15f / this.transform.lossyScale.z);
            Common.DrawGizmoArrow(Vector3.zero, (Vector3.up * 0.3f) / this.transform.lossyScale.y, Common.HueColourValue(HueColorName.Green), 0.15f / this.transform.lossyScale.y);
        }

#if UNITY_EDITOR
        [Button]
        public void GenerateIcon()
        {
            Dictionary<Renderer, int> dicRenderers = new Dictionary<Renderer, int>();

            if (renderers.Count == 0)
            {
                foreach (Renderer renderer in (renderers.Count == 0 ? new List<Renderer>(this.transform.root.GetComponentsInChildren<Renderer>()) : renderers))
                {
                    dicRenderers.Add(renderer, renderer.gameObject.layer);
                    renderer.gameObject.layer = tempLayer;
                }
            }

            Camera cam = new GameObject().AddComponent<Camera>();
            cam.transform.SetParent(this.transform);
            cam.orthographic = true;
            cam.targetTexture = RenderTexture.GetTemporary(iconResolution, iconResolution, 16);
            cam.clearFlags = CameraClearFlags.Color;
            cam.stereoTargetEye = StereoTargetEyeMask.None;
            cam.orthographicSize = size / 2;
            cam.cullingMask = 1 << tempLayer;
            cam.enabled = false;

            cam.transform.position = this.transform.position + (this.transform.forward * -1);
            cam.transform.rotation = this.transform.rotation;
            Light camLight = cam.gameObject.AddComponent<Light>();
            camLight.transform.SetParent(cam.transform);
            camLight.type = LightType.Directional;
            camLight.enabled = true;


            string iconPath = null;
            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                // Prefab editor
                iconPath = PrefabStageUtility.GetCurrentPrefabStage().assetPath;
                cam.scene = PrefabStageUtility.GetCurrentPrefabStage().scene;
            }
            else if (PrefabUtility.GetNearestPrefabInstanceRoot(this))
            {
                // Prefab in scene
                iconPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(this);
            }
            else
            {
                // Not a prefab
                Debug.LogError("Unable to create an icon file if the object is not a prefab!");
            }

            cam.Render();

            RenderTexture orgActiveRenderTexture = RenderTexture.active;
            RenderTexture.active = cam.targetTexture;

            generatedIcon = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.ARGB32, false);
            generatedIcon.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
            generatedIcon.Apply(false);

            RenderTexture.active = orgActiveRenderTexture;

            // Clean up after ourselves
            cam.targetTexture = null;
            RenderTexture.ReleaseTemporary(cam.targetTexture);

            foreach (KeyValuePair<Renderer, int> renderer in dicRenderers)
            {
                renderer.Key.gameObject.layer = renderer.Value;
            }

            DestroyImmediate(cam.gameObject);

            if (iconPath != null)
            {
                Debug.Log("Item path: " + iconPath);
                byte[] bytes = generatedIcon.EncodeToPNG();
                string path = iconPath.Replace(".prefab", closeUpPreview ? "-close-up.png" : ".png");
                System.IO.File.WriteAllBytes(path, bytes);
                AssetDatabase.Refresh();
                generatedIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                TextureImporter textureImporter = (TextureImporter)TextureImporter.GetAtPath(path);
                textureImporter.alphaIsTransparency = true;
                EditorUtility.SetDirty(textureImporter);
                textureImporter.SaveAndReimport();
                Debug.Log("Icon generated: " + path);

                AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
                if (settings != null)
                {
                    string guid = AssetDatabase.AssetPathToGUID(path);
                    AddressableAssetEntry entry = settings.FindAssetEntry(guid);

                    if (entry == null)
                    {
                        Debug.Log("Added icon to addressable");
                        entry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
                        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryAdded, entry, true, false);
                    }

                    string prefabGuid = AssetDatabase.AssetPathToGUID(iconPath);
                    AddressableAssetEntry prefabEntry = settings.FindAssetEntry(prefabGuid);

                    if (prefabEntry != null)
                    {
                        entry.SetAddress(prefabEntry.address + (closeUpPreview ? ".IconClose" : ".Icon"), false);
                        Debug.Log("Icon Addressable: " + entry.address);
                    }
                }
                else
                {
                    Debug.LogError("Could not find AddressableAssetSettings, cannot set the icon to addressable");
                    return;
                }
            }
        }

        [Button]
        public void AlignCamera()
        {
            SceneView.lastActiveSceneView.LookAt(this.transform.position + (this.transform.forward * -1), this.transform.rotation, size / 2);
            SceneView.lastActiveSceneView.orthographic = true;
        }

        [Button]
        public void AlignFromCamera()
        {
            SceneView.RepaintAll();
            SceneView.lastActiveSceneView.camera.transform.position -= 2 * this.transform.position;
            this.transform.LookAt(-SceneView.lastActiveSceneView.camera.transform.position);
            SceneView.lastActiveSceneView.camera.transform.position += 2 * this.transform.position;
        }
#endif
    }
}