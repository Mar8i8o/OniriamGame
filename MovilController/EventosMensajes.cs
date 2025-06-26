using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventosMensajes : MonoBehaviour
{
    public GameObject[] mensajesSpam;

    public TimeController timeController;

    public float tiempoMensajesSpam;

    int tiempoEntreAnuncios;

    public GameObject[] todosLosMensajes;
    GuardarController guardarController;

    private void Awake()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        todosLosMensajes = GameObject.FindGameObjectsWithTag("Mensaje");
        mensajesOrdenar = GameObject.FindGameObjectsWithTag("Mensaje");
    }
    void Start()
    {
        tiempoEntreAnuncios = 2;

        OrdenarMensajes();
        //PlayerPrefs.DeleteAll();



    }

    void Update()
    {

    }

    public float ConsultarMensajesActivos()
    {
        int mensajesActivos = 0;

        for (int i = 0; i < todosLosMensajes.Length; i++)
        {
            if(todosLosMensajes[i].GetComponent<MensajeController>().active)
            {
                mensajesActivos++;
            }
        }

        return mensajesActivos;
    }


    private void FixedUpdate()
    {
        if (guardarController.guardando)
        {
            for (int i = 0; i < todosLosMensajes.Length; i++) 
            {
                todosLosMensajes[i].GetComponent<MensajeController>().GuardarDatos();
            }
        }
    }

    private void LateUpdate()
    {
        //OrdenarCorreos();
    }
    
    public void ControlarCorreosSpam()
    {
        tiempoMensajesSpam += Time.deltaTime;

        if (tiempoMensajesSpam > tiempoEntreAnuncios)
        {
            int aleatorio = Random.Range(0, mensajesSpam.Length);

            for (int i = 0; i < mensajesSpam.Length; i++)
            {
                if (!mensajesSpam[i].GetComponent<MensajeController>().active)
                {
                    MensajeController mensajeController = mensajesSpam[i].GetComponent<MensajeController>();
                    mensajeController.totalSegundos = timeController.totalSegundos;
                    mensajeController.dia = timeController.dia;
                    mensajeController.active = true;
                    //mensajeController.GuardarDatos();
                    //mensajesSpam[i].transform.SetAsFirstSibling();
                    mensajesSpam[i].SetActive(true);

                    OrdenarMensajes();

                    break;
                }
            }

            tiempoMensajesSpam = 0;

        }
    }

    public SacarMovilController sacarMovilController;
    public MovilController movilController;

    public GameObject buttons;
    public GameObject panelMostrarMensaje;

    public bool esperandoMensaje;

    public void ControlarSelleccionesMensajes()
    {
        for(int i = 0; i < todosLosMensajes.Length; i++) 
        {
            MensajeController mensajeController = todosLosMensajes[i].GetComponent<MensajeController>();

            if(mensajeController.active)
            {
                //mensajeController.DetectarSeleccion();
            }
        }
    }

    public AudioSource sonidoMensaje;

    public void EnviarMensajes(GameObject queMensaje)
    {

        esperandoMensaje = true;

        MensajeController mensajeController = queMensaje.GetComponent<MensajeController>();
        mensajeController.totalSegundos = timeController.totalSegundos;
        mensajeController.dia = timeController.dia+1;
        mensajeController.mes = timeController.mes;
        mensajeController.active = true;
        //mensajeController.GuardarDatos();
        //mensajesSpam[i].transform.SetAsFirstSibling();
        queMensaje.SetActive(true);

        mensajeController.LlegarMensaje();
        movilController.CerrarMovil();
        if(!sacarMovilController.movilSacado) sacarMovilController.SacarMovil();
        if(sacarMovilController.raycast.usingMovil)
        {
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(movilController.primeraOpcionMensaje);
            esperandoMensaje = false;
        }

        mensajeController.AbrirMensaje();
        sonidoMensaje.Play();

        buttons.SetActive(false);


        OrdenarMensajes();
    }

    GameObject[] mensajesOrdenar;
    public GameObject[] mensajesOrdenados;
    public float[] horas;
    public List<float> horasList;
    public void OrdenarMensajes()
    {

        horasList.Clear();

        //mensajesOrdenar = GameObject.FindGameObjectsWithTag("Mensaje");
        mensajesOrdenados = new GameObject[mensajesOrdenar.Length];

        if (mensajesOrdenar.Length > 0)
        {
            List<double> horasList = new List<double>();
            for (int i = 0; i < mensajesOrdenar.Length; i++)
            {

                double totalSegundos = mensajesOrdenar[i].GetComponent<MensajeController>().totalSegundos;
                double dia = mensajesOrdenar[i].GetComponent<MensajeController>().dia;
                double mes = mensajesOrdenar[i].GetComponent<MensajeController>().mes;
                horasList.Add(totalSegundos / 600 + dia * 100 + mes * 100000);

            }

            horasList.Sort();
            horasList.Reverse();

            for (int j = 0; j < mensajesOrdenar.Length; j++)
            {
                for (int i = 0; i < mensajesOrdenar.Length; i++)
                {
                    double totalSegundos = mensajesOrdenar[i].GetComponent<MensajeController>().totalSegundos;
                    double dia = mensajesOrdenar[i].GetComponent<MensajeController>().dia;
                    double mes = mensajesOrdenar[i].GetComponent<MensajeController>().mes;
                    double tiempoNoticia = totalSegundos / 600 + dia * 100 + mes * 100000;

                    if (tiempoNoticia == horasList[j])
                    {
                        //print(horasList[j] + mensajesOrdenar[i].gameObject.name);
                        mensajesOrdenar[i].transform.SetSiblingIndex(j);
                        mensajesOrdenados[j] = mensajesOrdenar[i];
                    }
                }
            }
        }
    }

}
