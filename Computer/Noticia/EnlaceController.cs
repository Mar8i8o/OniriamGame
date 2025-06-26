using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnlaceController : MonoBehaviour
{
    NoticiasManager noticiasManager;
    CorreosManager correosManager;

    public string enlaceID;

    public bool isCorreo;

    void Start()
    {
        if (!isCorreo) { noticiasManager = GameObject.Find("GameManager").GetComponent<NoticiasManager>(); }
        else { correosManager = GameObject.Find("GameManager").GetComponent<CorreosManager>(); }
    }

    void Update()
    {
        
    }

    public void UsarEnlace()
    {
        if(isCorreo)
        {
            correosManager.enlaceId = enlaceID;
            correosManager.UsarEnlace();
        }
        else
        {
            noticiasManager.enlaceId = enlaceID;
            noticiasManager.UsarEnlace();
        }
    }
}
