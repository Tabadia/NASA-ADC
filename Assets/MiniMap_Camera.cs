using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap_Camera : MonoBehaviour
{
    public GameObject mainCam;

    void Update()
    {
        transform.position = mainCam.transform.position;
        transform.rotation = mainCam.transform.rotation;
    }
}
