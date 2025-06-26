using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeerTextoController : MonoBehaviour
{
    public TextMeshProUGUI textoTXT;
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

    public void MostrarTexto(string texto)
    {
        textoTXT.text = texto;
        mostrandoPensamiento = true;
        anim.SetBool("MostrandoPensamiento", mostrandoPensamiento);

        //Invoke(nameof(DejarDeMostrarTexto), 5);
    }

    public void DejarDeMostrarTexto()
    {
        mostrandoPensamiento = false;
        anim.SetBool("MostrandoPensamiento", mostrandoPensamiento);
    }
}
