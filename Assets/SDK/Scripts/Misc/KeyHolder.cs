using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    public class KeyHolder : Holder
    {
        public bool isKeyLock;
        public UnityEvent keySnapEvent;
        public UnityEvent keyUnSnapEvent;

        public Rigidbody rigidBody;

        private FixedJoint fixedJoint;
 //ProjectCore
    }
}