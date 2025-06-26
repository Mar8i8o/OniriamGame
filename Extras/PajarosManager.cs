using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PajarosManager : MonoBehaviour
{

    public bool unicaVez;

    TimeController timeController;

    public GameObject[] puntosPajaros;
    public GameObject[] puntosSpawnPajaros;
    public bool playerCerca;
    public float distanca;

    public GameObject player;
    public GameObject pajaroPrefab;

    public int pajaros;

    public LayerMask playerLayer;
    public Vector3 detectionBoxSize = new Vector3(5f, 5f, 5f);
    public Vector3 offsetBox;

    public float tiempoSinPlayerCerca;

    public bool forzarVolar;

    public GameObject cadaverCuervo;

    void Start()
    {
        player = GameObject.Find("Player");
        if(!unicaVez)timeController = GameObject.Find("GameManager").GetComponent<TimeController>();

        SpawnearPajaros();
    }

    // Update is called once per frame
    void Update()
    {
        distanca = Vector3.Distance(transform.position, player.transform.position);

        playerCerca = Physics.CheckBox(transform.position + offsetBox, detectionBoxSize/2,Quaternion.identity, playerLayer);

        if(unicaVez) { return; }

        if(!playerCerca && pajaros < puntosSpawnPajaros.Length) 
        {
            tiempoSinPlayerCerca += Time.deltaTime;

            if(tiempoSinPlayerCerca > 60)
            {
                if (timeController.hora < 21 && timeController.hora > 6)
                {
                    forzarVolar = true;
                    //SpawnearPajaros();
                    Invoke(nameof(DesactivarForzarVolar), 0.5f);
                    tiempoSinPlayerCerca = 0;
                }
            }
        }
        else
        {
            tiempoSinPlayerCerca = 0;
        }
    }

    public void DesactivarForzarVolar()
    {
        forzarVolar = false;
        SpawnearPajaros();

        if (timeController.dia == 4) { cadaverCuervo.SetActive(true); }
        else { cadaverCuervo.SetActive(false); }
    }

    public bool forzarIdle;
    public int idleForzado;

    public float distanciaIrse;

    public void SpawnearPajaros()
    {
        for (int i = 0; i < puntosSpawnPajaros.Length; i++) 
        {
            if (!unicaVez)
            {
                if (i == timeController.dia) break;
            }

;           GameObject instancia = Instantiate(pajaroPrefab, puntosSpawnPajaros[i].transform.position, Quaternion.identity);
            instancia.GetComponent<PajaroController>().pajarosManager = this;
            instancia.GetComponent<PajaroController>().forzarIdle = forzarIdle;
            instancia.GetComponent<PajaroController>().distanciaIrse = distanciaIrse;

            if(forzarIdle)
            {
                instancia.GetComponent<PajaroController>().anim.SetInteger("NumIdle", idleForzado);
            }

            pajaros++;
        }
    }

    private void OnDrawGizmos()
    {
        // Cambiar el color del gizmo dependiendo si el jugador está cerca
        Gizmos.color = playerCerca ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position + offsetBox, detectionBoxSize);
    }
}
