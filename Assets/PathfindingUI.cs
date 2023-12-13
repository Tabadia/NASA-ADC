using UnityEngine;
using TMPro;

public class PathfindingUI : MonoBehaviour
{
    public TextMeshProUGUI coordinateText; // Reference to your TextMeshPro object
    public GameObject imageObject; // Reference to the GameObject with the image

    int imageWidth = 500;
    int imageHeight = 500;

    float originalX = 250;
    float originalY = 0;

    void Update()
    {
        UpdateCoordinatesOnMouseOver();
    }

    void UpdateCoordinatesOnMouseOver()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform raycast without layer mask
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector2 relativeCoordinates = GetRelativeCoordinates(hit.point, new Vector3(originalX, originalY, 0));
            // Debug.Log("Relative Coordinates: " + relativeCoordinates);

            // Display coordinates on TextMeshPro object
            if (coordinateText != null)
            {
                coordinateText.text = "from: " + relativeCoordinates.ToString();
            }
        }
    }

    Vector2 GetRelativeCoordinates(Vector3 worldPoint, Vector3 imagePosition)
    {
        var WORLD_WIDTH = Mathf.Sqrt(1024) * 100;
        worldPoint = imageObject.transform.InverseTransformPoint(worldPoint);
        //its a square
        Vector2 ratio = new Vector2(WORLD_WIDTH, WORLD_WIDTH) / new Vector2(imageWidth, imageHeight);
        Debug.Log(ratio);
        //bottom left is (0,0)

        //translate relative to bottom left
        // worldPoint -= imagePosition;
        worldPoint -= new Vector3(imageWidth / 2, imageHeight / 2);
        worldPoint = worldPoint * -1;

        Debug.Log("WORLD" + worldPoint.ToString());

        //check out of bounds
        if (worldPoint.x < 0 || worldPoint.y < 0)
        {
            return new Vector2(Mathf.Infinity, Mathf.Infinity);
        }

        Vector2 scaled = new Vector2(worldPoint.x, worldPoint.y) * ratio;
        return scaled;
    }
}