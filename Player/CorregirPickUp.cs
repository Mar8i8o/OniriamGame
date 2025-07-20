using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorregirPickUp : MonoBehaviour
{
    public bool algoDentro;
    public bool sacandoObjeto;

    public Transform pickUpDelante;
    public Transform pickUpAtras;
    public Transform pickUpGuardado;

    public LayerMask capasDetectables;

    public GameObject parent;

    Raycast raycast;

    void Start()
    {
        raycast = GameObject.Find("Main Camera").GetComponent<Raycast>();
    }

    public float tiempoSacandoObjeto;

    void Update()
    {
        if (!raycast.itemGuardado)
        {
            if (algoDentro)
            {
                parent.transform.position = Vector3.MoveTowards(parent.transform.position, pickUpAtras.position, 2 * Time.deltaTime);
            }
            else
            {
                parent.transform.position = Vector3.MoveTowards(parent.transform.position, pickUpDelante.position, 2 * Time.deltaTime);
            }
        }
        else
        {
            parent.transform.position = Vector3.MoveTowards(parent.transform.position, pickUpGuardado.position, 3 * Time.deltaTime);
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.isTrigger)
        {
            if (((1 << other.gameObject.layer) & capasDetectables) != 0)
            {
                algoDentro = true;
                sacandoObjeto = false;
                tiempoSacandoObjeto = 0;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.isTrigger)
        {
            if (((1 << other.gameObject.layer) & capasDetectables) != 0)
            {
                algoDentro = true;
                sacandoObjeto = false;
                tiempoSacandoObjeto = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
        {
            if (((1 << other.gameObject.layer) & capasDetectables) != 0)
            {
                //sacandoObjeto = true;
                algoDentro = false;
                tiempoSacandoObjeto = 0;
            }
        }
    }
}
