using UnityEngine;

namespace ThunderRoad
{
    /// <summary>
    /// Any item with this module will be edible.
    /// </summary>
    [System.Serializable]
    public class ItemModuleEdible : ItemModuleMouthTouch
    {
        public string nextStageItemID;
        public string onConsumeAudioContainerAddress;
        public float healthGain;
        public float focusGain;
        public float consumeTime = 0;
        public bool transferCustomData = true;

    }
}