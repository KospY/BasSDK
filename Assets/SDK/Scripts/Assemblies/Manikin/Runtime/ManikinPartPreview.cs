using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    //TODO, change script name to IManikinPartPreview

    public interface IManikinPartPreview
    {
#if UNITY_EDITOR
        Texture2D GetOrCreatePreview(string id, string path, int width, int height);
        Texture2D CreatePreview(string id, string path, int width, int height);
        Texture2D GetPreview(string id, string path);
#endif
    }
}
