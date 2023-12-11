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


    float heightChangeMult;
    double[,] heightMap;
    void Start()
    {

        moonMapper = new MoonMapper(heightFilePath, slopeFilePath, latitudeFilePath, longtitudeFilePath);
        heightMap = moonMapper.heightMap;

        heightChangeMult = GameObject.Find("Mesh").GetComponent<MeshGen2>().heightMultiplier;
        PathFind();

    }
    void PathFind()
    {
        if (Camera.main.name == "PlayerCam")
        {
            player = GameObject.Find("PlayerObj");
        }
        else player = GameObject.Find("PlayerObj2");
        GridCoordinates playerPos = new GridCoordinates((int)player.transform.position.x, (int)player.transform.position.y);

        GridCoordinates endPos = new GridCoordinates(100, 100);
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
        GridCoordinates[] dirs = moonMapper.GenerateAllEightDirections();


        List<int> path = moonMapper.FindPath(playerPos, endPos, 0);

        //List<int> path = new List<int>
        //{
        //    1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2
        //};
        Vector3[] lineVertexes = new Vector3[path.Count];
        int idx = 0;

        foreach (int dir in path)
        {
            GridCoordinates direction = dirs[dir];
            playerPos.xCoord += direction.xCoord;
            playerPos.yCoord += direction.yCoord;
            //find the height value from the height CSV.
            double newY = heightMap[(int)(playerPos.xCoord * 10.24), (int)(playerPos.yCoord*10.24)] 
                   * heightChangeMult + 2;
            lineVertexes[idx] = new Vector3(playerPos.xCoord, (float)newY, playerPos.yCoord);
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