//#define SGMARKDOWN

#if UNITY_2021_2_OR_NEWER
#define HAVE_VALIDATE_MATERIAL
#endif

using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

#if SGMARKDOWN

public class ShaderSorceryInspector : Needle.MarkdownShaderGUI
{
    protected new virtual void MaterialChanged(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        Debug.Log("ShaderSorceryInspector: Caught: MaterialChanged");

        foreach (var mat in materialEditor.targets)
        {
            Material material = (Material) mat;
            ShadowoodMaterialValidation(material);
        }

        base.MaterialChanged(materialEditor, properties);
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        var targetMat = materialEditor.target as Material;
        if (!targetMat) return;

        if (targetMat.HasProperty("_UseReveal")) Reveal(properties);
        base.OnGUI(materialEditor, properties);
    }

    //[MenuItem("ThunderRoad (SDK)/EnforceShaderKeywordLogic")]
    public static void EnforceShaderKeywordLogic()
    {
        int count = 0;

        // Find all materials in the project
        string[] guids = AssetDatabase.FindAssets("t:Material");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (material != null && material.shader != null)
            {
                // Only apply this to specific shaders
                if (!material.shader.name.Contains("LitMoss -") && !material.shader.name.Contains("Foliage -")) continue;

                var changed = ShadowoodMaterialValidation(material);
                if (changed)
                {
                    EditorUtility.SetDirty(material); // Mark as dirty so it can be saved
                    Debug.Log("Shader keywords changed: " + material.name, material);
                    count++;
                }
            }
        }

        if (count > 0)
        {
            // Save all changes to disk
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        Debug.Log("Shader keywords enforced on (" + count + ") materials.");
    }


