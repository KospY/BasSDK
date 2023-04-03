using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	public class LevelModuleTutorial : LevelModule
    {
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllTutorialID")] 
#endif
        public string tutorialID;

        public string uiMaterialAddress = "Bas.Material.UI.Gamepad";
        public bool launchOnStart;

        public float delayToStart = 3.0f;

        public string safeWeaponReferenceID = "";

        public float drivePositionSpring = 100;
        public float drivePositionDamper = 2;
        public float slerpPositionSpring = 100;
        public float slerpPositionDamper = 0;
        public float spawnPositionHeight = 2;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllTutorialID()
        {
            return Catalog.GetDropdownAllID(Category.Tutorial);
        } 
#endif

    }
}
