using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TMPro
{
    public static class TMP_ResourceLoader
    {
        public static TMP_Settings LoadSettings()
        {
            return Addressables.LoadAssetAsync<TMP_Settings>($"TMP.Settings").WaitForCompletion();
        }

        public static TMP_FontAsset LoadFontAsset(string name)
        {
            return Addressables.LoadAssetAsync<TMP_FontAsset>($"TMP.Font.{name}").WaitForCompletion();
        }

        public static Material LoadMaterial(string name)
        {
            return Addressables.LoadAssetAsync<Material>($"TMP.Material.{name}").WaitForCompletion();
        }

        public static TMP_ColorGradient LoadColorGradient(string name)
        {
            return Addressables.LoadAssetAsync<TMP_ColorGradient>($"TMP.Gradient.{name}").WaitForCompletion();
        }

        public static TMP_SpriteAsset LoadSpriteAsset(string name)
        {
            return Addressables.LoadAssetAsync<TMP_SpriteAsset>($"TMP.Sprite.{name}").WaitForCompletion();
        }
    }
}