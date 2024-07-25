using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	[Serializable]
	public class LevelData : CatalogData
	{
#if ODIN_INSPECTOR
		[BoxGroup("Level")]
#endif
		public string name;
#if ODIN_INSPECTOR
		[BoxGroup("Level")]
#endif
		[TextArea]
		public string description;
#if ODIN_INSPECTOR
		[BoxGroup("Level")]
#endif
		public string descriptionLocalizationId;
#if ODIN_INSPECTOR
		[BoxGroup("Level")]
#endif
		public string sceneAddress;
		[NonSerialized]
		public IResourceLocation sceneLocation;
#if ODIN_INSPECTOR
		[BoxGroup("Level")]
#endif
		public bool showOnlyDevMode = false;

#if ODIN_INSPECTOR
		[BoxGroup("Level")]
#endif
		public bool showInLevelSelection = true;

#if ODIN_INSPECTOR
		[BoxGroup("Map")]
#endif
		public string worldmapPrefabAddress = "Bas.Image.Map.Default";
#if ODIN_INSPECTOR
		[BoxGroup("Map")]
#endif
		public string worldmapTextureAddress = "Bas.Worldmap.Eraden";
#if ODIN_INSPECTOR
		[BoxGroup("Map")]
#endif
		public string worldmapLabel;
#if ODIN_INSPECTOR
        [BoxGroup("Map")]
#endif
        [Tooltip("The audio container that will play when the player clicks to travel to this location from the worldmap")]
        public string worldMapTravelAudioContainerAddress = "Bas.AudioGroup.UI.LightClick";  
        [NonSerialized] public IResourceLocation worldMapTravelAudioContainerLocation;
        
		[NonSerialized]
		public IResourceLocation worldmapPrefabLocation;
		[NonSerialized]
		public GameObject worldmapPrefab;
		[NonSerialized]
		public IResourceLocation worldmapTextureLocation;
		[NonSerialized]
		public Texture2D worldmapTexture2D;
		[NonSerialized]
		public int worldmapHash;

#if ODIN_INSPECTOR
		[BoxGroup("Map location")]
#endif
		public int mapLocationIndex;

#if ODIN_INSPECTOR
		[BoxGroup("Map location")]
#endif
		public bool showOnMap = true;

#if ODIN_INSPECTOR
		[BoxGroup("Map location")]
#endif
		public bool hideOnAndroid;

#if ODIN_INSPECTOR
		[BoxGroup("Map location")]
#endif
		public string mapLocationIconAddress = "Bas.Icon.Location.Default";
		[NonSerialized]
		public IResourceLocation mapLocationIconLocation;
		[NonSerialized]
		public Texture2D mapLocationIcon;

#if ODIN_INSPECTOR
		[BoxGroup("Map location")]
#endif
		public string mapLocationIconHoverAddress = "Bas.Icon.Location.DefaultHover";
		[NonSerialized]
		public IResourceLocation mapLocationIconHoverLocation;
		[NonSerialized]
		public Texture2D mapLocationIconHover;

#if ODIN_INSPECTOR
		[BoxGroup("Map location")]
#endif
		public string mapPreviewImageAddress;
		[NonSerialized]
		public IResourceLocation mapPreviewImageLocation;
		[NonSerialized]
		public Texture2D mapPreviewImage;

#if ODIN_INSPECTOR
		[BoxGroup("Modes"), ShowInInspector]
#endif
		public List<Mode> modes;

#if ODIN_INSPECTOR
		[BoxGroup("Modes")]
#endif
		public int modePickCountPerRank = 2;

#if ODIN_INSPECTOR
		[BoxGroup("Cameras")]
#endif
		public List<CustomCameras> customCameras;

#if ODIN_INSPECTOR
		[BoxGroup("AI")]
#endif
		public ThrowableReference throwableRefType = ThrowableReference.Item;
#if ODIN_INSPECTOR
		[BoxGroup("AI")]
#endif
		public string improvisedThrowableID = null;

		public enum ThrowableReference
		{
			Item,
			Table
		}


		[Serializable]
		public struct CustomCameras
		{
			public Vector3 position;
			public Quaternion rotation;
		}


		[Serializable]
		public class Mode
		{
			[JsonMergeKey]
			public string name = "Default";
			public string displayName = "{Default}";
			[TextArea]
			public string description = "{NoDescription}";

#if ODIN_INSPECTOR
			public List<ValueDropdownItem<string>> GetAllGameModeID()
			{
				return Catalog.GetDropdownAllID(Category.GameMode);
			}

			[ValueDropdown(nameof(GetAllGameModeID))]
#endif
			public List<string> allowGameModes;

			public int mapOrder;

			public PlayerDeathAction playerDeathAction = PlayerDeathAction.AskReload;

            public enum PlayerDeathAction
            {
				None,
				AskReload,
				ReloadLevel,
				LoadHome,
				PermaDeath,
            }

#if ODIN_INSPECTOR
            [ShowInInspector]
#endif
			public List<LevelModule> modules = new List<LevelModule>();
#if ODIN_INSPECTOR
			[ShowInInspector]
#endif
			public List<OptionBase> availableOptions = new List<OptionBase>();

		}

		public enum MusicType
		{
			Background,
			Combat,
		}

		public override int GetCurrentVersion()
		{
			return 3;
		}

	}
}
