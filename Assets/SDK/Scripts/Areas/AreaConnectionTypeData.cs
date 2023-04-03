namespace ThunderRoad
{
    using System;
    using UnityEngine;

    public class AreaConnectionTypeData : CatalogData
    {
        [Serializable] public class PrefabAdressTable : DropTable<string> { };

        public Vector2 size;
        public PrefabAdressTable randomBlockerTableAdress;
        public PrefabAdressTable randomGateTableAdress;

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            if (randomBlockerTableAdress != null) randomBlockerTableAdress.CalculateWeight();
            if (randomGateTableAdress != null) randomGateTableAdress.CalculateWeight();
        }
    }
}
