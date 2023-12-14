using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

    public Vector2 playerPos1 = new Vector2(5, 5);
    public Vector2 endPos1 = new Vector2(10, 10);
    public int optimization = 0;

    double[,] heightMap;

    async void Start()
    {


        moonMapper = new MoonMapper(heightFilePath, slopeFilePath, latitudeFilePath, longtitudeFilePath);
        heightMap = moonMapper.heightMap;



        //heightChangeMult = GameObject.Find("Mesh").GetComponent<MeshGen2>().heightMultiplier;

    }
    Vector3 rotate_point(float cx, float cy, float angle, Vector3 p)
    {
        float s = Mathf.Sin(angle);
        float c = Mathf.Cos(angle);

        // translate point back to origin:
        p.x -= cx;
        p.y -= cy;

        // rotate point
        float xnew = p.x * c - p.y * s;
        float ynew = p.x * s + p.y * c;

        // translate point back:
        p.x = xnew + cx;
        p.y = ynew + cy;
        return p;
    }
    Vector3 FindMeshVertice(MeshFilter mesh, int x, int y)
    {
        //we want to round down.
        int xIdx = x % 100;
        int yIdx = y % 100;
        return mesh.mesh.vertices[xIdx + yIdx * 100];
    }
    public void PathFind()
    {
        GridCoordinates playerPos = new GridCoordinates((int)playerPos1.x, (int)playerPos1.y);
        GridCoordinates endPos = new GridCoordinates((int)endPos1.x, (int)endPos1.y);
        //GridCoordinates playerPos = new GridCoordinates((int)player.transform.position.x, (int)player.transform.position.y);

        //GridCoordinates endPos = new GridCoordinates(100, 100);
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
        GridCoordinates[] dirs = MoonMapper.GenerateAllEightDirections();


        List<int> path = moonMapper.FindPath(playerPos, endPos, optimization);

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
            //find the height value from the height CSV
            RaycastHit hit;

            
            if (!Physics.Raycast(new Vector3(playerPos.xCoord, 1000, playerPos.yCoord), Vector3.down, out hit, Mathf.Infinity))
            {
                
                continue;
            }
            MeshFilter mesh = hit.transform.gameObject.GetComponent<MeshFilter>();
            
            var newY = FindMeshVertice(mesh, playerPos.xCoord, playerPos.yCoord).y + 2;
            lineVertexes[idx] = new Vector3(playerPos.xCoord, (float)newY, playerPos.yCoord);
            idx++;
        }

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("GET A LIFE. And a line renderer component.");
            return;
        }
        //Vector3 midPoint = (lineVertexes[0] + lineVertexes[lineVertexes.Length-1]) / 2;

        lineRenderer.positionCount = path.Count;
        lineRenderer.colorGradient = lineGradient;
        lineRenderer.SetPositions(lineVertexes);
    }


}

