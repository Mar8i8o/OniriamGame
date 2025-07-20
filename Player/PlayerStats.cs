using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.HighDefinition;

public class PlayerStats : MonoBehaviour
{
    public float hambre;
    public Image barraHambre;

    public float agua;
    public Image barraAgua;

    public float pis;
    public Image barraPis;

    public float cansancio;
    public Image barraCansancio;

    public float dinero;

    public float locura;
    public float locuraMomentanea;
    public bool enemigoCerca;

    public float fumada;
    public float pastillas;

    [HideInInspector] public float tiempoParaCorregirLocura;

    public CharcosSpawner charcosSpawner;
    PensamientoControler pensamientoControler;
    Raycast playerScript;
    CamaraFP camaraFP;
    CamaController camaController;
    ParpadeoController parpadeoController;
    TimeController timeController;

    GameObject player;
    Rigidbody rb;
    GuardarController guardarController;
    public GameObject canvas;

    public Volume volume;
    public FilmGrain grain;

    public ChromaticAberration chromaticAberration;
    public LensDistortion lensdistortion;

    DreamController dreamController;

    public bool paralisis;
    public float cantidadParalisis;

    BrazoController brazoConReloj;

    public bool muetraPensamientos;
    public bool actualizaStats;

    public bool puedeHacerPis;

    GameObject posicionPis;
    Vector3 pisPositionInicial;

    AudioSource rugidoEstomagoSound;

    bool pensamientoCansancio;
    bool pensamientoCansancio2;
    bool pensamientoPis;
    bool pensamientoPis2;
    bool pensamientoComida;
    bool pensamientoComida2;
    bool pensamientoSed;

    public TriggerSonido triggerInteriorCasa;

    Vector3 rotacionInicial;

    public int pulsaciones;
    public bool nervioso;
    public bool panico;

    float tiempoActualizarPulsaciones;

    private void Start()
    {
        posicionPis = GameObject.Find("PisPosition");
        dreamController = GetComponent<DreamController>();
        guardarController = GetComponent<GuardarController>();
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();
        playerScript = GameObject.Find("Main Camera").GetComponent<Raycast>();
        camaraFP = GameObject.Find("Player").GetComponent<CamaraFP>();
        player = GameObject.Find("Player");
        controlarCaerse = player.GetComponent<ControlarCaerse2>();

        if (!dreamController.inDream)
            camaController = GameObject.Find("CamaTrigger").GetComponent<CamaController>();

        parpadeoController = GetComponent<ParpadeoController>();

        if (!dreamController.inDream)
            rugidoEstomagoSound = GameObject.Find("RugidoEstomagoSound").GetComponent<AudioSource>();

        rb = player.GetComponent<Rigidbody>();

        if (!dreamController.inDream)
            brazoConReloj = GameObject.Find("BrazoConReloj").GetComponent<BrazoController>();

        if (!dreamController.inDream)
            timeController = GameObject.Find("GameManager").GetComponent<TimeController>();

        pulsaciones = 90;
        //ControlarPulsaciones();

        volume.profile.TryGet<FilmGrain>(out grain);
        volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        volume.profile.TryGet<LensDistortion>(out lensdistortion);

        pisPositionInicial = posicionPis.transform.localPosition;

        if (!dreamController.inDream)
        {
            pis = PlayerPrefs.GetFloat("Pis", pis);
            //cansancio = PlayerPrefs.GetFloat("Cansancio", cansancio);
            cansancio = 0;
            hambre = PlayerPrefs.GetFloat("Hambre", hambre);
            agua = PlayerPrefs.GetFloat("Agua", agua);
            dinero = PlayerPrefs.GetFloat("Dinero", dinero);
        }

        locura = PlayerPrefs.GetFloat("Locura", locura);
    }

