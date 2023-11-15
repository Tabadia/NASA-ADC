using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))] //ensure object has mesh components

public class MeshGenerator : MonoBehaviour
{
	Mesh mesh;
    List<float> heightList;
	Vector3[] vertices;
	int[] triangles;

    // set the size of the mesh base
	public int xSize = 100;
	public int zSize = 100;
    
	public float strength = 0.3f;

	void Start() {
        // Setup the mesh
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;

        ReadCSV();
		//CreateShape();
		// UpdateMesh();
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
						float y = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
						print(i + " " + x + " " + z);
						vertices[i] = new Vector3(x, y, z);
						print(vertices[i]);
						x++;
						i++;
		            }
					z++;
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
}

