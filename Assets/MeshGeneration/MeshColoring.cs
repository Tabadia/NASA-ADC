using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static Unity.VisualScripting.Member;
using System.Threading.Tasks;

public class MeshColoring : MonoBehaviour
{
    public Gradient coloringGradient;
    GameObject meshGameObject;
    MeshFilter[] children;
    public int angleRadius = 3;

    public int azimuthRadius = 3;
    public int shadingRadius;

    public Vector3 EARTH_LOCATION = new Vector3(361000, 0, -42100);
    public int chunkSize = 100;
    // Start is called before the first frame update
    async void Start()
    {
        meshGameObject = GameObject.Find("Mesh");
        await Task.Delay(10000);
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
        } else if(mode == "shade") 
        {
            shadeMesh();
        }
    }
    float normalize(float num, float min, float max)
    {
        return (num - min) / (max - min);
    }

    public Color getColor(float num) {
        return coloringGradient.Evaluate(num);
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

                colors[i] = getColor(normalize(point.y, low, high));
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
                    int idx = i + x * 100;
                    Vector3[] coords = getCoords(idx, tris, vertices, azimuthRadius);
                    int max = FindMax(coords);
                    int min = FindMin(coords);
                    float distance;
                    double angle;
                    if (min == max)
                    {
                        angle = 0;
                    }
                    distance = Vector3.Distance(coords[min], coords[max]);
                    angle = Math.Atan((coords[max].y - coords[min].y) / distance) * 180/Math.PI;
                    
                    if (angle < 0) angle *= -1;
                    var color = getColor(normalize((float)angle, -90, 90));
                    for (var y = 0; y < azimuthRadius; y++)
                    {
                        for (var z = 0; z < azimuthRadius; z++)
                        {

                            if (colors.Length <= y + idx + z * 100) continue;
                            colors[y + idx + z * 100] = color;
                        }
                    }
                }
            }
            //dont blow up pc PLS
            mesh.colors32 = colors;
       }
    }
    private void colorMeshBasedOnAzimuth() {
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
                    int idx = i + x*chunkSize;

                    float xVal = -999999; 
                    float yVal = -999999;

                    for(var y = 0; y < azimuthRadius;  y++) {
                        for(var z = 0; z < azimuthRadius; z++) {
                            int newIdx = idx + y + z*100;
                            if (newIdx >= colors.Length) continue;

                            Vector3 vertice = vertices[newIdx];
                            xVal = Math.Max(vertice.x, xVal);
                            yVal = Math.Max(vertice.y, yVal);
                        }
                    }

                    double azimuth = Math.Atan((xVal - EARTH_LOCATION.x) / (yVal - EARTH_LOCATION.y)) * 180/Math.PI;

                    var normalized = normalize((float)azimuth, -90, -89.9f);
                    for (var y = 0; y < azimuthRadius;  y++) {
                        for(var z = 0; z < azimuthRadius; z++) {
                            int newIdx = idx + y + z*100;

                            if (newIdx >= colors.Length) continue;
                            colors[newIdx] = getColor(normalized);
                        }
                    }
                }
            }
            mesh.colors32 = colors;
       }
    }
    private void shadeMesh() {
        /*
            This function is very similar to coloring based on angle. It colors vertices based on black and white, making it look like there is light shining on it. 
            Performance is very similar to ColorMeshBasedOnAngle().
        */
        foreach(MeshFilter meshFilter in children) {
            MeshColoring mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices; 
            int[] tris = mesh.triangles;
            // create new colors array where the colors will be created.
            Color32[] colors = new Color32[vertices.Length];
            for (var i = 0; i < chunkSize; i += shadingRadius)
            {
                for(var x = 0; x < chunkSize; x += shadingRadius)
                {
                    int idx = i + x * 100;
                    Vector3[] coords = getCoords(idx, tris, vertices, shadingRadius);
                    int max = FindMax(coords);
                    int min = FindMin(coords);
                    float distance;
                    double angle;
                    if (min == max)
                    {
                        angle = 0;
                    }
                    distance = Vector3.Distance(coords[min], coords[max]);
                    angle = Math.Atan((coords[max].y - coords[min].y) / distance) * 180/Math.PI;

                    var color = Color.Lerp(Color.black, Color.white, (float)normalize(angle, -90, 90));
                    for (var y = 0; y < shadingRadius; y++)
                    {
                        for (var z = 0; z < shadingRadius; z++)
                        {

                            if (colors.Length <= y + idx + z * 100) continue;
                            colors[y + idx + z * 100] = color;
                        }
                    }
                }
            }
            //dont blow up pc PLS
            mesh.colors32 = colors;
        }
    }
    public void backToHomeScene()
    {
        SceneManager.LoadScene(sceneName: "Home Screen");
    }
}
