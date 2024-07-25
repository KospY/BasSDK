using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace ThunderRoad.Manikin
{
    public static class ManikinPartLabel
    {
        [MenuItem("Assets/Manikin/Add Manikin Label")]
        static void AddManikinLabel()
        {
            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;

            string[] guids = Selection.assetGUIDs;
            foreach (string guid in guids)
            {
                var entry = aaSettings.FindAssetEntry(guid);
                if(entry != null)
                {
                    entry.SetLabel("Manikin", true, true, true);
                }
            }
        }

        [MenuItem("Assets/Manikin/Add Manikin Label", true)]
        static bool AddManikinLabelValidation()
        {
            if(Selection.activeGameObject != null)
            {
                return IsPart(Selection.activeGameObject);
            }

            if(Selection.activeObject != null)
            {
                return IsWardrobe(Selection.activeObject);
            }

            return false;
        }

        [MenuItem("Assets/Manikin/Add Part Label")]
        static void AddPartLabel()
        {
            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;

            string[] guids = Selection.assetGUIDs;
            foreach (string guid in guids)
            {
                var entry = aaSettings.FindAssetEntry(guid);
                if (entry != null)
                {
                    entry.SetLabel("Part", true, true, true);
                }
            }
        }

        [MenuItem("Assets/Manikin/Add Part Label", true)]
        static bool AddPartLabelValidation()
        {
            if (Selection.activeGameObject != null)
            {
                return IsPart(Selection.activeGameObject);
            }
            return false;
        }

        [MenuItem("Assets/Manikin/Add Wardrobe Label")]
        static void AddWardrobeLabel()
        {
            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;

            string[] guids = Selection.assetGUIDs;
            foreach (string guid in guids)
            {
                var entry = aaSettings.FindAssetEntry(guid);
                if (entry != null)
                {
                    entry.SetLabel("Wardrobe", true, true, true);
                }
            }
        }

        [MenuItem("Assets/Manikin/Add Wardrobe Label", true)]
        static bool AddWardrobeLabelValidation()
        {
            if (Selection.activeObject != null)
            {
                return IsWardrobe(Selection.activeObject);
            }
            return false;
        }

        static bool IsWardrobe(Object obj)
        {
            return obj.GetType() == typeof(ManikinWardrobeData);
        }

        static bool IsPart(GameObject obj)
        {
            return obj.GetComponent<ManikinPart>() != null;
        }
    }
}