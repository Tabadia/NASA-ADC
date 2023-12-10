using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour 
{
    [Header("Checkboxes")]
    public Toggle fpsToggle;
    public Toggle slopeToggle;
    public Toggle coordsToggle;

    [Header("Displayed Stuffs")]
    public GameObject fps;
    public GameObject slope;
    public GameObject coords;


    // Start is called before the first frame update
    void Start()
    {
        // is liquid cooling really that much of a crazy play?

        fpsToggle.onValueChanged.AddListener(delegate {
            fps.SetActive(fpsToggle.isOn);
        });

        slopeToggle.onValueChanged.AddListener(delegate {
            slope.SetActive(slopeToggle.isOn);
        });

        coordsToggle.onValueChanged.AddListener(delegate {
            coords.SetActive(coordsToggle.isOn);
        });
    }

    // Update is called once per frame
    void Update()
    {
        // update everything nice
        if (fps.activeInHierarchy == true) fpsToggle.isOn = true;
        if (slope.activeInHierarchy == true) slopeToggle.isOn = true;
        if (coords.activeInHierarchy == true) coordsToggle.isOn = true;
    }
}
