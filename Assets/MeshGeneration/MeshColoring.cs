using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;
using System.Threading.Tasks;

public class MeshColoring : MonoBehaviour
{
    public Gradient coloringGradientHeight;
    public Gradient coloringGradientAngle;

    GameObject meshGameObject;
    MeshFilter[] children;
    public int angleRadius = 3;

    public int azimuthRadius = 3;

    public Vector3 EARTH_LOCATION = new Vector3(361000, 0, -42100);
    public int chunkSize = 100;
    // Start is called before the first frame update
    async void Start()
    {
        meshGameObject = GameObject.Find("Mesh");
        children = meshGameObject.GetComponentsInChildren<MeshFilter>();
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
        } else if(mode == "azimuth")
        {
            colorMeshBasedOnAzimuth();
        }
    }
    float normalize(float num, float min, float max)
    {
        return (num - min) / (max - min);
    }
    public Color getColor(float num, string mode) {
        if (mode == "height")
            return coloringGradientHeight.Evaluate(num);
        else 
            return coloringGradientAngle.Evaluate(num);
    }
    private void ColorMeshBasedOnHeight()
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
            Color32[] colors = new Color32[vertices.Length];
            int i = 0;
            foreach (Vector3 point in vertices)
            {

                colors[i] = getColor(normalize(point.y, low, high), "height");
                i++;
            }
            mesh.colors32 = colors;
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
    //void ColorMeshBasedOnAngle()
    //{
    //    // create new colors array where the colors will be created.
    //    for (var child = 0; child < 1024; child++) {

    //        Mesh mesh = children[child].mesh;
    //        Vector3[] vertices = mesh.vertices;
    //        int[] tris = mesh.triangles;

    //        Color[] colors = new Color[vertices.Length];
    //        for (var i = 0; i < tris.Length; i += 3)
    //        {
    //            int idx1 = tris[i], idx2 = tris[i + 1], idx3 = tris[i + 2];
    //            Vector3[] yCoords = new Vector3[]
    //            {
    //                vertices[idx1],
    //                vertices[idx2],
    //                vertices[idx3]
    //            };
    //            int max = 0;
    //            int min = 2;
    //            float distance = Vector3.Distance(yCoords[min], yCoords[max]);
    //            double angle = Math.Atan((yCoords[max].y - yCoords[min].y) / distance) * 180;
    //            if (angle < 0) angle *= -1;
    //            var color = getColor(normalize((float)angle, -90, 90), "angle");
    //            colors[idx1] = color;
    //            colors[idx2] = color;
    //            colors[idx3] = color;
    //        }
    //        mesh.colors = colors;
    //    }

    //}
    private void ColorMeshBasedOnAngle()
    {
        /*
         * Edwin's Note:
         * DO NOT put Debug.Log things here. There are insane amount of calculations being made, even a small slowdown can fuck your pc.
         */
        /*
        PERFORMANCE BENCHMARK: On my pc(1024 chunks): 1.78s
        */
        //trust me.

        var area = chunkSize * chunkSize;
        for (var child = 0; child < area; child++)
        {
            Mesh mesh = children[child].mesh;
            Vector3[] vertices = mesh.vertices;
            int[] tris = mesh.triangles;
            // create new colors array where the colors will be created.
            Color32[] colors = new Color32[vertices.Length];
            for (var i = 0; i < chunkSize; i += angleRadius)
            {
                for (var x = 0; x < chunkSize; x += angleRadius)
                {
                    int idx = i + x * 100;
                    Vector3[] coords = getCoords(idx, tris, vertices, angleRadius);
                    int max = 0;
                    int min = 2;
                    float distance;
                    double angle;
                    if (min == max)
                    {
                        angle = 0;
                    }
                    distance = Vector3.Distance(coords[min], coords[max]);
                    angle = Math.Atan((coords[max].y - coords[min].y) / distance) * 180 / Math.PI;

                    if (angle < 0) angle *= -1;
                    var color = getColor(normalize((float)angle, -90, 90), "angle");
                    for (var y = 0; y < angleRadius; y++)
                    {
                        for (var z = 0; z < angleRadius; z++)
                        {

                            if (colors.Length <= y + idx + z * 100) continue;
                            colors[y + idx + z * 100] = color;
                        }
                    }
                }
            }
            //dont blow up pc PLS
            // nuh uh
            mesh.colors32 = colors;
        }
    }

    private void colorMeshBasedOnAzimuth() {
        float min = 9999f;
        float max = -9999f;
        /*
        Again, if you wanna Debug.Log, set area to something smaller.
        PERFORMANCE NOT DOCUMENTED
        */
        var area = chunkSize * chunkSize;
        for(var child = 0; child < area; child++) { 
            Mesh mesh = children[child].mesh;
            Vector3[] vertices = mesh.vertices; 
            int[] tris = mesh.triangles;
            // create new colors array where the colors will be created.
            Color32[] colors = new Color32[vertices.Length];
            
            for (var i = 0; i < chunkSize; i += azimuthRadius)
            {
                for(var x = 0; x < chunkSize; x += azimuthRadius)
                {
                    
                    int idx = i + x * chunkSize;
                    Vector3 vertice = vertices[idx];
                    PolarCoordinates coords = MoonCalculator.GetCartesianToSphericalCoordinates(new CartesianCoordinates(vertice.x, vertice.y, vertice.z));

                    double azimuth = MoonCalculator.GetAzimuthAngleOneCartesianInput(coords, MoonCalculator.EarthCoordinates);
                    min = Mathf.Min(min, (float)azimuth);
                    max = Mathf.Max(max, (float)azimuth);
                    float normalized = normalize((float)azimuth, min, max);
                    for (var y = 0; y < azimuthRadius;  y++) {
                        for(var z = 0; z < azimuthRadius; z++) {
                            int newIdx = idx + y + z*100;

                            if (newIdx >= colors.Length) continue;
                            colors[newIdx] = getColor(normalized, "height");
                        }
                    }
                }
            }
            mesh.colors32 = colors;
       }
    }
}
