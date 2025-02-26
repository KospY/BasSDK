using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "ThunderRoad/Creatures/Face Property Binding")]
    public class FacePropertyBindings : ScriptableObject
    {
#if ODIN_INSPECTOR
        [TableList(AlwaysExpanded = true, DrawScrollView = true)]
#endif
        public List<FaceAnimator.SerializeableBinding> allBindables;
    }
#endif
}
