using UnityEngine;

namespace ThunderRoad
{
    public class AreaGameObjectDisableOnInitialization : MonoBehaviour
    {
        public Area _area = null;
        private void Awake()
        {
            if(_area == null) _area = GetComponentInParent<Area>();
            if (_area == null) return;
            if (!_area.IsInitialized)
            {
                gameObject.SetActive(false);
                _area.onPlayerEnter.AddListener(OnAreaEnter);
            }
        }

        private void OnDestroy()
        {
            if (_area)
            {
                _area.onPlayerEnter.RemoveListener( OnAreaEnter);
            }
        }

        private void OnAreaEnter()
        {
            // We do this only the first time the player enter
            if (_area)
            {
                _area.onPlayerEnter.RemoveListener(OnAreaEnter);
            }

            gameObject.SetActive(true);
        }
    }
}