using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MinimapController : MonoBehaviour
{   
    public Camera cam1;
    public Camera cam2;
    void Start()
    {
        cam1.enabled = false;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            cam1.enabled = !cam1.enabled;
            cam2.enabled = !cam2.enabled;

            Debug.Log(cam1.enabled + "," + cam2.enabled + !cam1.enabled + !cam2.enabled);
        }
    }
}