#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class SGMarkdownDefine : MonoBehaviour
{
    static SGMarkdownDefine()
    {
        AddDefineUtility.AddDefines(new[] {"SGMARKDOWN"});
    }
}
#endif
