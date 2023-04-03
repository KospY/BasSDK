namespace ThunderRoad
{
    using UnityEngine;


    public class AreaGameObjectDisableOnHide : MonoBehaviour
    {
        #region SerializedFields
        [SerializeField] private Area _area = null;
        [SerializeField] private GameObject[] _gameObjectToDisable = null;
        #endregion SerializedFields

        #region Method
        void Awake()
        {
            if (AreaManager.Instance != null && _area == null)
            {
                _area = GetComponentInParent<Area>();
            }

            if (_area != null
                && _gameObjectToDisable != null
                && _gameObjectToDisable.Length > 0)
            {
                _area.onHideChange += OnAreaHide;
            }
        }

        private void OnDestroy()
        {
            if (_area != null
                && _gameObjectToDisable != null
                && _gameObjectToDisable.Length > 0)
            {
                _area.onHideChange -= OnAreaHide;
            }
        }

        public void OnAreaHide(bool isHidden)
        {
            for (int i = 0; i < _gameObjectToDisable.Length; i++)
            {
                if (_gameObjectToDisable[i] != null)
                {
                    _gameObjectToDisable[i].SetActive(!isHidden);
                }
            }
        }
        #endregion Method

        #region Tools
#if UNITY_EDITOR
        public void SetComponentToHide(GameObject[] gameObject)
        {
            _gameObjectToDisable = gameObject;
        }
        public void SetArea(Area area)
        {
            _area = area;
        }
#endif //UNITY_EDITOR
        #endregion Tools
    }
}
