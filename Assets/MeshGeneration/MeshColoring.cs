using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static Unity.VisualScripting.Member;


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

        float maxValue = int.MinValue;
        int maxIndex = -1;
        int index = -1;

        foreach (var num in nums)
        {
            index++;

            if (num.y >= maxValue)
            {
                maxValue = num.y;
                maxIndex = index;
            }
        }

        return maxIndex;
    }
    int FindMin(Vector3[] nums)
    {
        float minValue = int.MaxValue;
        int minIndex = -1;
        int index = -1;

        foreach (var num in nums)
        {
            index++;

            if (num.y <= minValue)
            {
                minValue = num.y;
                minIndex = index;
            }
        }

        return minIndex;
    }
    Vector3[] getCoords(int triangleIdx, int[] tris, Vector3[] vertices, int radius)
    {
        Vector3[] coords = new Vector3[36];
        //get vertices in a square
        for(var i = 0; i < radius; i++)
        {
            for(var j = 0; j < radius; j++)
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

        //trust me.

        var radius = 3; 
        var area = chunkSize * chunkSize;
        for(var child = 0; child < area; child++) { 
            Mesh mesh = children[child].mesh;
            Vector3[] vertices = mesh.vertices;
            int[] tris = mesh.triangles;
            // create new colors array where the colors will be created.
            Color[] colors = new Color[vertices.Length];
            for (var i = 0; i < chunkSize; i += radius)
            {
                for(var x = 0; x < chunkSize; x += radius)
                {
                    int idx = i + x * 100;
                    Vector3[] coords = getCoords(idx, tris, vertices, radius);
                    int max = FindMax(coords);
                    int min = FindMin(coords);
                    float distance;
                    double angle;
                    if (min == max)
                    {
                        angle = 0;
                    }
                    distance = Vector3.Distance(coords[min], coords[max]);
                    angle = Math.Atan((coords[max].y - coords[min].y) / distance) * 180;
                    
                    if (angle < 0) angle *= -1;
                    var color = coloringGradient.Evaluate(normalize((float)angle, -90, 90));
                    for (var y = 0; y < radius; y++)
                    {
                        for (var z = 0; z < radius; z++)
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
    public void backToHomeScene()
    {
        SceneManager.LoadScene(sceneName: "Home Screen");
    }
}
