using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventoManchasController : MonoBehaviour
{
    public int hora;
    public int minutos;

    public int dia;

    public ManchaParedController[] manchasASpawnear;

    [HideInInspector] public float totalSegundos;

    TimeController timeControler;
    EventosMensajes eventosMensajes;
    GuardarController guardarController;

    public bool manchaSpawneada;

    void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name + "manchaSpawned", System.Convert.ToInt32(manchaSpawneada)) == 0) { manchaSpawneada = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "manchaSpawned", System.Convert.ToInt32(manchaSpawneada)) == 1) { manchaSpawneada = true; }

        timeControler = GameObject.Find("GameManager").GetComponent<TimeController>();
        eventosMensajes = GameObject.Find("GameManager").GetComponent<EventosMensajes>();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();


    }

    // Update is called once per frame
    void Update()
    {

        //print("total segundos evento:" + totalSegundos + " otro:" + timeControler.totalSegundos );

        totalSegundos = GetTotalSeconds(hora, minutos);

        if (!manchaSpawneada)
        {

            if (dia == timeControler.dia)
            {

                if (totalSegundos <= timeControler.totalSegundos && totalSegundos != 0)
                {
                    for (int i = 0; i < manchasASpawnear.Length; i++) 
                    {
                        manchasASpawnear[i].Activar();
                    }
                    manchaSpawneada = true;
                }

            }

        }

        //if (mensajeEnviado) gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt(gameObject.name + "manchaSpawned", System.Convert.ToInt32(manchaSpawneada));
        }
    }

    public float GetTotalSeconds(int horas, int minutos)
    {
        int totalSegundos = horas * 3600 + minutos * 60;
        return totalSegundos;
    }
}
