using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshButtonHandler : MonoBehaviour
{

    MeshGenerator mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GameObject.Find("Mesh").GetComponent<MeshGenerator>();
    }

    public void ColorMeshByHeight()
    {
        mesh.changeMode("height");
    }
    public void ColorMeshByAngle() {
        mesh.changeMode("angle");
    }
}
