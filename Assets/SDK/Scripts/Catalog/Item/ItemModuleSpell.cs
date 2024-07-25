using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	public class ItemModuleSpell : ItemModule
    {
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllSpellID))] 
#endif
        public string spellId;
        [NonSerialized]
        public SpellData spellData;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllSpellID()
        {
            return Catalog.GetDropdownAllID<SpellData>();
        } 
#endif

    }
}
