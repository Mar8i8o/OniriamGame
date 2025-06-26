using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornoController : MonoBehaviour
{
    public Animator palanca;

    public bool encendido;

    public GameObject luzHorno;

    ElectricidadController electricidadController;

    PensamientoControler pensamientoControler;

    public float tiempoEncendido;

    public AudioSource sonidoTimerHorno;
    public AudioSource sonidoHorno;
    
    void Start()
    {
        electricidadController = GameObject.Find("ElectricidadControler").GetComponent<ElectricidadController>();
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();

        ApagarHorno();
    }

    // Update is called once per frame
    void Update()
    {
        if (encendido)
        {
            if (!electricidadController.electricidad)
            {
                ApagarHorno();
            }

            tiempoEncendido += Time.deltaTime;

            if (tiempoEncendido > 60)
            { 
                sonidoTimerHorno.Play();
                ApagarHorno();
            }

        }

    }

    public void Interactuar()
    {
        if (encendido) 
        {
            ApagarHorno();
        }
        else
        {
            if (electricidadController.electricidad)
            {
                EncenderHorno();
            }
            else
            {
                pensamientoControler.MostrarPensamiento("No se puede encender sin electricidad", 1);
            }

        }
    }

    public void EncenderHorno()
    {
        encendido = true;
        luzHorno.SetActive(true);
        palanca.SetBool("Encendido", true);
        tiempoEncendido = 0;
        sonidoHorno.Play();
    }

    public void ApagarHorno()
    {
        encendido = false;
        luzHorno.SetActive(false);
        palanca.SetBool("Encendido", false);
        sonidoHorno.Stop();
    }
}
