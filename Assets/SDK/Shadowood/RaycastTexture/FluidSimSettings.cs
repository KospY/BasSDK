#if FLUXY
using System;
using Fluxy;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class FluidSimSettings
{
    [Tooltip("Enable this while playing with settings")]
    [GUIColor("@realtimeUpdate ? Color.red : Color.white")]
    public bool realtimeUpdate;
    
    public float gravity = -25;

    public Vector3 constantForce = Vector3.zero;

    [Range(0, 1)] public float viscosity = 0.937f;

    [Tooltip("How strong the shoreline will slow down velocity/cause water collision, value of 1 will stop the water dead still where the shore is near")] [UnityEngine.Range(0, 1)]
    public float shoreCollision = 0.6f;

    [Tooltip("Introduces foam where the shore edge is, water will pick this up and flow it down stream")] [UnityEngine.Range(0, 1)]
    public float foamDensity = 1;

    //

    [FoldoutGroup("NormalMap Distort")] public bool useNormalMap = false;

    
    [Tooltip("Normal map to add swirly, follows the UV of the mesh")] //
    [ShowIf("@useNormalMap")]
    [FoldoutGroup("NormalMap Distort")]
    [InlineButton("FindTextures","Default")]
    public Texture2D surfaceNormals;

    [FoldoutGroup("NormalMap Distort")] [ShowIf("@useNormalMap")] //
    public Vector2 normalTiling = Vector2.one;

    [FoldoutGroup("NormalMap Distort")]
    [Tooltip("if set to 1 will overpower/replace other effects like gravity")] //
    [ShowIf("@useNormalMap")]
    [Range(0,2)]
    public float normalStrength = 0.5f;
    
    [HideInInspector]
    public bool setup; // TODO remove

    //public enum eFluidMode
    //{
    //    oldSplat,
    //    new
    //}

    //
    
    /// <summary>
    /// Pull settings from existing fluxy components
    /// </summary>
    public FluidSimSettings(FluxyContainer fluxyContainer, FluxyTarget fluxyTarget)
    {
        if (fluxyContainer.surfaceNormals != null && fluxyContainer.normalScale > 0) useNormalMap = true;
        surfaceNormals = fluxyContainer.surfaceNormals;
        normalStrength = fluxyContainer.normalScale;
        normalTiling = fluxyContainer.normalTiling;

        gravity = fluxyContainer.gravity.y;

        constantForce = fluxyContainer.externalForce;
        viscosity = fluxyContainer.viscosity;

        shoreCollision = fluxyTarget.velocityWeight;
        foamDensity = fluxyTarget.densityWeight;

        FindTextures();
    }
    
    public void FindTextures()
    {
        Debug.Log("FluidSimSettings: FindTextures");
        const string normalName = "FluidSimDistortion";
        surfaceNormals = Resources.Load<Texture2D>(normalName);
    }

    public void ApplySettings(FluxyContainer fluxyContainer, FluxyTarget fluxyTarget)
    {
        Debug.Log("FluidSimSettings: ApplySettings");
        
        if (useNormalMap)
        {
            fluxyContainer.surfaceNormals = surfaceNormals;
            fluxyContainer.normalScale = normalStrength;
            fluxyContainer.normalTiling = normalTiling;
        }
        else
        {
            fluxyContainer.normalScale = 0;
        }

        fluxyContainer.gravity = new Vector3(0, gravity, 0);
        fluxyContainer.externalForce = constantForce;
        fluxyContainer.viscosity = viscosity;

        fluxyTarget.velocityWeight = shoreCollision;
        fluxyTarget.densityWeight = foamDensity;

        fluxyContainer.GetComponent<FluxySolver>().enabled = Application.isPlaying ? false : realtimeUpdate;
    }
}


#endif