# OpenGraphGUI
[![openupm](https://img.shields.io/npm/v/com.robproductions.opengraphgui?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.robproductions.opengraphgui/) ![GitHub release (latest by date)](https://img.shields.io/github/v/release/RobProductions/OpenGraphGUI?logo=github)

An open-source Editor GUI for use with URP ShaderGraphs in Unity, aiming to clean up the look of Material Properties while providing ease-of-use via tagging.

### Custom GUI right from ShaderGraph

<img width = "800" src="Documentation~/DocAssets/SingleLineTags.gif">

The default Inspector view for ShaderGraph-based materials can be somewhat bland at times, and often can't match the more streamlined look of the default Shaders provided by the Built-In renderer or other Render Pipelines. **OpenGraphGUI** opens up new options. 

## Why use OpenGraphGUI?

**OpenGraphGUI** lets you customize the appearance of your material properties right from ShaderGraph. Simply prefix your property names in the Shader's Blackboard with certain special characters, and any material using that Shader will display the custom GUI. This is a lightweight and easy to use script that provides more control over the design of your Shaders.

<img width = "700" src="Documentation~/DocAssets/AllTagsScreen.jpg">

If you're looking for a more comprehensive and feature-complete Shader GUI package, I highly recommend [Shader Graph Markdown](https://assetstore.unity.com/packages/tools/gui/shader-graph-markdown-194781) which this project is inspired from. They will be able to provide much more support and QOL features for developers needing extensive custom GUIs. The intent of OpenGraphGUI is to provide a simple open source alternative that the community is free to edit and improve as needed. 

### Match the look of built-in URP Materials

**OpenGraphGUI** was built to be a replacement for the default ShaderGraph GUI provided by Unity. The feature set allows you to add cleaner layouts for your material properties such as the "Single Line Texture" that you'd find in default URP materials. It also provides labels and other organizational enhancements to give you more control over the look of your GUI.

## Usage

In ShaderGraph, simply change your *"Custom Editor GUI"* setting to the named class *RPOpenGraphGUI*, and you're all set to start prefixing!

<img width = "250" src="Documentation~/DocAssets/CustomEditorGUI.jpg">

### Labels & Alignment

> Use these naming rules to adjust the spacing of your Material's inspector and organize fields. Note that these properties are only meant to send information to OpenGraphGUI and will not be rendered, so you can use any type you prefer. For consistency, choosing a Boolean for these properties is simplest.

Prefix one of your property names with the **star symbol (\*)** and that field will become a bold label. For example, if you want a bold label with the text **"Lighting"**, use the name `*Lighting` for that property in ShaderGraph. 

Create a property with the name **\[Centered\]** to adjust the spacing of the Inspector fields to match the look of non-ShaderGraph default materials. Use a property called **\[RightBound\]** to return the field spacing to the default ShaderGraph look. 

<img width = "800" src="Documentation~/DocAssets/AlignmentTags.gif">

When in the **\[Centered\]** mode, use a property called **\[MaxField\]** to let the value field take up more width in the inspector. This look is meant to match the "expanded" feel of color tints for single-line textures and improve the accessibility of fields by making them easier to edit. Use a property called **\[MinField\]** to return to the default spacing style. 

<img width = "500" src="Documentation~/DocAssets/FieldWidthScreenshot.jpg">

Use the **hashtag symbol (#)** as a prefix for one of your properties and you will create a Foldout Group with your property name as the title. Each subsequent property will become a part of the group. You can create up to 128 unique foldout groups by labeling new foldout properties in sequence without having to tag the end of each group. If you do wish to close the previous Foldout Group, simply create a property that consists of only a **hashtag symbol (#)** without any name. The following properties will exist outside of the last group. 

<img width = "550" src="Documentation~/DocAssets/FoldoutGroupsScreenshot.jpg">

### Property Rendering Features

> Use these naming rules to adjust how each field is rendered in the inspector.

Prefix a texture property with the **percent symbol (%)** and it will show as a single line texture property instead of the big thumbnail version. Single line textures are commonly used in the built-in materials and offer a cleaner look that takes up less space in your Inspector. Example name: `%MySingleLineTexture`

Follow up a single-line texture property with a property that uses the **ampersand prefix (&)** and the resulting property will be shown on the same line as the texture. OpenGraphGUI calls this a "linked property", and you'll commonly find these used as "tint colors" in the default materials. Example name: `&MyLinkedProperty`

Follow up any texture property with a property that uses the **arrow prefix (^)** and that property will only become visible when the texture above it has been populated by the user (not null). This is called a "dependent visible" property. You'll find this on many custom shaders that utilize a strength value, appearing only when the asociated texture is present. Example name: `^MyDependentProperty`

<img width = "800" src="Documentation~/DocAssets/DependentTags.gif">

**NEW!** Prefix any vector property with **2~**, **3~**, or **4~** to show it as a Vector2, Vector3, or Vector4 field respectively. By default, ShaderGraph material inspectors render all vectors as Vector4, so this extra information is necessary to tell OpenGraphGUI which type you want to use. Example name: `2~MyVector2` or `3~MyVector3`

### Extensions

**OpenGraphGUI** also supports custom property extensions if you'd like to keep the ease of use of prefixing without re-writing the whole package. 

To create a custom renderer for one of your properties, simply extend the *RPOpenGraphGUI* class with your own. In the constructor, initialize the "renderExtensions" dictionary like so:

```
public MyOpenGraphGUIExtension()
{
	//First initialize the property
	renderExtensions = new Dictionary<string, System.Action<MaterialEditor, MaterialProperty>>();
	//Then add in your custom render function,
	//with the key being the name of the property you want to override
	renderExtensions.Add("MyProperty", MyCustomRenderFunction);
}
```

Finally, set the *Custom Editor GUI* of your ShaderGraph to your custom class. That's it! Now you've got a custom function to render the property of your choice which is used in place of the drawing code that OpenGraphGUI would've used once it finds that property. 

## Recommended Installation

1. Open the [Package Manager](https://docs.unity3d.com/2020.3/Documentation/Manual/upm-ui.html) in Unity
2. Click the '+' icon and hit *"Add package from git URL"*
3. Add the GitHub "HTTPS Clone" URL for OpenGraphGUI: [https://github.com/RobProductions/OpenGraphGUI.git](https://github.com/RobProductions/OpenGraphGUI.git)
4. Wait for download to complete
5. Add the custom GUI class *RPOpenGraphGUI* to the *Custom Editor GUI* field in your ShaderGraph of choice

If you're looking to add a specific release of OpenGraphGUI, you can specify a release tag with the hashtag like so: "https://github.com/RobProductions/OpenGraphGUI.git#ReleaseNumber"

## Manual Installation

You can also manually install OpenGraphGUI by downloading the project as .zip and placing the contents into your *Assets* folder or into the *Packages* folder to create an embedded package. I don't recommend going this route unless you have specific reason to keep a static local copy. 

If the Package Manager installation fails due to version requirements, you may be able force OpenGraphGUI to work by downloading the project as .zip and editing the "package.json" file to lower the Unity requirement. Deleting the .meta files may also be necessary and shouldn't break anything... I think. Then use the *"Add package from disk"* option to add your custom version instead. 

## How to Contribute

This open source project is free for all to suggest improvements, and I'm hoping that more contributors could help clean up the code and add further features as suggested by the community. These are the recommended steps to add your contribution:

1. Fork the repository to your GitHub account
2. Clone the code to the Assets folder in any Unity project (as long as it does not include OpenGraphGUI as a package)
3. Create a new branch or work off of your own "working branch"
4. When your changes are complete, submit a pull request to merge your code; ideally to the "working-branch" used to test changes before main
5. If the PR is approved, aggregate changes will eventually be merged into main and a new release tag is created

## Credits & Details

Created by [RobProductions](https://twitter.com/RobProductions). RobProductions' Steam games can be found [here](https://store.steampowered.com/developer/robproductions).

### Requirements

- Tested with Unity 2020.3.26f1, though it will likely work in earlier versions too
- Tested in URP version 10.9, though it will likely work in earlier versions too

In theory **OpenGraphGUI** should work with regular Shaders and even HDRP ShaderGraphs too since it only requires looking at the display name of each property. This is useful because you can easily acquire the same clean look in your ShaderLab shaders by simply naming the properties in your shader file rather than writing a custom GUI to handle it. 

### Limitations

- Though there shouldn't be any performance concerns with the prefixing approach (Unity should automatically cull out properties internally that are not used), it does bloat up the ShaderGraph quite a bit. Since this is meant to be a simple enhancement, I didn't take the route of editing the ShaderGraph GUI itself, so prefixed properties won't look any different from non-prefixed ones.
- There are certain rules you must follow for your properties to show up as expected. All linked properties must follow a single line texture property. Dependent visible properties must follow another dependent visible property, a texture property, or a linked property (since it will be bundled with the texture). If those rules are violated, OpenGraphGUI will throw a warning to the console with some specifics about what went wrong.

### License

This work is licensed under the MIT License. The intent for this software is to expand the extend the functionality of ShaderGraph freely and openly, without requirement of attribution. The code may be considered "open source" and could include snippets from multiple collaborators. Contributors may be named in the README.md and Documentation files. 

The code is provided "as is" and without warranty. 
