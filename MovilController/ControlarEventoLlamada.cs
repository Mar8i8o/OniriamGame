using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlarEventoLlamada : MonoBehaviour
{
    public int hora;
    public int minutos;

    public int dia;

    public string nombreUsuarioLlamada;

    public string idDialogo;

    public bool blockColgar;

    [HideInInspector] public float totalSegundos;

    TimeController timeControler;
    LlamadasController eventosLlamadas;
    GuardarController guardarController;

    public bool llamadaEnviada;


    public GameObject panelLlamada;

    void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name + "llamadaRealizada", System.Convert.ToInt32(llamadaEnviada)) == 0) { llamadaEnviada = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "llamadaRealizada", System.Convert.ToInt32(llamadaEnviada)) == 1) { llamadaEnviada = true; }

        timeControler = GameObject.Find("GameManager").GetComponent<TimeController>();
        eventosLlamadas = GameObject.Find("GameManager").GetComponent<LlamadasController>();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();


    }

    // Update is called once per frame
    void Update()
    {

        //print("total segundos evento:" + totalSegundos + " otro:" + timeControler.totalSegundos);

        totalSegundos = GetTotalSeconds(hora, minutos);


        if (!llamadaEnviada)
        {
            if (!panelLlamada.activeSelf)
            {
                if (dia == timeControler.dia)
                {

                    if (totalSegundos <= timeControler.totalSegundos && totalSegundos != 0)
                    {
                        EnviarLlamada();
                    }

                }
            }

        }

        //if (mensajeEnviado) gameObject.SetActive(false);
    }

    public void EnviarLlamada()
    {
        if(blockColgar) eventosLlamadas.bloquearColgar = true;
        else { eventosLlamadas.bloquearColgar = false; }
        eventosLlamadas.GenerarRecibirLlamada(nombreUsuarioLlamada, idDialogo);
        llamadaEnviada = true;
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt(gameObject.name + "llamadaRealizada", System.Convert.ToInt32(llamadaEnviada));
        }
    }

    public float GetTotalSeconds(int horas, int minutos)
    {
        int totalSegundos = horas * 3600 + minutos * 60;
        return totalSegundos;
    }
}
