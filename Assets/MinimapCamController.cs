using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamController : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = cam.transform.rotation;   
    }
}
