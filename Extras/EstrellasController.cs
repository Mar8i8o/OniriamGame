using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstrellasController : MonoBehaviour
{
    public Renderer estrellas;  // El material cuya opacidad se desea cambiar.
    TimeController timeController;

    public float opacidadDia;
    public float opacidadNoche;

    public float opacidad;

    public float variacionSpeed;

    public LluviaController lluviaController;

    void Start()
    {
       timeController = GameObject.Find("GameManager").GetComponent<TimeController>();
    }

    void Update()
    {
        if(timeController.hora >= 21 || timeController.hora <= 6)
        {
            if (!lluviaController.lloviendo)
            {
                if (opacidad < opacidadNoche) opacidad += Time.deltaTime * variacionSpeed;
                if (opacidad > opacidadNoche) opacidad -= Time.deltaTime * variacionSpeed;
            }
            else //APAGA LAS ESTRELLAS SI LLUEVE
            {
                if (opacidad < opacidadDia) opacidad += Time.deltaTime * variacionSpeed;
                if (opacidad > opacidadDia) opacidad -= Time.deltaTime * variacionSpeed;
            }
        }
        else
        {
            if (opacidad < opacidadDia) opacidad += Time.deltaTime * variacionSpeed;
            if (opacidad > opacidadDia) opacidad -= Time.deltaTime * variacionSpeed;
        }

        estrellas.material.color = new Color(estrellas.material.color.r, estrellas.material.color.g, estrellas.material.color.b, opacidad);
    }

}
