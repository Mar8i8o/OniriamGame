using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarPlayer : MonoBehaviour
{
    public bool playerDentro;
    public bool playerDentroUnico;

    public bool extendido;
    public DetectarPlayer detectarPlayerPadre;
    public DoorController doorController;
    //public GameObject extensible;
    //public EnemyController enemyController;

    private void Update()
    {
        //if (padre) { extensible.SetActive(doorController.puertaAbierta || doorController.entreAbierta); }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (!extendido) { playerDentro = true; playerDentroUnico = true; }
            else 
            {
                detectarPlayerPadre.playerDentro = doorController.puertaAbierta || doorController.entreAbierta;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (!extendido) { playerDentro = false; playerDentroUnico = false; }
            else { detectarPlayerPadre.playerDentro = false; }
        }
    }

}
