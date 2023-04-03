namespace ThunderRoad
{
    using System.Collections.Generic;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#else
    using EasyButtons;
#endif


    public class AreaBehaviourDisableOnHide : MonoBehaviour
    {
        #region SerializedFields
        [SerializeField] Area _area = null;
        [SerializeField] private Behaviour[] _behaviourToDisable = null;
        #endregion SerializedFields

        #region Method
        void Awake()
        {
            if (AreaManager.Instance != null && _area == null)
            {
                _area = GetComponentInParent<Area>();
            }

            if (_area != null
                && _behaviourToDisable != null
                && _behaviourToDisable.Length > 0)
            {
                _area.onHideChange += OnAreaHide;
            }
        }

        private void OnDestroy()
        {
            if (_area != null
                && _behaviourToDisable != null
                && _behaviourToDisable.Length > 0)
            {
                _area.onHideChange -= OnAreaHide;
            }
        }

        public void OnAreaHide(bool isHidden)
        {
            for (int i = 0; i < _behaviourToDisable.Length; i++)
            {
                if (_behaviourToDisable[i] != null)
                {
                    _behaviourToDisable[i].enabled = !isHidden;
                }
            }
        }
        #endregion Method

        #region Tools
#if UNITY_EDITOR

        public List<string> typeOfComponentToDisable = null;

        [Button]
        public void SetComponentFromType()
        {
            List<Behaviour> componentToDisable = new List<Behaviour>();

            if (typeOfComponentToDisable != null
                && typeOfComponentToDisable.Count > 0)
            {
                for (int i = 0; i < typeOfComponentToDisable.Count; i++)
                {                    
                    if (GetComponent(typeOfComponentToDisable[i]) is Behaviour behaviour)
                    {
                        componentToDisable.Add(behaviour);
                    }
                }

                foreach (Transform child in transform)
                {
                    for (int i = 0; i < typeOfComponentToDisable.Count; i++)
                    {
                        if (child.GetComponent(typeOfComponentToDisable[i]) is Behaviour monoBehaviour)
                        {
                            componentToDisable.Add(monoBehaviour);
                        }
                    }
                }
            }

            if (componentToDisable.Count > 0)
            {
                _behaviourToDisable = componentToDisable.ToArray();
            }
        }

        public void SetComponentToHide(Behaviour[] behaviours)
        {
            _behaviourToDisable = behaviours;
        }

        public void SetArea(Area area)
        {
            _area = area;
        }
#endif //UNITY_EDITOR
        #endregion Tools
    }
}