using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       Vector3 playerPos = player.position;
       transform.position = new Vector3(playerPos.x, playerPos.y - 1, playerPos.z);
    }
}
