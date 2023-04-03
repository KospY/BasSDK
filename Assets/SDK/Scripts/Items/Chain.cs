using UnityEngine;
public class Chain : MonoBehaviour
{
    [Header("Instanced Mesh Details")]
    [SerializeField, Tooltip("The Mesh of chain link to render")] public Mesh link;
    [SerializeField, Tooltip("The chain link material, must have gpu instancing enabled!")] public Material linkMaterial;

    [Space]


    [Space]

    [Header("Verlet Parameters")]

    [SerializeField, Tooltip("The distance between each link in the chain")] float nodeDistance = 0.35f;
    [SerializeField, Tooltip("The radius of the sphere collider used for each chain link")] float nodeColliderRadius = 0.2f;

    private float gravityStrength = 2;

    [SerializeField, Tooltip("The number of chain links. Decreases performance with high values and high iteration")] public int totalNodes = 100;

    [SerializeField, Range(0, 1), Tooltip("Modifier to dampen velocity so the simulation can stabilize")] float velocityDampen = 0.95f;

    [SerializeField, Range(0, 0.99f), Tooltip("The stiffness of the simulation. Set to lower values for more elasticity")] float stiffness = 0.8f;


    [SerializeField, Tooltip("Setting this will test collisions for every n iterations. Possibly more performance but less stable collisions")] public int iterateCollisionsEvery = 1;

    [SerializeField, Tooltip("Iterations for the simulation. More iterations is more expensive but more stable")] public int iterations = 100;

    private int colliderBufferSize = 1;

    Collider[] colliderHitBuffer;

    // Need a better way of stepping through collisions for high Gravity
    // And high Velocity
    Vector3 gravity;

    Vector3 HeadPosition;

    [Space]

    Vector3[] previousNodePositions;

    Vector3[] currentNodePositions;
    private Quaternion[] currentNodeRotations;

    SphereCollider nodeCollider;
    private GameObject nodeTester;
    Matrix4x4[] matrices;

    //[SerializeField]
    public GameObject TetheredJointParent;

    public GameObject TetheredObject;

    [SerializeField]
    public Transform HeadAnchorTransform;


    public bool isCharmChain = false;

    [SerializeField]
    public bool isChainInverted = false;

    private int chainInvertedSign = 1;

    [SerializeField] private Vector3 ChainLinkRotationTest;
    [SerializeField] private Vector3 test;
    //public float ChainScale;
    public float ChainScale = 1;
    private Vector3 ChainScaleVector = Vector3.one;

    void Start()
    {
        //totalNodes += TagChainNodes;
        currentNodePositions = new Vector3[totalNodes];
        previousNodePositions = new Vector3[totalNodes];
        currentNodeRotations = new Quaternion[totalNodes];

        colliderHitBuffer = new Collider[colliderBufferSize];
        gravity = new Vector3(0, -gravityStrength, 0);

        // using a single dynamically created GameObject to test collisions on every node
        nodeTester = new GameObject();
        nodeTester.name = "Chain Node Collider";
        nodeTester.layer = 10;
        nodeCollider = nodeTester.AddComponent<SphereCollider>();
        nodeCollider.radius = nodeColliderRadius;
        nodeCollider.isTrigger = true; //prevent RB collisions, only use internally for custom chain physics
        nodeTester.transform.parent = this.transform;


        //init nodes and start positions
        matrices = new Matrix4x4[totalNodes];
        HeadPosition = HeadAnchorTransform.position;
        //TetheredObject.transform.localScale *= ChainScale;
        ChainScaleVector *= ChainScale;
        nodeDistance *= ChainScale;

        if (isChainInverted)
        {
            chainInvertedSign = -1;
        }

        Vector3 startPosition = HeadPosition;
        for (int i = 0; i < totalNodes; i++)
        {

            currentNodePositions[i] = startPosition;
            currentNodeRotations[i] = Quaternion.identity;
            previousNodePositions[i] = startPosition;
            matrices[i] = Matrix4x4.TRS(startPosition, Quaternion.identity, Vector3.one);
            startPosition.y -= nodeDistance * (chainInvertedSign);
        }

        //init tethered object parent position to tail of chain
        if (isCharmChain)
        {
            TetheredJointParent.transform.position = currentNodePositions[totalNodes - 1];
        }

        
    }


    void Update()
    {
        Graphics.DrawMeshInstanced(link, 0, linkMaterial, matrices, totalNodes); // Instanced drawing here is really performant over using GOs
    }

    private void FixedUpdate()
    {
        HeadPosition = HeadAnchorTransform.position; 
        Simulate();
        for (int i = 0; i < iterations; i++)
        {
            ApplyConstraint();

            if (iterateCollisionsEvery == 0)
            {
                AdjustCollisions();
            }
            else if (i % iterateCollisionsEvery == 0)
            {
                AdjustCollisions();
            }
        }

        SetAngles();
        TranslateMatrices();
    }

    private void Simulate()
    {
        var fixedDt = Time.fixedDeltaTime;
        Vector3 velocity = Vector3.zero;
        for (int i = 1; i < totalNodes; i++)
        {
            velocity = currentNodePositions[i] - previousNodePositions[i];
            velocity *= velocityDampen;

            previousNodePositions[i] = currentNodePositions[i];

            // calculate new position
            Vector3 newPos = currentNodePositions[i] + velocity;
            newPos += gravity * fixedDt * chainInvertedSign;
            currentNodePositions[i] = newPos;
        }
        
        /*
        //if charm chain, move tethered so that it follows the tail of the chain
        if (TetheredObject != null && isCharmChain)
        {
            Rigidbody tetheredRB = TetheredObject.GetComponent<Rigidbody>();
            tetheredRB.MovePosition(currentNodePositions[totalNodes - 1]);
            //TetheredObject.transform.position = currentNodePositions[totalNodes - 1];
            
        }
        */
    }

