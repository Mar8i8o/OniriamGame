using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricidadController : MonoBehaviour
{
    public bool electricidad;

    public bool puertaAbiera;

    public Animator animPuerta;

    public Collider colPalancas;
    void Start()
    {
        if (puertaAbiera)
        {

            EncenderAnim();

            animPuerta.SetBool("PuertaAbierta", true);
            colPalancas.enabled = true;

            Invoke(nameof(ApagarAnim), 3);
        }
        else
        {
            EncenderAnim();

            animPuerta.SetBool("PuertaAbierta", false);
            colPalancas.enabled = false;

            Invoke(nameof(ApagarAnim), 3);
        }
    }

    public void Usar()
    {
        if (puertaAbiera) 
        {

            EncenderAnim();

            puertaAbiera = false;
            animPuerta.SetBool("PuertaAbierta", false);
            colPalancas.enabled = false;

            Invoke(nameof(ApagarAnim), 3);
        }
        else
        {

            EncenderAnim();


            puertaAbiera = true;
            animPuerta.SetBool("PuertaAbierta", true);
            colPalancas.enabled = true;

            Invoke(nameof(ApagarAnim), 3);
        }
    }

    void ApagarAnim()
    {
        animPuerta.enabled = false;
    }

    void EncenderAnim()
    {
        CancelInvoke(nameof(ApagarAnim));
        animPuerta.enabled = true;
    }
}
