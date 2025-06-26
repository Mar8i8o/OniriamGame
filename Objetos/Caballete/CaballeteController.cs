using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaballeteController : MonoBehaviour
{
    public GameObject lienzoPoint;
    public ItemAtributes itemAtributesCaballete;
    public BoxCollider colider;
    public Rigidbody rb;

    public ItemAtributes lienzo;

    public bool tieneLienzo;

    Raycast raycast;
    void Start()
    {
        raycast = GameObject.Find("Main Camera").GetComponent<Raycast>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tieneLienzo) 
        {
            lienzo.gameObject.transform.position = Vector3.MoveTowards(lienzo.transform.position, lienzoPoint.transform.position, 0.1f);
            lienzo.gameObject.transform.rotation = lienzoPoint.transform.rotation;
        }
    }

    public void ColocarLienzo(GameObject cual)
    {
        colider.enabled = false;

        raycast.ForzarSoltarObjeto();

        //cual.transform.position = lienzoPoint.transform.position;
        cual.transform.rotation = lienzoPoint.transform.rotation;
        cual.GetComponent<ItemAtributes>().clavado = true;
        cual.GetComponent<ItemAtributes>().rb.isKinematic = true;

        lienzo = cual.GetComponent<ItemAtributes>();


        rb.isKinematic = true;
        itemAtributesCaballete.clavado = true;

        raycast.canPickUp = false;
        Invoke(nameof(ReactivarCanPickUp), 0.2f);

        lienzo.enCaballete = cual;
        lienzo.caballeteController = gameObject.GetComponent<CaballeteController>();
        tieneLienzo = true;
    }

    public void ReactivarCanPickUp()
    {
        raycast.canPickUp = true;
    }

    public void QuitarLienzo()
    {
        tieneLienzo = false;
        colider.enabled = true;
        rb.isKinematic = false;
    }
}

