using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

/// <summary>
/// This is a globally accessible "wrapper" class that helps
/// to shorten the OpenGraphGUI namespace for final use. 
/// With this inheritance setup, you simply need to type in "RPOpenGraphGUI"
/// to the "Custom Editor GUI" field in ShaderGraph rather than the
/// lengthy name RobProductions.OpenGraphGUI.Editor.OpenGraphGUIEditor.
/// </summary>
public class RPOpenGraphGUI : RobProductions.OpenGraphGUI.Editor.OpenGraphGUIEditor
{
	//Leave empty to inherit all default methods + properties
}

namespace RobProductions.OpenGraphGUI.Editor
{
	/// <summary>
	/// This is the main Shader Graph custom GUI that renders the material properties.
	/// Inherits from ShaderGUI, which is what the Editor uses to draw the inspector. 
	/// </summary>
	public class OpenGraphGUIEditor : ShaderGUI
	{
		/// <summary>
		/// Width of the default property value editor box.
		/// </summary>
		const float rightValueBoxSize = 100f;
		/// <summary>
		/// Width of the texture field on default texture property renders.
		/// </summary>
		const float texFieldBoxSize = 65f;
		/// <summary>
		/// Width of vector field on Vec2 and Vec3 property renders.
		/// </summary>
		const float vecFieldBoxSize = 220f;
		/// <summary>
		/// Heuristic for when extra field shrinking kicks in on small inspectors.
		/// </summary>
		const float minLabelWidthAdjustField = 320f;
		/// <summary>
		/// How much extra shrinking to add in small inspector situations.
		/// </summary>
		const float fieldAdjustExtraRatio = .15f;
		/// <summary>
		/// Ratio of expanded fields to inspector width
		/// </summary>
		const float expandedFieldRatio = .55f;
		/// <summary>
		/// The amount of tab spacing used when a property is dependent visible.
		/// </summary>
		const float dependentVisibleTabSpace = 12f;
		/// <summary>
		/// The amount of vertical padding after a dependent visible property is shown.
		/// </summary>
		const float dependentVisibleVerticalSpace = 5f;
		/// <summary>
		/// Amount of tab spacing for single line texture property.
		/// </summary>
		const float singleLineTexTabSpace = 2f;
		/// <summary>
		/// Amount of spacing above label properties.
		/// </summary>
		const float labelTopSpace = 4f;

		const string centeredSpacingName = "[centered]";
		const string rightBoundSpacingName = "[rightbound]";
		const string maxFieldSpacingName = "[maxfield]";
		const string minFieldSpacingName = "[minfield]";

		const string labelPrefix = "*";
		const string foldoutPrefix = "#";
		const string singleLineTexPrefix = "%";
		const string dependentVisibleTextPrefix = "^";
		const string linkedPropertyPrefix = "&";
		const string vec2Prefix = "2~";
		const string vec3Prefix = "3~";
		const string vec4Prefix = "4~";
		private const string indentPrefix = "-";

		/// <summary>
		/// Representation of a property with the linkedPropertyPrefix.
		/// This is used to pre-gather linked properties and display them
		/// ahead of when we would otherwise reach them.
		/// </summary>
		class LinkedProperty
		{
			public MaterialProperty matProperty;
			public int propIndex;
			public bool wasRendered = false;
		}

		private MaterialEditor matEditor;
		private Dictionary<string, LinkedProperty> currentLinkedProperties = new Dictionary<string, LinkedProperty>();
		private bool fieldCenteredMode = false;
		private bool fieldExpandedMode = false;

		private bool hadOneFoldout = false;
		private bool currentlyInFoldout = false;
		private int currentFoldoutIndex = 0;
		private bool bottomOptionsFoldout = true;

		/// <summary>
		/// Bool list for each foldout encountered. Supports up to 128 foldouts.
		/// </summary>
		private bool[] foldoutArray = new bool[128];

		protected Dictionary<string, System.Action<MaterialEditor, MaterialProperty>> renderExtensions = null;

		public OpenGraphGUIEditor()
		{
			renderExtensions = null;
			for(int i = 0; i < foldoutArray.Length; i++)
			{
				//Initialize foldouts to show by default
				foldoutArray[i] = true;
			}
		}

		//BASE GUI STRUCTURE

