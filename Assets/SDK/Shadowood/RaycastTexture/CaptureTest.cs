using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CaptureTest : MonoBehaviour
{
    public bool debug;
    public bool debugDepth;

    public int res = 64;

    public CaptureRay captureRay = new CaptureRay();

    private List<RayData> colors = new List<RayData>();


    void Capture()
    {
        colors.Clear();
        for (int x = 0; x < res; x++)
        {
            for (int y = 0; y < res; y++)
            {
                Vector3 rayStart = transform.TransformPoint(new Vector3(x, y, 0));
                Vector3 rayDir = transform.forward;

                var col = captureRay.Capture(rayStart, rayDir);
                colors.Add(new RayData(rayStart, rayDir, col));
            }
        }

        //captureRay.CaptureFinished();
    }

    public float debugSize = 0.1f;

    void OnDrawGizmos()
    {
        if (!debug) return;
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;

        if (colors.Count <= 0) return;

        /*
        int i = 0;
        for (int x = 0; x < res; x++)
        {
            for (int y = 0; y < res; y++)
            {
               
                Vector3 rayStart = transform.TransformPoint(new Vector3(x, y, 0));
                Vector3 rayDir = transform.forward;

                Gizmos.color = colors[i];
                
                Gizmos.DrawRay(rayStart, rayDir);
                
                i++;
            }
        }*/

        foreach (var rayData in colors)
        {
            //Debug.Log(rayData.col.linear);
            Gizmos.color = new Color(rayData.col.r, rayData.col.g, rayData.col.b, 1);
            if (debugDepth) Gizmos.color = new Color(rayData.col.a, rayData.col.a, rayData.col.a, 1);

            //Gizmos.DrawRay(rayData.pos, rayData.dir);
            Gizmos.DrawCube(rayData.pos, Vector3.one * debugSize);
        }
    }
}

