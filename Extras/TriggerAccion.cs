using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAccion : MonoBehaviour
{
    public GameObject accion;

    public bool isTrigger;

    public bool unicaVez;

    public void IniciarAccion()
    {
        print("accion");
        accion.SetActive(true);

        if(unicaVez) { Destroy(gameObject); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (isTrigger)
            {
                accion.SetActive(true);
                if (unicaVez) { Destroy(gameObject); }
            }
        }
    }
}
