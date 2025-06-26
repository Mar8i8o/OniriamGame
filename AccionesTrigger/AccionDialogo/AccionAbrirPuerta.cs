using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionAbrirPuerta : MonoBehaviour
{

    public bool puertaDoble;

    public bool entreAbrirPuerta;
    public bool cerrarPuerta;

    public DoorController puerta1;
    public DoorController puerta2;
    void Start()
    {

        if(entreAbrirPuerta)
        {

            puerta1.EntreAbrirPuerta();
            if(puertaDoble) { puerta2.EntreAbrirPuerta(); }

        }
        else
        {
            if (cerrarPuerta)
            {
                puerta1.SetCerrarPuerta();
                if (puertaDoble) { puerta2.SetCerrarPuerta(); }
            }
            else 
            { 
                puerta1.SetPuertaAbierta();
                if (puertaDoble) { puerta2.SetPuertaAbierta(); }
            }
        }
    }

}
