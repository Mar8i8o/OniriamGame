using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlarEventoNPC : MonoBehaviour
{
    public int hora;
    public int minutos;
    public int dia;

    [HideInInspector] public float totalSegundos;

    TimeController timeControler;
    ControladorVisitas controladorVisitas;
    GuardarController guardarController;

    public bool eventoEnviado;

    public GameObject npcEnviar;


    void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name + "llamadaRealizada", System.Convert.ToInt32(eventoEnviado)) == 0) { eventoEnviado = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "llamadaRealizada", System.Convert.ToInt32(eventoEnviado)) == 1) { eventoEnviado = true; }

        timeControler = GameObject.Find("GameManager").GetComponent<TimeController>();
        controladorVisitas = GameObject.Find("GameManager").GetComponent<ControladorVisitas>();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();


    }

    // Update is called once per frame
    void Update()
    {

        //print("total segundos evento:" + totalSegundos + " otro:" + timeControler.totalSegundos);

        totalSegundos = GetTotalSeconds(hora, minutos);


        if (!eventoEnviado)
        {
                if (dia == timeControler.dia)
                {

                    if (totalSegundos <= timeControler.totalSegundos && totalSegundos != 0)
                    {
                        controladorVisitas.GenerarVisita(npcEnviar);
                        eventoEnviado = true;
                    }

                }

        }

        //if (mensajeEnviado) gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt(gameObject.name + "NPCLlamado", System.Convert.ToInt32(eventoEnviado));
        }
    }

    public float GetTotalSeconds(int horas, int minutos)
    {
        int totalSegundos = horas * 3600 + minutos * 60;
        return totalSegundos;
    }
}
