using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ThunderRoad.Manikin
{
    [CreateAssetMenu(menuName = "Manikin/Editor Locations Labels")]
    public class ManikinEditorLocationLabels : ScriptableObject
    {
	    
#if UNITY_EDITOR
	    
	    public static ManikinEditorLocationLabels Get()
	    {
		    string pathToAsset = "Assets/SDK/Scripts/Assemblies/Manikin/Config/HumanoidLocationLabels.asset";
		    ManikinEditorLocationLabels labels = AssetDatabase.LoadAssetAtPath<ManikinEditorLocationLabels>(pathToAsset);
		    if (labels == null)
		    {
			    Debug.Log($"Creating new ManikinEditorLocationLabels. It is missing from {pathToAsset}");
			    labels = ScriptableObject.CreateInstance<ManikinEditorLocationLabels>();
			    AssetDatabase.CreateAsset(labels, pathToAsset);
			    AssetDatabase.SaveAssets();
		    }
		    return labels;
	    }
#endif
        const int bitMaskLength = 32; //32 for now, but we could extend this to use up to 4 channels of a UV for 128.

        [System.Serializable] //string[] in list isn't serializable without this wrapper?
        public class ChannelData
        {
            public string channelName;
            public string[] layerNames;
            public string[] bitMaskNames; //we'll associated the bitmasks with the channel for now.
        }
        public List<ChannelData> channels = new List<ChannelData>();   

        
        public string GetLayerName(string channel, int layer)
        {
	        //get the channelData
	        ChannelData channelData = channels.Find(x => x.channelName == channel);
	        for (int i = 0; i < channelData.layerNames.Length; i++)
	        {
		        if (i == layer)
		        {
			        return channelData.layerNames[i];
		        }
	        }
	        return string.Empty;
        }
        
        public int GetLayer(string channel, string layer)
        {
	        //get the channelData
	        ChannelData channelData = channels.Find(x => x.channelName == channel);
	        //get the layer index
	        for (int i = 0; i < channelData.layerNames.Length; i++)
	        {
		        if (channelData.layerNames[i] == layer)
		        {
			        return i;
		        }
	        }

	        return -1;
        }
        
        public string[] GetChannelNames()
        {
            string[] names = new string[channels.Count];
            for(int i = 0; i < channels.Count; i++)
            {
                names[i] = channels[i].channelName;
            }
            return names;
        }

        public string[] GetLayerNamesByChannelName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            for (int i = 0; i < channels.Count; i++)
            {
                if (channels[i].channelName == name)
                {
                    if (channels[i].layerNames != null)
                    {
                        return channels[i].layerNames;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        public string[] GetBitMaskNamesByChannelName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            for (int i = 0; i < channels.Count; i++)
            {
                if(channels[i].channelName == name)
                {
                    if (channels[i].bitMaskNames != null)
                    {
                        return channels[i].bitMaskNames;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }
    }
}