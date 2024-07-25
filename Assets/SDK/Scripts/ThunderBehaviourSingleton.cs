namespace ThunderRoad
{
    using UnityEngine;

    public class ThunderBehaviourSingleton<T> : ThunderBehaviour where T : ThunderBehaviour
    {
        #region Fields
        private static T _instance = null;
        #endregion Fields

        #region Properties
        public static T Instance 
        {
            get
            {
#if UNITY_EDITOR
                // Find instance in scene in editor
                if (_instance == null && Application.isPlaying == false)
                {
                    _instance = FindObjectOfType<T>();
                }
#endif //UNITY_EDITOR

                return _instance;
            }
        }
        public static bool HasInstance { get { return _instance != null; } }
        #endregion Properties

        #region Methods
        void Awake()
        {
            if (_instance != null)
            {
                Debug.LogError("Two singleton of type " + GetType().Name + " are present. New one will replace the other");
            }

            _instance = this as T;
            OnSetInstance();
        }

        protected virtual void OnSetInstance() { }

        private void OnDestroy()
        {
            OnInstanceDestroyed?.Invoke();
            _instance = null;
        }
        #endregion Methods

        #region Events
        public delegate void InstanceDestroyed();
        public event InstanceDestroyed OnInstanceDestroyed;
        #endregion Events
    }
}