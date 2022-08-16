using UnityEngine;

namespace ThunderRoad
{
	/// <summary>
	/// Scripts which use this implementation of MonoBehaviour will be subscribed to the managed update loops
	/// </summary>
	///https://docs.unity3d.com/Manual/ExecutionOrder.html
	public class ThunderBehaviour : MonoBehaviour
    {
        private int fixedIndex = -1;

        private int updateIndex = -1;

        private int lateUpdateIndex = -1;

        // These methods must be overriden by child classes to specify which loops they want to use
        protected virtual ManagedLoops ManagedLoops => 0;

        public ManagedLoops GetManagedLoops => ManagedLoops;

        public void OnEnable()
        {
#if PrivateSDK
            UpdateManager.AddBehaviour(this);
#endif
            ManagedOnEnable();
        }

        public void OnDisable()
        {
#if PrivateSDK
            UpdateManager.RemoveBehaviour(this);
#endif
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
