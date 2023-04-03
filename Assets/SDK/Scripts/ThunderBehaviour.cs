using System;
using UnityEditor;
using UnityEngine;

namespace ThunderRoad
{
    [Flags]
    public enum ManagedLoops
    {
        FixedUpdate = 1,
        Update = 2,
        LateUpdate = 4
    }

    /// <summary>
    /// Scripts which use this implementation of MonoBehaviour will be subscribed to the managed update loops
    /// </summary>
    ///https://docs.unity3d.com/Manual/ExecutionOrder.html
    public class ThunderBehaviour : MonoBehaviour
    {
        public Type ThunderBehaviourType
        {
            get
            {
                if (_type is null)
                {
                    _type = this.GetType();
                }
                return _type;
            }
        }
        //The actual type of this object
        private Type _type;
        
        /// <summary>
        /// Overrides Unitys gameObject engine call and gets the cached gameObject for this ThunderBehaviour
        /// </summary>
        public new GameObject gameObject
        {
            get
            {
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying) return base.gameObject;
#endif                
                if (_gameObject is null && _gameObject == null)
                {
                    _gameObject = base.gameObject;
                }
                return _gameObject;
            }
        }

        protected GameObject _gameObject = null;
        
        /// <summary>
        /// Overrides Unitys transform engine call and gets the cached transform for this ThunderBehaviour
        /// </summary>
        public new Transform transform
        {
            get {
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying) return base.transform;
#endif                
                if (_transform is null && _transform == null) // do a cheaper null ref check first, then do the unity engine check
                {
                    _transform = base.transform;
                }
                return _transform;
            }
        }

        public Transform baseTransform => base.transform;
        
        protected Transform _transform;

        private int fixedIndex = -1;

        private int updateIndex = -1;

        private int lateUpdateIndex = -1;

        // These methods must be overriden by child classes to specify which loops they want to use
        /// <summary>
        /// Defines which loops will be executed on this behaviour. It is checked during OnEnable and OnDisable
        /// </summary>
        public virtual ManagedLoops EnabledManagedLoops => 0;
        
        public void OnEnable()
        {
            ManagedOnEnable();
        }

        public void OnDisable()
        {
            ManagedOnDisable();
        }

        protected virtual void ManagedOnEnable()
        { }

        protected virtual void ManagedOnDisable()
        { }

        // Internal is used as the access modifier for these because they need to be called by the UpdateManager

        internal void SetIndex(ManagedLoops loops, int index)
        {
            switch (loops)
            {
                case ManagedLoops.FixedUpdate:
                    fixedIndex = index;
                    break;
                case ManagedLoops.Update:
                    updateIndex = index;
                    break;
                case ManagedLoops.LateUpdate:
                    lateUpdateIndex = index;
                    break;
                default:
                    break;
            }
        }

        internal int GetIndex(ManagedLoops loops) =>
            loops switch {
                ManagedLoops.FixedUpdate => fixedIndex,
                ManagedLoops.Update => updateIndex,
                ManagedLoops.LateUpdate => lateUpdateIndex,
                _ => -1,
            };

        protected internal virtual void ManagedFixedUpdate()
        { }

        protected internal virtual void ManagedUpdate()
        { }
        protected internal virtual void ManagedLateUpdate()
        { }
    }
}
