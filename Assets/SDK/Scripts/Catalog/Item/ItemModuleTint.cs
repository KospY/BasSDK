using UnityEngine;

namespace ThunderRoad
{
	public class ItemModuleTint : ItemModule
	{
		public Color tintColour = Color.white;
		public string shaderProperty = "_BaseColor";
		private int shaderPropertyId;
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			shaderPropertyId = Shader.PropertyToID(shaderProperty);
			foreach(Renderer renderer in item.renderers)
			{
				if (renderer.gameObject.TryGetOrAddComponent(out MaterialInstance materialInstance))
				{
					foreach (var material in materialInstance.materials)
					{
						material.SetColor(shaderPropertyId, tintColour);
					}
				}
			}
		}
	}
}
