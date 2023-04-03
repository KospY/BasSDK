namespace ThunderRoad
{
    public class ItemModuleReturnInInventory : ItemModule
    {
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

    }
}