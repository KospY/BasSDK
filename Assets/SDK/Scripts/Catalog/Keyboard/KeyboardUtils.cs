using Newtonsoft.Json;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif
using ThunderRoad;
using ThunderRoadVRKBSharedData;
using UnityEngine;

public class KeyboardUtils : MonoBehaviour
{
#if UNITY_EDITOR
    [Button]
    public void RenumberKeys()
    {
        //all of the keys should have colliders
        var children = gameObject.GetComponentsInChildren<KeyButton>();
        for (var i = 0; i < children.Length; i++)
        {
            var child = children[i];
            child.keyId = $"{i:000}";
            child.transform.name = child.keyId;
        }
    }
    
    [TextArea]
    public string keyboardDataInput = "Provide a csv of the keys in order, ie \"q\",\"w\",\"e\"";
    public string layerName = "LAYERNAME";
    public string inheritFromName = "INHERITFROM";
    [Button]
    public void GenerateKeyboardLayerData()
    {
        KeyboardLayerConfiguration data = new KeyboardLayerConfiguration();
        
        data.inheritsFromLayer = inheritFromName;
        data.keys = new StringKeyDictionary();
        Debug.Log(keyboardDataInput);
        //data should be in CSV format, trim off the  first and last quotation
        //var input = keyboardDataInput.Trim('\"');
        //split it on the "," to get each set of chars
        var splitter = "\",\"";
        var splits = keyboardDataInput.Split(splitter);
        for (int i = 0; i < splits.Length; i++)
        {
            string val = splits[i];
            if (i == 0) val = val.TrimStart('\"');
            if (i == splits.Length-1) val = val.TrimEnd('\"');
            
            //Debug.Log($"key: {val}");
            data.keys.Add($"{i:000}", new KeyConfiguration() {
                actionType = KeyActionTypes.OutputLabel,
                label = val,
                actionArg = val,
                overrideProperties = new KeyProperties()
            });
        }
        output = JsonConvert.SerializeObject(data, Formatting.Indented, Catalog.GetJsonNetSerializerSettings());
    }
    
    [TextArea]
    public string output = "Will be replaced with KeyboardLayerConfiguration output";
    private void OnDrawGizmosSelected()
    {
        var children = gameObject.GetComponentsInChildren<KeyButton>();
        for (var i = 0; i < children.Length; i++)
        {
            var child = children[i];
            Transform childTransform = child.transform;
            drawString(child.keyId, childTransform.position, Color.red, Vector2.zero, 30);
        }
    }

    static public void drawString(string text, Vector3 worldPosition, Color textColor, Vector2 anchor, float textSize = 15f)
    {
#if UNITY_EDITOR
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        if (!view)
            return;
        Vector3 screenPosition = view.camera.WorldToScreenPoint(worldPosition);
        if (screenPosition.y < 0 || screenPosition.y > view.camera.pixelHeight || screenPosition.x < 0 || screenPosition.x > view.camera.pixelWidth || screenPosition.z < 0)
            return;
        var pixelRatio = UnityEditor.HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.right).x - UnityEditor.HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.zero).x;
        UnityEditor.Handles.BeginGUI();
        var style = new GUIStyle(GUI.skin.label)
        {
            fontSize = (int)textSize,
            normal = new GUIStyleState() { textColor = textColor }
        };
        Vector2 size = style.CalcSize(new GUIContent(text)) * pixelRatio;
        var alignedPosition =
            ((Vector2)screenPosition +
             size * ((anchor + Vector2.left + Vector2.up) / 2f)) * (Vector2.right + Vector2.down) +
            Vector2.up * view.camera.pixelHeight;
        GUI.Label(new Rect(alignedPosition / pixelRatio, size / pixelRatio), text, style);
        UnityEditor.Handles.EndGUI();
#endif
    }
#endif
}
