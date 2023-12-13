using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PathfindingUI : MonoBehaviour
{
    public TextMeshProUGUI coordinateText; // Reference to your TextMeshPro object
    public TextMeshProUGUI coordinateText2; // Reference to your TextMeshPro object
    public GameObject imageObject; // Reference to the GameObject with the image

    public GameObject startCircle;
    public GameObject endCircle;

    int imageWidth = 500;
    int imageHeight = 500;

    float originalX = 10000;
    float originalY = 0;

    public Vector2 start = new Vector2(0, 0);
    public Vector2 destination = new Vector2(0, 0);
    public int optimization = 1;

    Vector2 coords;

    
    void Update()
    {
        UpdateCoordinatesOnMouseOver();

        if (Input.GetMouseButtonDown(0))
        {
            startCircle.SetActive(true);
            startCircle.transform.position = Input.mousePosition;

            start = new Vector2(coords.x, coords.y);
        }

        if (Input.GetMouseButtonDown(1))
        {
            endCircle.SetActive(true);
            endCircle.transform.position = Input.mousePosition;

            destination = new Vector2(coords.x, coords.y);
        } 
    }

    //public void OnPointerClick(PointerEventData pointerEventData)
    //{
    //      Debug.LogError("CLICKED "); 
    //}

    void UpdateCoordinatesOnMouseOver()
    {
        Vector3 mousePos = Input.mousePosition;

        coords = GetRelativeCoordinates(mousePos, new Vector3(originalX, originalY, 0));

            // Display coordinates on TextMeshPro object
        coordinateText.text = "from: " + start.ToString();
        coordinateText2.text = "to: " + destination.ToString();


    }

    Vector2 GetRelativeCoordinates(Vector3 worldPoint, Vector3 imagePosition)
    {

        // Adjust for the image position and convert world coordinates to relative coordinates based on the image size

        var WORLD_WIDTH = 3169f;
        worldPoint = imageObject.transform.InverseTransformPoint(worldPoint);

        //its a square 
        Vector2 ratio = new Vector2(WORLD_WIDTH, WORLD_WIDTH) / new Vector2(imageWidth, imageHeight);
        //bottom left is (0,0)
        
        worldPoint += new Vector3(imageWidth / 2, imageHeight / 2);

        Vector2 scaled = new Vector2(worldPoint.x, worldPoint.y) * ratio;
        if (scaled.x < 0 || scaled.y < 0 || scaled.x > WORLD_WIDTH || scaled.y > WORLD_WIDTH)
        {
            return new Vector2(Mathf.Infinity, Mathf.Infinity);
        }

        return scaled;
    }
    
}