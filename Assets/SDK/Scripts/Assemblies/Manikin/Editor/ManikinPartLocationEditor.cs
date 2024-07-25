using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Manikin
{
    [CustomEditor(typeof(ManikinWardrobeData))]
    public class ManikinPartLocationEditor : Editor
    {
        SerializedProperty editorLocationLabels;
        SerializedProperty assetPrefab;
        SerializedProperty occlusionID;
        SerializedProperty occlusionIDHash;

        SerializedProperty channels;
        SerializedProperty layers;
        SerializedProperty fullyOccludedLayers;
        SerializedProperty partialOccludedLayers;
        SerializedProperty partialOccludedMasks;

        SerializedProperty tags;
        SerializedProperty dependencies;

        private void OnEnable()
        {
            editorLocationLabels = serializedObject.FindProperty("editorLocationLabels");
            assetPrefab = serializedObject.FindProperty("assetPrefab");
            occlusionID = serializedObject.FindProperty("occlusionID");
            occlusionIDHash = serializedObject.FindProperty("occlusionIDHash");

            channels = serializedObject.FindProperty("channels");
            layers = serializedObject.FindProperty("layers");
            fullyOccludedLayers = serializedObject.FindProperty("fullyOccludedLayers");
            partialOccludedLayers = serializedObject.FindProperty("partialOccludedLayers");
            partialOccludedMasks = serializedObject.FindProperty("partialOccludedMasks");

            tags = serializedObject.FindProperty("tags");
            dependencies = serializedObject.FindProperty("dependencies");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ManikinWardrobeData partLocation = target as ManikinWardrobeData;

            EditorGUILayout.PropertyField(assetPrefab);
            EditorGUILayout.PropertyField(editorLocationLabels);
            GUILayout.Space(20);

            if (editorLocationLabels.objectReferenceValue != null)
            {
                ManikinEditorLocationLabels locationLabels = editorLocationLabels.objectReferenceValue as ManikinEditorLocationLabels;
                string[] channelDisplayNames = locationLabels.GetChannelNames();

                channels.arraySize = EditorGUILayout.IntField("Number of Channels", channels.arraySize);
                for(int index = 0; index < channels.arraySize; index++)
                {
                    layers.arraySize = channels.arraySize;
                    fullyOccludedLayers.arraySize = channels.arraySize;
                    partialOccludedLayers.arraySize = channels.arraySize;
                    partialOccludedMasks.arraySize = channels.arraySize;

                    SerializedProperty channel = channels.GetArrayElementAtIndex(index);
                    SerializedProperty layer = layers.GetArrayElementAtIndex(index);
                    SerializedProperty fullyOccludedLayer = fullyOccludedLayers.GetArrayElementAtIndex(index);
                    SerializedProperty partialOccludedLayer = partialOccludedLayers.GetArrayElementAtIndex(index);
                    SerializedProperty partialOccludedMask = partialOccludedMasks.GetArrayElementAtIndex(index);

                    int selectedChannel = 0;
                    string channelName = channel.stringValue;
                    string[] layerDisplayNames = locationLabels.GetLayerNamesByChannelName(channelName);

                    for (int i = 0; i < channelDisplayNames.Length; i++)
                    {
                        if (channelName.Equals(channelDisplayNames[i]))
                        {
                            selectedChannel = i;
                            break;
                        }
                    }

                    EditorGUILayout.BeginVertical("HelpBox");
                    selectedChannel = EditorGUILayout.Popup("Channel", selectedChannel, channelDisplayNames);
                    channel.stringValue = channelDisplayNames[selectedChannel];

                    if (layerDisplayNames != null)
                    {
                        layer.intValue = EditorGUILayout.Popup("Layer", layer.intValue, layerDisplayNames);
                        fullyOccludedLayer.intValue = EditorGUILayout.MaskField("Fully Occluded Layers", fullyOccludedLayer.intValue, layerDisplayNames);

                        if ((fullyOccludedLayer.intValue & (1 << layer.intValue)) != 0)
                        {
                            EditorGUILayout.HelpBox("This occludes itself!", MessageType.Warning);
                        }

                        partialOccludedLayer.intValue = EditorGUILayout.MaskField("Partial Occluded Layers", partialOccludedLayer.intValue, layerDisplayNames);

                        if ((partialOccludedLayer.intValue & (1 << layer.intValue)) != 0)
                        {
                            EditorGUILayout.HelpBox("This occludes itself!", MessageType.Warning);
                        }
                    }

                    if (partialOccludedLayer.intValue > 0 || partialOccludedLayer.intValue == -1) //-1 == everything
                    {
                        string[] bitmasksNames = locationLabels.GetBitMaskNamesByChannelName(channel.stringValue);
                        if (bitmasksNames != null && bitmasksNames.Length > 0)
                        {
                            partialOccludedMask.intValue = EditorGUILayout.MaskField("Partial Occluded Mask", partialOccludedMask.intValue, bitmasksNames);
                        }
                        else
                        {
                            EditorGUILayout.HelpBox("No bitmask labels set for this channel!", MessageType.Warning);
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            }

            EditorGUILayout.PropertyField(occlusionID);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(occlusionIDHash);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(tags);
            EditorGUILayout.PropertyField(dependencies);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
