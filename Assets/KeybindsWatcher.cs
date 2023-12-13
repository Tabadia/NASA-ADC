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
    public GameObject settingsPanel;
    public GameObject pathfindingPanel;

    // used to see mesh gen progress
    public MeshGen2 mesh;

    // Start is called before the first frame update
    void Start()
    {
        // have the panel off by default, since it should only show when the user wants to switch a page
        escPanel.SetActive(false);
        settingsPanel.SetActive(false);
        pathfindingPanel.SetActive(false);
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

    void closeAllPanels()
    {
        escPanel.SetActive(false);
        settingsPanel.SetActive(false);
        pathfindingPanel.SetActive(false);
    }

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
                else if (settingsPanel.activeInHierarchy == true)
                {
                    lockMouse();

                    settingsPanel.SetActive(false);
                }
                else if (pathfindingPanel.activeInHierarchy == true)
                {
                    lockMouse();

                    pathfindingPanel.SetActive(false);
                }
                else
                {
                    // remove cursor lock
                    // see playerCam.cs (20:0) for info
                    unlockMouse();

                    escPanel.SetActive(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.S))
            {
                OpenPanel(settingsPanel);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.P))
            {
                OpenPanel(pathfindingPanel);
            }
        }
    }

    public void OpenPanel(GameObject panel)
    {
        closeAllPanels();

        if (panel.activeInHierarchy) lockMouse(); else unlockMouse();

        panel.SetActive(!panel.activeInHierarchy);
    }
}