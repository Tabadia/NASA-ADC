using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class MeshGen2 : MonoBehaviour
{
    public string csvFilePath; // Path to the CSV file
    public int chunkSize = 100; // Size of each chunk
    public float scale = 1f; // Scaling factor for the mesh
    public float heightMultiplier = 1f; // Multiplier for height values
    public Material chunkMaterial; // Material to apply to chunks

    void Start()
    {
        GenerateMeshFromCSV();
    }

    void GenerateMeshFromCSV()
    {
        if (!File.Exists(csvFilePath))
        {
            Debug.LogError("CSV file not found.");
            return;
        }

        string[] fileData = File.ReadAllLines(csvFilePath);

        int width = fileData[0].Split(',').Length;
        int height = fileData.Length;

        Material sharedMaterial = Instantiate(chunkMaterial); // Instantiate the shared material

        for (int y = 0; y < height; y += chunkSize)
        {
            for (int x = 0; x < width; x += chunkSize)
            {
                List<Vector3> vertices = new List<Vector3>();
                List<int> triangles = new List<int>();

                for (int yChunk = y; yChunk < Mathf.Min(y + chunkSize, height); yChunk++)
                {
                    string[] row = fileData[yChunk].Split(',');

                    for (int xChunk = x; xChunk < Mathf.Min(x + chunkSize, width); xChunk++)
                    {
                        float.TryParse(row[xChunk], out float heightValue);

                        vertices.Add(new Vector3(xChunk * scale, heightValue * heightMultiplier, yChunk * scale));
                    }
                }

                // Generate triangles
                for (int yChunk = 0; yChunk < chunkSize - 1; yChunk++)
                {
                    for (int xChunk = 0; xChunk < chunkSize - 1; xChunk++)
                    {
                        int topLeft = xChunk + yChunk * chunkSize;
                        int topRight = (xChunk + 1) + yChunk * chunkSize;
                        int bottomLeft = xChunk + (yChunk + 1) * chunkSize;
                        int bottomRight = (xChunk + 1) + (yChunk + 1) * chunkSize;

                        triangles.Add(topLeft);
                        triangles.Add(bottomLeft);
                        triangles.Add(topRight);

                        triangles.Add(topRight);
                        triangles.Add(bottomLeft);
                        triangles.Add(bottomRight);
                    }
                }

                Mesh mesh = new Mesh();
                mesh.vertices = vertices.ToArray();
                mesh.triangles = triangles.ToArray();
                mesh.RecalculateNormals();

                GameObject chunkObject = new GameObject("Chunk_" + x + "_" + y);
                chunkObject.transform.parent = transform;

                MeshFilter meshFilter = chunkObject.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = chunkObject.AddComponent<MeshRenderer>();

                meshFilter.mesh = mesh;
                meshRenderer.material = sharedMaterial; // Apply the shared material
            }
        }
    }
}
