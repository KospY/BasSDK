using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
	public class UiItemSpawnerItemElement : UIItemSpawnerGridElement
	{
		[SerializeField]
		private TextMeshProUGUI tier;
		[SerializeField]
		private RawImage tierColor;
		[SerializeField]
		private GameObject counter;
		[SerializeField]
		private TextMeshProUGUI counterText;
        [SerializeField]
        private RawImage droppedIcon;

	}
}
