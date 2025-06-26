using UnityEngine;
using System;

public class ExitTrigger : MonoBehaviour
{
    public event Action OnPlayerEnter;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnter?.Invoke();  // Llama al m�todo que le hayas asignado
        }
    }
}
