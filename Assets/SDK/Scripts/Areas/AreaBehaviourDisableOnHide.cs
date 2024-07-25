namespace ThunderRoad
{
    using System.Collections.Generic;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#else
    using TriInspector;
#endif


    public class AreaBehaviourDisableOnHide : MonoBehaviour
    {
        #region SerializedFields
        [SerializeField] Area _area = null;
        [SerializeField] private Behaviour[] _behaviourToDisable = null;
        #endregion SerializedFields

        #region Method


#endregion Method

        #region Tools
 //UNITY_EDITOR
        #endregion Tools
    }
}