using System.ComponentModel;
using UnityEditor.AddressableAssets.Settings;

namespace ThunderRoad
{
    [DisplayName("ThunderRoad")]
    public class ThunderRoadAAGroupSchema : AddressableAssetGroupSchema
    {
        public bool sharedBundle;
    }
}