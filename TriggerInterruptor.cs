using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerInterruptor : MonoBehaviour
{
    public GameObject luz;
    public GameObject luzEncendidoGO;
    public GameObject luzApagadoGO;

    public bool encendido;

    ElectricidadController electricidadController;
    PensamientoControler pensamientoControler;

    public bool tieneAnimacion;

    public Animator anim;

    public bool isVent;
    public GameObject vent;
    public Vector3 speedVent;

    public GameObject luzAux;
    void Start()
    {
        electricidadController = GameObject.Find("ElectricidadControler").GetComponent<ElectricidadController>();
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();
        if (encendido)
        {
            EncenderLuz();
        }
        else
        {
            ApagarLuz();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (encendido)
        {
            if (isVent) { vent.transform.Rotate(speedVent * Time.deltaTime); }
            else
            {
                if (!luz.activeSelf && electricidadController.electricidad)
                {
                    luz.SetActive(true);
                    luzEncendidoGO.SetActive(true);
                    luzApagadoGO.SetActive(false);

                    if (luzAux != null) { luzAux.SetActive(false); }

                }
                else if (luz.activeSelf && !electricidadController.electricidad)
                {
                    luz.SetActive(false);
                    luzEncendidoGO.SetActive(false);
                    luzApagadoGO.SetActive(true);

                    if (luzAux != null) { luzAux.SetActive(true); }
                }
            }

        }
        
    }

    public void Interactuar()
    {

        if (electricidadController.electricidad)
        {

            if (encendido)
            {
                ApagarLuz();
            }
            else
            {
                EncenderLuz();
            }

        }
        else
        {
            pensamientoControler.MostrarPensamiento("No puedo encender la luz sin electricidad", 1);
        }
    }

    public void ApagarLuz()
    {
        if (!isVent)
        {
            if (electricidadController.electricidad)
            {
                luz.SetActive(false);
                luzEncendidoGO.SetActive(false);
                luzApagadoGO.SetActive(true);
            }
        }
        encendido = false;
        if (luzAux != null) { luzAux.SetActive(true); }

        if (tieneAnimacion) 
        {
            EncenderAnim();
            anim.SetBool("On", encendido);
            Invoke(nameof(ApagarAnim), 3);
        }

    }

    public void EncenderLuz()
    {
        if (!isVent)
        {
            if (electricidadController.electricidad)
            {
                luz.SetActive(true);
                luzEncendidoGO.SetActive(true);
                luzApagadoGO.SetActive(false);
            }
        }
        encendido = true;
        if (luzAux != null) { luzAux.SetActive(false); }

        if (tieneAnimacion) 
        {
            EncenderAnim();
            anim.SetBool("On", encendido);
            if (!isVent) Invoke(nameof(ApagarAnim), 3);
        }
    }

    void ApagarAnim()
    {
        anim.enabled = false;
    }

    void EncenderAnim()
    {
        CancelInvoke(nameof(ApagarAnim));
        anim.enabled = true;
    }

}
