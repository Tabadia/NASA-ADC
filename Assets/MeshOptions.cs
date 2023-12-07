using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshOptions : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject meshOptions;
    void Start()
    {
        meshOptions.SetActive(false);
    }
    bool open = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Cursor.lockState = CursorLockMode.None;
            meshOptions.SetActive(!open);
            open = !open;
        }
    }
}
