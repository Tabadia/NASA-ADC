using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))] //ensure object has mesh components

public class MeshGenerator : MonoBehaviour
{
	Mesh mesh;
    List<float> heightList;
	Vector3[] vertices;
	int[] triangles;

    // set the size of the mesh base
	private int xSize = 100;
	private int zSize = 100;
    
	public float strength = 0.3f;

	void Start() {
        // Setup the mesh
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;

        //ReadCSV();
		CreateShape();
		UpdateMesh();
		ColorMeshBasedOnAngle();

    }

	void ReadCSV(){
		print("starting to read");
		vertices = new Vector3[(xSize + 1) * (zSize + 1)];
		string fileName = @".\Assets\MeshGeneration\test.csv";
        //try {
            // Create a StreamReader
            using (StreamReader reader = new StreamReader(fileName))
            {
				int i = 0;
				int x = 0;
				int z = 0;
                string line;
                // Read line by line
                while ((line = reader.ReadLine()) != null){
					var values = line.Split(',');
		            foreach (var value in values){
						float y = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat) - 532f;
						print(i + " " + x + " " + z);
						vertices[i] = new Vector3(x, y, z);
						print(vertices[i]);
						x++;
						i++;
		            }
					z++;
					x = 0;
                }
            }
        //}
        //catch (Exception exp){
        //    print(exp.Message);
        //}
		print("done reading");
		triangles = new int[xSize * zSize * 6];

		int vert = 0;
		int tris = 0;

		for (int z = 0; z < zSize; z++) {
			for (int x = 0; x < xSize; x++) {
				triangles[tris + 0] = vert + 0;
				triangles[tris + 1] = vert + xSize + 1;
				triangles[tris + 2] = vert + 1;
				triangles[tris + 3] = vert + 1;
				triangles[tris + 4] = vert + xSize + 1;
				triangles[tris + 5] = vert + xSize + 2;

				vert++;
				tris += 6;
			}
			vert++;
		}

		UpdateMesh();
        // heightList = new List<float>();
        // for (int i = 1; i <= 1; i++){
        //     using(var reader = new StreamReader(@".\Assets\MeshGeneration\HeightMapPart" + i + ".csv")){
        //         while (!reader.EndOfStream){
        //             var line = reader.ReadLine();
        //             var values = line.Split(',');
        //             foreach (var value in values){
        //                 heightList.Add(float.Parse(value));
        //             }
        //         }
        //     }
        // }
        // foreach(float l in heightList){
        //     Debug.Log(l);
        // }
    }

    void CreateShape() {
		print(xSize);
		print(zSize);
		print((xSize + 1) * (zSize + 1));
		vertices = new Vector3[(xSize + 1) * (zSize + 1)];

		for (int i = 0, z = 0; z <= zSize; z++){
			for (int x = 0; x <= xSize; x++){
				float y = Mathf.PerlinNoise(x * strength, z * strength) * 2f;
				vertices[i] = new Vector3(x, y, z);
				i++;
			}
		}
		triangles = new int[xSize * zSize * 6];

		int vert = 0;
		int tris = 0;

		for (int z = 0; z < zSize; z++)
		{
			for (int x = 0; x < xSize; x++)
			{
				triangles[tris + 0] = vert + 0;
				triangles[tris + 1] = vert + xSize + 1;
				triangles[tris + 2] = vert + 1;
				triangles[tris + 3] = vert + 1;
				triangles[tris + 4] = vert + xSize + 1;
				triangles[tris + 5] = vert + xSize + 2;

				vert++;
				tris += 6;
			}
			vert++;
		}
	}

	void UpdateMesh() {
        mesh.Clear();

		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.RecalculateNormals();
	}
	float normalize(float num, float min, float max)
	{
		return (num - min) / (max - min);
	}
    void ColorMeshBasedOnHeight()
    { 
        Vector3[] vertices = mesh.vertices;

        // create new colors array where the colors will be created.
        Color[] colors = new Color[vertices.Length];
		int i = 0;
		float high = 0f;
		float low = 0f;	
		foreach(Vector3 point in vertices)
		{
			if (point.y > high) high = point.y;
			if (point.y < low) low = point.y;

            colors[i] = Color.Lerp(Color.white, Color.black, normalize(point.y, low, high));
			i++;
		}
        mesh.colors = colors;
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
	void ColorMeshBasedOnAngle()
	{
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
			Debug.Log("Distance:" + distance);
			Debug.Log("");
			Debug.Log(angle);
			var color = Color.Lerp(Color.black, Color.white, normalize((float)angle, -90, 90)) ;
			colors[idx1] = color;
			colors[idx2] = color;
			colors[idx3] = color;
		}
		
        mesh.colors = colors;
    }
}

