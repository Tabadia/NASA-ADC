using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is to handle all the button clicks that will occur on the home screen

public class ButtonEventsManager : MonoBehaviour
{
    public GameObject keybindsPanel;
    public GameObject quitPanel;
    public GameObject informationAndCreditsPanel;

    // Start is called before the first frame update
    void Start()
    {
        keybindsPanel.SetActive(false);
        quitPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape") && keybindsPanel.activeInHierarchy == true)
        {
            keybindsPanel.SetActive(false);
        }

        if (Input.GetKeyDown("escape") && quitPanel.activeInHierarchy == true)
        {
            quitPanel.SetActive(false);
        }

        if (Input.GetKeyDown("escape") && informationAndCreditsPanel.activeInHierarchy == true)
        {
            informationAndCreditsPanel.SetActive(false);
        }
    }

    public void whenKeybindsButtonClicked()
    {
        if (keybindsPanel.activeInHierarchy == true)
        {
            keybindsPanel.SetActive(false);
        } else
        {
            keybindsPanel.SetActive(true);
        }
    }

    public void whenOpenSimulationButtonClicked()
    {
        SceneManager.LoadScene(sceneName:"Lighting Test");
    }

    public void whenQuitButtonClick()
    {
        quitPanel.SetActive(true);
    }

    public void whenCreditsAndInfoButtonClick()
    {
        if (informationAndCreditsPanel.activeInHierarchy == true)
        {
            informationAndCreditsPanel.SetActive(false);
        }
        else
        {
            informationAndCreditsPanel.SetActive(true);
        }
    }
    public void switchToMeshScene()
    {
        SceneManager.LoadScene(sceneName: "ProMesh 2");
    }
    public void quitGame(bool value)
    {
              
        if (value == true)
        {
            Debug.Log("Quit");
        } else
        {
            quitPanel.SetActive(false);
        }
    }
    
}