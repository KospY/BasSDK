using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill
{
    public class SkillIntimidation : SkillData
    {
        // to be used to temporarily add a modifier to a fear value
        public static Vector2Int scareModifier = new Vector2Int();

#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Intimidating kills")]
#endif
        public float intimidateRadius = 6f;

#if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup("Intimidating kills/Horiz1", Width = 0.5f, PaddingRight = 10f)]
        [InlineProperty]
        [HideLabel]
        [Title("Generic kill")]
#endif
        public FearAction genericKill = new FearAction();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup("Intimidating kills/Horiz1", Width = 0.5f)]
        [InlineProperty]
        [HideLabel]
        [Title("Dismember kill")]
#endif
        public FearAction dismember = new FearAction();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup("Intimidating kills/Horiz2", Width = 0.5f, PaddingRight = 10f)]
        [InlineProperty]
        [HideLabel]
        [Title("Decapitate kill")]
#endif
        public FearAction decapitate = new FearAction();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup("Intimidating kills/Horiz2", Width = 0.5f)]
        [InlineProperty]
        [HideLabel]
        [Title("Burn kill")]
#endif
        public FearAction burnKill = new FearAction();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup("Intimidating kills/Horiz3", Width = 0.5f, PaddingRight = 10f)]
        [InlineProperty]
        [HideLabel]
        [Title("Electrocution kill")]
#endif
        public FearAction electrocutionKill = new FearAction();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup("Intimidating kills/Horiz3", Width = 0.5f)]
        [InlineProperty]
        [HideLabel]
        [Title("Impact kill")]
#endif
        public FearAction impactKill = new FearAction();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup("Intimidating kills/Horiz4", Width = 0.5f, PaddingRight = 10f)]
        [InlineProperty]
        [HideLabel]
        [Title("Impale kill")]
#endif
        public FearAction impaleKill = new FearAction();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup("Intimidating kills/Horiz4", Width = 0.5f)]
        [InlineProperty]
        [HideLabel]
        [Title("Limb tear kill")]
#endif
        public FearAction ripping = new FearAction();

        [System.Serializable]
        public class FearAction
        {
#if ODIN_INSPECTOR && UNITY_EDITOR
            [HorizontalGroup("Fields", PaddingLeft = 20f, PaddingRight = 10f)]
            [MinMaxSlider(0, 10, ShowFields = true)]
            [LabelWidth(165f)]
#endif
            public Vector2Int minMaxCreaturesScared = new Vector2Int(1, 2);
#if ODIN_INSPECTOR && UNITY_EDITOR
            [HorizontalGroup("Fields")]
            [LabelWidth(65f)]
#endif
            public bool alliesOnly = true;

        }

    }
}
