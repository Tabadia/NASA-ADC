using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Coordinates : MonoBehaviour
{
    public GameObject p1;
    public GameObject p2;
    public GameObject coordinates;
    public TextMeshProUGUI previewWindowText;

    public Vector3 peakNearShackletonCoords = new Vector3(0, 0, 0); //someone who knows, pls do this

    private Vector3 EARTH_COORDS = new Vector3(361000, 0, -42100);
    TextMeshProUGUI coordText;
    void Start()
    {
        coordText = coordinates.GetComponent<TextMeshProUGUI>();
    }
    float getDistFromEarth(Vector3 pos) {
        return Vector3.Distance(pos, EARTH_COORDS);
    }
    double getAngleFromEarth(float height, float distance) {
        return Mathf.Atan(height / distance) * 180;
    }
    float getAzimuthToEarth(Vector3 pos) {
        return Mathf.Atan((pos.x - EARTH_COORDS.x) / (pos.y - EARTH_COORDS.y)) * 180/Mathf.PI;
    }

    string getActiveAstronaut(int number)
    {
        CameraController cameraController = GameObject.Find("CameraController").GetComponent<CameraController>();

        string statement = "";

        if (cameraController.m_MainCamera.enabled == true && number == 1)
        {
            statement = " (me)";
            previewWindowText.text = "Astronaut 2's View";
        }
        else if (cameraController.m_CameraTwo.enabled == true && number == 2)
        {
            statement = " (me)";
            previewWindowText.text = "Astronaut 1's View";
        }

        Debug.Log("OMGGGGG" + statement);
        return statement;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p1Pos = p1.transform.position;
        p1Pos.y -= peakNearShackletonCoords.y;
        Vector3 p2Pos = p2.transform.position;
        p2Pos.y -= peakNearShackletonCoords.y;

        float p1Dist = getDistFromEarth(p1Pos);
        
        string astronaut1Data = "Astronaut 1" + getActiveAstronaut(1)
        + "\n\n" 
        + "Position: " + p1Pos.ToString() + "\n\n"
        + "Distance to Earth: " + p1Dist + "\n\n" 
        + "Elevation angle to Earth: " + getAngleFromEarth(p1Pos.y, p1Dist) + "\n\n" 
        + "Azimuth angle: " + getAzimuthToEarth(p1Pos);

        float p2Dist = getDistFromEarth(p2Pos);
        
        string astronaut2Data = "Astronaut 2" + getActiveAstronaut(2) 
        + "\n\n"
        + "Position: " + p2Pos.ToString() + "\n\n"
        + "Distance to Earth: " + p2Dist + "\n\n" 
        + "Elevation angle to Earth: " + getAngleFromEarth(p2Pos.y, p2Dist) + "\n\n" 
        + "Azimuth angle: " + getAzimuthToEarth(p2Pos);
        
        coordText.text = astronaut1Data + "\n\n\n\n"
        + astronaut2Data;
    }
}