    private void Update()
    {
        VolumeControl();

        if (cantidadParalisis > 0)
            ControlParalisis();

        if (actualizaStats && !dreamController.inDream)
            ActualizarStats();

        if (Time.frameCount % 30 == 0 && !dreamController.inDream)
        {
            ControlarPensamientos();
            PintarBarras();
            ControlarPuedeHacerPis();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerPrefs.DeleteAll();
            pensamientoControler.MostrarPensamiento("BorrarPlayerPrefs", 1);
        }
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            if (!dreamController.inDream)
            {
                PlayerPrefs.SetFloat("Pis", pis);
                PlayerPrefs.SetFloat("Cansancio", cansancio);
                PlayerPrefs.SetFloat("Hambre", hambre);
                PlayerPrefs.SetFloat("Agua", agua);
                PlayerPrefs.SetFloat("Dinero", dinero);
            }

            PlayerPrefs.GetFloat("Locura", locura);
        }
    }

    public void ControlarPuedeHacerPis()
    {
        //puedeHacerPis = triggerInteriorCasa.isInTrigger;
        puedeHacerPis = player.transform.position.y > 1 && player.transform.position.y < 1.05f;
    }

    public void ActualizarStats()
    {
        hambre = Mathf.Clamp(hambre, 0, 100);
        agua = Mathf.Clamp(agua, 0, 100);
        fumada = Mathf.Clamp(fumada, 0, 50);

        hambre -= Time.deltaTime * 0.12f;
        agua -= Time.deltaTime * 0.12f;

        if (cansancio >= 100)
        {
            cansancio = 0;
            if (camaController.usandoCama)
            {
                camaController.EmpezarDormirse();
            }
            else
            {
                NoquearPlayer();
            }
        }
        else if (cansancio >= 0)
        {
            if (timeController.hora >= 2 && timeController.diaDormido == 0)
            {
                cansancio += Time.deltaTime * 1f;
                print("Cansarse muy rapido");
            }
            else
            {
                cansancio += Time.deltaTime * 0.18f;
            }
        }

        fumada -= Time.deltaTime * 0.3f;

        pis += Time.deltaTime * 0.12f;

        if (pis > 100)
        {
            ControlarPuedeHacerPis();

            if (puedeHacerPis)
            {
                pis = 0;

                if (!playerScript.tumbado)
                {
                    charcosSpawner.SpawnearCharco();
                    pensamientoControler.MostrarPensamiento("Vaya, me he meado encima...", 5f);
                }
                else
                {
                    camaController.estaMeada = true;
                    pensamientoControler.MostrarPensamiento("Vaya, me he meado en la cama...", 5f);
                }
            }
        }
        else if (pis < -10)
        {
            pis = -10;
        }

        if (enemigoCerca)
        {
            tiempoParaCorregirLocura += Time.deltaTime;
            if (tiempoParaCorregirLocura > 4)
                enemigoCerca = false;
        }
        else if (!enemigoCerca && locuraMomentanea > 0)
        {
            locuraMomentanea -= Time.deltaTime * 40;
        }
    }

    public void PintarBarras()
    {
        barraHambre.fillAmount = hambre / 100f;
        barraAgua.fillAmount = agua / 100f;
        barraCansancio.fillAmount = cansancio / 100f;
        barraPis.fillAmount = pis / 100f;
    }

    ControlarCaerse2 controlarCaerse;
    [HideInInspector]public bool noqueandose;
    public void NoquearPlayer()
    {
        if (noqueandose) return;

        noqueandose = true;
        rotacionInicial = player.transform.eulerAngles;

        camaraFP.GuardarController();
        camaraFP.enabled = false;

        if (!playerScript.sentado)
        {
            //player.GetComponent<CharacterController>().enabled = false;
            //rb.isKinematic = false;
            //rb.AddForce(transform.forward * 1, ForceMode.Impulse);
            controlarCaerse.Desmayarse();
        }

        canvas.SetActive(false);
        brazoConReloj.puedeSacarBrazo = false;
        parpadeoController.spawnAbriendoOjos = true;

        Invoke(nameof(CerrarOjos), 1f);
        Invoke(nameof(ActivarGuardar), 7f);
        Invoke(nameof(Dormirse), 10f);
    }

    public void ActivarGuardar()
    {
        guardarController.Guardar();
    }

    public void Dormirse()
    {
        dreamController.Dormirse();
    }

    public void CerrarOjos()
    {
        parpadeoController.beingKO = true;
        parpadeoController.SetCerrarOjos(200);
    }

    public void ControlarPensamientos()
    {

        if (pensamientoControler.mostrandoPensamiento) { return; }

        if (!camaController.durmiendose)
        {
            if (cansancio > 80 && !pensamientoCansancio)
            {
                if (muetraPensamientos)
                    pensamientoControler.MostrarPensamiento("Estoy cansado... deberia ir a dormir", 1);

                pensamientoCansancio = true;

                parpadeoController.cerrandoOjos = true;
                parpadeoController.velocidad = 1;
            }
            else if (cansancio > 90 && !pensamientoCansancio2)
            {
                parpadeoController.cerrandoOjos = true;
                parpadeoController.velocidad = 5;
            }
            else if (cansancio < 79)
            {
                pensamientoCansancio = false;
                pensamientoCansancio2 = false;
            }
        }

        if (pis > 70 && !pensamientoPis)
        {
            if (muetraPensamientos)
                pensamientoControler.MostrarPensamiento("Necesito ir a mear", 1);

            pensamientoPis = true;
        }
        else if (pis > 90 && !pensamientoPis2)
        {
            if (muetraPensamientos)
                pensamientoControler.MostrarPensamiento("No aguanto, tengo que ir a mear", 1);

            pensamientoPis2 = true;
            camaraFP.speed = camaraFP.initialSpeed / 2f;
        }
        else if (pis < 70)
        {
            camaraFP.speed = camaraFP.initialSpeed;
            pensamientoPis = false;
            pensamientoPis2 = false;
        }

        if (hambre < 20 && !pensamientoComida)
        {
            if (muetraPensamientos)
                pensamientoControler.MostrarPensamiento("Tengo hambre... deberia comer algo", 1);

            pensamientoComida = true;
        }
        else if (hambre < 10 && !pensamientoComida2)
        {
            if (muetraPensamientos)
                pensamientoControler.MostrarPensamiento("Tengo mucha hambre", 1);

            rugidoEstomagoSound.Play();
            pensamientoComida2 = true;
        }
        else if (hambre > 21)
        {
            pensamientoComida = false;
            pensamientoComida2 = false;
            rugidoEstomagoSound.Stop();
        }

        if (agua < 20 && !pensamientoSed)
        {
            if (muetraPensamientos)
                pensamientoControler.MostrarPensamiento("Tengo sed... deberia beber algo", 1);

            pensamientoSed = true;
        }
        else if (agua > 21)
        {
            pensamientoSed = false;
        }
    }

    public void ControlParalisis()
    {
        grain.intensity.value = cantidadParalisis / 100f;
        chromaticAberration.intensity.value = cantidadParalisis / 100f;

        if (!paralisis)
        {
            cantidadParalisis -= Time.deltaTime * 5;
        }
    }

    public void VolumeControl()
    {
        if (!paralisis && locuraMomentanea > 0)
        {
            grain.intensity.value = locura / 100f + locuraMomentanea / 100f;
        }

        if(fumada > 0)
        {
            lensdistortion.intensity.value = fumada / 100f;
            chromaticAberration.intensity.value = fumada / 100f;
        }
    }

    public void ControlarPulsaciones()
    {
        float aleatorio;
        if (!nervioso && !panico)
        {
            aleatorio = Random.Range(90, 100 + locura / 2);
            tiempoActualizarPulsaciones = 2f;
        }
        else if (panico)
        {
            aleatorio = Random.Range(200, 250 + locura / 2);
            tiempoActualizarPulsaciones = 0.4f;
        }
        else // nervioso || fumada > 10
        {
            aleatorio = Random.Range(120, 130 + locura / 2);
            tiempoActualizarPulsaciones = 0.6f;
        }

        if (pulsaciones <= aleatorio)
            pulsaciones++;
        else if (pulsaciones > aleatorio)
            pulsaciones--;

        Invoke(nameof(ControlarPulsaciones), tiempoActualizarPulsaciones);
    }

    [ContextMenu(itemName: "Delete_PlayerPrefs")]
    public void deletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All player prefs deleted");
    }
}
