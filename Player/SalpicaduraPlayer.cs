using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalpicaduraPlayer : MonoBehaviour
{
    public bool inAgua;

    float contador;

    public float frecuencia;
    public GameObject prefab;

    public CharacterController characterController;

    public Animator[] waterSprites;
    public int indice;
    void Start()
    {

        waterSprites = new Animator[10];

        for (int i = 0; i < 10; i++) 
        {
            GameObject instancia = Instantiate(prefab, Vector3.zero, transform.rotation);
            waterSprites[i] = instancia.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(inAgua)
        {
            if (characterController.velocity.x > 0.1f || characterController.velocity.z > 0.1f)
            {
                contador += Time.deltaTime;

                if (contador > frecuencia)
                {
                    contador = 0;
                    //Instantiate(prefab, transform.position, transform.rotation);

                    if (indice > waterSprites.Length) indice = 0;

                    waterSprites[indice].gameObject.transform.position = new Vector3(transform.position.x, posicionY, transform.position.z);
                    waterSprites[indice].SetTrigger("Start");
                }
            }
            else
            {
                contador = frecuencia;
            }
        }
        else
        {
            contador = frecuencia;
        }
    }

    public float posicionY;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Water"))
        {
            inAgua = true;
            contador = frecuencia;
            posicionY = other.transform.position.y;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Water"))
        {
            inAgua = false;
        }
    }
}
