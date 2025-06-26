using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlarEventoNoticia : MonoBehaviour
{
    public int hora;
    public int minutos;

    public int dia;

    public NoticiasController noticiaParaEnviar;

    [HideInInspector] public float totalSegundos;

    TimeController timeControler;
    NoticiasManager eventosCorreos;
    GuardarController guardarController;

    public bool noticiaEnviada;


    void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name + "noticiaEnviada", System.Convert.ToInt32(noticiaEnviada)) == 0) { noticiaEnviada = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "noticiaEnviada", System.Convert.ToInt32(noticiaEnviada)) == 1) { noticiaEnviada = true; }

        timeControler = GameObject.Find("GameManager").GetComponent<TimeController>();
        eventosCorreos = GameObject.Find("GameManager").GetComponent<NoticiasManager>();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();


    }

    // Update is called once per frame
    void Update()
    {

        //print("total segundos evento:" + totalSegundos + " otro:" + timeControler.totalSegundos);

        totalSegundos = GetTotalSeconds(hora, minutos);

        if (!noticiaEnviada)
        {

            if (dia == timeControler.dia)
            {

                if (totalSegundos <= timeControler.totalSegundos && totalSegundos != 0)
                {
                    eventosCorreos.SetNoticia(noticiaParaEnviar);
                    noticiaEnviada = true;
                }

            }

        }

        //if (mensajeEnviado) gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt(gameObject.name + "noticiaEnviada", System.Convert.ToInt32(noticiaEnviada));
        }
    }

    public float GetTotalSeconds(int horas, int minutos)
    {
        int totalSegundos = horas * 3600 + minutos * 60;
        return totalSegundos;
    }
}
