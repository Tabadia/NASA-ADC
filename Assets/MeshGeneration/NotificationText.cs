using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class NotificationText : MonoBehaviour
{
    MeshGen2 mesh;

    TMP_Text text;

    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        text = GetComponent<TMP_Text>();
        mesh = GameObject.Find("Mesh").GetComponent<MeshGen2>();
        slider = GameObject.Find("Chunks Slider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mesh.isFinished)
        {
            text.text = "";
            return;
        }
        text.text = mesh.chunksGenerated + " / 1024";
        slider.value = mesh.chunksGenerated;
    }
}
