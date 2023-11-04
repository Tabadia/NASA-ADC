using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is to handle all the button clicks that will occur on the home screen

public class ButtonEventsManager : MonoBehaviour
{
    public GameObject keybindsPanel;

    // Start is called before the first frame update
    void Start()
    {
        keybindsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape") && keybindsPanel.activeInHierarchy == true)
        {
            keybindsPanel.SetActive(false);
        }
    }

    public void whenKeybindsButtonClicked()
    {
        if (keybindsPanel.activeInHierarchy == true)
            keybindsPanel.SetActive(false);
        else
            keybindsPanel.SetActive(true);
    }
}