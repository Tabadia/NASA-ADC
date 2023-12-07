using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject[] UIsToHideAndShow;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var UIComponent in UIsToHideAndShow)
        {
            UIComponent.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.name.IndexOf("Player") != -1)
        {
            Vector3 pos = new Vector3(10, 633, 28);
            GameObject.Find("Player2").transform.position = pos;
            GameObject.Find("Player").transform.position = pos;
            foreach (var UIComponent in UIsToHideAndShow)
            {
                UIComponent.SetActive(true);
            }
        }
    }

}
