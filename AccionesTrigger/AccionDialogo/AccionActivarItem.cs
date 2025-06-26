using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionActivarItem : MonoBehaviour
{
    public GameObject objetoActivar;
    public bool desactiva;
    void Start()
    {
        objetoActivar.SetActive(!desactiva);
    }


}
