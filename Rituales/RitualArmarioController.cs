using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualArmarioController : MonoBehaviour
{

    public bool puertaAbierta;
    public bool playerDentro;

    public bool ritualActivo;

    public TriggerPuertaArmario puertaArmario1;
    public TriggerPuertaArmario puertaArmario2;

    TimeController timeController;
    Raycast ray;

    void Start()
    {
        timeController = GameObject.Find("GameManager").GetComponent<TimeController>();
        ray = GameObject.Find("Main Camera").GetComponent<Raycast>();
    }

    void Update()
    {
        puertaAbierta = puertaArmario1.puertaAbierta && puertaArmario2.puertaAbierta;

        if (!ritualActivo)
        {
            if (timeController.hora == 3)
            {
                if (!puertaAbierta && playerDentro && ray.hasObject)
                {
                    if (ray.itemAtributes.isVela && ray.itemAtributes.velaEncendida)
                    {
                        ritualActivo = true;
                    }
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            playerDentro = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.transform.CompareTag("Player"))
        {
            playerDentro = false;
        }
    }
}
