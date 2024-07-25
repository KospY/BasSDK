using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [System.Serializable]
    public abstract class StanceNode
    {
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, HideLabel, PreviewField(ObjectFieldAlignment.Left, Height = 60f), FoldoutGroup("$prettifiedID", expanded: true), HorizontalGroup("$prettifiedID/Horiz", 65f)]
#if UNITY_EDITOR
        [OnValueChanged(nameof(EditorUpdateAddress))]
#endif
#endif
        public AnimationClip animationClip;
        [JsonMergeKey]
#if ODIN_INSPECTOR
        [LabelWidth(35), EnableIf("customID"), HorizontalGroup("$prettifiedID/Horiz"), VerticalGroup("$prettifiedID/Horiz/Fields")]
#endif
        public string id = "[New stance animation]";
#if ODIN_INSPECTOR
        [LabelWidth(65), HorizontalGroup("$prettifiedID/Horiz"), VerticalGroup("$prettifiedID/Horiz/Fields")]
#endif
        public string address;
#if ODIN_INSPECTOR
        [LabelWidth(125), HorizontalGroup("$prettifiedID/Horiz"), VerticalGroup("$prettifiedID/Horiz/Fields"), HorizontalGroup("$prettifiedID/Horiz/Fields/Row3", Order = 0)]
#endif
        public float animationSpeed = 1;
#if ODIN_INSPECTOR
        [LabelWidth(75), HorizontalGroup("$prettifiedID/Horiz"), VerticalGroup("$prettifiedID/Horiz/Fields"), HorizontalGroup("$prettifiedID/Horiz/Fields/Row3", Order = 1), ShowIf("showDifficulty")]
#endif
        public int difficulty;
#if ODIN_INSPECTOR
        [LabelWidth(135), HorizontalGroup("$prettifiedID/Horiz"), VerticalGroup("$prettifiedID/Horiz/Fields"), HorizontalGroup("$prettifiedID/Horiz/Fields/Row3", Order = 3, Width = 150)]
#endif
        public bool allowPlayAndMove = true;
#if ODIN_INSPECTOR
        [LabelWidth(75), HorizontalGroup("$prettifiedID/Horiz"), VerticalGroup("$prettifiedID/Horiz/Fields"), HorizontalGroup("$prettifiedID/Horiz/Fields/Row4"), ShowIf("showWeight")]
#endif
        public float weight;

        public StanceData stanceData;
        [NonSerialized]
        public bool populated = false;

        protected bool isPlaying => Application.isPlaying;

        public virtual bool customID => true;

        public virtual bool showDifficulty => false;

        public virtual bool showWeight => false;

#if ODIN_INSPECTOR
        protected string prettifiedID => Utils.AddSpacesToSentence(id, true);

#if UNITY_EDITOR
        protected void EditorUpdateAddress() => address = animationClip ? Catalog.GetAddressFromPrefab(animationClip) : "";
#endif
#endif

    }
}
