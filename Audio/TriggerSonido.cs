using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSonido : MonoBehaviour
{
    public AudioSource audioSourceInterior;  // Audio dentro del trigger
    public AudioSource audioSourceExterior;  // Audio fuera del trigger

    GuardarController guardarController;

    public float fadeDuration = 2.0f;        // Duración de la transición

    public bool isInTrigger = false;        // Bandera para saber si el player está dentro del trigger
    private bool isFading = false;           // Evitar solapamiento de fades

    public bool blockCambiaAudio;

    void Start()
    {
        // Asegúrate de que los audios empiezan en el estado correcto
        //audioSourceExterior.volume = 1.0f;   // El audio exterior empieza sonando
        //audioSourceInterior.volume = 0.0f;   // El audio interior empieza silenciado

        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();

        if (PlayerPrefs.GetInt(gameObject.name + "dentro", System.Convert.ToInt32(isInTrigger)) == 0) { isInTrigger = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "dentro", System.Convert.ToInt32(isInTrigger)) == 1) { isInTrigger = true; }
    }

    private void LateUpdate()
    {
        if(guardarController.guardando)
        {
            Guardar();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            isInTrigger = true;
            if(!blockCambiaAudio) StartCoroutine(CrossFade(audioSourceExterior, audioSourceInterior, fadeDuration));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            isInTrigger = false;
            if(!blockCambiaAudio) StartCoroutine(CrossFade(audioSourceInterior, audioSourceExterior, fadeDuration));
        }
    }

    IEnumerator CrossFade(AudioSource fromAudio, AudioSource toAudio, float duration)
    {
        isFading = true;

        print("CrossFade");

        float startVolumeFrom = fromAudio.volume;  // Volumen inicial del audio que se va a desvanecer
        toAudio.volume = 0;                        // Asegúrate de que el audio al que se hace fade in empieza en 0
        toAudio.Play();

        float timer = 0f;
        while (timer < duration)
        {
            float progress = timer / duration;

            // Ajusta los volúmenes suavemente entre los dos audios
            fromAudio.volume = Mathf.Lerp(startVolumeFrom, 0, progress);  // Fade out del audio actual
            toAudio.volume = Mathf.Lerp(0, 1.0f, progress);               // Fade in del nuevo audio, siempre hasta 1

            timer += Time.deltaTime;
            yield return null;
        }

        // Asegúrate de que el fade ha terminado completamente
        fromAudio.Stop();        // Detenemos el audio que terminó
        fromAudio.volume = startVolumeFrom;  // Restablecemos el volumen original por si se necesita más adelante
        toAudio.volume = 1.0f;   // Garantiza que el audio al que se hace fade in esté a volumen 1

        isFading = false;
    }

    public void Guardar()
    {
        PlayerPrefs.SetInt(gameObject.name + "dentro", System.Convert.ToInt32(isInTrigger));
    }
}

