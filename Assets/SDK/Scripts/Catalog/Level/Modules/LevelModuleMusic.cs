using System.Collections;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class LevelModuleMusic : LevelModule
    {
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllMusicID")] 
#endif
        public string dynamicMusic = "";

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllMusicID()
        {
            return Catalog.GetDropdownAllID(Category.Music);
        } 
#endif

 //ProjectCore
    }
}
