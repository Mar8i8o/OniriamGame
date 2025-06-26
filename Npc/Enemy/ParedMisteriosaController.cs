using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParedMisteriosaController : MonoBehaviour
{
    public AudioSource audioPuerta;
    public DoorController doorController;

    TimeController timeController;

    void Start()
    {
        timeController = GameObject.Find("GameManager").GetComponent<TimeController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeController.hora > 21 || timeController.hora < 6)
        {
            if(!audioPuerta.isPlaying && !doorController.puertaAbierta)
            {
                audioPuerta.Play();
            }
        }
        else
        {
            if (audioPuerta.isPlaying)
            {
                audioPuerta.Stop();
            }
        }

        if (doorController.puertaAbierta)
        {
            if (audioPuerta.isPlaying)
            {
                audioPuerta.Stop();
            }
        }
    }
}
