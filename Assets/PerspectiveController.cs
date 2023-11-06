using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveController : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera topDownCamera;
    void Start()
    {
        firstPersonCamera.enabled = true;
        topDownCamera.enabled = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            firstPersonCamera.enabled = !firstPersonCamera;
            topDownCamera.enabled = !topDownCamera.enabled;
        }
    }
}
