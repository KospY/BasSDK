using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/RenderCubemap")]
    [AddComponentMenu("ThunderRoad/Levels/Preview Cubemap")]
    public class RenderCubemap : MonoBehaviour
    {
#if UNITY_EDITOR
        public string assetBundleName;
        public int size = 128;
        public UnityEngine.Experimental.Rendering.DefaultFormat defaultFormat = UnityEngine.Experimental.Rendering.DefaultFormat.LDR;
        public UnityEngine.Experimental.Rendering.TextureCreationFlags textureCreationFlags = UnityEngine.Experimental.Rendering.TextureCreationFlags.None;

        void OnValidate()
        {
            if (assetBundleName == null || assetBundleName == "") assetBundleName = this.gameObject.scene.name + "Preview";
        }

        [Button("Create level preview")]
        void CreateCubemap()
        {
            Cubemap cubemap = new Cubemap(size, defaultFormat, textureCreationFlags);
            Camera cam = this.gameObject.AddComponent<Camera>();
            cam.RenderToCubemap(cubemap);
            string cubemapPath = this.gameObject.scene.path.Replace(".unity", ".cubemap");
            AssetDatabase.CreateAsset(cubemap, cubemapPath);
            AssetImporter.GetAtPath(cubemapPath).SetAssetBundleNameAndVariant(assetBundleName, "");
            UnityEngine.Rendering.Universal.UniversalAdditionalCameraData cameraData = cam.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
            if (cameraData) DestroyImmediate(cameraData);
            DestroyImmediate(cam);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Debug.Log("Cubemap created in " + cubemapPath);
        }
#endif
    }
}