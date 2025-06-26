using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LluviaController : MonoBehaviour
{

    public string[] climaID;

    public OpacidadNube nubeNormal;
    public OpacidadNube[] nubesCielo;

    public bool nublado;
    public bool lloviendo;

    public float speed;

    public ParticleSystem particulasLluvia;

    public Material materialGrava;
    public float metallic;

    public AudioSource audioSourceActual;

    public AudioSource audioSourceInterior;
    public AudioSource audioSourceExterior;

    float startVolume;

    public TriggerSonido triggerInteriorCasa;

    TimeController timeController;

    public bool noche;

    public float fogDensity;

    public string idDiaActual;

    private void Awake()
    {
        //nubesCielo[0].gameObject.SetActive(true);
        //nubesCielo[1].gameObject.SetActive(true);
        nubesCielo[2].gameObject.SetActive(true);
        nubesCielo[3].gameObject.SetActive(true);
        //nubeNormal.gameObject.SetActive(true);
    }
    void Start()
    {

        timeController = GameObject.Find("GameManager").GetComponent<TimeController>();

        audioSourceActual.volume = 0;

        idDiaActual = climaID[timeController.dia - 1];

        if(idDiaActual == "Soleado")
        {
            nublado = false;
            lloviendo = false;
        }
        else if(idDiaActual == "Nublado")
        {
            nublado = true;
            lloviendo = false;
        }
        else if (idDiaActual == "Lluvia")
        {
            nublado = true;
            lloviendo= true;
        }

        if (!lloviendo)
        {
            audioSourceInterior.volume = 0;
            audioSourceExterior.volume = 0;
        }
    }
    private void Update()
    {

        DetectarDia();

        materialGrava.SetFloat("_Metallic", metallic);

        if(triggerInteriorCasa.isInTrigger)
        {
            audioSourceActual = audioSourceInterior;
        }
        else
        {
            audioSourceActual = audioSourceExterior;
        }

        if (lloviendo)
        {
            nublado = true;
            if(!particulasLluvia.isEmitting)particulasLluvia.Play();

            if(metallic < 1) metallic += Time.deltaTime * speed;

            if(!audioSourceActual.isPlaying) { audioSourceActual.Play(); }
            triggerInteriorCasa.blockCambiaAudio = false;
            SubirVolumen();

        }
        else
        {
            if (particulasLluvia.isEmitting)
            { 
                particulasLluvia.Stop();
                audioSourceInterior.volume = 0;
                audioSourceExterior.volume = 0;
            }
            if (metallic > 0) metallic -= Time.deltaTime;
            triggerInteriorCasa.blockCambiaAudio = true;

            BajarVolumen();

        }

        ControlNubes();
    }

    public GameObject[] ventanas;
    public bool ventanasActivas;

    public void ActivarVentanas()
    {
        print("Activar ventanas");
        StartCoroutine(ActivarVentanasSecuencial());
        ventanasActivas = true;
    }

    private IEnumerator ActivarVentanasSecuencial()
    {
        foreach (GameObject ventana in ventanas)
        {
            ventana.SetActive(true);
            yield return new WaitForSeconds(5f);
        }
    }

    public void DetectarDia()
    {
        if (timeController.hora >= 19 || timeController.hora <= 6)
        {
            noche = true;
            if(!ventanasActivas)
            {
                ActivarVentanas();
            }
            //if (fogDensity > 0) fogDensity -= Time.deltaTime * 0.01f;
        }
        else
        {
            noche = false;

            //if (fogDensity < 0.02) fogDensity += Time.deltaTime * 0.01f;
        }

        /*
        if(RenderSettings.fog == true)
        {
            RenderSettings.fogDensity = fogDensity;
        }
        */
    }

    public void ControlNubes()
    {
        if (nublado)
        {

            if (!noche)
            {
                /*
                if (nubesCielo[0].opacity < 1)
                {
                    nubesCielo[0].opacity += Time.deltaTime * speed;
                }
                if (nubesCielo[1].opacity < 1)
                {
                    nubesCielo[1].opacity += Time.deltaTime * speed;
                }
                */
                if (nubesCielo[2].opacity < 0.5)
                {
                    nubesCielo[2].opacity += Time.deltaTime * speed;
                }
                if (nubesCielo[3].opacity < 1) //fondo
                {
                    nubesCielo[3].opacity += Time.deltaTime * speed;
                }
            }
            else //APAGA LAS NUBES DE NOCHE CUANDO ESTA NUBLADO
            {
                /*
                if (nubesCielo[0].opacity > 0)
                {
                    nubesCielo[0].opacity -= Time.deltaTime * speed;
                }
                if (nubesCielo[1].opacity > 0)
                {
                    nubesCielo[1].opacity -= Time.deltaTime * speed;
                }
                */
                if (nubesCielo[2].opacity > 0)
                {
                    nubesCielo[2].opacity -= Time.deltaTime * speed;
                }
                if (nubesCielo[3].opacity > 0) //fondo
                {
                    nubesCielo[3].opacity -= Time.deltaTime * speed;
                }
            }
        }
        else //CIELO DESPEJADO
        {

            /*
            if (nubesCielo[0].opacity > 0)
            {
                nubesCielo[0].opacity -= Time.deltaTime * speed;
            }
            if (nubesCielo[1].opacity > 0)
            {
                nubesCielo[1].opacity -= Time.deltaTime * speed;
            }
            */
            if (nubesCielo[2].opacity > 0)
            {
                nubesCielo[2].opacity -= Time.deltaTime * speed;
            }
            if (nubesCielo[3].opacity > 0) //fondo
            {
                nubesCielo[3].opacity -= Time.deltaTime * speed;
            }
        }
    }

    public void SubirVolumen()
    {
        if (audioSourceActual.volume < 1)
        {
            audioSourceActual.volume += Time.deltaTime * speed;
        }
    }

    public void BajarVolumen()
    {
        if (audioSourceActual.isPlaying)
        {
            if (audioSourceActual.volume > 0)
            {
                audioSourceActual.volume -= Time.deltaTime * speed;
            }
            else
            {
                audioSourceActual.Stop();
            }
        }
    }

}