		/// <summary>
		/// Main GUI function that will render this panel.
		/// </summary>
		/// <param name="materialEditor"></param>
		/// <param name="properties"></param>
		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			matEditor = materialEditor;
			Material material = materialEditor.target as Material;

			fieldCenteredMode = false;
			fieldExpandedMode = false;
			hadOneFoldout = false;
			currentlyInFoldout = false;
			SetUtilityLabelWidth();

			RenderPropertiesList(properties,material);

			RenderBottomOptions();
		}
		
		private static MaterialProperty FindKeywordProperty(string keywordRef, MaterialProperty[] properties)
		{
			var keywordProp = ShaderGUI.FindProperty(keywordRef, properties, false);

			// special case: bool properties have to be named MY_PROP_ON in ShaderGraph to be exposed,
			// but the actual property is named "MY_PROP" and the keyword is still "MY_PROP_ON".
			if (keywordProp == null && keywordRef.EndsWith("_ON", StringComparison.Ordinal))
				keywordProp = ShaderGUI.FindProperty(keywordRef.Substring(0, keywordRef.Length - "_ON".Length), properties, false);

			return keywordProp;
		}

		/// <summary>
		/// Render the list of properties that are given by the Shader.
		/// This function iterates across each MaterialProperty and determines
		/// how it should be rendered using the OpenGraphGUI tags
		/// if any pop up in the property display names.
		/// </summary>
		/// <param name="properties"></param>
		void RenderPropertiesList(MaterialProperty[] properties, Material material)
		{
			//Track count of foldouts encountered
			int foldoutCount = 0;

			//Track if the last property was non-texture type or
			//contained a non-null input texture
			bool lastWasPopulated = true;

			//First do an initial iteration to find linked properties
			currentLinkedProperties.Clear();
			for (int i = 0; i < properties.Length; i++)
			{
				var thisProp = properties[i];

				if (thisProp.flags.HasFlag(MaterialProperty.PropFlags.HideInInspector))
				{
					//Don't account for this property since it's meant to be hidden
					continue;
				}

				if(thisProp.displayName.StartsWith(linkedPropertyPrefix))
				{
					//This is a linked property, so add it to the dictionary

					var link = new LinkedProperty();
					link.matProperty = thisProp;
					link.propIndex = i;
					link.wasRendered = false;

					currentLinkedProperties.Add(thisProp.displayName, link);
				}
				else if (thisProp.displayName.StartsWith(foldoutPrefix))
				{
					//This is a foldout property
					foldoutCount++;
				}
			}

			//Reset current foldout to 0
			currentFoldoutIndex = 0;
			
			//foreach (var materialShaderKeyword in material.shaderKeywords)
			//{
			//	Debug.Log("materialShaderKeyword: " + materialShaderKeyword);
			//}

			//Now iterate across the properties for real and render them
			for (int i = 0; i < properties.Length; i++)
			{
				var thisProp = properties[i];

				if (thisProp.flags.HasFlag(MaterialProperty.PropFlags.HideInInspector))
				{
					//Don't draw this property since it is meant to be hidden
					continue;
				}

				//Check if this property has a custom extension
				if(renderExtensions != null && renderExtensions.ContainsKey(thisProp.displayName))
				{
					//Invoke the custom render function passed into the dictionary
					renderExtensions[thisProp.displayName].Invoke(matEditor, thisProp);
				}
				else
				{
					var propName = thisProp.displayName;
					propName = Regex.Replace(propName, @"\[[^()]*\]", string.Empty);

					var dependsItem = Regex.Match(thisProp.displayName, @"\[[^()]*\]").Value;
					
					if (!string.IsNullOrEmpty(dependsItem))
					{
						List<string> dependsList = new List<string>();
						dependsItem = dependsItem.Replace("[", "");
						dependsItem = dependsItem.Replace("]", "");
						//if (dependsItem.EndsWith("_ON")) dependsItem = dependsItem.Replace("_ON","");
						if (dependsItem.Contains("&&"))
						{
							dependsList = dependsItem.Split("&&").ToList();
						}
						else
						{
							dependsList.Add(dependsItem);
						}
						
						if (dependsItem.Contains("||") || dependsItem.Contains("("))
						{
						}
						else
						{

							bool hide = true;

							foreach (var dep in dependsList)
							{
								var depends = dep.Replace(" ", "");

								if (dependsList.Count > 1)
								{
									//Debug.Log("Multiple: " + propName +" : "+ depends);
								}

								//var key = FindKeywordProperty(depends,properties);
								//if(key!=null)Debug.Log("Key:" + propName+" - "+key.name +":"+key.intValue);

								var keywordPresent = Array.IndexOf(material.shaderKeywords, depends) >= 0;
								string keywordNom = depends;
								//if (!keywordPresent)
								//{
								//	keywordNom = depends + "_ON";
								//	keywordPresent = Array.IndexOf(material.shaderKeywords, depends + "_ON") >= 0;
								//}
								// TOOO: add OFF keyword?
								if (keywordPresent)
								{
									//Debug.Log(propName + " - keywordPresent: " + keywordNom + ":" + material.IsKeywordEnabled(keywordNom));
									if (material.IsKeywordEnabled(keywordNom)) hide = false;
								}
								else
								{

									//Debug.Log(propName + " - keywordMissing: " + keywordNom + ":"+material.IsKeywordEnabled(depends));
									// if (Shader.IsKeywordEnabled(depends))

									var propertyIndex = material.shader.FindPropertyIndex(depends);
									if (propertyIndex >= 0)
									{

										var propertyType = material.shader.GetPropertyType(propertyIndex);

										//Debug.Log(propName + " - propertyPresent: " + depends + ":" +propertyType  );
										switch (propertyType)
										{
											case ShaderPropertyType.Float:
												hide = material.GetFloat(depends) <= 0;
												break;
											case ShaderPropertyType.Texture:
												hide = material.GetTexture(depends) == null;
												break;
											case ShaderPropertyType.Int:
												hide = material.GetInt(depends) <= 0;
												break;
										}
									}
								}

								//var keywords = material.shader.keywordSpace.FindKeyword(depends);
								//if (keywords.isValid)
								//{
								//	if (material.IsKeywordEnabled(depends)) hide = true;
								//}
								//else
								//{
								//	if (material.HasFloat(depends) && material.GetFloat(depends) <= 0) hide = true;
								//	if (material.HasInt(depends) && material.GetInt(depends) <= 0) hide = true;
								//}

								//propName += " (" + depends + " - " + hide + ")";
								if (hide) continue;
							}

							if (hide) continue;
						}
					}

					var lowerPropName = propName.ToLower();

					//Check the initial type of the property based on name
					if(lowerPropName == centeredSpacingName)
					{
						//Use center spacing and don't render this property
						SetFieldCenteredMode(true);
					}
					else if (lowerPropName == rightBoundSpacingName)
					{
						//Use right bound spacing and don't render this property
						SetFieldCenteredMode(false);
					}
					else if (lowerPropName == maxFieldSpacingName)
					{
						//Use the maximum field width and don't render this property
						SetFieldExpandedMode(true);
					}
					else if (lowerPropName == minFieldSpacingName)
					{
						//Use min field width and don't render this property
						SetFieldExpandedMode(false);
					}
					else if(currentLinkedProperties.ContainsKey(propName) && DoRenderFoldoutProp())
					{
						//This is a linked property, so check if it was rendered already
						var thisLinkedProp = currentLinkedProperties[propName];
						if(thisLinkedProp.wasRendered)
						{
							//It was drawn already by some the link parent
						}
						else
						{
							//Was not drawn?!
							//That's a warning since it's expected to have had a parent render it
							GraphLog("Linked property was not drawn by parent." +
								"Ensure that a single line texture property (" + singleLineTexPrefix + ") precedes this item - " 
								+ propName);
							RenderVisibleProperty(thisLinkedProp.matProperty, propName, i);
						}
					}
					else if (propName.StartsWith(foldoutPrefix))
					{
						//This is a foldout type, so create a new group

						//Trim the foldout prefix
						propName = propName.Substring(foldoutPrefix.Length);
						if(propName.Trim().Length > 0)
						{
							//There is a foldout name to use

							if(currentlyInFoldout)
							{
								//Stop the previous foldout
								EditorGUILayout.EndFoldoutHeaderGroup();
							}

							//Update the current foldout index to the new value before setting it
							currentFoldoutIndex++;
							//This is done first so that later on, DoRenderProp() can tell if the last foldout is unfolded,
							//While still allowing this value to change when it encountered a new foldout.

							//This actually means the first item (0) will be skipped
							//But that's okay because it is never referenced.

							//Render the foldout
							foldoutArray[currentFoldoutIndex] = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutArray[currentFoldoutIndex], propName);

							//Finally, track that we encountered at least one foldout
							hadOneFoldout = true;
							//And tell the next properties that we are in a foldout
							currentlyInFoldout = true;
							
						}
						else
						{
							//End the last foldout if there is one
							if(currentlyInFoldout)
							{
								EditorGUILayout.EndFoldoutHeaderGroup();
								currentlyInFoldout = false;
							}
						}
						
					}
					else if(propName.StartsWith(labelPrefix) && DoRenderFoldoutProp())
					{
						//This is a label type, so show a bold header instead of the property

						//Trim the label prefix
						propName = propName.Substring(labelPrefix.Length);

						RenderLabelProperty(propName);
					}
					else if(propName.StartsWith(indentPrefix) && DoRenderFoldoutProp())
					{
						//Trim the label prefix
						propName = propName.Substring(indentPrefix.Length);
						
						RenderVisibleProperty(thisProp, propName, i);
					}
					else if (propName.StartsWith(dependentVisibleTextPrefix) && DoRenderFoldoutProp())
					{
						//It is dependent, so we will conditionally render it

						//Trim the dependent visible prefix
						propName = propName.Substring(dependentVisibleTextPrefix.Length);

						if (lastWasPopulated)
						{
							RenderDependentVisibleProperty(thisProp, propName, i);
						}
						else
						{
							//Don't render this property
						}
					}
					else if (DoRenderFoldoutProp())
					{
						//It's not dependent, so update populated state based on this
						if (thisProp.type == MaterialProperty.PropType.Texture)
						{
							var tex = thisProp.textureValue;
							lastWasPopulated = tex != null;
						}
						else
						{
							lastWasPopulated = true;
						}
						//Render as an "OpenGraphGUI" property; i.e. it will draw
						RenderVisibleProperty(thisProp, propName, i);
					}
				}
			}
		}

		/// <summary>
		/// Render the bottom emission and instancing fields that are present
		/// on default ShaderGraph GUIs.
		/// </summary>
		void RenderBottomOptions()
		{
			//If we had one foldout earlier, draw this as it's own foldout group
			if(hadOneFoldout)
			{
				if(currentlyInFoldout)
				{
					EditorGUILayout.EndFoldoutHeaderGroup();
				}
				bottomOptionsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(bottomOptionsFoldout, "Advanced");
			}

			//If we don't use the group OR we do & it's unfolded, show the options
			if(!hadOneFoldout || bottomOptionsFoldout)
			{
				matEditor.RenderQueueField();

				matEditor.EnableInstancingField();
				matEditor.DoubleSidedGIField();
				matEditor.EmissionEnabledProperty();
				//Lightmap Emission may be a built-in only concept(?)
				//matEditor.LightmapEmissionProperty();
			}
		}

		//PROPERTY RENDERING

		/// <summary>
		/// Render a property that is dependently visible on an above property.
		/// These types of properties are simply tabbed over by a few pixels
		/// to show the dependency.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="labelName"></param>
		/// <param name="index"></param>
		void RenderDependentVisibleProperty(MaterialProperty v, string labelName, int index)
		{

			//Shift over by a small amount to show the dependency
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.Space(dependentVisibleTabSpace);
			//Adjust label width to compensate
			SetUtilityLabelWidth(dependentVisibleTabSpace);
			//Render the property like we would any other
			RenderVisibleProperty(v, labelName, index);

			//Add in flexible space to separate the fields just in case
			//we're in the centered mode
			GUILayout.FlexibleSpace();
			//Reset the label width
			SetUtilityLabelWidth();

			EditorGUILayout.EndHorizontal();
			//Add extra padding since the horizontal seems to bring them closer
			EditorGUILayout.Space(dependentVisibleVerticalSpace);
		}

		/// <summary>
		/// Render a property which could be one of the custom OpenGraphGUI types.
		/// Otherwise, render the default property view.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="labelName"></param>
		/// <param name="index"></param>
		void RenderVisibleProperty(MaterialProperty v, string labelName, int index)
		{
			bool lastMixedValue = EditorGUI.showMixedValue;
			EditorGUI.showMixedValue = v.hasMixedValue;

			if (labelName.StartsWith(singleLineTexPrefix) || labelName.EndsWith("& "))
			{
				if(v.type == MaterialProperty.PropType.Texture)
				{
					//This is a single line texture type

					//Trim the single line tex prefix
					if(labelName.EndsWith("& "))
					{
						labelName = labelName.Substring(0,labelName.Length-3);
					} else {
						labelName = labelName.Substring(singleLineTexPrefix.Length);
					}

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space(singleLineTexTabSpace);
					//Adjust label width to compensate
					SetUtilityLabelWidth(singleLineTexTabSpace + 5f);
					//Look for a linked property that sits on the same line
					var link = FindLinkedPropertyWithIndex(index + 1);
					if (link != null)
					{
						matEditor.TexturePropertySingleLine(new GUIContent(labelName), v, link.matProperty);
						link.wasRendered = true;
					}
					else
					{
						matEditor.TexturePropertySingleLine(new GUIContent(labelName), v);
					}
					//Add flexible space for the centered mode
					GUILayout.FlexibleSpace();
					//End the horizontal line
					EditorGUILayout.EndHorizontal();
					SetUtilityLabelWidth();
					EditorGUILayout.Space(2f);
				}
				else
				{
					//Labeled as a single line tex type, but not a texture
					GraphLog("Property was labeled as a single line texture (" + singleLineTexPrefix
						+ "), but not a Texture type - " + labelName);
					RenderDefaultPropertyView(v, labelName);
				}
			}
			else if(labelName.StartsWith(vec2Prefix) || labelName.StartsWith(vec3Prefix) || labelName.StartsWith(vec4Prefix))
			{
				if(v.type == MaterialProperty.PropType.Vector)
				{
					//This is a vector type

					int vecCount = 2;
					string thisPrefix = vec2Prefix;
					if(labelName.StartsWith(vec3Prefix))
					{
						vecCount = 3;
						thisPrefix = vec3Prefix;
					}
					else if (labelName.StartsWith(vec4Prefix))
					{
						vecCount = 4;
						thisPrefix = vec4Prefix;
					}

					//Trim the vector prefix
					labelName = labelName.Substring(thisPrefix.Length);

					if(vecCount == 4)
					{
						RenderDefaultPropertyView(v, labelName);
					}
					else
					{
						//Render custom vector fields

						//Set the label width for vectors
						var lastLabelWidth = EditorGUIUtility.labelWidth;
						EditorGUIUtility.labelWidth = 0f;

						EditorGUI.BeginChangeCheck();
						Vector4 vec;
						if(vecCount == 3)
						{
							vec = EditorGUILayout.Vector3Field(labelName, v.vectorValue, GUILayout.ExpandWidth(true));
						}
						else
						{
							vec = EditorGUILayout.Vector2Field(labelName, v.vectorValue, GUILayout.ExpandWidth(true));
						}
						if (EditorGUI.EndChangeCheck())
						{
							v.vectorValue = vec;
						}

						EditorGUIUtility.labelWidth = lastLabelWidth;
					}
				}
				else
				{
					//Labeled as a vector but not one
					GraphLog("Property was labeled as a Vector (" + vec2Prefix + " or " + vec3Prefix + " or " + vec4Prefix
						+ "), but was not a Vector type - " + labelName);
					RenderDefaultPropertyView(v, labelName);
				}
			}
			else
			{
				RenderDefaultPropertyView(v, labelName);
			}

			EditorGUI.showMixedValue = lastMixedValue;
		}

		/// <summary>
		/// Render the default editor code for this property.
		/// </summary>
		/// <param name="v"></param>
		void RenderDefaultPropertyView(MaterialProperty v, string customName = "")
		{

			string finalName = (customName == "") ? v.displayName : customName;

			//Note: May want to check ShaderUtil.GetPropertyType for more complex fields
			switch(v.type)
			{
				case MaterialProperty.PropType.Texture:
					//Seems that default texture property rendering is more
					//thin than what we get in the ShaderGraph default GUI.
					//This step makes it more wide to match that look.
					var lastFieldWidth = EditorGUIUtility.fieldWidth;
					SetUtilityFieldWidth(texFieldBoxSize);
					var newName = Regex.Replace(v.displayName, @"\[[^()]*\]", string.Empty);
					if (newName.StartsWith(indentPrefix))
					{
						//Trim the label prefix
						newName = newName.Substring(indentPrefix.Length);
					}

					if (newName.EndsWith(" & ")) newName = newName.Substring(0, newName.Length - 3);
					matEditor.TextureProperty(v, newName);
					SetUtilityFieldWidth(lastFieldWidth);
					break;
				case MaterialProperty.PropType.Range:
					//Override for slider properties which should always show their slider
					var lastLabel = EditorGUIUtility.labelWidth;
					EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - rightValueBoxSize - 30f;
					matEditor.RangeProperty(v, finalName);
					EditorGUIUtility.labelWidth = lastLabel;
					break;
				default:
					matEditor.ShaderProperty(v, finalName);
					break;
			}
			

		}

		/// <summary>
		/// Render a bold label in this space.
		/// </summary>
		/// <param name="propName"></param>
		void RenderLabelProperty(string propName)
		{
			EditorGUILayout.Space(labelTopSpace);
			EditorGUILayout.LabelField(propName, EditorStyles.boldLabel);
		}
		
		/// <summary>
		/// Render indented property.
		/// </summary>
		/// <param name="propName"></param>
		void RenderIndentedProperty(string propName)
		{
			EditorGUILayout.Space(labelTopSpace);
			EditorGUI.indentLevel++;
			EditorGUILayout.LabelField(propName, EditorStyles.label);
			EditorGUI.indentLevel--;
		}

		//QUERY

		/// <summary>
		/// Check if we should render the current prop
		/// based on foldout status.
		/// </summary>
		/// <returns></returns>
		bool DoRenderFoldoutProp()
		{
			return (!currentlyInFoldout || foldoutArray[currentFoldoutIndex]);
		}

		//EDITOR GUI

		/// <summary>
		/// Set whether to use centered or right bound field spacing in the GUI.
		/// </summary>
		/// <param name="v"></param>
		void SetFieldCenteredMode(bool v)
		{
			fieldCenteredMode = v;
			SetUtilityLabelWidth();
		}

		/// <summary>
		/// Set whether the field should expand to fill the Inspector
		/// (common for the Built-In materials)
		/// Or whether it should keep fields at minimum size
		/// (common for Shader-Graph default look).
		/// </summary>
		/// <param name="v"></param>
		void SetFieldExpandedMode(bool v)
		{
			fieldExpandedMode = v;
			SetUtilityLabelWidth();
		}

		/// <summary>
		/// Set the label width (main layout logic)
		/// </summary>
		/// <param name="offset"></param>
		void SetUtilityLabelWidth(float offset = 0f)
		{
			if(fieldCenteredMode)
			{
				//Set the width to "default"
				//Which seems to be how the non-ShaderGraph built-in URP materials are handled
				EditorGUIUtility.labelWidth = 0f;
				if (fieldExpandedMode)
				{
					EditorGUIUtility.fieldWidth = (EditorGUIUtility.currentViewWidth * expandedFieldRatio) - offset;
					if(EditorGUIUtility.currentViewWidth < minLabelWidthAdjustField)
					{
						EditorGUIUtility.fieldWidth -= EditorGUIUtility.currentViewWidth * fieldAdjustExtraRatio;
					}
				}
				else
				{
					EditorGUIUtility.fieldWidth = rightValueBoxSize * .6f;
				}
			}
			else
			{
				//Set the width of the left side to be current size minus some constant pixel value
				//This seems to be how the default ShaderGUI for ShaderGraphs is handled
				EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - rightValueBoxSize - offset;
				EditorGUIUtility.fieldWidth = 0f;
			}
		}

		/// <summary>
		/// Set the override field width for special situations
		/// </summary>
		/// <param name="size"></param>
		void SetUtilityFieldWidth(float size)
		{
			EditorGUIUtility.fieldWidth = size;
		}

		//UTILITY

		LinkedProperty FindLinkedPropertyWithIndex(int index)
		{
			foreach(string thisKey in currentLinkedProperties.Keys)
			{
				if(currentLinkedProperties[thisKey].propIndex == index)
				{
					return currentLinkedProperties[thisKey];
				}
			}

			return null;
		}

		/// <summary>
		/// Log a warning or error with OpenGraphGUI.
		/// </summary>
		/// <param name="v"></param>
		void GraphLog(string v)
		{
			Debug.Log("OpenGraphGUI: " + v);
		}
	}
}
