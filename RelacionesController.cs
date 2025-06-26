using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelacionesController : MonoBehaviour
{
    public float relacionAlterKid;
    public float relacionAlterKiller;

    DreamController dreamController;
    GuardarController guardarController;
    void Start()
    {
        dreamController = GameObject.Find("GameManager").GetComponent<DreamController>();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();

        GetDatos();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(guardarController.guardando)
        {
            Guardar();
        }
    }

    public void Guardar()
    {
        PlayerPrefs.SetFloat("RelacionAlterKid", relacionAlterKid);
        PlayerPrefs.SetFloat("RelacionAlterKiller", relacionAlterKiller);
    }

    public void GetDatos()
    {
        relacionAlterKid = PlayerPrefs.GetFloat("RelacionAlterKid", relacionAlterKid);
        relacionAlterKiller = PlayerPrefs.GetFloat("RelacionAlterKiller", relacionAlterKiller);
    }

}
