using UnityEditor;
using UnityEngine;

namespace ThunderRoad
{
    public class EditorInputDialog : EditorWindow
    {
        private string inputText;
        private string message;
        private string okButton;
        private string cancelButton;

        private bool result = false;
        private bool done = false;

        public static bool Show(string title, string message, string defaultText, string okButton, string cancelButton, out string text)
        {
            var window = CreateInstance<EditorInputDialog>();
            window.titleContent = new GUIContent(title);
            window.message = message;
            window.inputText = defaultText;
            window.okButton = okButton;
            window.cancelButton = cancelButton;

            window.position = new Rect(
                Screen.width / 2f,
                Screen.height / 2f,
                350,
                110
            );

            // Show as tiny modal popup
            window.ShowModalUtility();

            text = window.inputText;
            return window.result;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField(message, EditorStyles.wordWrappedLabel);
            GUI.SetNextControlName("inputField");
            inputText = EditorGUILayout.TextField(inputText);

            GUILayout.Space(10);

            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button(okButton))
                {
                    result = true;
                    done = true;
                    Close();
                }
                if (GUILayout.Button(cancelButton))
                {
                    result = false;
                    done = true;
                    Close();
                }
            }

            // Auto-focus text field first frame
            if (Event.current.type == EventType.Repaint)
            {
                EditorGUI.FocusTextInControl("inputField");
            }
        }

        private void OnDisable()
        {
            // Ensure window exits modal loop when closed
            if (!done)
            {
                result = false;
                done = true;
            }
        }
    }

}
