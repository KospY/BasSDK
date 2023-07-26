using UnityEngine.UI;

namespace ThunderRoad
{
    /// <summary>
    /// Special class used as a empty UI graphic component so it can be used as a raycast target
    /// </summary>
    public class RaycastTarget : Graphic 
    {
        public override void SetMaterialDirty() { }
        public override void SetVerticesDirty() { }
        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear();
        }
    }
}
