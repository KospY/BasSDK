using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;
using UnityEngine.Serialization;

namespace ThunderRoad.AssetSorcery
{
    /// <summary>
    /// See: https://docs.unity3d.com/Manual/SL-MultipleProgramVariants.html
    /// </summary>
    [Serializable]
    public class AssetSorceryShaderKeyword
    {
        [HideInInspector] public string displayName;

        [ReadOnly] public string line;

        [HideInInspector] public string replace;

        public string replaceDesktop;
        public string replaceMobile;
        public string replaceEditor;

        //[HideInInspector]
        [ReadOnly] public string keywordName;

        //[HideInInspector]
        [ReadOnly] public string keywordLast;

        [ReadOnly] public eKeywordType keywordType;

        public eKeywordType keywordTypeReplace;

        //[HideInInspector]
        [ReadOnly] public eKeywordDomain keywordDomain;

        // [HideInInspector]
        [ReadOnly] public bool local = false;

        [FormerlySerializedAs("enabled")]
        public bool enabledDesktop = true;
        public bool enabledMobile = true;
        
        [Space]
        public bool hideDesktop = false;
        public bool hideMobile = false;
        
        [Space]
        //public bool disableUIToggle = false;
        [Tooltip("If the keyword this pertains to is removed from the shader, the entry here remains but 'present' will be unticked.")]
        public bool present = false;

        [Tooltip("User commentary")] [HideInInspector]
        public string comment;

        [Tooltip("Instruction count")] [HideInInspector]
        public float instructions;

#if ODIN_INSPECTOR
        [MultiLineProperty(6)] [HideInInspector]
#endif
        public string mali;

        //

        public enum eKeywordType
        {
            none,
            shader_feature,
            multi_compile,
            multi_compile_fog
        }

        public enum eKeywordDomain
        {
            none,
            _vertex,
            _fragment,
            _hull,
            _domain,
            _geometry,
            _raytracing
        }

        public void DisplayNameCalc()
        {
            var foundItem = this;

            var keyDomainText = " : " + keywordDomain.ToString();
            if (keyDomainText.Contains("_")) keyDomainText = keyDomainText.Replace("_", " ");

            var keyTypeText = keywordType.ToString();
            if (keywordTypeReplace != eKeywordType.none && keywordTypeReplace != keywordType)
            {
                keyTypeText = keyTypeText + " -> " + keywordTypeReplace;
            }

            if (keyTypeText.Contains("_")) keyTypeText = keyTypeText.Replace("_", " ");

            if (keywordDomain == AssetSorceryShaderKeyword.eKeywordDomain.none) keyDomainText = "";

            string keywordEndText = keywordName;
            if (keywordEndText.Contains("__ ")) keywordEndText = keywordEndText.Replace("__ ", "");
            if (keywordEndText.Contains("_ ")) keywordEndText = keywordEndText.Replace("_ ", "");
            if (keywordEndText.Contains("_")) keywordEndText = keywordEndText.Replace("_", " ");

            string instructionsCount = "";
            if (instructions != 0)
            {
                instructionsCount = " (" + instructions.ToString() + ")";
            }

            var o = foundItem;
            if (!string.IsNullOrEmpty(o.replace))
            {
                o.replaceDesktop = o.replace;
                o.replaceMobile = o.replace;
                o.replace = "";
            }

            var nameDesk = "";
            var nameMobile = "";

            nameDesk = (enabledDesktop ? "ON - " : "OFF - ");
            nameMobile = (foundItem.enabledMobile ? "ON - " : "OFF - ");
            if ((foundItem.enabledDesktop && !string.IsNullOrEmpty(foundItem.replaceDesktop))) nameDesk = "REP - ";
            if ((foundItem.enabledMobile && !string.IsNullOrEmpty(foundItem.replaceMobile))) nameMobile = "REP - ";


            var prefix = (foundItem.present ? "" : " -- ") + nameDesk + nameMobile;

            foundItem.displayName = prefix + keywordEndText + " : " + keyTypeText + keyDomainText + instructionsCount;
        }

        public bool GetEnabled()
        {
            switch (AssetSorceryPlatform.AssetSorceryGetPlatformCalculated())
            {
                case AssetSorceryPlatformRuntime.ePlatformAS.Desktop:
                    return enabledDesktop;
                    break;
                case AssetSorceryPlatformRuntime.ePlatformAS.Mobile:
                    return enabledMobile;
                    break;
            }

            return false;
        }
    }
}
