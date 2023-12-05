using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPS : MonoBehaviour
{
    TextMeshProUGUI text;
    int frames;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(counter());
    }
    private void Update()
    {
        frames++;
    }
    IEnumerator counter()
    {
        yield return new WaitForSeconds(1);
        text.text = frames.ToString();
        frames = 0;
        StartCoroutine(d());
    }
}