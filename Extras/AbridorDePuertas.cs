using UnityEngine;

public class AbridorDePuertas : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("PomoPuerta"))
        {
            DoorController doorController = other.gameObject.GetComponent<DoorController>();

            if (doorController.sePuedeAbrir && !doorController.puertaAbierta)
            {
                doorController.SetPuertaAbierta();
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("PomoPuerta"))
        {
            DoorController doorController = other.gameObject.GetComponent<DoorController>();

            if (doorController.sePuedeAbrir && !doorController.puertaAbierta)
            {
                doorController.SetPuertaAbierta();
            }
        }
    }

}
