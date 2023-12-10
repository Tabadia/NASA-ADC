using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class KeybindsWatcher : MonoBehaviour
{
    // Define the panel that should show when the esc key is pressed
    public GameObject escPanel;

    // used to remove the screen rotation
    public Transform orientation;
    public GameObject meshOptions;

    // used to see mesh gen progress
    public MeshGen2 mesh;

    // Start is called before the first frame update
    void Start()
    {
        // have the panel off by default, since it should only show when the user wants to switch a page
        escPanel.SetActive(false);
        meshOptions.SetActive(false);
    }

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

    bool coloringOptionsOpen = false;

    // Update is called once per frame
    void Update()
    {
        if (mesh.isFinished == true)
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
                }
                else
                {
                    // remove cursor lock
                    // see playerCam.cs (20:0) for info
                    unlockMouse();

                    escPanel.SetActive(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                escPanel.SetActive(false);
                OpenSettings();
            }
        }
    }

    public void OpenSettings()
    {
        if (coloringOptionsOpen) lockMouse(); else unlockMouse();

        meshOptions.SetActive(!coloringOptionsOpen);
        coloringOptionsOpen = !coloringOptionsOpen;
    }
}