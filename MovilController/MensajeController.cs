using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MensajeController : MonoBehaviour
{
    public string mensaje;
    public string usuario;
    public string numero;

   
    public float totalSegundos;

    public bool activeInicio;
    public bool active;
    public int dia;
    public int mes;

    public bool tieneNombre;
    public string idDialogo;

    public bool contesta;
    public float tiempoEnContestar;
    public bool constestaUnaVez;

    public bool tieneContacto;
    public ContactoController contactoController;

    GuardarController guardarController;
    TimeController timeController;
    EventosMensajes eventosMensajes;

    public bool leido;
    public GameObject punto;

    public Color opacidadNormal;
    public Color opacidadBaja;

    public TextMeshProUGUI mensajeTXT;
    public TextMeshProUGUI usuarioTXT;
    public TextMeshProUGUI horaTXT;

    public MovilController movilControler;
    public ButtonMovilController buttonMovilControler;

    public GameObject panelMensajes;

    private void Awake()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        timeController = GameObject.Find("GameManager").GetComponent<TimeController>();
        eventosMensajes = GameObject.Find("GameManager").GetComponent<EventosMensajes>();
    }
    void Start()
    {
        GetDatos();
        if (!active) gameObject.SetActive(false);

        if (activeInicio && timeController.dia <= 1) { LlegarMensaje(); }

        //buttonMovilControler = GetComponent<ButtonMovilController>();
        //movilControler = GameObject.Find("MovilItem").GetComponent<MovilController>();

        opacidadNormal = new Color(mensajeTXT.color.r, mensajeTXT.color.g, mensajeTXT.color.b, 1);
        opacidadBaja = new Color(mensajeTXT.color.r, mensajeTXT.color.g, mensajeTXT.color.b, 0.5f);

        if(tieneContacto) 
        {
            usuario = contactoController.usuario;
            numero = contactoController.numero;
        }

    }

    public void LlegarMensaje()
    {
        if (tieneContacto)
        {
            contactoController.contesta = contesta;
            contactoController.idDialogo = idDialogo;
        }

        print("MensajesActivos: " + eventosMensajes.ConsultarMensajesActivos());

        gameObject.transform.SetAsLastSibling();

        if(eventosMensajes.ConsultarMensajesActivos() == 1 && panelMensajes.activeSelf)
        {
            print("SelecionarMensajeAux");
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        mensajeTXT.text = mensaje;
        usuarioTXT.text = usuario;

        float hora = Mathf.FloorToInt(totalSegundos / 3600);
        float minutes = Mathf.FloorToInt(totalSegundos / 60);
        minutes = Mathf.FloorToInt(minutes % 60);
        float seconds = Mathf.FloorToInt(totalSegundos % 60);

        horaTXT.text = string.Format("{0:00}:{1:00}", hora, minutes);

        if (leido) 
        {
            punto.SetActive(false);
            usuarioTXT.color = opacidadBaja;
            mensajeTXT.color = opacidadBaja;
            horaTXT.color = opacidadBaja;
        }
        else
        {
            punto.SetActive(true);
            usuarioTXT.color = opacidadNormal;
            mensajeTXT.color = opacidadNormal;
            horaTXT.color= opacidadNormal;
        }
    }

    public void GuardarDatos()
    {
        PlayerPrefs.SetFloat(gameObject.name + "totalSegundos", totalSegundos);
        PlayerPrefs.SetInt(gameObject.name + "dia", dia);
        PlayerPrefs.SetInt(gameObject.name + "mes", mes);
        PlayerPrefs.SetInt(gameObject.name + "active", System.Convert.ToInt32(active));
        PlayerPrefs.SetInt(gameObject.name + "leido", System.Convert.ToInt32(leido));
    }

    public void GetDatos()
    {
        //asunto = PlayerPrefs.GetString(gameObject.name + "asunto", asunto);
        //correo = PlayerPrefs.GetString(gameObject.name + "correo", correo);
        //if (PlayerPrefs.GetInt(gameObject.name + "tieneEnlace", System.Convert.ToInt32(tieneEnlace)) == 0) { tieneEnlace = false; }
        //else if (PlayerPrefs.GetInt(gameObject.name + "tieneEnlace", System.Convert.ToInt32(tieneEnlace)) == 1) { tieneEnlace = true; }

        dia = PlayerPrefs.GetInt(gameObject.name + "dia", dia);
        mes = PlayerPrefs.GetInt(gameObject.name + "mes", mes);
        totalSegundos = PlayerPrefs.GetFloat(gameObject.name + "totalSegundos", totalSegundos);
        if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(activeInicio)) == 0) { active = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(activeInicio)) == 1) { active = true; }

        if (PlayerPrefs.GetInt(gameObject.name + "leido", System.Convert.ToInt32(leido)) == 0) { leido = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "leido", System.Convert.ToInt32(leido)) == 1) { leido = true; }
    }

    private void FixedUpdate()
    {
        if (guardarController.guardando)
        {
            //GuardarDatos();
        }
    }

    private void OnEnable()
    {
        //print("Enabled" + gameObject.transform.GetSiblingIndex());
        /*

        
        
        */

        DetectarSeleccion();

    }

    public void DetectarSeleccion()
    {
        if (gameObject.transform.GetSiblingIndex() == 0)
        {
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(gameObject);
            buttonMovilControler.selecionado = true;
        }
        else if (eventosMensajes.ConsultarMensajesActivos() == 1)
        {
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(gameObject);
            buttonMovilControler.selecionado = true;
        }
    }

    public void AbrirMensaje()
    {
        print("AbrirMensaje");
        leido = true;

        if (tieneContacto)
        {
            movilControler.AbrirMensaje(mensaje, usuario, totalSegundos, gameObject, contactoController.contesta, contactoController.idDialogo, contactoController.numero, contactoController.tiempoEnContestar, gameObject.GetComponent<MensajeController>());
        }
        else
        {
            movilControler.AbrirMensaje(mensaje, usuario, totalSegundos, gameObject, contesta, idDialogo,usuario, 60, gameObject.GetComponent<MensajeController>());
        }
    }


}
