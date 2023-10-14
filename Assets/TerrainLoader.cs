using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLoader : MonoBehaviour
{
    public Terrain terrain;
    public string csvFilePath = "Assets/your_data.csv"; // Update with your CSV file path
    public int tileResolution = 513; // The resolution of individual terrain tiles

    void Start()
    {
        LoadHeightmapFromCSV();
    }

    void LoadHeightmapFromCSV()
    {
        // Load the CSV file
        string[] csvData = System.IO.File.ReadAllLines(csvFilePath);

        int csvResolution = csvData.Length;

        List<List<double>> trimmedData = new List<List<double>>();

        double minHeight = double.MaxValue;
        double maxHeight = double.MinValue;

        for (int csvY = 0; csvY < csvResolution; csvY++)
        {
            string[] rowData = csvData[csvY].Split(',');

            // Check if the row has non-empty values
            if (Array.Exists(rowData, x => !string.IsNullOrEmpty(x.Trim())))
            {
                List<double> trimmedRow = new List<double>();

                for (int csvX = 0; csvX < rowData.Length; csvX++)
                {
                    if (double.TryParse(rowData[csvX], out double heightValue))
                    {
                        trimmedRow.Add(heightValue);

                        // Update min and max height values
                        if (heightValue < minHeight)
                            minHeight = heightValue;
                        if (heightValue > maxHeight)
                            maxHeight = heightValue;
                    }
                }

                // Check if the row has non-empty values
                if (trimmedRow.Count > 0)
                {
                    trimmedData.Add(trimmedRow);
                }
            }
        }

        int newCsvResolution = trimmedData.Count;

        // Resize the terrain to match the new resolution
        terrain.terrainData = new TerrainData();
        terrain.terrainData.heightmapResolution = newCsvResolution;

        // Calculate the number of tiles needed for the entire terrain
        int tileCount = (int)Math.Ceiling((double)newCsvResolution / tileResolution);

        terrain.terrainData.size = new Vector3(tileCount * terrain.terrainData.size.x, terrain.terrainData.size.y, tileCount * terrain.terrainData.size.z);

        for (int tileY = 0; tileY < tileCount; tileY++)
        {
            for (int tileX = 0; tileX < tileCount; tileX++)
            {
                float[,] heights = new float[tileResolution, tileResolution];

                for (int y = 0; y < tileResolution; y++)
                {
                    for (int x = 0; x < tileResolution; x++)
                    {
                        int csvX = tileX * tileResolution + x;
                        int csvY = tileY * tileResolution + y;

                        if (csvX < newCsvResolution && csvY < newCsvResolution)
                        {
                            // Normalize the height value to the 0 to 1 range with double precision
                            double normalizedHeight = NormalizeHeight(trimmedData[csvY][csvX], minHeight, maxHeight);
                            heights[y, x] = (float)normalizedHeight; // Cast to float
                        }
                    }
                }

                // Apply the heights to the terrain tile
                terrain.terrainData.SetHeights(tileX * tileResolution, tileY * tileResolution, heights);
            }
        }
    }

    // Normalize a height value to the 0 to 1 range with double precision
    double NormalizeHeight(double heightValue, double min, double max)
    {
        if (min == max) return 0.0; // Avoid division by zero

        return (heightValue - min) / (max - min);
    }
}
