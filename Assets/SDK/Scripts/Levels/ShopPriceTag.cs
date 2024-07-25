using UnityEngine;
using TMPro;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class ShopPriceTag : MonoBehaviour
    {
        public TMP_Text textPrice;
        public TMP_Text tierNumberText;
        public UIText tierUiText;
        public Image tierBackground;

    }
}