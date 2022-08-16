using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
	[HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/SpellTouchEventLinker")]
	[AddComponentMenu("ThunderRoad/Spell Touch Event Linker")]
	[RequireComponent(typeof(Collider))]
	public class SpellTouchEventLinker : MonoBehaviour
	{
		public enum Step
		{
			Enter,
			Exit
		}
		[Serializable]
		public class SpellUnityEvent
		{
			public string spellId;
			[NonSerialized]
			public int spellHashId;
			public Step step;
			public float minCharge = 0;
			public UnityEvent onActivate;
		}
		public List<SpellUnityEvent> spellEvents = new List<SpellUnityEvent>();

	}
}
