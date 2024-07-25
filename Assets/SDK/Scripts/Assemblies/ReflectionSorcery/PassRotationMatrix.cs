using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PassRotationMatrix : MonoBehaviour
{
    public MaterialPropertyBlock matBlock;

    //public bool setRotation;
    public bool setInversion, setPosition, setSize, setCubePosition;

    public bool invertX, invertY, invertZ;

    public List<Renderer> renderers = new List<Renderer>();

    public Vector3 position, scale, cubeposition;

    public enum eType
    {
        portal,
        mirror
    }

    // TODO Fix reflection type then re-enable
    [HideInInspector] public eType type = eType.portal;

    public bool autoRun = true;

    [HideInInspector] public Matrix4x4 cubeRotMatrix;
    [HideInInspector] public Matrix4x4 parentMatrix;
    public Matrix4x4 capturedMatrix;

    //


    private void OnEnable()
    {
        if (matBlock == null) return;
        //if (Application.isPlaying)
        //{
        //    matBlock.SetFloat("_Debug", 0);
        //}
        //else
        //{
        //    matBlock.SetFloat("_Debug", 1);
        //}
    }

    public void Setup()
    {
        if (matBlock == null) matBlock = new MaterialPropertyBlock();
    }

    public void Run()
    {
    }
}
