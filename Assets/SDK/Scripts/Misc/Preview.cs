using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;

#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Items/Preview.html")]
    [AddComponentMenu("ThunderRoad/Items/Preview")]
    public class Preview : MonoBehaviour
    {
        [Tooltip("If ticked, will generate a separate preview with \"Close-up\", which is used for close-up preview in the item spawner")]
        public bool closeUpPreview;
        [Tooltip("Determine size of the preview. Scales with X Scale")]
        public float size = 1;
        [Tooltip("Default resolution of the generated preview")]
        public int iconResolution = 512;
        [Tooltip("Temporarily changes layer when taking a picture. Change this if the layers are already in use/changed.")]
        public int tempLayer = 2;
        [Tooltip("Point light intensity")]
        public float pointLightIntensity = 1;
        [Tooltip("Ambient light intensity")]
        public float ambientIntensity = 1;
        [Tooltip("Try to update the catalog with the addressable path of the generated icon.")]
        public bool updateCatalog = false;
        [Tooltip("List of renderers which can be used in the preview. Not neccessary for weapon mesh.")]
        public List<Renderer> renderers;
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif	    
        [NonSerialized]
        public Texture2D generatedIcon;

        protected virtual void OnDrawGizmosSelected()
        {
            size = transform.lossyScale.x;
            Gizmos.color = Common.HueColourValue(HueColorName.Green);
            Gizmos.matrix = transform.localToWorldMatrix;
            Transform thisTransform = this.transform;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(size / thisTransform.lossyScale.x, size / thisTransform.lossyScale.y, 0));
            Common.DrawGizmoArrow(Vector3.zero, (Vector3.back * 0.3f) / thisTransform.lossyScale.z, Color.blue, 0.15f / thisTransform.lossyScale.z);
            Common.DrawGizmoArrow(Vector3.zero, (Vector3.up * 0.3f) / thisTransform.lossyScale.y, Common.HueColourValue(HueColorName.Green), 0.15f / thisTransform.lossyScale.y);

            //draw where the lights will be
            //get the bounds of the renderers
            Bounds bounds = new Bounds();
            //get the item renderers

            if (GetComponentInParent<Item>() is Item parentItem)
            {
                foreach (Renderer renderer in parentItem.GetComponentsInChildren<Renderer>())
                {
                    if (renderer == null)
                    { continue; }

                    bounds.Encapsulate(renderer.bounds);
                }
            }

            //top light
            var topLightPos = (thisTransform.forward * 2);
            Common.DrawGizmoArrow(topLightPos, (-thisTransform.forward), Color.yellow, Vector3.Distance(topLightPos, Vector3.zero));
            //left light
            var leftLightPos = (-thisTransform.right * 2);
            Common.DrawGizmoArrow(leftLightPos, (thisTransform.right), Color.yellow, Vector3.Distance(leftLightPos, Vector3.zero));
            //right light
            var rightLightPos =(thisTransform.right * 2);
            Common.DrawGizmoArrow(rightLightPos, (-thisTransform.right), Color.yellow, Vector3.Distance(rightLightPos, Vector3.zero));
        }

