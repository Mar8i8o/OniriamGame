using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dia1_Controller : MonoBehaviour
{
    TimeController timeController;
    TutorialController tutorialController;
    EventosCorreos eventosCorreos;
    LlamadasController llamadasController;
    GameObject player;
    public EventosMensajes eventosMensajes;
    public int indiceHistoria;
    public GameObject correoJefe1;
    public GameObject mensajeAmigoD1;
    public GameObject mensajeJefeD1;
    public DoorController puertaAtico;
    public DoorController puertaPrincipal;
    public DetectarPlayer detectarPlayerFuera;
    public PensamientoControler pensamientoControler;
    public GameObject[] desaparecerDia1;
    public GameObject panelLlamadas;
    public ContactoController contactoMadre;
    public ContactoController contactoJefe;

    public TriggerSonido triggerInterior;


    void Start()
    {
        timeController = GameObject.Find("GameManager").GetComponent<TimeController>();
        tutorialController = GameObject.Find("GameManager").GetComponent<TutorialController>();
        eventosCorreos = GameObject.Find("GameManager").GetComponent<EventosCorreos>();
        llamadasController = GameObject.Find("GameManager").GetComponent<LlamadasController>();
        //eventosMensajes = GameObject.Find("GameManager").GetComponent<EventosMensajes>();
        player = GameObject.Find("Player");

        puertaAtico.iniciaDialogo = false;

        for(int i = 0; i < desaparecerDia1.Length; i++) 
        {
            desaparecerDia1[i].SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(indiceHistoria == 0)
        {
            if(!tutorialController.tutorial)
            {
                indiceHistoria++;
            }
        }
        else if(indiceHistoria == 1) 
        {
            if(SeHaPasado(12,32))
            {
                eventosCorreos.EnviarCorreo(correoJefe1);
                eventosMensajes.EnviarMensajes(mensajeJefeD1);

                contactoMadre.contesta = true;
                contactoMadre.dialogoUnico = true;
                contactoMadre.idDialogo = "madre_d1_1";

                indiceHistoria++;
            }
        }
        else if (indiceHistoria == 2)
        {
            if (SeHaPasado(13, 00))
            {
                llamadasController.esperaLlamadaInfinita = true;
                pensamientoControler.MostrarPensamiento("Seguro que es mi jefe, no he programado nada aún", 2);
                llamadasController.GenerarRecibirLlamada("Brad Jefe", "jefe_d1_1");
                llamadasController.bloquearColgar = true;

                llamadasController.tienePensamiento = true;
                llamadasController.pensamiento = "Deberia ponerme a trabajar";

                indiceHistoria++;

            }
        }
        else if (indiceHistoria == 3)
        {
            if (!eventoTimbreFinish)
            {
                if (SeHaPasado(18, 00) && triggerInterior.isInTrigger)
                {
                    if (!detectarPlayerFuera.playerDentro && !eventoTimbreActivo)
                    {
                        puertaPrincipal.sonidoGolpearPuerta.Play();
                        print("GolpearPuerta");
                        LlamarPensamieto("Han golpeado la puerta", 3, 2);
                        eventoTimbreActivo = true;
                        indiceHistoria = 4;
                    }
                }
            }
        }
        else if (indiceHistoria == 4)
        {
            if (SeHaPasado(19, 00))
            {
                llamadasController.GenerarRecibirLlamada("Noah", "amigo_d1_2");
                llamadasController.bloquearColgar = true;
                indiceHistoria++;
            }
        }
        else if (indiceHistoria == 5)
        {
            if (!panelLlamadas.activeSelf)
            {
                if (SeHaPasado(20, 30))
                {
                    pensamientoControler.MostrarPensamiento("Está anocheciendo, deberia cerrar con pestillo", 2);
                    eventoTimbreActivo = false;
                    indiceHistoria++;
                }
            }
        }
        else if (indiceHistoria == 6)
        {
            if (SeHaPasado(21, 30))
            {
                pensamientoControler.MostrarPensamiento("Ya ha anochecido, deberia irme a dormir", 2);
                indiceHistoria++;
            }
        }




        if (eventoTimbreActivo)
        {
            if (Vector3.Distance(player.transform.position, puertaPrincipal.gameObject.transform.position) <= 2)
            {
                sonidoCorrerPasillo.Play();
                LlamarPensamieto("Seguro que es algún gracioso que se piensa que aún es Halloween", 3, 2);
                eventoTimbreFinish = true;
                eventoTimbreActivo = false;

            }
        }

    }

    public bool eventoTimbreFinish;
    public bool eventoTimbreActivo;

    public AudioSource sonidoCorrerPasillo;

    public bool ComprobarHora(float hora, float minutos)
    {
        int segundosUsuario = (int)(hora * 3600) + (int)(minutos * 60);
        int margen = 300; // 5 minutos en segundos

        // Comprobar si el tiempo del usuario es mayor pero dentro del margen permitido
        return segundosUsuario > timeController.totalSegundos &&
               segundosUsuario <= timeController.totalSegundos + margen;
    }

    public bool SeHaPasado(int horas, int minutos)
    {
        TimeSpan objetivo = new TimeSpan(horas, minutos, 0);
        TimeSpan actual = TimeSpan.FromSeconds(timeController.totalSegundos);
        return actual > objetivo;
    }

    bool llamandoPensamiento;
    string pensamientoAMostrar;
    float duracionPensamientoAMostrar;
    public void LlamarPensamieto(string pensamiento, float tiempo, float duracion)
    {
        pensamientoAMostrar = pensamiento;
        duracionPensamientoAMostrar = duracion;
        Invoke(nameof(MostrarPensamiento), tiempo);
        llamandoPensamiento = true;
    }

    void MostrarPensamiento()
    {
        pensamientoControler.MostrarPensamiento(pensamientoAMostrar, duracionPensamientoAMostrar);
        llamandoPensamiento = false;
    }

}
