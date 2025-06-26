using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBeberAgua : MonoBehaviour
{

    PlayerStats playerStats;

    Raycast ray;

    public bool botellaColocada;
    public GameObject botella;

    public ItemAtributes botellaScript;
    public Transform positionBotella;

    public ParticleSystem particulasAgua;
    public float duracionParticulas;
    float duracionParticulasInicial;

    public Collider col;

    public Ducha aguaController;

    public float velocidadLlenarBotella;

    [HideInInspector]public bool llenandoBotella;
    float tiempoLlenandoBotella;

    void Start()
    {
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
        ray = GameObject.Find("Main Camera").GetComponent<Raycast>();

        duracionParticulasInicial = particulasAgua.startLifetime;
    }

    // Update is called once per frame
    void Update()
    {

        //tiempoSinBeber += Time.deltaTime;
        //if(tiempoSinBeber > 1 && !botellaColocada) { particulasAgua.startLifetime = duracionParticulasInicial; }
        if(bebiendoAgua)
        {
            tiempoSinBeber += Time.deltaTime;
            if(tiempoSinBeber > 1)
            {
                bebiendoAgua = false;
                if(sonidoBeber != null) { sonidoBeber.Stop(); } 
            }
        }

        if(botellaColocada)
        {
            botella.transform.position = Vector3.MoveTowards(botella.transform.position, positionBotella.position, 5 * Time.deltaTime);
            botella.transform.rotation = positionBotella.rotation;

            particulasAgua.startLifetime = duracionParticulas;

            if (aguaController.palancaAbierta) 
            {   
                if (llenandoBotella)
                {
                    botellaScript.scriptLiquido.cantidadDeAgua += Time.deltaTime * velocidadLlenarBotella;
                    botellaScript.scriptLiquido.usos = 4;

                    tiempoLlenandoBotella += Time.deltaTime;
                    botellaScript.col.enabled = false;

                    if (tiempoLlenandoBotella > 2)
                    {
                        botellaScript.col.enabled = true;
                        llenandoBotella = false;
                        if (ray.hasObject) 
                        {
                            botellaScript.rb.isKinematic = false;
                            Soltar();
                        }
                        else { ray.CogerObjeto(botellaScript.gameObject); }
                    }
                }
            }
            else
            {
                //botellaScript.col.enabled = true;
            }

            if(botellaScript.pickUp)
            {
                Soltar();
            }

        }
    }

    public void Soltar()
    {
        particulasAgua.startLifetime = duracionParticulasInicial;
        col.enabled = true;
        botellaColocada = false;
    }

    public void Interactuar() //CLICK UNA VEZ
    {
        if(ray.hasObject)
        {
            if(ray.itemAtributes.isWater || !ray.itemAtributes.isLata) //COLOCAR BOTELLA
            {
                if (ray.itemAtributes.scriptLiquido.usos >= 4) { return; }
                botella = ray.itemAtributes.gameObject;
                botellaScript = ray.itemAtributes;
                ray.ForzarSoltarObjeto();
                botellaColocada = true;
                llenandoBotella = true;
                botellaScript.rb.isKinematic = true;

                particulasAgua.Clear();
                particulasAgua.startLifetime = duracionParticulas;
                tiempoLlenandoBotella = 0;

                //Invoke(nameof(DesbloquearBotella),1);

                if(aguaController.palancaAbierta) botellaScript.col.enabled = false;
                col.enabled = false;
            }
        }
    }

    public void DesbloquearBotella()
    {
        botellaScript.col.enabled = true;
    }

    float tiempoSinBeber;
    bool bebiendoAgua;
    public AudioSource sonidoBeber;
    public void MantenerPulsado()
    {
        if (aguaController.palancaAbierta)
        {
            print("BeberAgua");
            playerStats.agua += Time.deltaTime * 5;

            //particulasAgua.startLifetime = duracionParticulas;
            tiempoSinBeber = 0; 
            bebiendoAgua = true;

            if(sonidoBeber != null)
            {
                if (!sonidoBeber.isPlaying) { sonidoBeber.Play(); }
            }
        }
    }
}
