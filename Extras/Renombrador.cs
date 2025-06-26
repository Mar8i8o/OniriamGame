using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renombrador : MonoBehaviour
{
    public GameObject[] pelotas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu(itemName: "Rename")]

    public void Rename()
    {
        for (int i = 0; i < pelotas.Length; i++)
        {
            pelotas[i].name = "Pelota_" + i; 
        }
    }
}
