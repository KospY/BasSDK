using UnityEngine;

namespace ThunderRoad
{
    public interface IToolControllable
    {
        public abstract bool IsCopyable();

        public void CopyControllableTo(UnityEngine.Object other)
        {
            GameObject gameObject = other as GameObject ?? (other is Component component ? component.gameObject : null);
            if (gameObject == null)
            {
                Debug.LogError("Tried to copy a component to an object which isn't in the scene!");
                return;
            }
            if (!IsCopyable())
            {
                Debug.LogWarning("Tried to copy a component type that doesn't support copying!");
                return;
            }
            (gameObject.AddComponent(GetType()) as IToolControllable).CopyFrom(this);
        }

        public abstract void CopyTo(UnityEngine.Object other);

        public abstract void CopyFrom(IToolControllable original);

        public abstract void Remove();

        public abstract Transform GetTransform();

        public abstract void ReparentAlign(Component other);

        public void ReparentAlignTransform(Component other)
        {
            Transform transform = GetTransform();
            transform.parent = other.transform;
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}
