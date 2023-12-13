using UnityEngine;
using TMPro;

public class PathfindingUI : MonoBehaviour
{
    public TextMeshProUGUI coordinateText; // Reference to your TextMeshPro object
    public GameObject imageObject; // Reference to the GameObject with the image

    int imageWidth = 500;
    int imageHeight = 500;

    float originalX = 10000;
    float originalY = 0;

    void Update()
    {
        UpdateCoordinatesOnMouseOver();
    }

    void UpdateCoordinatesOnMouseOver()
    {
        Vector3 mousePos = Input.mousePosition;

            Vector2 relativeCoordinates = GetRelativeCoordinates(mousePos, new Vector3(originalX, originalY, 0));
            // Debug.Log("Relative Coordinates: " + relativeCoordinates);

            // Display coordinates on TextMeshPro object
                coordinateText.text = "from: " + relativeCoordinates.ToString();
            
        
    }

    Vector2 GetRelativeCoordinates(Vector3 worldPoint, Vector3 imagePosition)
    {

        // Adjust for the image position and convert world coordinates to relative coordinates based on the image size

        var WORLD_WIDTH = 3169f;
        worldPoint = imageObject.transform.InverseTransformPoint(worldPoint);

        //its a square 
        Vector2 ratio = new Vector2(WORLD_WIDTH, WORLD_WIDTH) / new Vector2(imageWidth, imageHeight);
        //bottom left is (0,0)
        
        //translate relative to bottom left
        // worldPoint -= imagePosition;
        worldPoint += new Vector3(imageWidth / 2, imageHeight / 2);
        //worldPoint += imageObject.transform.position;
        Debug.Log("WORLD" + worldPoint.ToString());

        //check out of bounds
        

        Vector2 scaled = new Vector2(worldPoint.x, worldPoint.y) * ratio;
        if (scaled.x < 0 || scaled.y < 0 || scaled.x > WORLD_WIDTH || scaled.y > WORLD_WIDTH)
        {
            return new Vector2(Mathf.Infinity, Mathf.Infinity);
        }

        return scaled;
    }
}