using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventosEntregarCartas : MonoBehaviour
{

    //public ItemAtributes carta_a_Entregar;
    public GameObject puntoEntregaCarta;


    void Start()
    {
        //PlayerPrefs.DeleteAll();
        puntoEntregaCarta = GameObject.Find("PuntoEntregaCarta");

        //Invoke(nameof(EntregarCarta), 10);
    }

    void Update()
    {
        
    }

    public void EntregarCarta(ItemAtributes carta_a_Entregar)
    {
        if (!carta_a_Entregar.active)
        {
            carta_a_Entregar.transform.position = puntoEntregaCarta.transform.position;
            carta_a_Entregar.active = true;
            carta_a_Entregar.mesh.SetActive(true);
            carta_a_Entregar.ActivarItem();
            //carta_a_Entregar.GuardarPosicion();
        }
    }

}
