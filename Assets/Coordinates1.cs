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


    TextMeshProUGUI coordText;
    void Start()
    {
        coordText = coordinates.GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        coordText.text = "Astronaut 1 Position:" + p1.transform.position.ToString() + '\n' + "Astronaut 2 Position:"+ p2.transform.position.ToString(); 
    }
}
