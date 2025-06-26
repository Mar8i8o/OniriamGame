using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadSong : MonoBehaviour
{
    public GameObject songEnVenta;
    public PlaySoundsMovil songEnVentaScript;

    public GameObject decargaCheckPrefab;
    public GameObject decargaCheckParent;

    PlayerStats playerStats;

    public float precio;

    public GameObject playSoundButt;
    public GameObject stopSoundButt;

    public GameObject downloadButt;
    public GameObject buyedButton;
    public bool buyed;

    public AudioSource audioSource;
    public AudioClip thisAudio;

    public bool soundActive;


    void Start()
    {
        //PlayerPrefs.DeleteAll();
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();

        if (PlayerPrefs.GetInt(gameObject.name + "buyed", System.Convert.ToInt32(buyed)) == 0) { buyed = false; }
        else { buyed = true; }

        if (buyed) 
        {
            buyedButton.SetActive(true);
            downloadButt.SetActive(false);
        }
        else
        {
            buyedButton.SetActive(false);
            downloadButt.SetActive(true);
        }

        if (!soundActive)
        {
            playSoundButt.SetActive(true);
            stopSoundButt.SetActive(false);

        }
        else
        {
            playSoundButt.SetActive(false);
            stopSoundButt.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        //print("Enable");
        CancelarPlay();
    }

    public void CancelarPlay()
    {
        soundActive = false;
        playSoundButt.SetActive(true);
        stopSoundButt.SetActive(false);
    }

    public void ComprarSonido()
    {
        if (!buyed)
        {
            if (playerStats.dinero >= precio)
            {
                buyed = true;
                songEnVenta.SetActive(true);
                songEnVentaScript.active = true;
                songEnVentaScript.GuardarDatos();

                print("CHECK");
                GameObject instancia = Instantiate(decargaCheckPrefab, decargaCheckParent.transform.position, Quaternion.identity);
                instancia.transform.SetParent(decargaCheckParent.transform);

                playerStats.dinero -= precio;

                buyedButton.SetActive(true);
                downloadButt.SetActive(false);
                
            }
        }
    }

    public void GuardarDatos()
    {
        PlayerPrefs.SetInt(gameObject.name + "buyed", System.Convert.ToInt32(buyed));
    }

    GameObject[] todosLosAudios;
    public void ReproducirSonido()
    {

        if (!soundActive)
        {
            playSoundButt.SetActive(false);
            stopSoundButt.SetActive(true);

            todosLosAudios = GameObject.FindGameObjectsWithTag("ButtonAudio");

            for (int i = 0; i < todosLosAudios.Length; i++)
            {
                todosLosAudios[i].GetComponent<DownloadSong>().soundActive = false;
            }

            audioSource.clip = thisAudio;
            audioSource.Play();
            soundActive = true;
        }
        else
        {
            playSoundButt.SetActive(true);
            stopSoundButt.SetActive(false);
            audioSource.Stop();
            soundActive = false;
        }

    }
}
