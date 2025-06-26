using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTalking : MonoBehaviour
{
    public AudioSource audioSource;

    public float frecuencia;

    public float timeLoop;

    public bool talking;

    void Start()
    {
        timeLoop = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {

        if (talking)
        {
            timeLoop += Time.deltaTime;

            if (timeLoop > frecuencia)
            {
                audioSource.Play();
                timeLoop = 0;
            }
        }
        else
        {
            timeLoop = 0.1f;
        }

    }
}
