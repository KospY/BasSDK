using System;
using UnityEngine;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    [RequireComponent(typeof(Ragdoll))]
    public class RagdollMassScalar : MonoBehaviour
    {
        [System.Serializable]
        public abstract class MassScalar<T>
        {
            [Delayed]
            public float totalMass = -1f;
            [HideInInspector, NonSerialized]
            protected float lastTotalMass = -1f;
            protected List<T> bodies;
            protected bool blockValidate = false;

            public void PopulateBodies(Transform ragdoll)
            {
                bodies = new List<T>();
                foreach (var part in ragdoll.GetComponentsInChildren<RagdollPart>())
                {
                    bodies.Add(GetBodyFromPart(part));
                }
            }

            public abstract T GetBodyFromPart(RagdollPart part);

            public abstract void SetMass(T t, float mass);

            public abstract float GetMass(T t);

            public abstract void DefaultMassesFallback();

            public void OnValidate(Transform ragdoll)
            {
                if (blockValidate) return;
                blockValidate = true;
                PopulateBodies(ragdoll);
                if (Application.isPlaying) return;
                if (totalMass < 0)
                {
                    totalMass = GetTotalMass<T>(bodies, GetMass);
                    lastTotalMass = totalMass;
                }
                if (lastTotalMass < 0) lastTotalMass = totalMass;
                if (!totalMass.IsApproximately(lastTotalMass))
                {
                    ScaleMass(bodies, totalMass / lastTotalMass, GetMass, SetMass);
                    lastTotalMass = totalMass;
                }
                blockValidate = false;
            }

            public void ChildrenChanged(Transform ragdoll)
            {
                PopulateBodies(ragdoll);
            }

            public void UpdateMasses()
            {
                blockValidate = true;
                float partsMass = GetTotalMass<T>(bodies, GetMass);
                if (!partsMass.IsApproximately(totalMass))
                {
                    totalMass = partsMass;
                }
                if (totalMass < 0)
                {
                    DefaultMassesFallback();
                }
                blockValidate = false;
            }
        }

        [System.Serializable]
        public class PhysicBodyScalar : MassScalar<PhysicBody>
        {
            public override PhysicBody GetBodyFromPart(RagdollPart part) => part.GetPhysicBody();

            public override float GetMass(PhysicBody t) => t.mass;

            public override void SetMass(PhysicBody t, float mass) => t.mass = mass;

            public override void DefaultMassesFallback() { }
        }

        [System.Serializable]
        public class HandledScalar : MassScalar<RagdollPart>
        {
            public override RagdollPart GetBodyFromPart(RagdollPart part) => part;

            public override float GetMass(RagdollPart t) => t.handledMass;

            public override void SetMass(RagdollPart t, float mass) => t.handledMass = mass;

            public override void DefaultMassesFallback()
            {
                blockValidate = true;
                totalMass = 0f;
                foreach (RagdollPart part in bodies)
                {
                    part.handledMass = part.GetPhysicBody().mass;
                    totalMass += part.GetPhysicBody().mass;
                }
                blockValidate = false;
            }
        }

        [System.Serializable]
        public class RagdolledScalar : MassScalar<RagdollPart>
        {
            public override RagdollPart GetBodyFromPart(RagdollPart part) => part;

            public override float GetMass(RagdollPart t) => t.ragdolledMass;

            public override void SetMass(RagdollPart t, float mass) => t.ragdolledMass = mass;

            public override void DefaultMassesFallback() { }
        }

        public PhysicBodyScalar standing = new PhysicBodyScalar();
        public HandledScalar handled = new HandledScalar();
        public RagdolledScalar ragdolled = new RagdolledScalar();

        private void Awake()
        {
            if (Application.isPlaying) Destroy(this);
        }

        private void OnValidate()
        {
            standing.OnValidate(transform);
            handled.OnValidate(transform);
            ragdolled.OnValidate(transform);
        }

        private void OnTransformChildrenChanged()
        {
            standing.ChildrenChanged(transform);
            handled.ChildrenChanged(transform);
            ragdolled.ChildrenChanged(transform);
        }

        private void OnDrawGizmos()
        {
            standing.UpdateMasses();
            handled.UpdateMasses();
            ragdolled.UpdateMasses();
        }

        public static float GetTotalMass<T>(List<T> bodies, Func<T, float> getPartMass)
        {
            float total = 0f;
            foreach (var body in bodies)
            {
                total += getPartMass(body);
            }
            return total;
        }

        public static void ScaleMass<T>(List<T> bodies, float scale, Func<T, float> getPartMass, Action<T, float> setPartMass)
        {
            foreach (var body in bodies)
            {
                setPartMass(body, getPartMass(body) * scale);
            }
        }
    }
}
