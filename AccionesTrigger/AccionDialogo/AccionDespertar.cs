using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionDespertar : MonoBehaviour
{
    public DreamController dreamController;

    public float tiempoEnDespertarse;
    void Start()
    {
        Invoke(nameof(Despertarse),tiempoEnDespertarse);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Despertarse()
    {
        dreamController.Despertarse();
    }
}
