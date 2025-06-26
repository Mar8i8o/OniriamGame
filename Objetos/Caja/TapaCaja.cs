using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapaCaja : MonoBehaviour
{
    
    public CajaController cajaController;

    public void Interactuar()
    {
        cajaController.Interactuar();
    }
}
