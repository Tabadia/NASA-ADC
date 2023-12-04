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

    public Vector3 peakNearShackletonCoords = new Vector3(0, 0, 0); //someone who knows, pls do this

    TextMeshProUGUI coordText;
    void Start()
    {
        coordText = coordinates.GetComponent<TextMeshProUGUI>();
    }
    float getDistFromEarth(Vector3 pos) {
        return Vector3.distance(p1Pos, new Vector3(361000, 0, -42100));
    }
    double getAngleFromEarth(float height, float distance) {
        return Math.Atan(height / distance) * 180;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 p1Pos = new Vector3(p1.position);
        p1Pos.y -= peakNearShackletonCoords.y;
        Vector3 p2Pos = new Vector3(p2.position);
        p2Pos.y -= peakNearShackletonCoords.y;

        // Waiting for Alex's util file.
        float azimuthAngle;

        float p1Dist = getDistFromEarth(p1Pos);
        
        string astronaut1Data = "Astronaut 1 Position:" + p1Pos.ToString() + "\n"
        + "Distance to Earth: " + p1Dist + "\n" 
        + "Elevation angle to Earth: " + getAngleFromEarth(p1Pos.y, p1Dist) + "\n" 
        + "Azimuth angle: " + "NOT IMPLEMENTED YET";

        float p2Dist = getDistFromEarth(p2Pos);
        
        string astronaut2Data = "Astronaut 2 Position:" + p2Pos.ToString() + "\n"
        + "Distance to Earth: " + p2Dist + "\n" 
        + "Elevation angle to Earth: " + getAngleFromEarth(p2Pos.y, p2Dist) + "\n" 
        + "Azimuth angle: " + "NOT IMPLEMENTED YET";
        
        coordText.text = astronaut1Data + "\n"
        + astronaut2Data;
    }
}
