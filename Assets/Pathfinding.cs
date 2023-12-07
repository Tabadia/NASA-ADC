using UnityEngine;
class Pathfinding : MonoBehaviour {
    public string heightFilePath;
    public string slopeFilePath;

    public string latitudeFilePath;

    public string longitudeFilePath;
    public Gradient lineGradient;
    MoonMapper moonMapper = new MoonMapper(heightFilePath, slopeFilePath, latitudeFilePath, longtitudeFilePath);
    public GameObject player;

    MeshGen2 meshGenerator;

    string[] fileData; 

    float[,] heightMap;
    void Start() {
        meshGenerator = GameObject.Find("Mesh").GetComponent<MeshGen2>();
        fileData = meshGenerator.fileData;
        int width = fileData[0].Split(',').Length;
        int height = fileData.Length;
        
        heightMap = new float[width, height];
        
        for(var i = 0; i < height; i++) {
            var line = fileData[i].Split(',');
            for(var j = 0; j < line.Length; j++) {
                line[j] = float.Parse(line[j], System.Globalization.CultureInfo.InvariantCulture);
            }
            heightMap[i] = line;
        }
    }
    void Update() {
        
        GridCoordinates playerPos = new GridCoordinates(player.x, player.y);
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
        Vector2[] dirs = new Vector2 { 
            new Vector2(-1, -1),
            new Vector2(0, -1),
            new Vector2(1, -1),
            new Vector2(-1, 0),
            new Vector2(1, 0),
            new Vector2(-1, 1),
            new Vector2(1, 0),
            new Vector2(1, 1)
        };

        List<int> path = moonMapper.findPath(playerPos, endPos);

        Vector3[] lineVertexes = new Vector3[path.Count];
        int idx = 0;

        foreach(int dir in path) {
            Vector2 direction = dirs[dir];
            playerPos.x += direction.x;
            playerPos.z += direction.y;
            //find the height value from the height CSV.
            float newY = heightMap[playerPos.x, playerPos.z] + 1;

            lineVertexes[idx] = new Vector3(direction.x, newY, direction.y);
            idx++;
        }
        GameObject line = new GameObject();
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPositions(lineVertexes);
    }
    

}