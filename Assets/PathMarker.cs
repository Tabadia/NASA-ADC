using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMarker : MonoBehaviour
{
    public GameObject pathMarkerPrefab;
    public void InstantiatePathMarker(Vector3 pos) {
        Instantiate(pathMarkerPrefab, pos, Quaternion.identity);
    }
}
