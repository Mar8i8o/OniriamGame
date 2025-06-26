using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliciaController : MonoBehaviour
{
    public bool policiaActivo;
    public bool puedesLlamarEmergencias;

    public GameObject prefabPolicia;
    public GameObject puntoSpawnPolicia;

    public int numVecesLlamadoPolicia;

    public DetectarPlayer detectarPlayer;

    GuardarController guardarController;

    public AudioSource sonidoSusto;
    public GameObject pantallaNegro;

    public DoorController puertaPrincipal;

    void Start()
    {

        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();

        GetDatos();
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            GuardarDatos();
        }
    }

    public void GuardarDatos()
    {
        PlayerPrefs.SetInt("NumVecesLlamadoPolicia", numVecesLlamadoPolicia);
    }

    public void GetDatos()
    {
        numVecesLlamadoPolicia = PlayerPrefs.GetInt("NumVecesLlamadoPolicia", numVecesLlamadoPolicia);

    }

    public void PoliciaEventLlamada() //LLAMAR POLICIA
    {
        print("policiaAvisada");
        puedesLlamarEmergencias = false;
        numVecesLlamadoPolicia++;

        Invoke(nameof(LlegarPolicia), 5);
    }

    public void LlegarPolicia()
    {
        Instantiate(prefabPolicia, puntoSpawnPolicia.transform.position, Quaternion.identity);
        policiaActivo = true;
    }
}
