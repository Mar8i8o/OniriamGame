using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContenidoCajonController : MonoBehaviour
{
    public GameObject cajon;

    public int objetos;

    public bool isCaja;
    public CajaController cajaController;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void anyadirObjetoCajon(ItemAtributes itemAtributes)
    {
        itemAtributes.gameObject.transform.SetParent(cajon.transform);
        itemAtributes.rb.isKinematic = true;
        itemAtributes.inCajon = true;
        itemAtributes.contenidoCajon = gameObject.GetComponent<ContenidoCajonController>();
        itemAtributes.cajonName = gameObject.name;
        objetos++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("PickableObject"))
        {
            ItemAtributes itemAtributes;
            itemAtributes = other.gameObject.GetComponent<ItemAtributes>();


            if(!itemAtributes.pickUp)
            {
                if (isCaja && itemAtributes.isCaja) {  }
                else
                {

                    if(isCaja) { if(cajaController.cajaItem.pickUp) { return; } }

                    if (!itemAtributes.inCajon)
                    {
                        other.gameObject.transform.SetParent(cajon.transform);
                        itemAtributes.inCajon = true;
                        itemAtributes.contenidoCajon = gameObject.GetComponent<ContenidoCajonController>();
                        itemAtributes.cajonName = gameObject.name;
                        objetos++;
                    }
                    //if (isCaja) { itemAtributes.rb.isKinematic = true;}
                }
            }

        }
    }


    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("PickableObject"))
        {
            ItemAtributes itemAtributes;
            itemAtributes = other.gameObject.GetComponent<ItemAtributes>();


            if (!itemAtributes.pickUp)
            {
                print("TocandoAlgo");
                other.gameObject.transform.SetParent(cajon.transform);
            }

        }
    }
    */


    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("PickableObject"))
        {

            ItemAtributes itemAtributes;
            itemAtributes = other.gameObject.GetComponent<ItemAtributes>();

            if(!itemAtributes.rb.isKinematic && !itemAtributes.col.isTrigger)
            {

                if (isCaja) { if (!cajaController.abierto) { return; } }
                if (isCaja) { if (cajaController.tiempoCajaAbierta < 1) { return; } }
                print("SalirCajon");
                itemAtributes.inCajon = false;
                objetos--;
            }

        }
    }
}
