using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathfindingUI : MonoBehaviour
{
    public TextMeshProUGUI coordinateText; // Reference to your TextMeshPro object
    public Transform groundPlane; // Reference to the ground plane (where the camera is looking down)

    void Update()
    {
        UpdateCoordinatesOnMouseClick();
        UpdateCoordinatesOnMouseMovement();
    }

    void UpdateCoordinatesOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            Vector3 cartesianCoordinates = GetCartesianCoordinates();
            Debug.Log("Cartesian Coordinates: " + cartesianCoordinates);

            // Display coordinates on TextMeshPro object
            if (coordinateText != null)
            {
                coordinateText.text = "Coordinates: " + cartesianCoordinates.ToString();
            }
        }
    }

    void UpdateCoordinatesOnMouseMovement()
    {
        Vector3 cartesianCoordinates = GetCartesianCoordinates();

        // Display coordinates on TextMeshPro object during mouse movement
        if (coordinateText != null)
        {
            coordinateText.text = "Coordinates: " + cartesianCoordinates.ToString();
        }
    }

    Vector3 GetCartesianCoordinates()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Use the ground plane instead of a texture
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}