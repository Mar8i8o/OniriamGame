using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObjetosCama : MonoBehaviour
{
    public int linternas;
    public int velas;
    public int atrapaSuenyos;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("PickableObject"))
        {

            ItemAtributes itemAtributes;
            itemAtributes = other.gameObject.GetComponent<ItemAtributes>();

            if (itemAtributes.isLinterna)
            {
                linternas++;
            }
            else if (itemAtributes.isAtrapasuenos) 
            {
                atrapaSuenyos++;
            }
            else if (itemAtributes.isVela)
            {
                velas++;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("PickableObject"))
        {

            ItemAtributes itemAtributes;
            itemAtributes = other.gameObject.GetComponent<ItemAtributes>();

            if (itemAtributes.isLinterna)
            {
                linternas--;
            }
            else if (itemAtributes.isAtrapasuenos)
            {
                atrapaSuenyos--;
            }
            else if (itemAtributes.isVela)
            {
                velas--;
            }

        }
    }
}