    private void ApplyConstraint()
    {
        //constrain head of chain to assigned headposition
        currentNodePositions[0] = HeadPosition;
        previousNodePositions[0] = HeadPosition;
        currentNodePositions[1] = HeadPosition;
        previousNodePositions[1] = HeadPosition;

        
        
        if (TetheredObject != null)
        {
            currentNodePositions[totalNodes - 1] = TetheredObject.transform.position;
            previousNodePositions[totalNodes - 1] = TetheredObject.transform.position;
        }
        
        

        for (int i = 0; i < totalNodes - 1; i++)
        {

            var node1 = currentNodePositions[i];
            var node2 = currentNodePositions[i + 1];

            // Get the current distance between rope nodes
            float currentDistance = (node1 - node2).magnitude;
            float difference = Mathf.Abs(currentDistance - nodeDistance);
            Vector3 direction = Vector3.zero;

            // determine what direction we need to adjust our nodes
            if (currentDistance > nodeDistance)
            {
                direction = (node1 - node2).normalized;
            }
            else if (currentDistance < nodeDistance)
            {
                direction = (node2 - node1).normalized;
            }

            // calculate the movement vector
            Vector3 movement = direction * difference;

            // apply correction
            currentNodePositions[i] -= (movement * stiffness);
            currentNodePositions[i + 1] += (movement * stiffness);

        }
        
           if (TetheredObject != null)
        {
            //currentNodePositions[totalNodes - 1] = TetheredObject.transform.position;
            //previousNodePositions[totalNodes - 1] = TetheredObject.transform.position;
        }

        if (TetheredObject != null && isCharmChain)
        {
            Rigidbody tetheredRB = TetheredObject.GetComponent<Rigidbody>();
            //tetheredRB.MovePosition(currentNodePositions[totalNodes - 1]);
            TetheredObject.transform.position = currentNodePositions[totalNodes - 1];

        }
    }

    private void AdjustCollisions()
    {
        for (int i = 0; i < totalNodes; i++) 
        {
            if (i % 2 == 0) continue;

            int result = -1;
   
            int layerMask = (int)1 << 31; // chain nodes only collide with PlayerHandAndFoot layer, any other layers required? DO NOT ALLOW COLLISIONS WITH CHAIN'S OWN LAYER 
            result = Physics.OverlapSphereNonAlloc(currentNodePositions[i], nodeColliderRadius + 0.01f, colliderHitBuffer, layerMask);
            if (result > 0)
            {
                for (int n = 0; n < result; n++)
                {
                    Vector3 colliderPosition = colliderHitBuffer[n].transform.position;
                    Quaternion colliderRotation = colliderHitBuffer[n].gameObject.transform.rotation;
                    Vector3 dir;
                    float distance;
                    Physics.ComputePenetration(nodeCollider, currentNodePositions[i], Quaternion.identity, colliderHitBuffer[n], colliderPosition, colliderRotation, out dir, out distance);
                    Debug.Log($"direction is {dir}");

                    if (dir.x == 0 && dir.z == 0) //((isChainInverted && dir.) || (!isChainInverted && dir.x == 0 && dir.z == 0))
                    {
                        continue;
                    }
                    else
                    {
                        dir = dir.normalized; //added to stop AABB errors, was returning +- Infinity at times, please leave comment in case AABB errors return
                        currentNodePositions[i] += dir * distance;
                    }
                }
            }
        }
    }

    void SetAngles()
    {
        var fixedDt = Time.fixedDeltaTime;

        currentNodeRotations[0] = HeadAnchorTransform.root.rotation;
        for (int i = 0; i < totalNodes - 1; i++) 
        {
            var node1 = currentNodePositions[i];
            var node2 = currentNodePositions[i + 1];

            var dir = (node2 - node1).normalized;
            if (dir != Vector3.zero)
            {
                //Quaternion desiredRotation = Quaternion.LookRotation(dir, Vector3.right);
                Quaternion desiredRotation = Quaternion.LookRotation(dir, Vector3.right);
                currentNodeRotations[i + 1] = desiredRotation.normalized; 
            }

            if (i % 2 == 0)
            {
                currentNodeRotations[i + 1] *= Quaternion.Euler(90, 0, 90);
            } else
            {
                currentNodeRotations[i + 1] *= Quaternion.Euler(0, 90, 0);
            }
        }
    }

    void TranslateMatrices()
    {
        for (int i = 0; i < totalNodes; i++)
        {
            matrices[i].SetTRS(currentNodePositions[i], currentNodeRotations[i].normalized, ChainScaleVector);
        }
    }

    private void OnDrawGizmos()
    {
        if (currentNodePositions != null && currentNodePositions.Length > 0)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(currentNodePositions[0], nodeColliderRadius);
            for (int i = 0; i < totalNodes; i++)
            {
                if (i == 1)
                {
                    Gizmos.color = Color.blue;
                }
                Gizmos.DrawWireSphere(currentNodePositions[i], nodeColliderRadius);
                Gizmos.color = Color.yellow;
            }
        }
    }
}
