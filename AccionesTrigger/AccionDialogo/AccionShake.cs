using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionShake : MonoBehaviour
{
    public CameraShake cameraShake;

    public bool desactivarShake;
    public float tiempoDesactivarShake;
    void Start()
    {
        ActivarShake();
        if(desactivarShake)Invoke(nameof(DesactivarShake), tiempoDesactivarShake);
    }

    public void ActivarShake()
    {
        cameraShake.shake = true;
    }

    public void DesactivarShake()
    {
        cameraShake.shake = false;
    }
}
