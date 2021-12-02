using UnityEngine;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class UIText : MonoBehaviour
    {
        [Multiline]
        public string text;
        public bool forceUpperCase;

    }
}
