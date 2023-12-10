using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
class Pathfinding : MonoBehaviour
{
    public string heightFilePath = "Assets/MeshGeneration/FY23_ADC_Height_PeakNearShackleton.csv";
    public string slopeFilePath = "Assets/MeshGeneration/FY23_ADC_Slope_PeakNearShackleton.csv";

    public string latitudeFilePath = "Assets/MeshGeneration/FY23_ADC_Latitude_PeakNearShackleton.csv";

    public string longtitudeFilePath = "Assets/MeshGeneration/FY23_ADC_Longitude_PeakNearShackleton.csv";
    public Gradient lineGradient;
    MoonMapper moonMapper;
    private GameObject player;


    double[,] heightMap;
    void Start()
    {
        StartCoroutine("dontCrashPls");
        moonMapper = new MoonMapper(heightFilePath, slopeFilePath, latitudeFilePath, longtitudeFilePath);
        heightMap = moonMapper.heightMap; //null??
        Debug.Log(heightMap);
        StopAllCoroutines();
    }
    async void dontCrashPls()
    {
        //100 seconds
        await Task.Delay(100000);
        //if Start() has been running for more than 100 seconds ABORT
        Application.Quit();
    }
    async void Update()
    {
        if (Camera.main.name == "playerCam")
        {
            player = GameObject.Find("Player");
        }
        else player = GameObject.Find("Player2");
        GridCoordinates playerPos = new GridCoordinates((int)player.transform.position.x, (int)player.transform.position.y);
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


        //List<int> path = moonMapper.FindPath(new GridCoordinates(playerPos.xCoord, (int)playerPos.yCoord), endPos, 0)

        List<int> path = new List<int>
        {
            1, 2, 5, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 5, 5, 5, 5, 7, 0, 0, 0, 0, 0, 0, 0, 0
        };
        Vector3[] lineVertexes = new Vector3[path.Count];
        int idx = 0;

        foreach (int dir in path)
        {
            Vector2 direction = dirs[dir];
            playerPos.xCoord += (int)direction.x;
            playerPos.yCoord += (int)direction.y;
            //find the height value from the height CSV.
            //Debug.Log(playerPos.xCoord);
            //Debug.Log(heightMap);
            //double newY = heightMap[playerPos.xCoord, playerPos.yCoord] + 1;
            lineVertexes[idx] = new Vector3(playerPos.xCoord + direction.x, 0, playerPos.yCoord + direction.y);
            idx++;
        }

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("GET A LIFE. And a line renderer component.");
            return;
        }
        lineRenderer.positionCount = path.Count;
        lineRenderer.colorGradient = lineGradient;
        lineRenderer.SetPositions(lineVertexes);
    }


}