using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCam : MonoBehaviour
{

    public GameObject player;
    public float heightAbovePlayer = 20f;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }
}
