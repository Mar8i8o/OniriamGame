using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarObjetoCabeza : MonoBehaviour
{
    public bool objetoEncima;
    public Raycast raycast;

    // Máscara de capas que quieres detectar (configúrala desde inspector)
    public LayerMask capasDetectar;

    private void OnTriggerStay(Collider other)
    {
        if (!other.isTrigger && ((capasDetectar.value & (1 << other.gameObject.layer)) != 0))
        {
            objetoEncima = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger && ((capasDetectar.value & (1 << other.gameObject.layer)) != 0))
        {
            objetoEncima = false;

            if (raycast.agachado)
            {
                if (!raycast.pulsandoAgachado)
                {
                    raycast.agachado = false;
                }
            }
        }
    }
}
