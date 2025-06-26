using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCarta : MonoBehaviour
{
    public EventosEntregarCartas eventosEntregarCartas;

    public int hora;
    public int minutos;

    public int dia;

    public ItemAtributes cartaAEntregar;

    [HideInInspector] public float totalSegundos;

    TimeController timeControler;
    EventosMensajes eventosMensajes;
    GuardarController guardarController;

    public bool cartaEnviada;

    void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name + "cartaEnviada", System.Convert.ToInt32(cartaEnviada)) == 0) { cartaEnviada = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "cartaEnviada", System.Convert.ToInt32(cartaEnviada)) == 1) { cartaEnviada = true; }

        timeControler = GameObject.Find("GameManager").GetComponent<TimeController>();
        eventosMensajes = GameObject.Find("GameManager").GetComponent<EventosMensajes>();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();

    }

    // Update is called once per frame
    void Update()
    {

        //print("total segundos evento:" + totalSegundos + " otro:" + timeControler.totalSegundos );

        totalSegundos = GetTotalSeconds(hora, minutos);

        if (!cartaEnviada)
        {

            if (dia == timeControler.dia)
            {

                if (totalSegundos <= timeControler.totalSegundos && totalSegundos != 0)
                {
                    eventosEntregarCartas.EntregarCarta(cartaAEntregar);
                    cartaEnviada = true;
                }

            }

        }

        //if (mensajeEnviado) gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt(gameObject.name + "cartaEnviada", System.Convert.ToInt32(cartaEnviada));
        }
    }

    public float GetTotalSeconds(int horas, int minutos)
    {
        int totalSegundos = horas * 3600 + minutos * 60;
        return totalSegundos;
    }
}
