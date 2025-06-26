using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PensamientoControler : MonoBehaviour
{
    public TextMeshProUGUI pensamientoTXT;
    public Animator anim;

    public bool mostrandoPensamiento;
    void Start()
    {
        //Invoke(nameof(LlamarPensamiento), 5);
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*
    public void LlamarPensamiento()
    {
        MostrarPensamiento("me he meado...", 5f);
    }
    */

    string pensamientoAux;
    float duracionAux;
    public void MostrarPensamiento(string pensamiento, float duracion)
    {
        if (!mostrandoPensamiento)
        {
            pensamientoTXT.text = pensamiento;
            mostrandoPensamiento = true;
            anim.SetBool("MostrandoPensamiento", mostrandoPensamiento);

            Invoke(nameof(DejarDeMostrarPensamiento), duracion);
        }
        else
        {
            pensamientoAux = pensamiento;
            duracionAux = duracion;

            //Invoke(nameof(MostrarPensamientoAux), duracion + 2);
        }
    }

    void MostrarPensamientoAux() //////SEGUNDO PENSAMIENTO PARA QUE NO SE SOLAPEN
    {
        pensamientoTXT.text = pensamientoAux;
        mostrandoPensamiento = true;
        anim.SetBool("MostrandoPensamiento", mostrandoPensamiento);

        Invoke(nameof(DejarDeMostrarPensamiento), duracionAux);
    }

    public void DejarDeMostrarPensamiento()
    {
        CancelInvoke(nameof(DejarDeMostrarPensamiento));
        mostrandoPensamiento = false;
        anim.SetBool("MostrandoPensamiento", mostrandoPensamiento);
    }
}
