---
parent: ModOptions
grand_parent: Guides
---
# ModOptions

## What are ModOptions

Mod options are a way for modders to add configuration options to their **scripted** mods which appear in the player menu.

They are attributes in c# which sit in top of static methods, properties or fields and automatically add the options to the player menu without the need for additional json.

![Untitled](ModOptions%20f0a58cc5464a4b6aa462f9b920320e46/Untitled.png)

## What are the features of ModOptions

- Supports multiple types out of the box:
    - float / int / bool / UnityEngine.Color / Any Enum / Strings / CatalogDatas
- Can be used on static methods, properties or fields.
- Supports Buttons or Left / Right arrows as Input options on the player menu - configurable within the mod option
- Option values are automatically saved to the players save files - this can be disabled per mod option
- Localization is supported for Mod Options names
- Mod Options can be grouped by categories
- Categories can be ordered
- Mod Options can be ordered within or outwith categories
- Predefined lists of default values for the options are used when no defined list of options values is given
- Mod Options can have tool tips which appear underneith the option when you hover over it

## When are ModOptions Loaded

Mod Options are loaded for each mod after all of the assemblies, addressable bundles and catalog files for all mods are loaded.

They are loaded before ThunderScripts.

## How do I use ModOptions

Mod options are attributes that can be put onto static method, properties or fields within a script. 

You can include them in any class, whether its a static class, monobehaviour, thunderbehaviour, CatalogData, or ThunderScript. The ModOptions **must** be static.

To load them in game, you just need to compile your dll for the game and include a manifest.json as normal when making mods.

The game will automatically find any ModOptions in your dll and load them.

We infer many things automatically with ModOptions  to give a nice out of the box experience. 

But it is very customizable. This guide will go through progressively more customized ModOption solutions

### Attributes

We have several attributes to help make ModOptions easier to use. The main one which supports all configuration options is `ModOption`, but there are others to help organise your options better.

- `[ModOptionCategory(string categoryName, int categoryOrder, string categoryLocalizationId)]` - Can be applied above any `ModOption` to set the Category
- `[ModOptionOrder(int order)]` - Can be applied above any `ModOption` to set the order that mod option appears
- `[ModOptionSaveValue(bool saveValue)]` - Can be applied above any `ModOption` to define if the value of this option should be saved to the players save file.
- `[ModOptionSave]` - Can be applied above any `ModOption` to make the option be saved to the players save file
- `[ModOptionDontSave]` - Can be applied above any `ModOption` to stop the option be saved to the players save file
- `[ModOptionTooltip(string tooltip, string tooltipLocalizationId)]` - Can be applied above any `ModOption` to set the tooltip
- `[ModOptionUI(ModOption.InteractionType interactionType]` - Can be applied above any `ModOption` to set the type of UI interaction to use
- `[ModOptionArrows]` - Can be applied above any `ModOption` to set the type of UI interaction to use Left/Right arrows to change the values
- `[ModOptionButton]` - Can be applied above any `ModOption` to set the type of UI interaction to use a Button to change the values
- `[ModOptionSlider]` - Can be applied above any `ModOption` to set the type of UI interaction to use a Slider to change the values

### ModOptionParameters

To understand how ModOptions work. The options are stored in a list of possible options which is of the `ModOptionParameter` type and its various subclasses we provide such as:

`ModOptionString`

`ModOptionBool`

`ModOptionFloat`

`ModOptionInt`

`ModOptionCatalogData`

`ModOptionColor`

For the above subclasses, the static `defaultValue` fields on them are used when no `ModOptionParameter` is provided by the user.

It includes the **title, titleLocalizationId** and the **value** which is returned to the member the ModOption is on.

As of U12.1 we do not support the extension of `ModOptionParameters` to provide your own subtypes as the UI does not support it yet.

### Simple Mod Options

The below mod option is a `bool` type and you can see examples of it on a field, property and method

Because no name is defined in the attribute, it will generate a ModOption in the book based off the field/property/method name called “Enable My Cool Feature”.

```csharp
//On a field
[ModOption]
private static bool EnableMyCoolFeature;

//On a property
[ModOption]
private static bool EnableMyCoolFeature { get; set; }

//On a method
[ModOption]
private static void EnableMyCoolFeature(bool value){
	Debug.Log($"EnableMyCoolFeature: {value}");
}
```

Because no values for the `ModOptionParameters` have been defined in the above example. Since it is a `bool` type, the default `ModOptionParameter` list which comes from `ModOptionBool.defaultValues` is used. This gives two options, `Enabled` and `Disabled` as the labels, which return `true` and `false`

