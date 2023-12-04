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
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        mesh = GameObject.Find("Mesh").GetComponent<MeshGen2>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mesh.isFinished)
        {
            text.text = "";
            return;
        }
        text.text = "Generating mesh:" + mesh.chunksGenerated + " chunks generated";
    }
}
