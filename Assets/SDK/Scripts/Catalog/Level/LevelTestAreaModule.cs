using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif


namespace ThunderRoad
{
    public class LevelTestAreaModule : LevelModule
    {
        #region Constants
        private const string AREA_MANAGER_ROOT_NAME = "AreaManager";
        #endregion Constants

        #region PublicFields
        public List<IAreaBlueprintGenerator.IAreaBlueprintGeneratorIdContainer> areaListToConnect;

        #endregion PublicFields

        #region Methods

        #endregion Methods
    }
}
