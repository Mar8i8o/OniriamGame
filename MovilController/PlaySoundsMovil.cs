using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundsMovil : MonoBehaviour
{
    public AudioClip thisAudio;
    public bool soundActive;
    public bool ringTone;
    public bool active;

    public AudioSource audioMovil;
    public AudioSource audioLlamada;
    public MovilController movilController;
    public ButtonMovilController buttonMovilController;

    public GameObject playButton;
    public GameObject ringToneButton;

    void Start()
    {
        GetDatos();
        ringToneButton.SetActive(ringTone);
        if (ringTone) audioLlamada.clip = thisAudio;
        if (!active) gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        if (gameObject.transform.GetSiblingIndex() == 0)
        {
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(gameObject);
            buttonMovilController.selecionado = true;
            buttonMovilController.tiempoSeleccionado = 0;
        }
    }

    private void OnDisable()
    {
        buttonMovilController.selecionado = false;
        buttonMovilController.tiempoSeleccionado = 0;
    }

    public float tiempoPress = 0;
    bool pressForChange;
    void Update()
    {
        playButton.SetActive(soundActive);
        ringToneButton.SetActive(ringTone);
        if (ringTone) audioLlamada.clip = thisAudio; 

        if (buttonMovilController.selecionado && !ringTone)
        {
            if (movilController.tiempoPanelSonidos > 0.1f)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    tiempoPress = 0;
                }
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    tiempoPress += Time.deltaTime;

                    if (tiempoPress > 1)
                    {
                        movilController.DisableAllRingtones();
                        ringTone = true;
                        audioLlamada.clip = thisAudio;
                        ringToneButton.SetActive(true);

                    }

                }
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    tiempoPress = 0;
                }
            }

        }
        if (buttonMovilController.selecionado && buttonMovilController.tiempoSeleccionado > 0.3)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                tiempoPress = 0;
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                tiempoPress += Time.deltaTime;

            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (tiempoPress < 1) PlaySound();
            }
        }


    }

    private void LateUpdate()
    {
        //GuardarDatos();
    }

    public void GuardarDatos()
    {
        print("saved");
        PlayerPrefs.SetInt(gameObject.name + "active", System.Convert.ToInt32(active));
        PlayerPrefs.SetInt(gameObject.name + "ringTone", System.Convert.ToInt32(ringTone));
    }

    public void GetDatos()
    {
        if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(active)) == 0) { active = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(active)) == 1) { active = true; }

        if (PlayerPrefs.GetInt(gameObject.name + "ringTone", System.Convert.ToInt32(ringTone)) == 0) { ringTone = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "ringTone", System.Convert.ToInt32(ringTone)) == 1) { ringTone = true; }
    }

    public void PlaySound()
    {
        if (soundActive)
        {
            movilController.DisableAllAudios();
            soundActive = false;
            audioMovil.clip = thisAudio;
            audioMovil.Stop();

        }
        else
        {
            movilController.DisableAllAudios();
            soundActive = true;
            audioMovil.clip = thisAudio;
            audioMovil.Play();
        }
    }

    public void Desactivar()
    {
        soundActive = false;
        audioMovil.Stop();
        //movilController.DisableAllAudios();
    }

    public void DesactivarRingTone()
    {
        ringTone = false;
        ringToneButton.SetActive(false);
    }
}
