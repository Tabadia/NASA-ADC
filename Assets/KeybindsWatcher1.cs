using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindsWatcher : MonoBehaviour
{
    // Define the panel that should show when the esc key is pressed
    public GameObject escPanel;

    // used to remove the screen rotation
    public Transform orientation;

    // Start is called before the first frame update
    void Start()
    {
        // have the panel off by default, since it should only show when the user wants to switch a page
        escPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {

            // toggle panel visibility accordingly
            if (escPanel.activeInHierarchy == true)
            {
                // enable cursor lock
                // see playerCam.cs (20:0) for info
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                escPanel.SetActive(false);
            } else
            {
                // remove cursor lock
                // see playerCam.cs (20:0) for info
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                escPanel.SetActive(true);
            }
        }
    }
}