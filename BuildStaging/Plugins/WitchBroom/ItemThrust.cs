using UnityEngine;
using ThunderRoad;

namespace WitchBroom
{
    // The item module will add a unity component to the item object. See unity monobehaviour for more information: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
    // This component will apply a force on the player rigidbody to the direction of an item transform when the trigger is pressed (see custom reference in the item definition component of the item prefab)
    public class ItemThrust : MonoBehaviour
    {
        protected float currentThrust;

        protected Item item;
        protected ItemModuleThrust module;

        protected EffectInstance effectInstance;
        protected RagdollHand rightHand;
        protected RagdollHand leftHand;
        protected Transform thrustTransform;

        protected void Awake()
        {
            item = this.GetComponent<Item>();
            item.OnHeldActionEvent += OnHeldAction;
            thrustTransform = item.GetCustomReference("Thrust");
            module = item.data.GetModule<ItemModuleThrust>();
            EffectData effectData = Catalog.GetData<EffectData>(module.effectId);
            effectInstance = effectData.Spawn(this.transform, false);
            effectInstance.Play();
            effectInstance.SetIntensity(0);
        }

        public void OnHeldAction(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
        {
            if (action == Interactable.Action.UseStart)
            {
                if (ragdollHand.side == Side.Right)
                {
                    rightHand = ragdollHand;
                }
                else
                {
                    leftHand = ragdollHand;
                }
            }
            else if (action == Interactable.Action.UseStop || action == Interactable.Action.Ungrab)
            {
                if (ragdollHand.side == Side.Right)
                {
                    rightHand = null;
                }
                else
                {
                    leftHand = null;
                }
            }
        }

        protected void Update()
        {
            currentThrust = 0;
            if (rightHand)
            {
                currentThrust += Mathf.Lerp(module.minForce, module.maxForce, PlayerControl.GetHand(rightHand.side).useAxis);
            }
            if (leftHand)
            {
                currentThrust += Mathf.Lerp(module.minForce, module.maxForce, PlayerControl.GetHand(leftHand.side).useAxis);
            }
            effectInstance.SetIntensity(Mathf.InverseLerp(module.minForce * 2, module.maxForce * 2, currentThrust));
        }

        protected void FixedUpdate()
        {
            if (currentThrust > 0)
            {
                Player.local.locomotion.rb.AddForce(thrustTransform.forward * currentThrust, ForceMode.Force);
            }
        }
    }
}