#if UNITY_EDITOR
        [Button]
        public void GenerateIcon()
        {
            //get the item component in the parent
            Item item = this.GetComponentInParent<Item>();

            Dictionary<Renderer, int> dicRenderers = new Dictionary<Renderer, int>();
            if (renderers == null)
            {
                Debug.LogError($"Renderers list is null in, {this.gameObject.GetPathFromRoot()} please assign the renderers to be used in the preview");
                return;
            }

            Transform thisTransform = this.transform;
            if (renderers.Count == 0)
            {
                foreach (Renderer renderer in (renderers.Count == 0 ? new List<Renderer>(thisTransform.root.GetComponentsInChildren<Renderer>()) : renderers))
                {
                    dicRenderers.Add(renderer, renderer.gameObject.layer);
                    renderer.gameObject.layer = tempLayer;
                }
            }

            Camera cam = new GameObject().AddComponent<Camera>();
            Transform camTransform = cam.transform;
            camTransform.SetParent(thisTransform);
            cam.orthographic = true;
            cam.targetTexture = RenderTexture.GetTemporary(iconResolution, iconResolution, 16);
            cam.clearFlags = CameraClearFlags.Color;
            cam.stereoTargetEye = StereoTargetEyeMask.None;
            cam.orthographicSize = size / 2;
            cam.cullingMask = 1 << tempLayer;
            cam.enabled = false;
            camTransform.position = thisTransform.position + (thisTransform.forward * -1);
            camTransform.rotation = thisTransform.rotation;
            cam.allowMSAA = true;
            cam.allowHDR = true;
            var srp = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
            srp.msaaSampleCount = 8;
            //spawn spoy lights for a tri-light setup for photo taking
            //top light
            Light topLight = new GameObject().AddComponent<Light>();
            topLight.type = LightType.Spot;
            topLight.gameObject.transform.SetParent(thisTransform);
            topLight.transform.position = thisTransform.position + (-thisTransform.forward * 2f);
            //shuold look down
            topLight.transform.LookAt(thisTransform.position);
            topLight.intensity = pointLightIntensity;
            topLight.range = 4f;
            topLight.spotAngle = 90;
            topLight.innerSpotAngle = 90;
            Light leftLight = new GameObject().AddComponent<Light>();
            leftLight.type = LightType.Spot;
            leftLight.gameObject.transform.SetParent(thisTransform);
            leftLight.transform.position = thisTransform.position + (-thisTransform.right * 2f);
            leftLight.transform.LookAt(thisTransform.position);
            leftLight.intensity = pointLightIntensity;
            leftLight.range = 4f;
            leftLight.spotAngle = 90;
            leftLight.innerSpotAngle = 90;
            Light rightLight = new GameObject().AddComponent<Light>();
            rightLight.type = LightType.Spot;
            rightLight.gameObject.transform.SetParent(thisTransform);
            rightLight.transform.position = thisTransform.position + (thisTransform.right * 2f);
            rightLight.transform.LookAt(thisTransform.position);
            rightLight.intensity = pointLightIntensity;
            rightLight.range = 4f;
            rightLight.spotAngle = 90;
            rightLight.innerSpotAngle = 90;

            //ensure scene lighting is enabled
            SceneView.lastActiveSceneView.sceneLighting = true;
            //find the default skybox material
            Material skybox = AssetDatabase.LoadAssetAtPath<Material>("Assets/SDK/Examples/Skybox/ItemPreviewSkybox.mat");
            Material previousSky = RenderSettings.skybox;

            var ambientMode = RenderSettings.ambientMode;
            var aIntensity = RenderSettings.ambientIntensity;
            var ambientLight = RenderSettings.ambientLight;
            var ambientSkyColor =  RenderSettings.ambientSkyColor;
            var ambientEquatorColor = RenderSettings.ambientEquatorColor;
            var ambientGroundColor = RenderSettings.ambientGroundColor;
            var subtractiveShadowColor = RenderSettings.subtractiveShadowColor;
            var fog =  RenderSettings.fog;
            var defaultReflectionMode = RenderSettings.defaultReflectionMode;
            var reflectionBounces = RenderSettings.reflectionBounces;
            var reflectionIntensity = RenderSettings.reflectionIntensity;
            var defaultReflectionResolution = RenderSettings.defaultReflectionResolution;

            Configure();

            string iconPath = null;
            PrefabStage currentPrefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if (currentPrefabStage != null)
            {
                // Prefab editor
                iconPath = currentPrefabStage.assetPath;
                cam.scene = currentPrefabStage.scene;
            }
            else if (PrefabUtility.GetNearestPrefabInstanceRoot(this))
            {
                // Prefab in scene
                iconPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(this);
            }
            else
            {
                // Not a prefab -> don't run the rest of the method then?
                Debug.LogError("Unable to create an icon file if the object is not a prefab!");
                Restore();
                return;
            }

            cam.Render();
            RenderTexture orgActiveRenderTexture = RenderTexture.active;
            RenderTexture camTargetTexture = cam.targetTexture;
            RenderTexture.active = camTargetTexture;

            generatedIcon = new Texture2D(camTargetTexture.width, camTargetTexture.height, TextureFormat.ARGB32, false);
            generatedIcon.ReadPixels(new Rect(0, 0, camTargetTexture.width, camTargetTexture.height), 0, 0);
            generatedIcon.Apply(false);

            RenderTexture.active = orgActiveRenderTexture;

            // Clean up after ourselves
            cam.targetTexture = null;
            RenderTexture.ReleaseTemporary(camTargetTexture);

            foreach (KeyValuePair<Renderer, int> renderer in dicRenderers)
            {
                renderer.Key.gameObject.layer = renderer.Value;
            }

            DestroyImmediate(cam.gameObject);
            DestroyImmediate(topLight.gameObject);
            DestroyImmediate(leftLight.gameObject);
            DestroyImmediate(rightLight.gameObject);

            Restore();
            CreateIconFile();

            void CreateIconFile()
            {
                if (iconPath.IsNullOrEmptyOrWhitespace())
                { return; }

                Debug.Log($"Item path: {iconPath}");

                byte[] bytes = generatedIcon.EncodeToPNG();
                string path = iconPath.Replace(".prefab", closeUpPreview ? "-close-up.png" : ".png");

                System.IO.File.WriteAllBytes(path, bytes);
                AssetDatabase.Refresh();

                generatedIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

                //get the texture importer
                AssetImporter assetImporter = AssetImporter.GetAtPath(path);

                if (assetImporter is not TextureImporter textureImporter)
                {
                    Debug.LogError("Could not find AssetImporter, cannot set the icon to sprite");
                    return;
                }

                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.alphaIsTransparency = true;
                EditorUtility.SetDirty(textureImporter);
                textureImporter.SaveAndReimport();

                // Select asset.
                Selection.activeObject = generatedIcon;

                Debug.Log($"Icon generated: {path}");
                //get the file name from the path
                string fileName = System.IO.Path.GetFileNameWithoutExtension(path);

                AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
                string prefabEntryAddress = string.Empty;
                if (settings != null)
                {
                    string guid = AssetDatabase.AssetPathToGUID(path);
                    AddressableAssetEntry entry = settings.FindAssetEntry(guid);

                    if (entry == null)
                    {
                        Debug.Log("Added icon to addressable");
                        //get the itemIcons group
                        AddressableAssetGroup group = settings.DefaultGroup;
                        entry = settings.CreateOrMoveEntry(guid, group);
                        entry.labels.Clear();
                        entry.SetLabel("Windows", true, true);
                        entry.SetLabel("Android", true, true);
                        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryAdded, entry, true, false);
                    }

                    string prefabGuid = AssetDatabase.AssetPathToGUID(iconPath);
                    AddressableAssetEntry prefabEntry = settings.FindAssetEntry(prefabGuid);

                    if (prefabEntry != null)
                    {
                        prefabEntryAddress = prefabEntry.address + (closeUpPreview ? ".IconClose" : ".Icon");
                        entry.SetAddress(prefabEntryAddress, false);
                        Debug.Log("Icon Addressable: " + entry.address);
                    }
                    if (entry != null)
                    {
                        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryAdded, entry, true, false);
                    }
                }
                else
                {
                    Debug.LogError("Could not find AddressableAssetSettings, cannot set the icon to addressable");
                    return;
                }

                if (Application.isPlaying)
                { return; }
                if (!updateCatalog)
                { return; }
                //update the json if we can
                Catalog.EditorLoadAllJson(true, includeMods: false);

                if (item == null)
                {
                    Debug.LogError("Could not find Item component in the parent, cannot update the icon in the json");
                    return;
                }
                //get the item data
                if (string.IsNullOrEmpty(item.itemId))
                    return;
                if (!Catalog.TryGetData(item.itemId, out ItemData itemData))
                    return;
                //append the filename as a subaddress so we use the sprites addressable
                string subAddress = $"{prefabEntryAddress}[{fileName}]";
                if (closeUpPreview)
                {
                    itemData.closeUpIconAddress = subAddress;

                }
                else
                {
                    itemData.iconAddress = subAddress;
                }
                Catalog.SaveToJson(itemData);
            }

            // Config for best generation.
            void Configure()
            {
                RenderSettings.skybox = skybox;
                RenderSettings.ambientMode = AmbientMode.Trilight;
                RenderSettings.ambientIntensity = ambientIntensity;
                RenderSettings.ambientLight = Color.white * ambientIntensity;
                RenderSettings.ambientSkyColor = Color.white * ambientIntensity;
                RenderSettings.ambientEquatorColor = Color.white * (ambientIntensity / 2f);
                RenderSettings.ambientGroundColor = Color.white * ambientIntensity;
                RenderSettings.subtractiveShadowColor = Color.white;
                RenderSettings.fog = false;
                RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
                RenderSettings.reflectionBounces = 5;
                RenderSettings.reflectionIntensity = 1;
                RenderSettings.defaultReflectionResolution = 128;
            }

            // Restore previous config.
            void Restore()
            {
                EditorUtility.ClearProgressBar();

                RenderSettings.skybox = previousSky;
                RenderSettings.ambientMode = ambientMode;
                RenderSettings.ambientIntensity = aIntensity;
                RenderSettings.ambientLight = ambientLight;
                RenderSettings.ambientSkyColor = ambientSkyColor;
                RenderSettings.ambientEquatorColor = ambientEquatorColor;
                RenderSettings.ambientGroundColor = ambientGroundColor;
                RenderSettings.subtractiveShadowColor = subtractiveShadowColor;
                RenderSettings.fog = fog;
                RenderSettings.defaultReflectionMode = defaultReflectionMode;
                RenderSettings.reflectionBounces = reflectionBounces;
                RenderSettings.reflectionIntensity = reflectionIntensity;
                RenderSettings.defaultReflectionResolution = defaultReflectionResolution;
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