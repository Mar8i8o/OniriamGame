using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespiracionController : MonoBehaviour
{
    public float tiempoEntreRespiraciones;
    public bool saleVaho;
    public bool seEscucha;

    public AudioSource audioCogerAire;
    public AudioSource audioSoltarAire;

    public ParticleSystem particulasVaho;



    void Start()
    {
        CogerAire();
    }

    void Update()
    {
        
    }

    public void CogerAire()
    {
        
        if (seEscucha)audioCogerAire.Play();
        Invoke(nameof(SoltarAire), tiempoEntreRespiraciones);
    }
    public void SoltarAire()
    {
        if (saleVaho) particulasVaho.Play();
        if (seEscucha)audioSoltarAire.Play();
        Invoke(nameof(CogerAire), tiempoEntreRespiraciones);
    }
}
