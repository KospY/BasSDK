using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    /// <summary>
    /// This component allows objects and items to be broken in various different ways.
    /// </summary>
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Items/Breakable.html")]
    public class Breakable : ThunderBehaviour
    {
        /// <summary>
        /// Default handle ID.
        /// </summary>
        private string DefaultHandleID { get; } = "ObjectHandleProp";

        /// <summary>
        /// Allow breakables to be broken?
        /// </summary>
        public static bool AllowBreaking { get; set; } = true;

        /// <summary>
        /// Allows two handles to be linked between meshes.
        /// 
        /// TODO: Possibly overhaul this to instead allow Handle[] and GetClosest to handler?
        ///       Also might be worth making this a struct, it should be immutable anyway.
        /// </summary>
        [Serializable]
        public class HandleLink
        {
            public Handle handleMain;
            public Handle handleSecondary;

            public HandleLink(Handle handleMain, Handle handleSecondary)
            {
                this.handleMain = handleMain;
                this.handleSecondary = handleSecondary;
            }
        }

        /// <summary>
        /// Used to create and edit breakable points targets
        /// </summary>
        [Serializable]
        public class BreakPoint
        {
            public enum BreakPointType
            {
                Sphere
            }

            public BreakPointType type;
            public Vector3 center;
            public float radius;

            public BreakPoint(BreakPointType type, Vector3 center, float radius, float length)
            {
                this.type = type;
                this.center = center;
                this.radius = radius;
            }

            /// <summary>
            /// Check if the target transform is within the contact point.
            /// </summary>
            public bool CheckIfHit(Transform transform, ContactPoint contactPoint)
            {
                Vector3 p = contactPoint.point;
                Vector3 c = transform.TransformPoint(center);

                switch (type)
                {
                    // Distance check using the sphere point type.
                    case BreakPointType.Sphere:
                    return (p - c).sqrMagnitude < radius * radius;
                }

                return false;
            }
        }

        public enum EnviroBreakMode
        {
            Never,
            Handled,
            Always,
        }

        [Tooltip("The parent Breakable of this item, if any."), Header("Parent Breakable")]
        public Breakable parentBreakable;
        
        [Tooltip("This is the mesh which is whole, containing JUST the unbroken mesh."), Header("Object holders")]
        public GameObject unbrokenObjectsHolder;

        [Tooltip("Put the parent object containing all the broken meshes here.")]
        public GameObject brokenObjectsHolder;

        [Tooltip("When enabled, break points allow the Breakables Item to only break if damaged inside these points. " +
                 "They will not break outside these points."), Header("Break points")]
        public bool useBreakPoints;

        [Tooltip("This is the list of the Break Points if you have any. Currently, only spheres are accepted.")]
        public List<BreakPoint> breakPoints = new List<BreakPoint>();

        [Tooltip("Whether or not the item can be broken, can be changed."), Header("Damage")]
        public bool canBreak = true;

        [Tooltip("Whether or not the item can break on static environment. Defaults to always."), Header("Damage")]
        public EnviroBreakMode breakOnEnviro = EnviroBreakMode.Always;

        [Tooltip("Whether or not this item can be broken by something that isn't a collision, can be changed.")]
        public bool contactBreakOnly = false;

        [Tooltip("Set if this item should or shouldn't use \"momentum health\". If set to false, the breakable will break in one sufficient hit.")]
        public bool useHealth = false;

        [Tooltip("How much \"health\" the breakable has. This health amount is decreased proportional to the strength of the hit.")]
        public float momentumHealth = 0f;

        [Tooltip("How much momentum is needed for a hit to count for damage.")]
        public float minimumDamageMomentum = 20f;

        [Tooltip("Applies a maximum amount of \"momentum health\" damage that can be dealt in a single hit Can be used to ensure that something can't be broken in a single hit, no matter what.")]
        public bool clampDamage = false;

        [Tooltip("The max collision momentum damage that can be taken in a single hit. Only applies if \"clampDamage\" is set to true.")]
        public float maxCollisionMomentum = float.MaxValue;

        [Tooltip("The min (x) and max (y) values for the mass factor when calculating momentum. Only tweak this if you find that heavy/lighter objects aren't properly affecting the breakable.")]
        public Vector2 momentumMassFactorClamp = new Vector2(1f, 5f);

        [Tooltip("When enabled, objects under a given mass won't be able to break this item." +
                 " Minimal mass is defined by \"minimalMassThreshold\"")]
        public bool ignoreObjectUnderCertainMass;

        [Tooltip("Objects under this mass won't be able to break this item." +
                 " Used when \"ignoreObjectUnderCertainMass\" is true")]
        public float minimalMassThreshold = 1.01f;

        [Tooltip("When enabled, the Item can instantly break if the Break Velocity is met.")]
        public bool canInstantaneouslyBreak = true;

        [Tooltip("Momentum damage required to instantly break the Item.")]
        public float instantaneousBreakDamage = 50f;

        [Tooltip("Cooldown between hits for the item to break."), Header("Time")]
        public float hitCooldownTime = .1f;

        [Tooltip("When ticked, linked items inside breakable will despawn.")]
        public bool despawnLinkedItem = true;

        [Tooltip("Delay before the linked items inside breakable will despawn")]
        public float despawnLinkedItemDelay = 5;

        [Tooltip(
             "These link between handles, which means that if the unbroken item has a handle, " +
             "and one of the breakables has the same handle, you will keep grabbing the handle " +
             "despite the held item switching to a broken version."), Header("Handles")]
        public HandleLink[] handleLinks = Array.Empty<HandleLink>();

        [Header("Explosion forces")]
        [Tooltip("When enabled, broken pieces of this item won't collide. Useful for big items, to avoid jitter.")]
        public bool ignoreCollisionOnBreak;

        [Tooltip("When enabled, broken pieces Will be pushed when parallel to the hit direction.")]
        public bool useExplosionForce;

        [Tooltip("Multiplication factor used when applying the explosion force. It multiplies the hit force.")]
        public float explosionForceFactor = 50f;

        [NonSerialized] public List<Item> subUnbrokenItems = new List<Item>();
        [NonSerialized] public List<PhysicBody> subUnbrokenBodies = new List<PhysicBody>();
        [NonSerialized] public List<Item> subBrokenItems = new List<Item>();
        [NonSerialized] public List<PhysicBody> subBrokenBodies = new List<PhysicBody>();
        [NonSerialized] public List<PhysicBody> allSubBodies = new List<PhysicBody>();

        private bool isInitialized = false;
        private Rigidbody currentRigidbody;
        private float lastHitTime;
        private Vector3[] subBrokenItemsBarycenters;
        private List<Damager> piercedDamagers = new List<Damager>();
        private List<(Transform transform, PhysicBody body)> bodyTransforms = new List<(Transform transform, PhysicBody body)>();

        [Header("Events")]
        public UnityEvent<float> onTakeDamage;
        public UnityEvent<float> onNonBreakHit;
        public UnityEvent<float> onBreak;

        [Header("Deprecated")]
        [Tooltip("Don't change this!")]
        public bool updated = false;
        [Tooltip("Number of hits required for the item to break.")]
        public int hitsUntilBreak = 1;
        [Tooltip("How much force is needed for the \"Hits Until Break\" to count as a hit.")]
        public float neededImpactForceToDamage = 20f;
        [Tooltip("Force required to instantly break the Item.")]
        public float instantaneousBreakVelocityThreshold = 50f;

        /// <summary>
        /// Used as a buffer to cache collision contact points
        /// </summary>
        private ContactPoint[] contactPoints = new ContactPoint[16];

        private List<Mesh> brokenItemMeshes = new List<Mesh>();

        /// <summary>
        /// Is this broken?
        /// </summary>
        public bool IsBroken { get; private set; }

        /// <summary>
        /// Unbroken item if any.
        /// </summary>
        public Item LinkedItem { get; private set; }

        public Vector3 ItemLocalBarycenter { get; private set; }

        [NonSerialized]
        public Collision breakingCollision;

#if UNITY_EDITOR
        /// <summary>
        /// Used from the custom editor to change the gizmos.
        /// </summary>
        [NonSerialized] public bool editingBreakpointsThroughEditor;


        private void OnValidate()
        {
            if (!updated)
            {
                momentumHealth = minimumDamageMomentum * hitsUntilBreak;
                if (hitsUntilBreak > 0)
                {
                    clampDamage = true;
                    maxCollisionMomentum = minimumDamageMomentum + 0.01f;
                }
                minimumDamageMomentum = neededImpactForceToDamage;
                instantaneousBreakDamage = instantaneousBreakVelocityThreshold;
                updated = true;
            }
	        if (Application.isPlaying)
		        return;
#if UNITY_EDITOR
	        if (UnityEditor.BuildPipeline.isBuildingPlayer)
		        return;
#endif
	        if (this.InPrefabScene())
		        return;
	        if (!gameObject.activeInHierarchy)
		        return;
            RetrieveSubItems();
            //get the parent breakable
            Breakable componentInParent = GetComponentInParent<Breakable>();
            if (componentInParent != this)
            {
	            parentBreakable = componentInParent;
            }
            //I am the parent, hide the broken stuff
            if (parentBreakable == null)
            {
            }
        }
#endif

        /// <summary>
        /// Cache the rigidbodies and items in lists
        /// </summary>
        public void RetrieveSubItems()
        {
            if (!unbrokenObjectsHolder || !brokenObjectsHolder)
            { return; }

            List<Breakable> subBreakables = new List<Breakable>();
            Rigidbody[] rigidbodies = unbrokenObjectsHolder.GetComponentsInChildren<Rigidbody>(true);
            for (int i = 0; i < rigidbodies.Length; i++)
            {
                Item isItem = rigidbodies[i].GetComponent<Item>();

                if (isItem != null)
                { subUnbrokenItems.Add(isItem); }
                else
                { subUnbrokenBodies.Add(rigidbodies[i].AsPhysicBody()); }

                allSubBodies.Add(rigidbodies[i].AsPhysicBody());
            }

            // GetComponentsInChildren Doesn't pick up current item for whatever reason
            if (TryGetComponent(out Rigidbody currentBody))
            {
                Item isItem = currentBody.GetComponent<Item>();

                if (isItem != null && !subUnbrokenItems.Contains(isItem))
                { subUnbrokenItems.Add(isItem); }
                else if (!subUnbrokenBodies.Contains(currentBody.AsPhysicBody()))
                { subUnbrokenBodies.Add(currentBody.AsPhysicBody()); }
            }

            rigidbodies = brokenObjectsHolder.GetComponentsInChildren<Rigidbody>(true);
            for (int i = 0; i < rigidbodies.Length; i++)
            {
                Breakable isSubBreakable = rigidbodies[i].gameObject.GetComponent<Breakable>();
                if (isSubBreakable)
                {
                    isSubBreakable.RetrieveSubItems();
                    subBreakables.Add(isSubBreakable);
                }

                Item isItem = rigidbodies[i].GetComponent<Item>();

                if (isItem)
                { subBrokenItems.Add(isItem); }
                else
                { subBrokenBodies.Add(rigidbodies[i].AsPhysicBody()); }

                allSubBodies.Add(rigidbodies[i].AsPhysicBody());

                //cache the mesh, it is getting removed somehow
                MeshFilter mf = rigidbodies[i].GetComponent<MeshFilter>();
                if (mf)
                {
                    brokenItemMeshes.Add(mf.mesh);
                }
            }

            // Remove sub breakables items & rb from this list, otherwise it un-parent them on break
            for (int i = 0; i < subBreakables.Count; i++)
            {
                Breakable subBreakable = subBreakables[i];

                for (int j = 0; j < subBreakable.subBrokenItems.Count; j++)
                { subBrokenItems.Remove(subBreakable.subBrokenItems[j]); }

                for (int j = 0; j < subBreakable.subBrokenBodies.Count; j++)
                { subBrokenBodies.Remove(subBreakable.subBrokenBodies[j]); }
            }
        }

        /// <summary>
        /// Set whether or not the breakable can be broken
        /// </summary>
        public void SetBreakable(bool active)
        {
        }

        public void Explode(float force, Vector3 origin, float radius, float upwardsModifier, ForceMode forceMode)
        {
        }

        /// <summary>
        /// Break the breakable.
        /// This is used as a fake method to bypass the real behaviour, to break via events for example.
        /// </summary>
        public void Break()
        {
        }

        /// <summary>
        /// Checks if two transforms are close enough.
        /// </summary>
        /// <param name="source">Source transform</param>
        /// <param name="other">Transform to compare the source against</param>
        /// <param name="positionThreshold">Allowed distance threshold for transforms to be considered close</param>
        /// <param name="rotationThreshold">Allowed rotation threshold (in degrees) for transforms to be considered close</param>
        /// <param name="checkRotation">If true, compares rotations.</param>
        /// <returns>True if threshold are low enough</returns>
        private bool IsTransformsRoughlyMatching(Transform source, Transform other,
            float positionThreshold = .2f,
            float rotationThreshold = 10f,
            bool checkRotation = false)
        {
            if (checkRotation && Quaternion.Angle(source.rotation, other.rotation) > rotationThreshold)
                return false;
            if ((source.position - other.position).magnitude > positionThreshold)
                return false;

            return true;
        }

        /// <summary>
        /// Fill the handle links list with handles that have close enough transformation.
        /// </summary>
        public void AutoMatchHandles()
        {
            if (!unbrokenObjectsHolder)
                return;
            if (!brokenObjectsHolder)
                return;

            Handle GetClosestHandle(List<Handle> handles, Handle handle)
            {
                float closestDistanceSqr = Mathf.Infinity;
                Handle closestHandle = null;
                foreach (Handle behaviour in handles)
                {
                    var directionToTarget = behaviour.transform.position - handle.transform.position;
                    var dSqrToTarget = directionToTarget.sqrMagnitude;

                    if (dSqrToTarget < closestDistanceSqr)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        closestHandle = behaviour;
                    }
                }

                return closestHandle;
            }

            var unbrokenHandles = unbrokenObjectsHolder.GetComponentsInChildren<Handle>();
            var brokenHandles = brokenObjectsHolder.GetComponentsInChildren<Handle>();

            var brokenHandlesList = new List<Handle>();
            brokenHandlesList.AddRange(brokenHandles);

            var handleLinksBuffer = new List<HandleLink>(Mathf.Max(unbrokenHandles.Length, brokenHandles.Length));

            for (var i = 0; i < unbrokenHandles.Length; i++)
            {
                var unbrokenHandle = unbrokenHandles[i];
                var closest = GetClosestHandle(brokenHandlesList, unbrokenHandle);
                if (!closest)
                    continue;

                if (IsTransformsRoughlyMatching(unbrokenHandle.transform, closest.transform))
                {
                    handleLinksBuffer.Add(new HandleLink(unbrokenHandle, closest));
                    brokenHandlesList.Remove(closest);
                }
            }

            handleLinks = handleLinksBuffer.ToArray();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            for (var i = 0; i < handleLinks.Length; i++)
            {
                var handleLink = handleLinks[i];
                if (handleLink == null)
                    continue;
                if (handleLink.handleMain == null)
                    continue;
                if (handleLink.handleSecondary == null)
                    continue;

                var color = Color.HSVToRGB(i / (float)handleLinks.Length, 1f, 1f);
                Gizmos.color = color;
                var p1 = handleLink.handleMain.transform.position;
                var p2 = handleLink.handleSecondary.transform.position;
                Gizmos.DrawLine(p1, p2);

                Gizmos.DrawWireSphere(p1, .03f);
                Gizmos.DrawWireSphere(p2, .03f);

                color.a = .1f;
                Gizmos.color = color;
                Gizmos.DrawSphere(p1, .03f);
                Gizmos.DrawSphere(p2, .03f);

                var matrix = Gizmos.matrix;

                var filter = handleLink.handleSecondary.GetComponentInParent<MeshFilter>();
                if (filter && handleLink.handleSecondary.gameObject.activeInHierarchy)
                {
                    Gizmos.matrix = filter.transform.localToWorldMatrix;

                    color.a = 1f;
                    Gizmos.DrawWireMesh(filter.sharedMesh);
                    color.a = .1f;
                    Gizmos.DrawMesh(filter.sharedMesh);
                }

                Gizmos.matrix = matrix;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!useBreakPoints)
                return;

            for (int i = 0; i < breakPoints.Count; i++)
            {
                var bp = breakPoints[i];
                var color = Color.HSVToRGB(i / (float)breakPoints.Count, 1f, 1f);
                Gizmos.color = color;
                switch (bp.type)
                {
                    case BreakPoint.BreakPointType.Sphere:
                    Gizmos.DrawWireSphere(transform.TransformPoint(bp.center), bp.radius);
                    color.a = .5f;
                    Gizmos.color = color;
                    if (editingBreakpointsThroughEditor)
                        Gizmos.DrawSphere(transform.TransformPoint(bp.center), bp.radius);
                    break;
                }
            }
        }
#endif //UNITY_EDITOR
    }
}