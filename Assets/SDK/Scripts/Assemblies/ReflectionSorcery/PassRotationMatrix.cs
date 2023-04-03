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

    private void Awake()
    {
        Setup();
    }

    private void OnValidate()
    {
        if (!isActiveAndEnabled) return;
        Run();
    }

    private void Update()
    {
        if (autoRun) Run();
    }

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
        if (renderers.Count == 0 && GetComponent<Renderer>()) renderers.Add(GetComponent<Renderer>());

        Setup();       
        //matBlock.Clear();
        
        if (setInversion)
        {
            Vector3 invert = Vector3.one;
            if (invertX) invert.Set(-1, 1, 1);
            if (invertY) invert.Set(invert.x, -1, invert.z);
            if (invertZ) invert.Set(invert.x, invert.y, -1);
            matBlock.SetVector("_Invert", invert);
        }

        {
            matBlock.SetMatrix("_CubeMatrix", cubeRotMatrix);
            matBlock.SetMatrix("_CubeMatrixParent", parentMatrix * cubeRotMatrix.inverse);
            matBlock.SetMatrix("_WorldRotMat", capturedMatrix);
        }

        if (setPosition)
        {
            Vector3 pos;

            switch (type)
            {
                case eType.portal:
                    pos = position;
                    break;
                case eType.mirror:
                    pos = transform.TransformPoint(position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            matBlock.SetVector("_CubePosition", pos);
        }

        if (setCubePosition)
        {
            matBlock.SetVector("_CubePos", cubeposition);
        }

        if (setSize)
        {
            matBlock.SetVector("_CubeSize", scale);
        }

        foreach (var r in renderers)
        {
            r.SetPropertyBlock(matBlock);
        }
    }
}
