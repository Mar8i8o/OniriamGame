using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComputerController : MonoBehaviour
{
    public GameObject[] paneles;
    public GameObject panelButtons;

    GuardarController guardarController;
    public GameObject[] botonesDescargarMusica;

    public GameObject pantallaEncendida;
    public GameObject pantallaApagada;
    public GameObject camaraComputer;

    public bool OniriamDesbloqueado;
    public GameObject buttonOniriam;

    public GameObject panelLogin;
    public TMP_InputField inputLogin;

    public MouseController mouseController;

    public string contrasena;

    PensamientoControler pensamientoControler;

    public GameObject icoDejarDeUsar;
    public GameObject icoLevantarse;

    private void Awake()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        botonesDescargarMusica = GameObject.FindGameObjectsWithTag("ButtonAudio");

        if (PlayerPrefs.GetInt("oniriamDesbloqueado", System.Convert.ToInt32(OniriamDesbloqueado)) == 0) { OniriamDesbloqueado = false; }
        else { OniriamDesbloqueado = true; }

        for (int i = 0; i < paneles.Length; i++)
        {
            paneles[i].gameObject.SetActive(true);
        }

        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();

    }
    void Start()
    {
        ApagarOrdenador();

        Invoke(nameof(CerrarPaneles), 1f);
        panelLogin.SetActive(true);
        panelButtons.SetActive(false);
    }

    public float tiempoPcEncendido;

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            for (int i = 0; i < botonesDescargarMusica.Length; i++) 
            {
                botonesDescargarMusica[i].GetComponent<DownloadSong>().GuardarDatos();
            }
            PlayerPrefs.SetInt("oniriamDesbloqueado", System.Convert.ToInt32(OniriamDesbloqueado));
        }

        buttonOniriam.SetActive(OniriamDesbloqueado);
    }

    public void DesactivarSonidos()
    {
        for (int i = 0; i < botonesDescargarMusica.Length; i++)
        {
            botonesDescargarMusica[i].GetComponent<DownloadSong>().CancelarPlay();
        }
    }

    private void Update()
    {
        if (pantallaEncendida.activeSelf)
        {
            tiempoPcEncendido += Time.deltaTime;
        }

        if(panelLogin.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                ComprobarContrasena();
            }
        }
    }

    public void CerrarPaneles()
    {
        for (int i = 0; i < paneles.Length; i++)
        {
            paneles[i].gameObject.SetActive(false);
        }
        //panelButtons.gameObject.SetActive(true);
    }

    public void EncenderOrdenador()
    {
        pantallaEncendida.SetActive(true);
        pantallaApagada.SetActive(false);
        camaraComputer.SetActive(true);
        tiempoPcEncendido = 0;
        inputLogin.text = "";

        icoLevantarse.SetActive(false);
        icoDejarDeUsar.SetActive(true);

    }


    public void ApagarOrdenador()
    {
        pantallaEncendida.SetActive(false);
        pantallaApagada.SetActive(true);
        camaraComputer.SetActive(false);
        tiempoPcEncendido = 0;

        DesactivarSonidos();


        icoLevantarse.SetActive(true);
        icoDejarDeUsar.SetActive(false);

    }

    public void ComprobarContrasena()
    {
        if(inputLogin.text.ToLower() == contrasena)
        {
            mouseController.NormalCursor();
            panelButtons.gameObject.SetActive(true);
            panelLogin.SetActive(false);
        }
        else
        {
            inputLogin.text = "";
            if(!pensamientoControler.mostrandoPensamiento)pensamientoControler.MostrarPensamiento("La contraseña es mi nombre y mi año de nacimiento", 3);
        }
    }

}
