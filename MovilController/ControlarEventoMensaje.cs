using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlarEventoMensaje : MonoBehaviour
{
    public int hora;
    public int minutos;

    public int dia;

    public GameObject mensajeParaEnviar;

    [HideInInspector] public float totalSegundos;

    TimeController timeControler;
    EventosMensajes eventosMensajes;
    GuardarController guardarController;

    public bool mensajeEnviado;

    void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name + "mensajeEnviado", System.Convert.ToInt32(mensajeEnviado)) == 0) { mensajeEnviado = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "mensajeEnviado", System.Convert.ToInt32(mensajeEnviado)) == 1) { mensajeEnviado = true; }

        timeControler = GameObject.Find("GameManager").GetComponent<TimeController>();
        eventosMensajes = GameObject.Find("GameManager").GetComponent <EventosMensajes>();
        guardarController = GameObject.Find("GameManager").GetComponent <GuardarController>();


    }

    // Update is called once per frame
    void Update()
    {

        //print("total segundos evento:" + totalSegundos + " otro:" + timeControler.totalSegundos );

        totalSegundos = GetTotalSeconds(hora, minutos);

        if (!mensajeEnviado)
        {

            if (dia == timeControler.dia)
            {

                if (totalSegundos <= timeControler.totalSegundos && totalSegundos != 0)
                {
                    eventosMensajes.EnviarMensajes(mensajeParaEnviar);
                    mensajeParaEnviar.GetComponent<MensajeController>().LlegarMensaje();
                    mensajeEnviado = true;
                }

            }

        }

        //if (mensajeEnviado) gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt(gameObject.name + "mensajeEnviado", System.Convert.ToInt32(mensajeEnviado));
        }
    }

    public float GetTotalSeconds(int horas, int minutos)
    {
        int totalSegundos = horas * 3600 + minutos * 60;
        return totalSegundos;
    }
}
