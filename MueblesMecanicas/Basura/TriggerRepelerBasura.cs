using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRepelerBasura : MonoBehaviour
{
    // Start is called before the first frame update

    public PensamientoControler pensamientoControler;
    bool blockPensar;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("PickableObject"))
        {
             ItemAtributes itemAtributes;

             itemAtributes = other.gameObject.GetComponent<ItemAtributes>();

            if (!itemAtributes.sePuedeTirar && !itemAtributes.isBasura)
            {
                print("Devolver Item");
                other.gameObject.GetComponent<Rigidbody>().AddForce(0, 50, -10);

                if(!blockPensar)
                {
                    pensamientoControler.MostrarPensamiento("No debería tirar eso ahí", 1);
                    blockPensar = true;
                    Invoke(nameof(UnblockPensar), 5f);
                }

            }
            else if (itemAtributes.isBasura)
            {
                other.gameObject.GetComponent<Rigidbody>().AddForce(0, 30, -20);

                if (!blockPensar)
                {
                    pensamientoControler.MostrarPensamiento("No debería tirar eso ahí", 1);
                    blockPensar = true;
                    Invoke(nameof(UnblockPensar), 5f);
                }

            }
        }
    }

    public void UnblockPensar()
    {
        blockPensar = false;
    }

}
