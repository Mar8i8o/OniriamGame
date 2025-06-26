using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWC : MonoBehaviour
{
    public GameObject camara;
    void Start()
    {
        camara.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camara.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camara.SetActive(false);
        }
    }
}
