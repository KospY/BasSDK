using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	public class ItemModuleSpell : ItemModule
    {
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllSpellID")] 
#endif
        public string spellId;
        [NonSerialized]
        public SpellData spellData;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllSpellID()
        {
            return Catalog.GetDropdownAllID(Category.Spell);
        } 
#endif

    }
}
