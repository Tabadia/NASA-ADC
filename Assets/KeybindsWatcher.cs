using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class KeybindsWatcher : MonoBehaviour
{
    // Define the panel that should show when the esc key is pressed
    public GameObject escPanel;

    // Define the panel that shold show up to edit mesh ooh fancy
    public GameObject meshEditor;

    // used to remove the screen rotation
    public Transform orientation;
    public GameObject meshOptions;
    // Start is called before the first frame update
    void Start()
    {
        // have the panel off by default, since it should only show when the user wants to switch a page
        escPanel.SetActive(false);
        meshEditor.SetActive(false);
    }
    bool coloringOptionsOpen = false;
    // Update is called once per frame

    void lockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void unlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {

            // toggle panel visibility accordingly
            if (escPanel.activeInHierarchy == true)
            {
                // enable cursor lock
                // see playerCam.cs (20:0) for info
                lockMouse();

                escPanel.SetActive(false);
            } else
            {
                // remove cursor lock
                // see playerCam.cs (20:0) for info
                unlockMouse();

                escPanel.SetActive(true);
            }
        }

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // toggle panel visibility accordingly
                if (meshEditor.activeInHierarchy == true)
                {
                    // enable cursor lock
                    // see playerCam.cs (20:0) for info
                    lockMouse();

                    meshEditor.SetActive(false);
                }
                else
                {
                    // remove cursor lock
                    // see playerCam.cs (20:0) for info
                    unlockMouse();

                    meshEditor.SetActive(true);
                }
            }
        }
        }

    public void closeMeshEditor()
    {
        meshEditor.SetActive(false);
    }
}