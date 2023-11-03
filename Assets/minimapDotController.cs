using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class minimapDotController : MonoBehaviour
{
    public GameObject player;
    public GameObject terrain;
    Vector3 pos;
    MeshRenderer render;
    Vector3 size;
    // Start is called before the first frame update
    void Start()
    {
        pos = player.transform.position;
        render = terrain.GetComponent<MeshRenderer>();
        size = render.bounds.size;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(size.x + " " + size.y);
        float xRatio = transform.position.x / size.x;
        float yRatio = transform.position.y / size.y;
        Debug.Log(xRatio + ' ' + yRatio);
    }
}
