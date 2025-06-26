using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AccionTransicionMusica : MonoBehaviour
{
    public AudioSource audioSourceActual;
    public AudioSource audioSourceACambiar;

    float startVolume;
    float volumenDeseado;
    public float duration;

    public bool isTrigger;
    bool usado;

    void Start()
    {
        startVolume = audioSourceActual.volume;

        //StartCoroutine(FadeOut(duration));
        //StartCoroutine(FadeIn(audioSourceACambiar.volume, duration));

        StartCoroutine(CrossFade(duration));

    }

    IEnumerator CrossFade(float duration)
    {
        float startVolumeACambiar = audioSourceACambiar.volume;
        audioSourceACambiar.volume = 0;
        audioSourceACambiar.Play();

        float timer = 0f;
        while (timer < duration)
        {
            float progress = timer / duration;
            audioSourceActual.volume = Mathf.Lerp(startVolume, 0, progress);
            audioSourceACambiar.volume = Mathf.Lerp(0, startVolumeACambiar, progress);

            timer += Time.deltaTime;
            yield return null;
        }

        audioSourceActual.Stop();
        audioSourceACambiar.volume = startVolumeACambiar;
    }



    IEnumerator FadeOut(float duration)
    {
        // Obtiene el volumen inicial

        // Reduce gradualmente el volumen a cero durante el tiempo especificado
        while (audioSourceActual.volume > 0)
        {
            audioSourceActual.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        // Detiene la reproducción después de que el volumen llega a cero
        audioSourceActual.Stop();
    }

    IEnumerator FadeIn(float targetVolume, float duration)
    {
        // Asegúrate de que el volumen inicial sea 0
        audioSourceACambiar.volume = 0;

        // Inicia la reproducción del audio si no se está reproduciendo
        if (!audioSourceACambiar.isPlaying)
        {
            audioSourceACambiar.Play();
        }

        // Incrementa gradualmente el volumen hasta el volumen objetivo (targetVolume)
        while (audioSourceACambiar.volume < targetVolume)
        {
            audioSourceACambiar.volume += targetVolume * Time.deltaTime / duration;
            yield return null;  // Espera hasta el siguiente frame
        }

        // Asegúrate de que el volumen sea exactamente el objetivo al finalizar
        audioSourceACambiar.volume = targetVolume;
    }



}
