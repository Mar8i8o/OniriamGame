using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UI;
using TMPro;

public class MovilController : MonoBehaviour
{
    public GameObject seleccionar;

    public GameObject[] mensajes;

    public GameObject panel;
    public GameObject layoutGroup;

    float offsetPanel;
    float offsetPos;

    public float multiplyPanel;
    public float multiplyPos;

    float tiempoMensajeAbierto=0;

    public Raycast cameraScript;
    public CamaraFP playerScript;

    [HideInInspector]public bool menuInicio;

    public GameObject[] todosLosAudios;
    public GameObject[] todosLosContactos;
    GuardarController guardarController;

    public bool encendido;


    private void Awake()
    {
        //AbrirMovil();
        ActivarPantallas();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        todosLosAudios = GameObject.FindGameObjectsWithTag("SonidoSlot");
        todosLosContactos = GameObject.FindGameObjectsWithTag("Contacto");

    }
    void Start()
    {
        CerrarMovil();
        ApagarPantalla();
        camaraMovil.SetActive(false);
        cameraScript = GameObject.Find("Main Camera").GetComponent<Raycast>();
        playerScript = GameObject.Find("Player").GetComponent<CamaraFP>();
        llamadasController = GameObject.Find("GameManager").GetComponent<LlamadasController>();

        if (linternaEndendida)
        {
            linternaTXT.text = "FLASHLIGHT:ON";
            linterna.SetActive(true);
        }
        else
        {
            linternaTXT.text = "FLASHLIGHT:OFF";
            linterna.SetActive(false);
        }

        if (sonido)
        {
            sonidoTXT.text = "SOUND:ON";
        }
        else
        {
            sonidoTXT.text = "SOUND:OFF";
        }
    }


    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            for (int i = 0; i < todosLosAudios.Length; i++)
            {
                todosLosAudios[i].GetComponent<PlaySoundsMovil>().GuardarDatos();
            }
            for (int i = 0; i < todosLosContactos.Length; i++)
            {
                todosLosContactos[i].GetComponent<ContactoController>().GuardarDatos();
            }
        }
    }
    // Update is called once per frame
    public GameObject panelLlamadaManual;
    public GameObject primeraOpcionLlamadaManual;
    void Update()
    {

        if (!cameraScript.usingMovil) return;

        if(panelLlamadaManual.activeSelf)
        {
            if (Input.GetAxis("Vertical") < 0) 
            {
                var eventSystem = EventSystem.current;
                eventSystem.SetSelectedGameObject(primeraOpcionLlamadaManual);
            }
        }
        //var eventSystem = EventSystem.current;
        //eventSystem.SetSelectedGameObject(seleccionar);

        if (cameraScript.usingMovil)
        {

            tiempoPanelSonidos += Time.deltaTime;

            if (Time.frameCount % 60 == 0)
            {
                /*
                mensajes = GameObject.FindGameObjectsWithTag("Mensaje");
                if (mensajes.Length != 0)
                {
                    if (mensajes.Length <= 4)
                    {
                        offsetPanel = 1;
                        panel.GetComponent<RectTransform>().anchorMax = new Vector3(transform.localScale.x, offsetPanel, transform.localScale.z);

                    }
                    else
                    {
                        offsetPanel = 1 + (mensajes.Length - 4) * multiplyPanel;
                        offsetPos = (mensajes.Length - 4) * multiplyPos;
                        panel.GetComponent<RectTransform>().anchorMax = new Vector3(transform.localScale.x, offsetPanel, transform.localScale.z);
                        layoutGroup.GetComponent<RectTransform>().pivot = new Vector3(0, -offsetPos, 0);
                        //verLayoutGroup.padding.top = -offsetPos;
                        //layoutGroup.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + offsetPos, transform.localPosition.z);
                    }
                }
                */
            }

            menuInicio = panelButtons.activeSelf;


                if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.M)) //VOLVER A INICIO
                {

                    if ((menuInicio && !panelDialogsLlamada.activeSelf && !blockApagarMovil) || llamadasController.esperandoLlamada) //DEJA DE FOCUSEAR EL MOVIL, YA SEA DESDE EL INICIO O CUANDO TE LLAMAN
                    {
                        print("Dejar de usar movil");
                        cameraScript.usingMovil = false;
                        cameraScript.tiempoConObjeto = 0;
                        cameraScript.puntero.enabled = true;
                        cameraScript.movimientoDespacio = true;
                        if(!cameraScript.tumbado)playerScript.freeze = false;
                        eventosMensajes.esperandoMensaje = false;
                        if (!llamadasController.esperandoLlamada)
                        {
                            ApagarPantalla();
                            encendido = false;
                        }


                    }

                    if (!llamadasController.esperandoLlamada && !panelDialogsLlamada.activeSelf)
                    {
                        CerrarMovil();
                        AbrirMovil();
                    }

                }

        }
    }

    [HideInInspector]public bool blockApagarMovil;

    public GameObject panelDialogsLlamada;

    public GameObject pantallaEncendido;
    public GameObject pantallaApagado;

    public GameObject canvasMovil;
    public void EncenderPantalla()
    {
        //camaraMovil.SetActive(true);
        //pantallaApagado.SetActive(false);
        //pantallaEncendido.SetActive(true);


        //canvasMovil.SetActive(true);

        print("encender");

        seleccionar.GetComponent<ButtonMovilController>().selecionado = true;

        //CerrarMovil();
        //AbrirMovil();
    }

    public void ApagarPantalla()
    {
        if (!llamadasController.esperandoLlamada)
        {
            //camaraMovil.SetActive(false);
            //pantallaApagado.SetActive(true);
            //pantallaEncendido.SetActive(false);

            //canvasMovil.SetActive(false);

            //print("apagar");
        }
    }

    GameObject[] botonesSonido;
    public AudioSource audioMovil;
    public void DisableAllAudios()
    {
        audioMovil.Stop();
        botonesSonido = GameObject.FindGameObjectsWithTag("SonidoSlot");

        for (int i = 0; i < botonesSonido.Length; i++) 
        {
            botonesSonido[i].GetComponent<PlaySoundsMovil>().Desactivar();
        }
    }

    public void DisableAllRingtones()
    {
        //audioMovil.Stop();
        botonesSonido = GameObject.FindGameObjectsWithTag("SonidoSlot"); 

        for (int i = 0; i < botonesSonido.Length; i++)
        {
            botonesSonido[i].GetComponent<PlaySoundsMovil>().DesactivarRingTone();
        }
    }

    public GameObject cogerLlamadaButton;
    public EventosMensajes eventosMensajes;
    public void AbrirMovil()
    {
        if (llamadasController.esperandoLlamada)
        {
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(cogerLlamadaButton);
            menuInicio = true;
        }
        else if(eventosMensajes.esperandoMensaje)
        {
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(primeraOpcionMensaje);
            menuInicio = true;
            eventosMensajes.esperandoMensaje = false;
        }
        else
        {
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(seleccionar);
            menuInicio = true;
        }
        //EncenderPantalla();
        //encendido = true;
    }

    public GameObject[] panelesMovil;
    public GameObject panelButtons;

    public GameObject camaraMovil;
    public void CerrarMovil()
    {
        for(int i = 0;i < panelesMovil.Length;i++) 
        {
            panelesMovil[i].SetActive(false);
        }
        mensajeAbierto = false;

        panelButtons.SetActive(true);

        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(null);

        
    }

    public void ActivarPantallas()
    {
        for (int i = 0; i < panelesMovil.Length; i++)
        {
            panelesMovil[i].SetActive(true);
        }
    }

    public TextMeshProUGUI horaMensaje;
    public TextMeshProUGUI UsuarioMensaje;
    public TextMeshProUGUI ContenidoMensaje;

    bool mensajeAbierto;

    public GameObject panelMostrarMensaje;
    public GameObject panelMensajes;
    public GameObject primeraOpcionMensaje;

    GameObject mensajeActual;

    public MensajeController mensajeActualController;
    public void AbrirMensaje(string mensaje, string usuario, float hora, GameObject queMensaje, bool elContesta, string idDialogo, string numero, float tiempoEnContestar, MensajeController mensajeController)
    {
        print("AbrirMensaje2");
        contesta = elContesta;
        usuarioActual = usuario;
        idDialogoActual = idDialogo;
        numeroActual = numero;
        tiempoEnContestarActual = tiempoEnContestar;
        
        panelMostrarMensaje.SetActive(true);
        panelMensajes.SetActive(false);

        mensajeActual = queMensaje;
        mensajeActualController = mensajeController;
        ContenidoMensaje.text = mensaje;
        UsuarioMensaje.text = usuario;

        float hour = Mathf.FloorToInt(hora / 3600);
        float minutes = Mathf.FloorToInt(hora / 60);
        minutes = Mathf.FloorToInt(minutes % 60);
        float seconds = Mathf.FloorToInt(hora % 60);

        horaMensaje.text = string.Format("{0:00}:{1:00}", hour, minutes);

        mensajeAbierto = true;
        tiempoMensajeAbierto = 0;

        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(primeraOpcionMensaje);

    }

    public void BorrarMensaje()
    {
        mensajeActual.GetComponent<MensajeController>().active = false;
        mensajeActual.SetActive(false);
    }

    public TextMeshProUGUI usuarioContactoTXT;
    public TextMeshProUGUI numeroUsuario;

    float tiempoEnContestarActual;

    public GameObject panelContactos;
    public GameObject panelContenidoContacto;

    string numeroActual;
    string usuarioActual;
    public string idDialogoActual;

    public ContactoController contactoActual;
    public void AbrirContacto(string usuario, string numero, string idDialogo, bool elcontesta, float tiempoEnContestar, ContactoController contactoController)
    {

        idDialogoActual = idDialogo;
        usuarioActual = usuario;
        numeroActual = numero;
        tiempoEnContestarActual = tiempoEnContestar;
        numeroUsuario.text = numero;
        usuarioContactoTXT.text = usuario;
        contesta = elcontesta;

        contactoActual = contactoController;

        panelContactos.SetActive(false);
        panelContenidoContacto.SetActive(true);
    }

    public LlamadasController llamadasController;
    bool contesta;
    public void Llamar() //SE LLAMA DESDE EL BOTON EN CONTACTOS
    {
        print("Llamar");
        llamadasController.PedirLlamada(idDialogoActual, usuarioActual, numeroActual, contesta, tiempoEnContestarActual, false);

        if (contactoActual != null)
        {
            if (contactoActual.dialogoUnico)
            {
                contactoActual.contesta = false;
            }
        }
        if(mensajeActual != null)
        {
            if(mensajeActualController.constestaUnaVez)
            {
                contesta = false;
            }
        }
    }

    public GameObject primeraOpcionAjustes;
    public GameObject panelOpciones;
    public GameObject linterna;

    public void AbrirAjustes()
    {
        panelOpciones.SetActive(true);
        panelButtons.SetActive(false);
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(primeraOpcionAjustes);
    }

    public bool linternaEndendida;
    public TextMeshProUGUI linternaTXT;
    public TextMeshProUGUI sonidoTXT;
    public void EncenderLinterna()
    {
        if (linternaEndendida)
        {
            linternaEndendida = false;
            linternaTXT.text = "FLASHLIGHT:OFF";
            linterna.SetActive(false);
        }
        else
        {
            linternaEndendida = true;
            linternaTXT.text = "FLASHLIGHT:ON";
            linterna.SetActive(true);
        }

    }

    public bool sonido;
    public void ActivarSonido()
    {
        if (sonido)
        {
            sonidoTXT.text = "SOUND:OFF";
            sonido = false;
        }
        else
        {
            sonidoTXT.text = "SOUND:ON";
            sonido = true;
        }
    }

    [HideInInspector]public float tiempoPanelSonidos;
    public void AbrirPanelSondios()
    {
        tiempoPanelSonidos = 0;
    }
}
