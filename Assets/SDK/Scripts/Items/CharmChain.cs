using UnityEngine;

public class CharmChain : MonoBehaviour
{
    [SerializeField]
    private GameObject ChainPrefab;

    [SerializeField] private GameObject CharmParent;


    private Chain charmChain;

    #region Charm Link Fields
    //[SerializeField, Tooltip("Number of nodes of the charm link, this changes the length of the chain")]
    //private int numberOfNodes = 25;

    [SerializeField, Tooltip("Iterations for the simulation of the chain/rope. More iterations is more expensive but more stable")]
    private int NumberOfIterationsForSimulation = 10;

    private GameObject RopePrefabInstance;

    private GameObject CharmObject;


    private float xScale;
    private float yScale;
    private float zScale;

    [SerializeField] public bool isTransformMoveTest = false;
    #endregion
    private void Awake()
    {
        RopePrefabInstance = Instantiate(ChainPrefab, transform);
        ChainPrefab.SetActive(true);

        //init chain properties
        charmChain = RopePrefabInstance.GetComponent<Chain>();
        charmChain.HeadAnchorTransform = transform;
        charmChain.iterations = NumberOfIterationsForSimulation;
        charmChain.isCharmChain = true;

        CharmParent = Instantiate(CharmParent);

        charmChain.TetheredJointParent = CharmParent;
        if (CharmParent != null && CharmParent.transform.childCount > 0)
        {
            CharmObject = CharmParent.transform.GetChild(0).gameObject;
            charmChain.TetheredObject = CharmObject;
        }

        if (!isTransformMoveTest)
        {
            CharmParent.transform.SetParent(transform, false);
            xScale = 1 / transform.localScale.x;
            yScale = 1 / transform.localScale.y;
            zScale = 1 / transform.localScale.z;

            Vector3 charmScale = CharmObject.transform.localScale;
            CharmObject.transform.localScale = new Vector3(charmScale.x * xScale, charmScale.y * yScale, charmScale.z * zScale);
        }
    }
}
