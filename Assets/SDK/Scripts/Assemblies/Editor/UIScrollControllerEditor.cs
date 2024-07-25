using UnityEditor;

namespace ThunderRoad
{
    [CustomEditor(typeof(UIScrollController))]
    public class UIScrollControllerEditor : Editor
    {
        // We need this override so that our custom fields of the UIScrollController class are exposed in the editor inspector
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}