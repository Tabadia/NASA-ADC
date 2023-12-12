using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touching : MonoBehaviour
{
    private void OnCollisionEnter()
    {
        Constants.meshBeingTouched = transform.gameObject.GetComponent<MeshFilter>();
        Debug.Log(transform.name);
    }
}
