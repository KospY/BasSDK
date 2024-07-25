using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIStatIcons : MonoBehaviour
    {
        public string filledStarAddress;
        public string emptyStarAddress;
        
        public IStats stat;
        public TMP_Text statName;
        public TMP_Text textValue;
        public GameObject starIconsGameObject;
        public Image[] starIcons;

    }
}
