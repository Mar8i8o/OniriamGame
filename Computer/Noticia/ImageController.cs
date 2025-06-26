using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    NoticiasManager noticiasManager;
    CorreosManager correosManager;

    public Sprite imagenActual;

    public bool isCorreo;

    void Start()
    {
        if (!isCorreo) { noticiasManager = GameObject.Find("GameManager").GetComponent<NoticiasManager>(); }
        else { correosManager = GameObject.Find("GameManager").GetComponent<CorreosManager>(); }
    }

    public void AbrirImagen()
    {
        if(isCorreo)
        {
            //correosManager.Image2GO
            correosManager.MostrarImagen(imagenActual);
        }
        else
        {
            noticiasManager.imagenActual.sprite = imagenActual;
            print("Cambiar imagen");
            noticiasManager.MostrarImagen();
        }

    }
}
