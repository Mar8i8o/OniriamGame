using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAbrirPuerta : MonoBehaviour
{
    public DoorController doorController;
    public bool entreAbrirPuerta;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if(entreAbrirPuerta) { doorController.EntreAbrirPuerta(); }
            else { doorController.SetPuertaAbierta(); }
            Destroy(gameObject);
        }
    }
}
