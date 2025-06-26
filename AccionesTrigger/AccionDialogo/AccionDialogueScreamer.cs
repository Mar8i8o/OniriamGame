using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionDialogueScreamer : MonoBehaviour
{

    public GameObject npc;

    public GameObject player;

    public bool iniciarAccion;

    public GameObject prefab;

    Vector3 puntoDelante;

    Vector3 posicionObjetivo;

    public GameObject pantallaNegro;

    public float tiempoActivarPantallaNegro;

    public DreamController dreamController;

    void Start()
    {
        puntoDelante = GenerarPuntoDelante();

        posicionObjetivo = new Vector3(puntoDelante.x, npc.transform.position.y, puntoDelante.z);

        Invoke(nameof(ActivarPantallaNegro), tiempoActivarPantallaNegro);

    }

    void Update()
    {
        npc.transform.position = Vector3.MoveTowards(npc.transform.position, posicionObjetivo, 20 * Time.deltaTime);
    }

    public void ActivarPantallaNegro()
    {
        //pantallaNegro.SetActive(true);
        dreamController.Despertarse();
    }

    Vector3 GenerarPuntoDelante()
    {
        // Calcular el punto frente a la cámara utilizando su dirección hacia adelante
        //Instantiate(prefab, player.transform.position + (player.transform.forward * 1), Quaternion.identity);
        return player.transform.position + (player.transform.forward * 0.3f);
    }
}
