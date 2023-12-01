using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColoring : MonoBehaviour
{
    GameObject meshGameObject;
    MeshFilter[] children;
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

                colors[i] = Color.Lerp(Color.white, Color.black, normalize(point.y, low, high));
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
            if (nums[i].y < min) min = nums[i].y; minIdx = i;
        }
        return minIdx;
    }
    public void ColorMeshBasedOnAngle()
    {
        foreach (var meshFilter in children)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            int[] tris = mesh.triangles;
            // create new colors array where the colors will be created.
            Color[] colors = new Color[vertices.Length];
            for (var i = 0; i < tris.Length; i += 3)
            {
                int idx1 = tris[i], idx2 = tris[i + 1], idx3 = tris[i + 2];
                Vector3[] yCoords = new Vector3[]
                {
                vertices[idx1],
                vertices[idx2],
                vertices[idx3]
                };
                int max = 0;
                int min = 2;
                float distance = Vector3.Distance(yCoords[min], yCoords[max]);
                double angle = Math.Atan((yCoords[max].y - yCoords[min].y) / distance) * 180;
                if (angle < 0) angle *= -1;
                var color = Color.Lerp(Color.black, Color.white, normalize((float)angle, -90, 90));
                colors[idx1] = color;
                colors[idx2] = color;
                colors[idx3] = color;
            }

            mesh.colors = colors;
        }
    }
}
