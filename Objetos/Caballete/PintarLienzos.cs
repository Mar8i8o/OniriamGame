using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PintarLienzos : MonoBehaviour
{
    public GameObject[] lienzos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.K)) 
        {
            PintarLienzo("Bosque");
        }
    }

    public void PintarLienzo(string id)
    {
        for(int i = 0; i < lienzos.Length; i++) 
        {
            ItemAtributes lienzoItemAtributes = lienzos[i].GetComponent<ItemAtributes>();
            if (lienzoItemAtributes.enCaballete) 
            {
                lienzoItemAtributes.lienzoId = id;
                break;
            }
        }
    }
}
