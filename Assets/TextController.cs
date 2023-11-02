using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform p1;
    public Transform p2;

    public Text coordinates;
    // Update is called once per frame
    void Update()
    {
        coordinates.text = "Position p1:" + p1.position + "Position p2:" + p2.position;
        Debug.Log(p1.position);
        Debug.Log(p2.position);
    }
}
