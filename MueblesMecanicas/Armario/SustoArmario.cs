using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SustoArmario : MonoBehaviour
{

    public GameObject enemy;

    public PlayerStats playerStats;

    public TriggerPuertaArmario[] triggerPuertaArmario;

    public int aleatorio;
    void Start()
    {
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();

        //print("EntreAbrirPuerta");
        //enemy.SetActive(true);
        //triggerPuertaArmario[0].EntreAbrirPuerta();
        //triggerPuertaArmario[1].EntreAbrirPuerta();

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!triggerPuertaArmario[0].isVisible)
        {
            if (Time.frameCount % 60 == 0)
            {
                aleatorio = Random.Range(0, 1000);

                if (aleatorio == 1)
                {
                    if (!triggerPuertaArmario[0].puertaAbierta && !triggerPuertaArmario[1].puertaAbierta)
                    {
                        print("EntreAbrirPuerta");
                        enemy.SetActive(true);
                        triggerPuertaArmario[0].EntreAbrirPuerta();
                        triggerPuertaArmario[1].EntreAbrirPuerta();
                    }
                }
            }
        }
        */
    }
}
