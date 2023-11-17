using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader2 : MonoBehaviour
{
    public Terrain terrain;
    public string csvFilePath = "Assets/your_data.csv"; // Update with your CSV file path
    public float maxHeight = 1000f; // Adjust this value based on your data

    void Start()
    {
        LoadHeightmapFromCSV();
    }

    void LoadHeightmapFromCSV()
    {
        // Load the CSV file
        string[] csvData = System.IO.File.ReadAllLines(csvFilePath);

        int resolution = csvData.Length;
        float[,] heights = new float[resolution, resolution];

        // Find the maximum height value in the CSV data
        float maxCSVHeight = 0f;
        foreach (string line in csvData)
        {
            string[] rowData = line.Split(',');
            foreach (string value in rowData)
            {
                float heightValue;
                if (float.TryParse(value, out heightValue))
                {
                    maxCSVHeight = Mathf.Max(maxCSVHeight, heightValue);
                }
            }
        }

        // Normalize the heights and store them in the heights array
        for (int y = 0; y < resolution; y++)
        {
            string[] rowData = csvData[y].Split(',');
            for (int x = 0; x < resolution; x++)
            {
                float heightValue;
                if (float.TryParse(rowData[x], out heightValue))
                {
                    // Normalize the height value
                    heights[y, x] = heightValue / maxCSVHeight * maxHeight;
                }
                else
                {
                    Debug.LogError("Failed to parse height data at (" + x + ", " + y + ")");
                }
            }
        }

        // Apply the normalized heights to the terrain
        terrain.terrainData.SetHeights(0, 0, heights);
    }
}
