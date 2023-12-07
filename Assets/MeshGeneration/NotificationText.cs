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
        text = GetComponent<TMP_Text>();
        mesh = GameObject.Find("Mesh").GetComponent<MeshGen2>();
        slider = GameObject.Find("Chunks Slider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mesh.isFinished)
        {
            text.text = "Finalizing...";
            overlay.SetActive(false);
            return;
        }
        text.text = mesh.chunksGenerated + " / 1024";
        slider.value = mesh.chunksGenerated;
    }

    public void CancelButtonClick()
    {
        SceneManager.LoadScene(sceneName: "Home Screen");
    }
}
