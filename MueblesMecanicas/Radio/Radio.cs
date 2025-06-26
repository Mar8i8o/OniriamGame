using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    bool encendida;

    public AudioSource radioSound;

    public AudioClip[] canciones;

    int indiceRadio;

    float time;

    GuardarController guardarController;

    public GameObject radioOn;
    public GameObject radioOff;
    void Start()
    {

        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        radioSound.clip = canciones[indiceRadio];

        time = PlayerPrefs.GetFloat("TimeRadio", 0);
        indiceRadio = PlayerPrefs.GetInt("IndiceRadio", 0);

        if (encendida ) 
        {
            radioSound.Play();
        }
        else
        {
            radioSound.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (encendida) 
        {
            if (radioSound.time + 0.1f >= radioSound.clip.length)
            {
                Debug.Log("El clip de audio ha terminado de reproducirse.");

                if (indiceRadio < canciones.Length)
                {
                    indiceRadio++;
                    radioSound.time = 0;
                    radioSound.clip = canciones[indiceRadio];
                    radioSound.Play();
                }
                else
                {
                    indiceRadio = 0;
                    radioSound.time = 0;
                    radioSound.clip = canciones[indiceRadio];
                    radioSound.Play();
                }
            }

            radioOff.SetActive(false);
            radioOn.SetActive(true);

        }
        else
        {
            radioOff.SetActive(true);
            radioOn.SetActive(false);
        }

        //print(radioSound.time); 
        
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetFloat("TimeRadio", time);
            PlayerPrefs.SetInt("IndiceRadio", indiceRadio);
        }
    }

    public void InteractuarRadio()
    {
        if (encendida)
        {
            time = radioSound.time; 
            encendida = false;
            radioSound.Stop();
        }
        else
        {
            radioSound.time = time;
            encendida = true;
            radioSound.Play();
        }
    }

}
