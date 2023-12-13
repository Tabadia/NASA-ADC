using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GenPanel : MonoBehaviour
{
    public MeshGen2 mesh;
    public GameObject chunksPercentageText;
    public Slider slider;
    public GameObject panel;

    public playerCam p1Cam;
    public playerCam p2Cam;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        // text = GetComponent<TMP_Text>();
        // meshGen = GameObject.Find("Mesh").GetComponent<MeshGen2>();
        // slider = GameObject.Find("Chunks Slider").GetComponent<Slider>(); 
        // p1Cam = GameObject.Find("PlayerCam").GetComponent<playerCam>();
        // p2Cam = GameObject.Find("Main Camera").GetComponent<playerCam>();

        // p1Cam.canMove = false;
        // p2Cam.canMove = false;
        text = chunksPercentageText.GetComponent<TextMeshProUGUI>();

        panel.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
       
        if(mesh.isFinished == true)
        {

            text.text = "Loading...";
            panel.SetActive(false);
            return;
        }
        /*
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        */

        double chunksGenerated = mesh.chunksGenerated;
        double percentage = CalculatePercentage(chunksGenerated, 1024);

        text.text = percentage.ToString("F2") + "%";
        slider.value = mesh.chunksGenerated;
    }

    static double CalculatePercentage(double value, double totalValue)
    {
        double percentage = (value / totalValue) * 100;

        return percentage;
    }

    public void cancelGen()
    {
        SceneManager.LoadScene(sceneName: "Home Screen");
    }
}
