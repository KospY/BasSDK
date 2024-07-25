using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SpellCastTest : SpellCastCharge
    {
        [BoxGroup("Item"), ValueDropdown("GetAllItemID")]
        public string itemId;

        [BoxGroup("Explosion"), ValueDropdown(nameof(GetAllSkillID))]
        public string remoteDetonationId = "RemoteDetonation";

        public static SkillRemoteDetonation remoteDetonationData;

        [NonSerialized]
        public ItemData itemData;

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            remoteDetonationData = Catalog.GetData<SkillRemoteDetonation>(remoteDetonationId);
            itemData = Catalog.GetData<ItemData>(itemId);
        }

        protected Item cup;

    }
}
