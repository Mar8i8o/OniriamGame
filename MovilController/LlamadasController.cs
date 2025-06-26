using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LlamadasController : MonoBehaviour
{

    #region VARIABLES

    public GameObject panelDialogoLlamada;
    public JSONConverter dialogScript;
    public DialogeController dialogeController;

    public GameObject camara;
    public CamaraFP camaraScript;

    public bool llamadaActiva;
    string idDialogo;

    public GameObject panelLlamando;
    public GameObject panelLlamada;
    public GameObject panelContactos;
    public GameObject panelInicial;
    public GameObject panelTeLlaman;

    public bool esperandoLlamada;
    public bool teLllaman;
    public float tiempoLlamada;

    public MovilController movilController;
    public AudioSource sonidoLlamada;
    Raycast playerScript;
    public float tiempoEsperandoLlamada;

    PoliciaController policiaController;

    public TextMeshProUGUI usuarioLlamandoTXT;
    public TextMeshProUGUI usuarioLlamadaTXT;
    public TextMeshProUGUI tiempoTXT;
    public string usuarioLlamada;
    public float tiempoProximaLlamadaPrueba;
    bool llamadaHistoriaPrueba;
    int momentoHistoriaPrueba = 0;

    PensamientoControler pensamientoControler;
    public bool tienePensamiento;
    public string pensamiento;

    #endregion

    void Start()
    {
        policiaController = GameObject.Find("GameManager").GetComponent<PoliciaController>();
        playerScript = GameObject.Find("Main Camera").GetComponent<Raycast>();
        sacarMovilController = GameObject.Find("Player").GetComponent<SacarMovilController>();
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();
    }

    [HideInInspector] public bool esperaLlamadaInfinita;

    void Update()
    {
        if(!llamadaActiva && !esperandoLlamada)ControlarLlamadas();
        if (!panelLlamarManual.activeSelf) { barraIntroducirNumero.text = ""; }
        //if (llamadaActiva) camara.transform.LookAt(npc.transform.position);
        if (esperandoLlamada && playerScript.usingMovil && !bloquearColgar)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1)) { ColgarLlamada(); }
        }

        if (llamadaActiva)
        {
            tiempoLlamada += Time.deltaTime;

            float hora = Mathf.FloorToInt(tiempoLlamada / 3600);
            float minutes = Mathf.FloorToInt(tiempoLlamada / 60);
            minutes = Mathf.FloorToInt(minutes % 60);
            float seconds = Mathf.FloorToInt(tiempoLlamada % 60);

            //temporizadorTMP.text = string.Format("{0:00}:{1:00}:{2:00}", hora, minutes, seconds);
            tiempoTXT.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (esperandoLlamada && !esperaLlamadaInfinita)
        {
            tiempoEsperandoLlamada += Time.deltaTime;

            if (tiempoEsperandoLlamada > 60)
            {
                ColgarLlamada();
                esperandoLlamada = false;
                tiempoEsperandoLlamada = 0;
                if (!playerScript.usingMovil) { movilController.ApagarPantalla(); }
            }
        }
    }

    #region AJUSTES_LLAMADA_PRINCIPAL

    public void PedirLlamada(string idContacto, string nameUsr, string numero, bool contesta, float tiempoEnContestar, bool hasAction) //INICIAR ESTAS LLAMANDO TU
    {

        idDialogo = idContacto;


        if (nameUsr != "") { usuarioLlamada = nameUsr; }
        else { usuarioLlamada = numero; }

        usuarioLlamandoTXT.text = usuarioLlamada;

        if (contesta) { Invoke(nameof(IniciarDialogoLlamada), tiempoEnContestar); }
        else { Invoke(nameof(ColgarLlamada), tiempoEnContestar); }

        esperandoLlamada = true;
        panelLlamando.SetActive(true);


    }
    public void GenerarRecibirLlamada(string thisusuarioLlamada, string thisidDialogo) //TE LLAMAN
    {
        usuarioLlamada = thisusuarioLlamada;
        idDialogo = thisidDialogo;
        RecibirLlamada();
        //llamadaHistoriaPrueba = true;
        //momentoHistoriaPrueba++;

        //tiempoProximaLlamadaPrueba = 0;
    }


    #endregion

    ////////////

    void IniciarDialogoLlamada() ////INICIAR DIALOGO LLAMADA
    {

        if (!movilController.encendido) { ColgarLlamada(); return; }

        tiempoEsperandoLlamada = 0;
        sonidoLlamada.Stop();
        panelTeLlaman.SetActive(false);
        esperandoLlamada = false;
        tiempoLlamada = 0;
        usuarioLlamadaTXT.text = usuarioLlamada;

        panelLlamando.SetActive(false);
        panelLlamada.SetActive(true);
        esperandoLlamada = false;
        dialogScript.idDialogo = idDialogo;
        panelDialogoLlamada.SetActive(true);
        dialogScript.EmpezarDialogo();
        dialogeController.EmpezarDialogos();


        camaraScript.freezeCamera = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        llamadaActiva = true;

    }

    #region BOTONES

    public void LlamarManual() //HACES TU LA LLAMADA MARCANDO EL NUMERO
    {
        usuarioLlamada = barraIntroducirNumero.text;
        if (barraIntroducirNumero.text == "112")
        {
            thisContesta = true;
            if (policiaController.puedesLlamarEmergencias)
            {
                if (policiaController.numVecesLlamadoPolicia == 0)
                {
                    idDialogo = "emergencias_1"; //TE CONTESTAN A BUENAS
                }
                else if (policiaController.numVecesLlamadoPolicia == 1)
                {
                    idDialogo = "emergencias_3"; //TE DICEN QUE ES LA ULTIMA VEZ QUE LLAMAS
                }
                else if (policiaController.numVecesLlamadoPolicia > 1)
                {
                    idDialogo = "emergencias_5"; //ULTIMA LLAMADA
                }
            }
            else
            {
                idDialogo = "emergencias_2"; //TE DICEN QUE YA HAN ENVIADO A ALGUIEN
            }
            tiempoEnContestarActual = 2;
        }
        else
        {
            thisContesta = false;
            tiempoEnContestarActual = 5;
        }
        PedirLlamada(idDialogo, usuarioLlamada, "0", thisContesta, tiempoEnContestarActual, false);
        panelLlamarManual.SetActive(false);
    }

    public void Back()
    {
        if (!panelDialogoLlamada.activeSelf)
        {
            esperandoLlamada = false;
            //panelDialogoLlamada.SetActive(false);
            panelLlamando.SetActive(false);
            llamadaActiva = false;
            ReactivarMovimiento();
            CancelInvoke(nameof(IniciarDialogoLlamada));
            //panelContactos.SetActive(false);
            //panelInicial.SetActive(true);

            //movilController.CerrarMovil();
            //movilController.AbrirMovil();
        }

    }

    public void CogerLlamada()
    {
        panelTeLlaman.SetActive(false);
        movilController.CerrarMovil();
        IniciarDialogoLlamada();
    }

    public void ButtonColgarLlamada()
    {
        if(bloquearColgar)
        {
            CogerLlamada();
        }
        else
        {
            ColgarLlamada();
        }
    }


    #endregion

    public void ControlarLlamadas()
    {
        if (momentoHistoriaPrueba == 0)
        {

            //tiempoProximaLlamadaPrueba += Time.deltaTime;

            if (tiempoProximaLlamadaPrueba > 10)
            {


                usuarioLlamada = "Paco";
                idDialogo = "emergencias_1";
                RecibirLlamada();
                llamadaHistoriaPrueba = true;
                //momentoHistoriaPrueba++;

                tiempoProximaLlamadaPrueba = 0;

            }

        }
    } ///PROVISIONAL

    public TextMeshProUGUI usuarioTeLlamaTXT;
    public GameObject camaraMovil;

    SacarMovilController sacarMovilController;
    void RecibirLlamada() 
    {
        if(!sacarMovilController.movilSacado)sacarMovilController.SacarMovil();
        movilController.eventosMensajes.esperandoMensaje = false;
        usuarioTeLlamaTXT.text = "" + usuarioLlamada;
        esperandoLlamada = true;
        movilController.CerrarMovil();
        //movilController.AbrirMovil();
        panelInicial.SetActive(false);
        panelTeLlaman.SetActive(true);

        movilController.DisableAllAudios();
        sonidoLlamada.Play();

        tiempoEsperandoLlamada = 0;
        movilController.EncenderPantalla();
        movilController.blockApagarMovil = true;
    }

    public TMP_InputField barraIntroducirNumero;
    public GameObject panelLlamarManual;
    bool thisContesta;
    float tiempoEnContestarActual;

    public bool bloquearColgar;

    public void ColgarLlamada()
    {

        print("ColgarLlamada");
        panelTeLlaman.SetActive(false);
        esperandoLlamada = false;
        movilController.CerrarMovil();
        movilController.AbrirMovil();

        sonidoLlamada.Stop();

         tiempoEsperandoLlamada = 0;

         esperandoLlamada = false;
         panelDialogoLlamada.SetActive(false);
         panelLlamando.SetActive(false);
         llamadaActiva = false;
         ReactivarMovimiento();
         CancelInvoke(nameof(IniciarDialogoLlamada));

         Invoke(nameof(UnblockUsingMovil), 0.1f);

         movilController.CerrarMovil();
         movilController.AbrirMovil();

         


    }
    public void UnblockUsingMovil()
    {
        movilController.blockApagarMovil = false;
    }
    public void ReactivarMovimiento()
    {
        //ColgarLlamada();
        AvanzarMomentoHistoria();
        CancelarLlamadaHistoria();

        // COLGAR

        panelTeLlaman.SetActive(false);
        esperandoLlamada = false;

        movilController.CerrarMovil();
        movilController.AbrirMovil();

        sonidoLlamada.Stop();

        tiempoEsperandoLlamada = 0;

        //
        camaraScript.freezeCamera = false;
        //camaraScript.freeze = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        llamadaActiva = false;

        movilController.blockApagarMovil = false;

        bloquearColgar = false;

        if (tienePensamiento)
        {
            pensamientoControler.MostrarPensamiento(pensamiento, 2);
            tienePensamiento = false;
        }
    }


    #region HISTORIA

    public void CancelarLlamadaHistoria()
    {
        llamadaHistoriaPrueba = false;
    }

    public void AvanzarMomentoHistoria()
    {
        if (llamadaHistoriaPrueba) momentoHistoriaPrueba++;
    }

    #endregion
}
