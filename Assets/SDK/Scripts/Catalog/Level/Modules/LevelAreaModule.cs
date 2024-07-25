using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif


namespace ThunderRoad
{
    public class LevelAreaModule : LevelModule
    {
        #region Constants

        private const string AREA_MANAGER_ROOT_NAME = "AreaManager";

        #endregion Constants

        #region InternalClass

        [Serializable]
        public class AreaSpawnerData
        {
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllAreaCollectionID))]
#endif
            public string areaCollectionId = "";
            public int numberCreature;
            public bool isSharedNPCAlert;
#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllAreaCollectionID()
            {
                return Catalog.GetDropdownAllID(Category.AreaCollection);
            }
#endif
        }

        #endregion InternalClass

        #region PublicFields

        public int areaFullMemoryDepth = 100;
        public int areaLiteMemoryDepth = 100;
        public int areaCullDepth = 2;

        public List<AreaSpawnerData> areaByLength;

        #endregion PublicFields

        #region Methods

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllAreaID()
        {
            return Catalog.GetDropdownAllID(Category.Area);
        }
#endif


        #endregion Methods
    }
}
