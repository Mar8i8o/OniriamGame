using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlarEventoCorreo : MonoBehaviour
{
    public int hora;
    public int minutos;

    public int dia;

    public GameObject correoParaEnviar;

    [HideInInspector] public float totalSegundos;

    TimeController timeControler;
    EventosCorreos eventosCorreos;
    GuardarController guardarController;

    public bool mensajeEnviado;

    public GameObject chekCorreo;
    public GameObject chekCorreoPosition;

    void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name + "correoEnviado", System.Convert.ToInt32(mensajeEnviado)) == 0) { mensajeEnviado = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "correoEnviado", System.Convert.ToInt32(mensajeEnviado)) == 1) { mensajeEnviado = true; }

        timeControler = GameObject.Find("GameManager").GetComponent<TimeController>();
        eventosCorreos = GameObject.Find("GameManager").GetComponent<EventosCorreos>();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();


    }

    // Update is called once per frame
    void Update()
    {

        //print("total segundos evento:" + totalSegundos + " otro:" + timeControler.totalSegundos);

        totalSegundos = GetTotalSeconds(hora, minutos);

        if (!mensajeEnviado)
        {

            if (dia == timeControler.dia)
            {

                if (totalSegundos <= timeControler.totalSegundos && totalSegundos != 0)
                {
                    eventosCorreos.EnviarCorreo(correoParaEnviar);
                    mensajeEnviado = true;

                    print("check");
                    GameObject instancia = Instantiate(chekCorreo, chekCorreoPosition.transform.position, Quaternion.identity);
                    instancia.transform.SetParent(chekCorreoPosition.transform);
                }

            }

        }

        //if (mensajeEnviado) gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt(gameObject.name + "correoEnviado", System.Convert.ToInt32(mensajeEnviado));
            print("guardarCorreo");
        }
    }

    public float GetTotalSeconds(int horas, int minutos)
    {
        int totalSegundos = horas * 3600 + minutos * 60;
        return totalSegundos;
    }
}
