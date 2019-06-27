using UnityEngine;
using BS;

namespace BasPluginExample
{
    // The item module will add a unity component to the item object. See unity monobehaviour for more information: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
    // This component will apply a shock effect on the item when trigger is pressed
    public class ItemShock : MonoBehaviour
    {
        protected EffectShock effectShock;

        protected void Awake()
        {
            this.GetComponent<Item>().OnHeldActionEvent += OnHeldAction;
        }

        protected void Start()
        {
            foreach (EffectData effectData in this.GetComponent<Item>().effects)
            {
                if (effectData is EffectShock)
                {
                    effectShock = effectData as EffectShock;
                    break;
                }
            }
        }

        public void OnHeldAction(Interactor interactor, Handle handle, Interactable.Action action)
        {
            if (action == Interactable.Action.UseStart)
            {
                if (effectShock != null) effectShock.Play(1, 9999999, 5);
            }
            else if (action == Interactable.Action.UseStop || action == Interactable.Action.Ungrab)
            {
                if (effectShock != null) effectShock.Stop();
            }
        }
    }
}