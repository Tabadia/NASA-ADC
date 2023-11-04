using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCam : MonoBehaviour

{
    public float sensY;
    public float sensX;

    public Transform orientation;
    public Transform player;

    float xRotation;
    float yRotation;
    Boolean camIsOnScene = false;
    void Start()
    {
        //make it so that mouse cant kleave the screen as well as making it invisible
        //this makes it so that the camera can rotate
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        
        

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -70f, 70f);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 30f);

        //rotate camera orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        Vector3 pos = player.position;
        pos.Set(pos.x, pos.y + 1, pos.z);
        transform.position = pos;
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
