using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionesLlave : MonoBehaviour
{
    public Collider coliderLlave;
    public GameObject piezaLlave;
    void Start()
    {
        SoltarLlave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUpLlave()
    {
        coliderLlave.enabled = false;
        print("cojer llave");
    }
    public void SoltarLlave()
    {
        coliderLlave.enabled = true;
        piezaLlave.layer = 14;
        //Invoke(nameof(CambiarLayer), 1);
    }

    public void CambiarLayer()
    {
        piezaLlave.layer = 14;
    }
}
