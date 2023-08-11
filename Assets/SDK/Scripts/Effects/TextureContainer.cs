using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/TextureContainer")]
    [CreateAssetMenu(menuName = "ThunderRoad/Textures/Texture Container")]
    public class TextureContainer : ScriptableObject
    {
        public List<Texture> textures;

        public Texture GetRandomTexture()
        {
            if (textures.Count == 0) return null;
            if (textures.Count == 1) return textures[0];
            int index = UnityEngine.Random.Range(0, textures.Count - 1);
            return PickTexture(index);
        }
        public Texture PickTexture(int index)
        {
            return textures[index];
        }
    }
}
