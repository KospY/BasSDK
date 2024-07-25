using UnityEditor;
using UnityEngine;

namespace ThunderRoad
{
    public class StringToHash : EditorWindow
    {
        string text = "";
        int hash = -1;
        [MenuItem("ThunderRoad (SDK)/Tools/String To AnimatorHash")]
        static void CreateWindow()
        {
            StringToHash window = (StringToHash)EditorWindow.GetWindowWithRect(typeof(StringToHash), new Rect(0, 0, 400, 120));
        }

        void OnGUI()
        {
            GUILayout.Label("Enter string");
            text = GUILayout.TextField(text);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Hash", GUILayout.Width(120)))
                hash = Animator.StringToHash(text);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Label(hash.ToString());
        }
    }
}
