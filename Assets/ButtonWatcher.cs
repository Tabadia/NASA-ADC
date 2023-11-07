using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonWatcher : MonoBehaviour
{
    public GameObject escPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void whenResumeButtonClicked()
    {
        // enable cursor lock
        // see playerCam.cs (20:0) for info
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        escPanel.SetActive(false);
    }

    public void whenExitButtonClicked()
    {
        // switch scene
        SceneManager.LoadScene(sceneName: "Home Screen");
    }
}
