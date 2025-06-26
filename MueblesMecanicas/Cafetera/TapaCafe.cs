using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapaCafe : MonoBehaviour
{
    public Animator animTapa;

    public float tiempoSinApuntar;
    public bool tapaAbierta;

    public bool llenandoCafe;

    public TriggerCafetera triggerCafetera;
    void Start()
    {
        ApagarAnim();
    }

    // Update is called once per frame
    void Update()
    {
        if (tapaAbierta) 
        {
            tiempoSinApuntar += Time.deltaTime;

            if (tiempoSinApuntar > 0.5f) 
            {
                animTapa.SetBool("Open", false);
                Invoke(nameof(ApagarAnim), 2);
            }
        }

        if (llenandoCafe)
        {
            triggerCafetera.cantidadCafe += Time.deltaTime * 10;

            if (triggerCafetera.cantidadCafe >= 10)
            {
                llenandoCafe = false;
            }
        }
    }

    public void ApuntarTapa()
    {
        tapaAbierta = true;
        AbrirTapa();
        tiempoSinApuntar = 0;
    }

    public void AbrirTapa()
    {
        EncenderAnim();
        print("abrirTapa");
        animTapa.SetBool("Open", true);
    }

    public void LlenarCafe()
    {
        llenandoCafe = true;
    }

    void ApagarAnim()
    {
        animTapa.enabled = false;
    }

    void EncenderAnim()
    {
        CancelInvoke(nameof(ApagarAnim));
        animTapa.enabled = true;
    }

}
