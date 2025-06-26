using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PasosController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioPasosMadera;
    public AudioClip audioPasosMetal;
    public AudioClip audioPasosGrava;
    public AudioClip audioPasosAsfalto;
    public AudioClip audioPasosHierba;
    public AudioClip audioPasosCeramica;
    public CharacterController characterController;
    CamaraFP camaraFP;
    public bool walking;

    public string idPasosActual;

    void Start()
    {
        camaraFP = GameObject.Find("Player").GetComponent<CamaraFP>();
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) walking = false;
        else walking = true;

        if(characterController.velocity.magnitude == 0) { walking = false; }

        if(walking && !audioSource.isPlaying && !camaraFP.freeze)
        {
            audioSource.Play();
        }
        else if(!walking && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if(audioSource.isPlaying && camaraFP.freeze) { audioSource.Stop(); }

    }

    public void EmpezarACorrer()
    {
        audioSource.pitch = 1.5f;
    }

    public void DejarDeCorrer()
    {
        audioSource.pitch = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.transform.tag);
        if (other.transform.CompareTag("woodFloor"))
        {
            audioSource.clip = audioPasosMadera;
            //print("pasosMadera");
            idPasosActual = "pasosMadera";
        }
        else if (other.transform.CompareTag("gravelFloor"))
        {
            audioSource.clip = audioPasosGrava;
            //print("pasosGrava");
            idPasosActual = "pasosGrava";
        }
        else if (other.transform.CompareTag("metalFloor"))
        {
            audioSource.clip = audioPasosMetal;
            //print("pasosMetal");
            idPasosActual = "pasosMetal";
        }
        else if (other.transform.CompareTag("concreteFloor"))
        {
            audioSource.clip = audioPasosAsfalto;
            //print("pasosConcreto");
            idPasosActual = "pasosConcreto";
        }
        else if (other.transform.CompareTag("grassFloor"))
        {
            audioSource.clip = audioPasosHierba;
            //print("pasosHierba");
            idPasosActual = "pasosHierba";
        }
        else if (other.transform.CompareTag("ceramicFloor"))
        {
            audioSource.clip = audioPasosCeramica;
            //print("pasosCeramica");
            idPasosActual = "pasosCeramica";
        }
    }

}
