using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenPanel : MonoBehaviour
{

    MeshGen2 meshGen; 
    playerCam p1Cam;
    playerCam p2Cam; 
    // Start is called before the first frame update
    void Start()
    {
        meshGen = GameObject.Find("Mesh").GetComponent<MeshGen2>();
        p1Cam = GameObject.Find("PlayerCam").GetComponent<playerCam>();
        p2Cam = GameObject.Find("PlayerCam2").GetComponent<playerCam>();

        p1Cam.canMove = false;
        p2Cam.canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
       
        if(meshGen.isFinished == true && transform.gameObject.activeSelf)
        {
            transform.gameObject.SetActive(false);
            p1Cam.canMove = true;
            p2Cam.canMove = true;
        }    
    }
}
