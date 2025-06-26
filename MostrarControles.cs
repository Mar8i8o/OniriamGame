using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MostrarControles : MonoBehaviour
{
    public TextMeshProUGUI pensamientoTXT;
    public Animator anim;

    public bool mostrandoControles;
    void Start()
    {
        //Invoke(nameof(LlamarPensamiento), 5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LlamarMostrarControles()
    {
        MostrarControlesText("me he meado...", 5f);
    }

    public void MostrarControlesText(string pensamiento, float duracion)
    {
        pensamientoTXT.text = pensamiento;
        mostrandoControles = true;
        anim.SetBool("MostrandoPensamiento", mostrandoControles);

        Invoke(nameof(DejarDeMostrarTexto), duracion);
    }

    public void DejarDeMostrarTexto()
    {
        mostrandoControles = false;
        anim.SetBool("MostrandoPensamiento", mostrandoControles);
    }
}
