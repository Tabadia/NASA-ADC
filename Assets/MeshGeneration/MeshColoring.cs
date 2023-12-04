using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;


public class MeshColoring : MonoBehaviour
{
    public Gradient coloringGradient;
    GameObject meshGameObject;
    MeshFilter[] children;

    public int chunkSize = 100;
    // Start is called before the first frame update
    void Start()
    {
        meshGameObject = GameObject.Find("Mesh");
        
        
    }

    public void changeMode(string mode)
    {
        children = meshGameObject.GetComponentsInChildren<MeshFilter>();
        Debug.Log(children.Length);
        if (mode == "angle")
        {
            ColorMeshBasedOnAngle();
        }
        else if (mode == "height")
        {
            ColorMeshBasedOnHeight();
        }
    }
    float normalize(float num, float min, float max)
    {
        return (num - min) / (max - min);
    }
    public void ColorMeshBasedOnHeight()
    {

        float high = 0f;
        float low = 0f;
        foreach (MeshFilter meshFilter in children)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            foreach (Vector3 point in vertices)
            {
                if (point.y > high) high = point.y;
                if (point.y < low) low = point.y;

            }
        }
        foreach (var meshFilter in children)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;

            // create new colors array where the colors will be created.
            Color[] colors = new Color[vertices.Length];
            int i = 0;
            foreach (Vector3 point in vertices)
            {

                colors[i] = coloringGradient.Evaluate(normalize(point.y, low, high));
                i++;
            }
            mesh.colors = colors;
        }
    }
    int FindMax(Vector3[] nums)
    {
        int maxIdx = 0;
        float max = -9999999;
        for (var i = 0; i < nums.Length; i++)
        {
            if (nums[i] == null) continue;
            if (nums[i].y > max) max = nums[i].y; maxIdx = i;
        }
        return maxIdx;
    }
    int FindMin(Vector3[] nums)
    {
        int minIdx = 0;
        float min = 9999999;
        for (var i = 0; i < nums.Length; i++)
        {
            if (nums[i] == null) continue;
            if (nums[i].y < min) min = nums[i].y; minIdx = i;
        }
        return minIdx;
    }
    Vector3[] getCoords(int triangleIdx, int[] tris, Vector3[] vertices)
    {
        Vector3[] coords = new Vector3[36];
        //get vertices in a square
        for(var i = 0; i < 6; i++)
        {
            for(var j = 0; j < 6; j++)
            {
                if (vertices.Length <= i + triangleIdx + j * 100) continue;
                coords[i] = vertices[i + triangleIdx + j*100];
            }
        }
        return coords;
    }
    public void ColorMeshBasedOnAngle()
    {
        /*
         * Edwin's Note:
         * DO NOT put Debug.Log things here. There are insane amount of calculations being made, even a small slowdown can fuck your pc.
         */
        var area = chunkSize * chunkSize;
        for(var child = 0; child < area; child++) { 
            Mesh mesh = children[child].mesh;
            Vector3[] vertices = mesh.vertices;
            int[] tris = mesh.triangles;
            // create new colors array where the colors will be created.
            Color[] colors = new Color[vertices.Length];

            for (var i = 0; i < chunkSize; i += 6)
            {
                for(var x = 0; x < chunkSize; x += 6)
                {
                    int idx = i + x * 100;
                    Vector3[] coords = getCoords(idx, tris, vertices);
                    int max = 0;
                    int min = 35;
                    while (coords[min] == null)
                    {
                        min--;
                    }
                    float distance = Vector3.Distance(coords[min], coords[max]);
                    double angle = Math.Atan((coords[max].y - coords[min].y) / distance) * 180;
                    if (angle < 0) angle *= -1;
                    var color = coloringGradient.Evaluate(normalize((float)angle, -90, 90));
                    for (var y = 0; y < 6; y++)
                    {
                        for (var z = 0; z < 6; z++)
                        {

                            if (colors.Length <= y + idx + z * 100) continue;
                            colors[y + idx + z * 100] = color;
                        }
                    }
                }
            }
            //dont blow up pc PLS
            mesh.colors = colors;
       }
    }
}
