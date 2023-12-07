using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NotificationText : MonoBehaviour
{
    public GameObject overlay;
    
    MeshGen2 mesh;

    TMP_Text text;

    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        text = GetComponent<TMP_Text>();
        mesh = GameObject.Find("Mesh").GetComponent<MeshGen2>();
        slider = GameObject.Find("Chunks Slider").GetComponent<Slider>();

        overlay.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(mesh.isFinished)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            text.text = "Finalizing...";
            overlay.SetActive(false);
            return;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        text.text = mesh.chunksGenerated + " / 1024";
        slider.value = mesh.chunksGenerated;
        overlay.SetActive(true);

    }

    public void CancelButtonClick()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(sceneName: "Home Screen");
    }
}
