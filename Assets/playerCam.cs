using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class playerCam : MonoBehaviour
    
{
    public Vector3 offset = Vector3.zero;
    public float topDownCameraHeight = 15f;
    public float sensY;
    public float sensX;

    public Transform orientation;
    public Transform player;

    public GameObject escPanel;

    float xRotation;
    float yRotation;

    bool isInF5Mode = false;
    float offsetY;
    void Start()
    {
        offsetY = offset.y;
        //make it so that mouse cant leave the screen as well as making it invisible
        //this makes it so that the camera can rotate
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // only execute the following if the esc panel (the panel that shows up when hitting "esc" is NOT visible
        if (!escPanel.activeInHierarchy)
        {
            
        Vector3 pos = player.position;
        pos.Set(pos.x + offset.x, pos.y + offset.y, pos.z + offset.z);
        transform.position = pos;
        //camera not enabled or is a minimap camera
        if (!GetComponent<Camera>().enabled) return;
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if(isInF5Mode)
            {
                offset.y = offsetY;
            } else
            { 
                offset.y = topDownCameraHeight;

                transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            }
            isInF5Mode = !isInF5Mode;
        }
        if (isInF5Mode) return; //lock mouse movement 
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
}
