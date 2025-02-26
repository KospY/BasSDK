using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Serialization;
using UnityEngine.VFX;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class SkillTreeCrystal : ThunderBehaviour
    {
        public RagdollHand hand;
        public HandlePose floatHandPose;

        [Header("Merge VFx")]
        public AnimationCurve glowCurve;
        public float timeGlowTransition = 1.0f;
        public float timeVfxTransition = 2.0f;
        public float timeVfxTransitionMin = 0.5f;

        [FormerlySerializedAs("mergeVfx")]
#if ODIN_INSPECTOR
        [TabGroup("Windows VFX")]
#endif        
        public VisualEffect mergeVfxWindows;
        [FormerlySerializedAs("mergeVfxTarget")]
#if ODIN_INSPECTOR
        [TabGroup("Windows VFX")]
#endif
        public Transform mergeVfxTargetWindows;

        [FormerlySerializedAs("linkVfx")]
#if ODIN_INSPECTOR
        [TabGroup("Windows VFX")]
#endif
        public VisualEffect linkVfxWindows;
        [FormerlySerializedAs("linkVfxTarget")]
#if ODIN_INSPECTOR
        [TabGroup("Windows VFX")]
#endif
        public Transform linkVfxTargetWindows;
#if ODIN_INSPECTOR
        [TabGroup("Android VFX")]
#endif
        public VisualEffect mergeVfxAndroid;
#if ODIN_INSPECTOR
        [TabGroup("Android VFX")]
#endif
        public Transform mergeVfxTargetAndroid;
#if ODIN_INSPECTOR
        [TabGroup("Android VFX")]
#endif
        public VisualEffect linkVfxAndroid;
#if ODIN_INSPECTOR
        [TabGroup("Android VFX")]
#endif
        public Transform linkVfxTargetAndroid;

#if UNITY_EDITOR
        [Button]
        public void CopyVfxToAndroid()
        {
            var clone = Instantiate(linkVfxWindows.transform.parent, linkVfxWindows.transform.parent.parent);
            clone.name = "VFX_Android";

            mergeVfxAndroid = clone.Find("vfx_crystal_merge_point1").GetComponent<VisualEffect>();
            mergeVfxTargetAndroid = mergeVfxAndroid.transform.Find("Target");
            mergeVfxAndroid.visualEffectAsset = AssetDatabase.LoadAssetAtPath<VisualEffectAsset>(
                AssetDatabase.GUIDToAssetPath("8661053b521d2844b9d1683386d25e12"));
            mergeVfxAndroid.SetVector4("Color", Utils.UnHDR(mergeVfxWindows.GetVector4("Source Color")));
            
            linkVfxAndroid = clone.Find("vfx_crystal_link").GetComponent<VisualEffect>();
            linkVfxTargetAndroid = linkVfxAndroid.transform.Find("Target");
            linkVfxAndroid.visualEffectAsset = AssetDatabase.LoadAssetAtPath<VisualEffectAsset>(
                AssetDatabase.GUIDToAssetPath("a268070ac4c448a4a9ade95f605d8cda"));
            linkVfxAndroid.SetVector4("Color", Utils.UnHDR(linkVfxWindows.GetVector4("Source Color")));
        }
#endif        
        protected VisualEffect mergeVfx;
        protected Transform mergeVfxTarget;

        protected VisualEffect linkVfx;
        protected Transform linkVfxTarget;
        
        public float linkMaxDistance = 2.0f;



        [Header("Skill tree")]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllSkillTreeID), AppendNextDrawer = true)]
#endif
        public string treeName;

        [ColorUsage(true, true)]
        public Color skillTreeEmissionColor = Color.white;


        [Header("Custom")]
        public bool overrideCrystalColors = false;

        [ColorUsage(true, true), ShowIf("overrideCrystalColors")]
        public Color baseColor;
        [ColorUsage(true, true), ShowIf("overrideCrystalColors")]
        public Color internalColor;
        [ColorUsage(true, true), ShowIf("overrideCrystalColors")]
        public Color animatedColor;
        [ColorUsage(true, true), ShowIf("overrideCrystalColors")]
        public Color emissionColor;
        [ColorUsage(true, true), ShowIf("overrideCrystalColors")]
        public Color linkVfxColor;
        [ColorUsage(true, true), ShowIf("overrideCrystalColors")]
        public Color mergeVfxColor;



#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllSkillTreeID()
        {
            return Catalog.gameData == null ? new List<ValueDropdownItem<string>>() : Catalog.GetDropdownAllID(Category.SkillTree);
        }
#endif
    }
}
