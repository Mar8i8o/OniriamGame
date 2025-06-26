using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotePelotas : MonoBehaviour
{
    public Rigidbody[] rb;
    public GameObject player;

    public Renderer objectRenderer;
    private MaterialPropertyBlock propBlock;

    //public LayerMask playerLayer;
    //public float detectionRadius;
    bool playerCerca;

    float distance;
    bool activo;

    void Start()
    {

        // Obtén la información actual del material
        objectRenderer.GetPropertyBlock(propBlock);

        // Asigna un nuevo color con la opacidad deseada
        Color baseColor = objectRenderer.sharedMaterial.GetColor("_BaseColor");
        baseColor = Color.blue;

        // Modifica la propiedad del color base del material en este objeto específico
        propBlock.SetColor("_BaseColor", baseColor);

        // Aplica los cambios
        objectRenderer.SetPropertyBlock(propBlock);


        //rb.isKinematic = true;
        player = GameObject.Find("Player");

        for(int i=0; i<rb.Length; i++)
        {
            rb[i].isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void FixedUpdate()
    {
        if (Time.frameCount % 60 == 0)
        {
            //playerCerca = Physics.CheckSphere(transform.position, detectionRadius, playerLayer);
            distance = Vector3.Distance(transform.position, player.transform.position);


            if (distance < 5)
            {
                if (!activo)
                {
                    for (int i = 0; i < rb.Length; i++)
                    {
                        rb[i].isKinematic = false;
                    }
                    activo = true;
                }
            }
            else
            {
                if (activo)
                {
                    for (int i = 0; i < rb.Length; i++)
                    {
                        rb[i].isKinematic = true;
                    }
                    activo = false;
                }
            }
        }
    }
}
