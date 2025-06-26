using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalancasElectricidad : MonoBehaviour
{
    public Animator anim;

    public ElectricidadController electricidadController;

    public GameObject luzElectricidad;

    void Start()
    {
        if (electricidadController.electricidad)
        {
            EncenderAnim();
            anim.SetBool("On", true);
            Invoke(nameof(ApagarAnim), 3);
        }
        else
        {
            EncenderAnim();
            anim.SetBool("On", false);
            Invoke(nameof(ApagarAnim), 3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Usar()
    {
        if (electricidadController.electricidad) 
        {
            EncenderAnim();
            electricidadController.electricidad = false;
            anim.SetBool("On", false);
            Invoke(nameof(ApagarAnim), 3);
            luzElectricidad.SetActive(true);
        }
        else
        {
            EncenderAnim();
            electricidadController.electricidad = true;
            anim.SetBool("On", true);
            Invoke(nameof(ApagarAnim), 3);
            luzElectricidad.SetActive(false);
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
