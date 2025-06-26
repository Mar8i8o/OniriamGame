using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlarmController : MonoBehaviour
{
    public bool usandoAlarma;
    public GameObject camara;
    public GameObject mainCamara;
    public GameObject posCam;

    public CamaraFP camaraScript;

    public GameObject interfaz;

    public float tiempoUsandoAlarma;
    float tiempoPress;

    float multiplicador;

    public GameObject alarmaOn;
    public GameObject alarmaOff;

    public GameObject interfazSetAlarm;

    TimeController timeController;

    public TextMeshProUGUI horaTxt;
    public TextMeshProUGUI alarmTxt;

    public GameObject horaTXTOBJ;
    public GameObject alarmTXTOBJ;

    public GameObject alarmIndicator;
    public TextMeshProUGUI alarmIndicatorText;

    public AlarmaEventsManager alarmaEventsManager;

    GameObject player;
    Raycast raycast;
    private void Awake()
    {
        timeController = GameObject.Find("GameManager").GetComponent<TimeController>();
        //alarmaEventsManager = GameObject.Find("GameManager").GetComponent<AlarmaEventsManager>();
        player = GameObject.Find("Player");
        camaraScript = player.GetComponent<CamaraFP>();
        camara = GameObject.Find("CameraParent");
        mainCamara = GameObject.Find("Main Camera");
        raycast = mainCamara.GetComponent<Raycast>();
    }
    void Start()
    {
        ApagarAnim();
    }

    void Update()
    {
        ActualizarHora();
        PintarAlarma();

        if (usandoAlarma)
        {
            camara.transform.position = Vector3.MoveTowards(camara.transform.position, posCam.transform.position, 5f * Time.deltaTime);
            mainCamara.transform.LookAt(transform.position);

            tiempoUsandoAlarma += Time.deltaTime;

            if (tiempoUsandoAlarma > 0.1f)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (alarmaEventsManager.alarmaActiva)
                    {
                        alarmaEventsManager.alarmaActiva = false;
                    }
                    else if (!alarmaEventsManager.alarmaActiva)
                    {
                        alarmaEventsManager.alarmaActiva = true;
                    }

                }
                if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.C))
                {
                    SalirAlarma();

                }
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    tiempoPress = 0;
                    multiplicador = 5;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    tiempoPress = 0;
                    multiplicador = 5;
                }

                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                {
                    alarmaEventsManager.totalSegundosAlarma += Time.deltaTime * 100 * multiplicador;
                    tiempoPress += Time.deltaTime;

                    if (alarmaEventsManager.totalSegundosAlarma > 86400) { alarmaEventsManager.totalSegundosAlarma -= 86400; }
                    if (alarmaEventsManager.totalSegundosAlarma < 0) alarmaEventsManager.totalSegundosAlarma += 86399;

                    if (tiempoPress > 4)
                    {
                        multiplicador = 100;
                    }
                    else if (tiempoPress > 2) 
                    {
                        multiplicador = 10;
                    }
                }
                if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                {
                    alarmaEventsManager.totalSegundosAlarma -= Time.deltaTime * 100 * multiplicador;
                    tiempoPress += Time.deltaTime;

                    if (alarmaEventsManager.totalSegundosAlarma > 86400) { alarmaEventsManager.totalSegundosAlarma -= 86400;}
                    if (alarmaEventsManager.totalSegundosAlarma < 0) alarmaEventsManager.totalSegundosAlarma += 86399;

                    if (tiempoPress > 4)
                    {
                        multiplicador = 100;
                    }
                    else if (tiempoPress > 2)
                    {
                        multiplicador = 10;
                    }
                }
            }
        }    

        alarmaOn.SetActive(alarmaEventsManager.alarmaActiva);
        alarmaOff.SetActive(!alarmaEventsManager.alarmaActiva);

        if (usandoAlarma)
        {
            interfazSetAlarm.SetActive(true);
            horaTXTOBJ.SetActive(false);
            alarmTXTOBJ.SetActive(true);
            alarmIndicator.SetActive(false);
        }
        else
        {
            interfazSetAlarm.SetActive(false);
            horaTXTOBJ.SetActive(true);
            alarmTXTOBJ.SetActive(false);

            if (alarmaEventsManager.alarmaActiva) { alarmIndicator.SetActive(true); }
            else { alarmIndicator.SetActive(false); }
        }

    }

    public AudioSource audioAlarma;
    public bool sonandoAlarma;
    public Animator animacionLuz;
    public void SonarAlarma()
    {

        EncenderAnim();

        audioAlarma.Play();
        sonandoAlarma = true;

        animacionLuz.SetBool("parpadeo", sonandoAlarma);

    }

    public GameObject luzAlarmaOn;
    public GameObject luzAlarmaOff;
    public Light luzAlarma;
    public void PararAlarma()
    {
        audioAlarma.Stop();
        sonandoAlarma = false;

        animacionLuz.SetBool("parpadeo", sonandoAlarma);

        luzAlarmaOn.SetActive(false);
        luzAlarmaOff.SetActive(true);
        luzAlarma.enabled = false;

        Invoke(nameof(ApagarAnim), 1);

    }

    float totalSegundos;
    public void ActualizarHora()
    {
        //temporizadorTMP.text = string.Format("{0:00}:{1:00}:{2:00}", hora, minutes, seconds);
        horaTxt.text = string.Format("{0:00}:{1:00}", timeController.hora, timeController.minutes);
    }

    public void PintarAlarma()
    {
        float hora = Mathf.FloorToInt(alarmaEventsManager.totalSegundosAlarma / 3600);
        float minutes = Mathf.FloorToInt(alarmaEventsManager.totalSegundosAlarma / 60);
        minutes = Mathf.FloorToInt(minutes % 60);
        float seconds = Mathf.FloorToInt(alarmaEventsManager.totalSegundosAlarma % 60);

        //temporizadorTMP.text = string.Format("{0:00}:{1:00}:{2:00}", hora, minutes, seconds);
        alarmTxt.text = string.Format("{0:00}:{1:00}", hora, minutes);
        alarmIndicatorText.text = string.Format("{0:00}:{1:00}", hora, minutes);
    }

    public void UsarAlarma()
    {
        if (!sonandoAlarma)
        {
            if (!raycast.tumbado)
            {

                /*
                usandoAlarma = true;

                interfaz.SetActive(false);

                camaraScript.freeze = true;
                camaraScript.freezeCamera = true;

                camaraScript.freeze = true;
                camaraScript.freezeCamera = true;

                tiempoUsandoAlarma = 0;

                raycast.enabled = false;

                */

            }
        }
        else
        {
            PararAlarma();
        }

    }

    public void SalirAlarma()
    {
        usandoAlarma = false;

        interfaz.SetActive(true);

        camaraScript.freeze = false;
        camaraScript.freezeCamera = false;

        camaraScript.freeze = false;
        camaraScript.freezeCamera = false;

        tiempoUsandoAlarma = 0;

        raycast.enabled = true;
        //camara.transform.SetParent(player.transform);
    }

    void ApagarAnim()
    {
        animacionLuz.enabled = false;
    }

    void EncenderAnim()
    {
        CancelInvoke(nameof(ApagarAnim));
        animacionLuz.enabled = true;
    }
}
