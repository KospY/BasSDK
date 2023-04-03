using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ThunderRoad.Manikin;

namespace ThunderRoad
{
    public class RagdollPartJointFixer : ThunderBehaviour
    {
        public bool initialized => _initialized;
        private bool _initialized;
        private RagdollPart part;
        private int collidingCount;
        
        public override ManagedLoops EnabledManagedLoops => ManagedLoops.Update;
        
        public void SetPart(RagdollPart part)
        {
            this.part = part;
            _initialized = true;
        }
        
        
    }
}
