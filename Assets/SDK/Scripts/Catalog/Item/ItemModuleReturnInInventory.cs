namespace ThunderRoad
{
    public class ItemModuleReturnInInventory : ItemModule
    {
        /// <summary>
        /// Item Should return to last holder instead of inventory
        /// </summary>
        public bool returnsToLastHolder;

        /// <summary>
        /// Should the item return when the player ungrab item
        /// </summary>
        public bool returnsOnItemRelease;

        /// <summary>
        /// Should the item return when the player doesn't look at it for X seconds ?
        /// </summary>
        public bool returnsWhenNotLookingAtIt = true;
        
        /// <summary>
        /// Should the item return when the item goes beyond ea distance threshold ?
        /// </summary>
        public bool returnsAfterReachingMaxDistance = true;

        /// <summary>
        /// Distance under which the item stays still
        /// </summary>
        public float maxDistanceBeforeReturning = 15;

        /// <summary>
        /// Time after which the item returns (when not looked)
        /// </summary>
        public float timeAfterWhichItemReturn = 2;


        /// <summary>
        /// Set true to have a returning animation.
        /// </summary>
        public bool useReturningAnimation;

        /// <summary>
        /// the velocity magnitude whern returning
        /// </summary>
        public float returningVelocityValue;

    }
}