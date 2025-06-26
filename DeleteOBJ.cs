using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOBJ : MonoBehaviour
{
    float tiempo;

    public float duracion;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;

        if( tiempo > duracion ) { Destroy(gameObject); }
    }
}
