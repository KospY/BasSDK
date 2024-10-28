using UnityEngine;

namespace ThunderRoad
{
    public class RagdollPartJointFixer : ThunderBehaviour
    {
        public bool initialized => _initialized;
        private bool _initialized;
        private RagdollPart part;
        private int collidingCount;
        
        //slice over 2 frames
        protected override int SliceOverNumFrames => 2;
        private static int nextTimeSliceId = 0;
        protected override int GetNextTimeSliceId => nextTimeSliceId++;
        public override ManagedLoops EnabledManagedLoops => ManagedLoops.Update;
        
        public void SetPart(RagdollPart part)
        {
            this.part = part;
            _initialized = true;
        }
        
        
    }
}
