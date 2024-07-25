using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This is an example script that demonstrates how to
/// extend the Open Graph GUI with a custom property drawer.
/// 
/// In order to see this working, simply use the name "RPOpenGraphGUIExtension"
/// in ShaderGraph as the custom GUI. Then add a property called "CustomRender"
/// and see that it uses the RenderTest function which replaces the name.
/// In a real extension, you can put whatever GUI code you want in place of RenderTest.
/// </summary>
public class RPOpenGraphGUIExtension : RPOpenGraphGUI
{
	/// <summary>
	/// There is one property which we can override to add our extension.
	/// This is done from within the constructor to improve performance.
	/// </summary>
	public RPOpenGraphGUIExtension() : base()
	{
		//First initialize the property
		renderExtensions = new Dictionary<string, System.Action<MaterialEditor, MaterialProperty>>();
		//Then add in your custom render function,
		//with the key being the name of the property you want to override
		renderExtensions.Add("CustomRender", RenderTest);
	}

	/// <summary>
	/// The required function type hands back MaterialEditor for the ShaderGUI
	/// and the MaterialProperty which contains info about the property that
	/// the name matched to. 
	/// 
	/// This function will be run instead of the usual property drawer,
	/// so if you want it to show up then you can call some Editor GUI code here.
	/// </summary>
	/// <param name="editor"></param>
	/// <param name="v"></param>
	void RenderTest(MaterialEditor editor, MaterialProperty v)
	{
		//Your custom handler GUI here ->
		editor.DefaultShaderProperty(v, "Proof that it renders extension");
	}
}
