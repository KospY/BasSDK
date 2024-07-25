using Shadowood;
using UnityEngine;

public class OceanVisibilityController : MonoBehaviour
{
    public bool hideUnderwater = true;

    public void Reset()
    {
        FindObjectOfType<OceanFogController>()?.FindAllOceanVisibilityControllers();
    }
}
