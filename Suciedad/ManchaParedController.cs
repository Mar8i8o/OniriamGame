using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManchaParedController : MonoBehaviour
{
    public bool active;
    public float suciedad;
    public bool limpiando;
    public float tiempoLimpiando;

    public GameObject mancha;
    public Renderer render;

    GuardarController guardarController;
    void Start()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();

        if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(active)) == 0) { active = false; }
        else { active = true; }

        if (active)
        {
            Activar();
            GetDatos();
        }
        else
        {
            Desactivar();
        }

        //PlayerPrefs.DeleteAll();
    }
    public void Limpiar()
    {
            limpiando = true;
            tiempoLimpiando = 0;
    }

    public void Desactivar()
    {
        active = false;
        mancha.GetComponent<MeshRenderer>().enabled = false;
        mancha.GetComponent<BoxCollider>().enabled = false;
        //transform.position = Vector3.zero;
        //mancha.transform.localScale = new Vector3(1, 1, 1);
        //gameObject.SetActive(false);
        
    }

    public void Activar()
    {
        mancha.GetComponent<MeshRenderer>().enabled = true;
        active = true;
        suciedad = 1;
        //charco.transform.localScale = new Vector3(1, 1, 1);
    }

    public void GetDatos()
    {
        suciedad = PlayerPrefs.GetFloat(gameObject.name + "suciedad", suciedad);
    }

    public void GuardarDatos()
    {
        PlayerPrefs.SetFloat(gameObject.name + "suciedad", suciedad);
        PlayerPrefs.SetInt(gameObject.name + "active", System.Convert.ToInt32(active));
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            GuardarDatos();
        }
    }
    private void Update()
    {


        if (limpiando)
        {
            tiempoLimpiando += Time.deltaTime;
            suciedad -= Time.deltaTime * 0.2f;

            if (tiempoLimpiando > 1)
            {
                limpiando = false;
            }
        }

        if (active)
        {
            if (suciedad < 0) { Desactivar(); }
            render.material.color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, suciedad);
        }


    }
}