```csharp
public static ModOptionBool[] defaultValues = new[] {
		new ModOptionBool("Disabled", "Default.Disabled", false),
		new ModOptionBool("Enabled", "Default.Enabled", true)
};
```

## Customising Mod Options

### Tooltips

In this example, we provide the name and tooltip in two different ways. The `ModOptionTooltip` attribute can be used, and will override any tooltip properties set in the ModOption itself. Its just another way to define the option which may be easier to write.

```csharp
[ModOption("Gravity Blast Force", "Sets the amount of force gravity blast uses")]
private static int force;
        
[ModOptionTooltip("Sets the amount of force gravity blast uses")]
[ModOption("Gravity Blast Force")]
private static int force;
```

### Ordering mod options

ModOptions are ordered by category order, then category name, then mod option order then mod option name.

You can set the mod order by either setting the `order` parameter on the `ModOption` attribute or use the `ModOptionOrder` attribute

```csharp
[ModOptionOrder(1)]
[ModOption("Gravity Blast Force", "Sets the amount of force gravity blast uses")]
private static int force;
        
[ModOptionTooltip("Changes Running Speed")]
[ModOption("Running Speed", order = 2)]
private static float speed;
```

### Setting the category

ModOptions appear in the player menu under the mods page. But they can be grouped under Category headings.

You can set the Category and Category order by either setting the `category` and `categoryOrder` parameter on the `ModOption` attribute or use the `ModOptionCategory` attribute

```csharp
[ModOptionCategory("Magic", 1)]
[ModOptionOrder(1)]
[ModOption("Gravity Blast Force", "Sets the amount of force gravity blast uses")]
private static int blastForce;
        
[ModOptionCategory("Magic", 1)]
[ModOptionOrder(2)]
[ModOption("Gravity Hover Force", "Sets the amount of force gravity hover uses")]
private static int hoverForce;
        
[ModOptionTooltip("Changes Running Speed")]
[ModOption("Running Speed", order = 2, category = "Locomotion", categoryOrder = 2)]
private static float speed;
```

### Saving selection options to the player options file

By default, all Mod Options will save their values to the players options file.
You can disable this by setting the `saveValue` parameter to false or using the `ModOptionDontSave` attribute

```csharp
[ModOptionDontSave]
[ModOption]
private static bool enableTK;
        
[ModOption(saveValue = false)]
private static Color lightningColor;
```

## Custom option values

By default the Mod Options will use predefined list of values based on the field/property type, or the parameter type of the method.

However you can provide your own list of option values by defining either static field or static method to return an array of `ModOptionParameters`

Here are some examples:

```csharp
				public static ModOptionBool[] booleanOption = {
            new ModOptionBool("Disabled", false),
            new ModOptionBool("Enabled", true)
        };

        public static ModOptionString[] stringOptionHighLow = {
            new ModOptionString("High", "High"),
            new ModOptionString("Low", "Low")
        };

        public static ModOptionString[] stringOptionSpawn = {
            new ModOptionString("Spawn", "123123123")
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
```

In order to use these lists of parameters you need to specify the name of the method or field in your ModOption using the `valueSourceName` parameter of the mod option. This tells it to use that member within the same class as your mod option

```csharp
[ModOption(name: "Color", tooltip: "Color Option", valueSourceName: nameof(colorOption), category = "Value Source Example", order = 2)]
private static void ColorOption(Color value)
{
		Debug.Log($"ColorOption value: {value}");
}
```

You can reuse these `ModOptionParameter[]` fields/methods for multiple options if you need to

You can also define them in another class, but if you do so you also need to provide the `valueSourceType` parameter to the mod option to tell it what class type holds your value source.

In the below example, we reference the `defaultValues` in the `ModOptionBool` class 

```csharp
[ModOption(name: "Boolean Field", tooltip: "Boolean Field Option", valueSourceName: nameof(ModOptionBool.defaultValues), valueSourceType = typeof(ModOptionBool), category = "Value Type Example")]
private static bool MyBoolField;
```

## Default values

To define which default value in the list of `ModOptionParameter[]` you need to specify the index referencing the value you want with the `defaultValueIndex` parameter. We acknowledge this is not the easiest of systems and will be making improvements in the future

```csharp
[ModOption("A Bool", "This is a boolean Option defaults to true", defaultValueIndex = 1)]
private static bool ABool { get; set; }
```

### Enum Types

Any Enum should work with ModOptions, it will automatically use all of the Enum values as the mod options

```csharp
public enum MyEnum
{
    Enum1,
    Enum2,
    Enum3
}

[ModOption(category = "Enum Example")]
private static void MyEnumMagic(MyEnum value)
{
	Debug.Log($"MyEnumMagic: {value}");
}
```