    // Shadowood: Unity changed it from "MaterialChanged" to "ValidateMaterial"
#if HAVE_VALIDATE_MATERIAL
    public override void ValidateMaterial(Material material)
    {
#else
    public void ValidateMaterial(Material material)
    {

#endif
        base.ValidateMaterial(material);
        ShadowoodMaterialValidation(material);
    }

    private new static bool ShadowoodMaterialValidation(Material material)
    {
        // Shadowood: handled transparent/opaque dropdowns and blend modes

        //var original = String.Join(", ", material.enabledKeywords.Select(o => o.name)).GetHashCode(); //material.enabledKeywords.ToList().Select(o=>o.name).ToString().GetHashCode();

        BaseShaderGUI.SurfaceType surfaceType = BaseShaderGUI.SurfaceType.Opaque;
        if (material.HasFloat("_Surface")) surfaceType = (BaseShaderGUI.SurfaceType) material.GetFloat("_Surface"); // UnityEditor.Rendering.Universal.Property.SurfaceType

        float alphaClip = 0;
        if (material.HasFloat("_AlphaClip")) alphaClip = material.GetFloat("_AlphaClip");


        //if (alphaClip == 0) material.SetFloat("_AlphaClipThreshold",0);
        if (material.HasFloat("_Surface"))
        {
            BaseShaderGUI.SetMaterialKeywords(material); // Also calls SetupMaterialBlendMode()
        }

        if (surfaceType == BaseShaderGUI.SurfaceType.Transparent || alphaClip == 1)
        {
            material.EnableKeyword("_ALPHATEST_ON");
        }
        else
        {
            material.DisableKeyword("_ALPHATEST_ON");
        }

        if (material.HasProperty("_BumpMap"))
        {
            if (material.GetTexture("_BumpMap") == null)
            {
                //if (material.HasProperty("_UseNormalMap")) material.SetFloat("_UseNormalMap", 0); // Removed, would disable and thus hide the drop well for a normal map making it impossible to add one
            }
            else
            {
                //material.SetFloat("_UseNormalMap", 1);
            }
        }

        if (material.HasProperty("_Moss"))
        {
            bool mossEnabled = material.GetFloat("_Moss") > 0.5f;

            if (!mossEnabled)
            {
                material.DisableKeyword("_USEMOSSVERTEXMASK_ON");
                material.DisableKeyword("_USESTOCHASTICMOSS_ON");
                material.DisableKeyword("_USEMOSSFRESNEL_ON");
            }
            else
            {
                if (material.HasProperty("_UseMossVertexMask") && material.GetFloat("_UseMossVertexMask") > 0.5f) material.EnableKeyword("_USEMOSSVERTEXMASK_ON");
                if (material.HasProperty("_UseMossFresnel") && material.GetFloat("_UseMossFresnel") > 0.5f) material.EnableKeyword("_USEMOSSFRESNEL_ON");
                if (material.HasProperty("_UseStochasticMoss") && material.GetFloat("_UseStochasticMoss") > 0.5f) material.EnableKeyword("_USESTOCHASTICMOSS_ON");
            }

            //material.EnableKeyword("_USEMOSSVERTEXMASK_ON");
        }

        //var changed = String.Join(", ", material.enabledKeywords.Select(o => o.name)).GetHashCode();
        //return changed != original; // Doesnt work, so just always return true for now

        return true;

        //if (surfaceType == BaseShaderGUI.SurfaceType.Opaque)
        //{
        //    var blendOpaque = material.GetFloat("_BlendOpaque");
        //    //BaseShaderGUI.SetupMaterialBlendMode(material);
        //}
    }

    void Reveal(MaterialProperty[] properties)
    {
        MaterialProperty useReveal = ShaderGUI.FindProperty("_UseReveal", properties);
        MaterialProperty layerMask = ShaderGUI.FindProperty("_LayerMask", properties);
        MaterialProperty layer0 = ShaderGUI.FindProperty("_Layer0", properties);
        MaterialProperty layer0NormalMap = ShaderGUI.FindProperty("_Layer0NormalMap", properties);
        MaterialProperty layer1 = ShaderGUI.FindProperty("_Layer1", properties);
        MaterialProperty layer1NormalMap = ShaderGUI.FindProperty("_Layer1NormalMap", properties);
        MaterialProperty bumpMap = ShaderGUI.FindProperty("_BumpMap", properties);


        //if (useReveal.floatValue > 0.5)
        {
            var keyName = "Reveal Layers";
            var state = SessionState.GetBool(keyName, true);
            var newState = CoreEditorUtils.DrawHeaderFoldout("Reveal Layers", state);

            if (newState != state) SessionState.SetBool(keyName, newState);
            state = newState;
            if (state)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                ///////

                EditorGUILayout.BeginVertical("HelpBox");

                if (GUILayout.Button("[Weapon] Auto-fill layer materials"))
                {
                    layerMask.textureValue = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/SDK/Examples/Reveal/Reveal_WeaponBlood_Mask.png", typeof(Texture2D));
                    layer0.textureValue = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/SDK/Examples/Reveal/Revealed_WeaponBlood_c.png", typeof(Texture2D));
                    layer0NormalMap.textureValue = bumpMap.textureValue;

                    layer1.textureValue = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/SDK/Examples/Reveal/Revealed_Burn_c.png", typeof(Texture2D));
                    layer1NormalMap.textureValue = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/SDK/Examples/Reveal/Revealed_Burn_n.png", typeof(Texture2D));
                }

                if (GUILayout.Button("[Armor] Auto-fill layer materials"))
                {
                    //armor should use a custom layer mask based on the albedo texture
                    //layerMask.textureValue = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SDK/Examples/Reveal/Reveal_WeaponBlood_Mask.png", typeof(Texture2D));
                    layer0.textureValue = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/SDK/Examples/Reveal/Revealed_Flesh_c.png", typeof(Texture2D));
                    layer0NormalMap.textureValue = bumpMap.textureValue;

                    layer1.textureValue = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/SDK/Examples/Reveal/Revealed_Burn_c.png", typeof(Texture2D));
                    layer1NormalMap.textureValue = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/SDK/Examples/Reveal/Revealed_Burn_n.png", typeof(Texture2D));
                }

                if (GUILayout.Button("Remove layer materials"))
                {
                    layerMask.textureValue = null;
                    layer0.textureValue = null;
                    layer0NormalMap.textureValue = null;

                    layer1.textureValue = null;
                    layer1NormalMap.textureValue = null;
                }

                EditorGUILayout.EndVertical();

                ///////

                EditorGUI.indentLevel--;
            }
        }
    }
}
#else
public class ShaderSorceryInspector : RobProductions.OpenGraphGUI.Editor.OpenGraphGUIEditor{}
#endif
