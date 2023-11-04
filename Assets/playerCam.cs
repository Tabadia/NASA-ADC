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

    void Start()
    {
        //make it so that mouse cant leave the screen as well as making it invisible
        //this makes it so that the camera can rotate
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        Vector3 pos = player.position;
        pos.Set(pos.x, pos.y + 1, pos.z);
        transform.position = pos;
        //camera not enabled or is a minimap camera
        if (!GetComponent<Camera>().enabled || name.IndexOf("MinimapCam") != -1) return;
        //get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        Debug.Log(name + "rotation changing");
        yRotation += mouseX;
        //yRotation = Mathf.Clamp(yRotation, -90f, 90f);
        xRotation -= mouseY;
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate camera orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
