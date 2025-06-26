using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PensamientoTrigger : MonoBehaviour
{
    PensamientoControler pensamientoControler;
    public string pensamiento;

    public float duracion = 1;

    void Start()
    {
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interactuar()
    {
        if (puedeMostrar)
        {
            CancelInvoke(nameof(UnblockMostrarPensamiento));
            pensamientoControler.MostrarPensamiento(pensamiento, duracion);
            puedeMostrar = false;
            Invoke(nameof(UnblockMostrarPensamiento), duracion + 1);
        }
    }

    public bool puedeMostrar = true;

    public void UnblockMostrarPensamiento()
    {
        puedeMostrar = true;
    }
}
