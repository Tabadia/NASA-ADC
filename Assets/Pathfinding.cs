using System.Collections.Generic;
using UnityEngine;
class Pathfinding : MonoBehaviour
{
    public string heightFilePath;
    public string slopeFilePath;

    public string latitudeFilePath;

    public string longtitudeFilePath;
    public Gradient lineGradient;
    MoonMapper moonMapper;
    public GameObject player;

    MeshGen2 meshGenerator;

    string[] fileData;

    float[,] heightMap;
    void Start()
    {
        moonMapper = new MoonMapper(heightFilePath, slopeFilePath, latitudeFilePath, longtitudeFilePath);
        meshGenerator = GameObject.Find("Mesh").GetComponent<MeshGen2>();
        fileData = meshGenerator.fileData;
        int width = fileData[0].Split(',').Length;
        int height = fileData.Length;

        heightMap = new float[width, height];

        for (var i = 0; i < height; i++)
        {
            var line = fileData[i].Split(',');
            for (var j = 0; j < line.Length; j++)
            {
                heightMap[i, j] = float.Parse(line[j], System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
    void Update()
    {

        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        GridCoordinates endPos = new GridCoordinates(50, 50);
        /*Index Mapping Notes
            0 = Bottom Left
            1 = Bottom Middle
            2 = Bottom Right
            3 = Middle Left
            4 = Middle Right
            5 = Top Left
            6 = Top Middle
            7 = Top Right
        */
        Vector2[] dirs = new Vector2[8];
        dirs[0] = new Vector2(-1, -1);
        dirs[1] = new Vector2(0, -1);
        dirs[2] = new Vector2(1, -1);
        dirs[3] = new Vector2(-1, 0);
        dirs[4] = new Vector2(1, 0);
        dirs[5] = new Vector2(-1, 1);
        dirs[6] = new Vector2(1, 0);
        dirs[7] = new Vector2(1, 1);


        List<int> path = moonMapper.FindPath(new GridCoordinates((int)playerPos.x, (int)playerPos.y), endPos, 0);

        Vector3[] lineVertexes = new Vector3[path.Count];
        int idx = 0;

        foreach (int dir in path)
        {
            Vector2 direction = dirs[dir];
            playerPos.x += direction.x;
            playerPos.y += direction.y;
            //find the height value from the height CSV.
            float newY = heightMap[(int)playerPos.x, (int)playerPos.y] + 1;

            lineVertexes[idx] = new Vector3(direction.x, newY, direction.y);
            idx++;
        }
        GameObject line = new GameObject();
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPositions(lineVertexes);
    }


}