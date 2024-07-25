using UnityEngine;
using UnityEngine.Rendering.Universal;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Misc/RenderCubemap.html")]
    [AddComponentMenu("ThunderRoad/Levels/Preview Cubemap")]
    public class RenderCubemap : MonoBehaviour
    {
        public string assetBundleName;
        public int size = 128;
        public UnityEngine.Experimental.Rendering.DefaultFormat defaultFormat = UnityEngine.Experimental.Rendering.DefaultFormat.LDR;
        public UnityEngine.Experimental.Rendering.TextureCreationFlags textureCreationFlags = UnityEngine.Experimental.Rendering.TextureCreationFlags.None;

        void OnValidate()
        {
            if (!gameObject.activeInHierarchy)
                return;
            if (assetBundleName == null || assetBundleName == "")
                assetBundleName = this.gameObject.scene.name + "Preview";
        }

        [Button("Create level preview")]
        public void CreateCubemap(bool skillCubemap = false)
        {
            Cubemap cubemap = new Cubemap(size, defaultFormat, textureCreationFlags);
            Camera cam = gameObject.AddComponent<Camera>();
            var cameraData = cam.GetOrAddComponent<UniversalAdditionalCameraData>();


            cam.RenderToCubemap(cubemap);

#if UNITY_EDITOR
            string cubemapPath;
            if (skillCubemap)
            {
                cubemapPath = $"Assets/Private/Bas-Assets/UI/SkillCubemaps/{gameObject.scene.name}.cubemap";
                var i = 1;
                while (AssetDatabase.LoadAssetAtPath<Cubemap>(cubemapPath) != null)
                {
                    cubemapPath = $"Assets/Private/Bas-Assets/UI/SkillCubemaps/{gameObject.scene.name}{i}.cubemap";
                    i++;
                }
            }
            else
            {
                cubemapPath = this.gameObject.scene.path.Replace(".unity", ".cubemap");
            }

            UnityEditor.AssetDatabase.CreateAsset(cubemap, cubemapPath);
            if (!skillCubemap)
            { UnityEditor.AssetImporter.GetAtPath(cubemapPath).SetAssetBundleNameAndVariant(assetBundleName, ""); }
#endif

            if (cameraData)
            { DestroyImmediate(cameraData); }
            DestroyImmediate(cam);

#if UNITY_EDITOR
            EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            Debug.Log("Cubemap created in " + cubemapPath);
#else
            Debug.Log("Cubemap created!");
#endif
        }
    }
}