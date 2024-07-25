using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{

    public interface IMonoBehaviourReference
    {
        public MonoBehaviour GetMonoBehaviourReference();
    }

    [System.Serializable]
    public class ListMonoBehaviourReference<I> where I : IMonoBehaviourReference
    {
        [SerializeField] private List<MonoBehaviour> _monoBehaviours;

        public int Count
        {
            get
            {
                return _monoBehaviours == null ? 0 : _monoBehaviours.Count;
            }
        }

        public bool TryGetAtIndex(int index, out I result)
        {
            result = default(I);
            if (_monoBehaviours == null) return false;
            if (index < 0) return false;
            if (index >= _monoBehaviours.Count) return false;

            if(_monoBehaviours[index] is I iMonoBehaviourReference)
            {
                result = iMonoBehaviourReference;
                return true;
            }

            return false;
        }

        public void Add(I toAdd)
        {
            if (_monoBehaviours == null)
            {
                _monoBehaviours = new List<MonoBehaviour>();
            }

            _monoBehaviours.Add(toAdd.GetMonoBehaviourReference());
        }

        public void RemoveAt(int index)
        {
            if (_monoBehaviours == null) return;
            if (index < 0) return;
            if (index >= _monoBehaviours.Count) return;

            _monoBehaviours.RemoveAt(index);
        }

        public void Clear()
        {
            _monoBehaviours.Clear();
        }
    }
}