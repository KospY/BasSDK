using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace TestModOptions
{
    public class TestOptions
    {
        public static ModOptionBool[] booleanOption = {
            new ModOptionBool("Disabled", false),
            new ModOptionBool("Enabled", true)
        };

        public static ModOptionString[] stringOptionHighLow = {
            new ModOptionString("High", "High"),
            new ModOptionString("Low", "Low")
        };

        public static ModOptionString[] stringOptionSpawn = {
            new ModOptionString("Spawn", "Spawn")
        };

        public static ModOptionFloat[] floatOption = {
            new ModOptionFloat("0.0", 0.0f),
            new ModOptionFloat("0.25", 0.25f),
            new ModOptionFloat("0.5", 0.5f),
            new ModOptionFloat("0.75", 0.75f),
            new ModOptionFloat("1.0", 1.0f)
        };

        public static ModOptionInt[] intOption = {
            new ModOptionInt("1", 1),
            new ModOptionInt("2", 2),
            new ModOptionInt("3", 3),
            new ModOptionInt("4", 4),
            new ModOptionInt("5", 5)
        };

        public static ModOptionColor[] colorOption = {
            new ModOptionColor("Red", Color.red),
            new ModOptionColor("Blue", Color.blue),
            new ModOptionColor("Green", Color.green)
        };

        public static ModOptionCatalogData[] ItemCatalogDatas()
        {
            List<ModOptionCatalogData> options = new List<ModOptionCatalogData>();
            foreach (CatalogData catalogData in Catalog.GetDataList(Category.Item))
            {
                if (catalogData is ItemData itemData)
                {
                    if (!itemData.purchasable && itemData.type != ItemData.Type.Weapon) continue;
                    options.Add(new ModOptionCatalogData(itemData.displayName, itemData));
                }
            }
            return options.ToArray();
        }

        [ModOption(name: "Item", tooltip: "Item Option", valueSourceName: nameof(ItemCatalogDatas), category = "Value Source Example", order = 1)]
        private static void ItemOption(CatalogData value)
        {
            Debug.Log($"ItemOption value: {value.id}");
        }

        [ModOption(name: "Color", tooltip: "Color Option", valueSourceName: nameof(colorOption), category = "Value Source Example", order = 2)]
        private static void ColorOption(Color value)
        {
            Debug.Log($"ColorOption value: {value}");
        }

        [ModOption(name: "Integer", tooltip: "Integer Option", valueSourceName: nameof(intOption), category = "Value Source Example")]
        private static void IntArrowListOption(int value)
        {
            Debug.Log($"IntOption value: {value}");
        }

        [ModOption(name: "Integer", tooltip: "Integer Option", valueSourceName: nameof(intOption), category = "Value Source Example", interactionType = InteractionType.ButtonList)]
        private static void IntButonListOption(int value)
        {
            Debug.Log($"IntOption value: {value}");
        }

        [ModOption(name: "Boolean", tooltip: "Boolean Option", valueSourceName: nameof(booleanOption), category = "Value Source Example", interactionType = InteractionType.ButtonList)]
        private static void ToggleBoolOption(bool value)
        {
            Debug.Log($"ToggleBoolOption value: {value}");
        }

        [ModOption(name: "Float", tooltip: "Float Option", valueSourceName: nameof(floatOption), category = "Value Source Example")]
        private static void FloatOption(float value)
        {
            Debug.Log($"FloatOption value: {value}");
        }

        [ModOption(name: "String", tooltip: "String Option", valueSourceName: nameof(stringOptionHighLow), category = "Value Source Example")]
        private static void StringOption(string value)
        {
            Debug.Log($"StringOption value: {value}");
        }

        [ModOption(name: "String", tooltip: "String Option", valueSourceName: nameof(stringOptionSpawn), category = "Value Source Example", interactionType = InteractionType.ButtonList)]
        private static void SpawnOption(string value)
        {
            Debug.Log($"Spawn Option value: {value}");
        }

        [ModOption(name: "Boolean Field", tooltip: "Boolean Field Option", valueSourceName: nameof(ModOptionBool.defaultValues), valueSourceType = typeof(ModOptionBool), category = "Value Type Example")]
        private static bool MyBoolField;

        [ModOption(name: "DefaultCatalogDataField", tooltip: "DefaultCatalogDataField", category = "Default Values Example")]
        private static CatalogData DefaultCatalogDataField;

        [ModOption(name: "DefaultBoolField", tooltip: "DefaultBoolField", category = "Default Values Example")]
        private static bool DefaultBoolField;

        [ModOption(name: "DefaultIntField", tooltip: "DefaultIntField", category = "Default Values Example")]
        private static int DefaultIntField;

        [ModOption(name: "DefaultFloatProperty", tooltip: "DefaultFloatProperty", category = "Default Values Example")]
        private static float DefaultFloatProperty { get; set; }

        [ModOption(name: "A Float", tooltip: "A float with no category")]
        private static float AFloatNoCat { get; set; }
        
        [ModOption(name: "A Bool", tooltip: "A bool with no category")]
        private static bool ABoolNoCat { get; set; }
        
        [ModOption(name: "DefaultColorMethod", tooltip: "DefaultColorMethod", category = "Default Values Example")]
        private static void DefaultColorMethod(Color value)
        {
            Debug.Log($"DefaultColorMethod: {value}");
        }

        [ModOption(name: "DefaultStringMethod", tooltip: "DefaultStringMethod", category = "Default Values Example")]
        private static void DefaultStringMethod(string value)
        {
            Debug.Log($"DefaultStringMethod: {value}");
        }

        [ModOption(category = "Default Values Example")]
        private static float MemeManLazySpecial;

        [ModOption(category = "Enum Example")]
        private static void MyEnumMagic(MyEnum value)
        {
            Debug.Log($"MyEnumMagic: {value}");
        }

        public enum MyEnum
        {
            Enum1,
            Enum2,
            Enum3
        }

        public static ModOptionInt[] someInts = {
            new ModOptionInt("First Enum", 0),
            new ModOptionInt("Third Enum", 2)
        };

        [ModOption(valueSourceName = nameof(someInts), category = "Enum Example")]
        private static void MyFilteredEnum(MyEnum value)
        {
            Debug.Log($"MyFilteredEnum: {value}");
        }
    }
}
