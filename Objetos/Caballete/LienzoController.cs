using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LienzoController : MonoBehaviour
{
    public ItemAtributes itemAtributesLienzo;

    public GameObject lienzoNormal;
    public GameObject lienzoBosque;
    void Start()
    {
        itemAtributesLienzo = GetComponent<ItemAtributes>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (itemAtributesLienzo.lienzoId == "Bosque")
        {
            lienzoNormal.SetActive(false);
            lienzoBosque.SetActive(true);
        }
        else
        {
            lienzoNormal.SetActive(true);
            lienzoBosque.SetActive(false);
        }
    }
}
