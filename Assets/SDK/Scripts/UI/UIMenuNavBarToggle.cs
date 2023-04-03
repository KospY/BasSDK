using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIMenuNavBarToggle : MonoBehaviour
    {
        public UICustomisableButton button;
        [SerializeField] private UIText title;

        [SerializeField] private Image frame;
        [SerializeField] private Image frameSelected;
        [SerializeField] private Sprite medievalFrame;
        [SerializeField] private Sprite medievalFrameSelected;
        [SerializeField] private Sprite modernFrame;
        [SerializeField] private Sprite modernFrameSelected;

        [SerializeField] private RawImage icon;
        [SerializeField] private RawImage iconSelected;

        [SerializeField] private Image warning;

        private UIMenu menu;
        
        private Texture medievalIcon;
        private Texture medievalIconSelected;
        private Texture modernIcon;
        private Texture modernIconSelected;
   
    }
 
}