using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeController : MonoBehaviour
{
    //public GameObject cafe;

    public ItemAtributes itemAtributes;

    public Rigidbody rb;

    public float cantidadCafe;

    public float offsetY;

    GuardarController guardarController;

    public float caliente;

    public ParticleSystem particulasHumo;


    void Start()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        cantidadCafe = PlayerPrefs.GetFloat(itemAtributes.gameObject.name + "CantidadCafe", 0);

    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, (cantidadCafe/200) - offsetY, transform.localPosition.z);

        /*
        if (cantidadCafe > 0 ) { BloquearDesbloquearRotacionX(true);}
        else BloquearDesbloquearRotacionX(false);
        */

        //if (cantidadCafe > 0 && !itemAtributes.inCafetera) { itemAtributes.gameObject.transform.eulerAngles = new Vector3(itemAtributes.gameObject.transform.rotation.x, itemAtributes.gameObject.transform.rotation.y, 0); }

        //print(itemAtributes.pickUp);

        if (cantidadCafe > 0)
        {
            if (caliente >= 0)
            {
                caliente -= Time.deltaTime;
                if (!particulasHumo.isEmitting) { particulasHumo.Play(); }
            }
            else
            {
                particulasHumo.Stop();
            }
        }
        else
        {
            particulasHumo.Stop();
        }

    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetFloat(itemAtributes.gameObject.name + "CantidadCafe", cantidadCafe);
        }
    }

    void BloquearDesbloquearRotacionX(bool bloquear)
    {
        // Obtiene las restricciones actuales
        RigidbodyConstraints constraints = rb.constraints;

        // Bloquea o desbloquea la rotación en el eje X
        if (bloquear)
        {
            constraints |= RigidbodyConstraints.FreezeRotationX;
        }
        else
        {
            constraints &= ~RigidbodyConstraints.FreezeRotationX;
        }

        // Aplica las nuevas restricciones al Rigidbody
        rb.constraints = constraints;
    }
}
