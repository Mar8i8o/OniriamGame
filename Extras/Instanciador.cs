using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instanciador : MonoBehaviour
{
    public GameObject prefab;

    public float frencuencia;

    public float cuantos;

    public float contador;

    //public Material[] materiales;

    void Start()
    {
        
    }

    void Update()
    {

        if (cuantos > 0)
        {
            contador += Time.deltaTime;

            if (contador > frencuencia)
            {
                contador = 0;
                GameObject instancia = Instantiate(prefab, transform.position, transform.rotation);
                //instancia.GetComponent<Renderer>().material = materiales[Random.Range(0, materiales.Length)];
                cuantos--;
            }
        }
    }
}
