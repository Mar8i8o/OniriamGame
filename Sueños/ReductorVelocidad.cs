using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReductorVelocidad : MonoBehaviour
{
    public CamaraFP camaraFP;

    public float setspeed;
    public float setRunspeed;

    float speedPlayer;

    public LayerMask playerLayer;
    public Vector3 detectionBoxSize = new Vector3(5f, 5f, 5f);
    public Vector3 offsetBox;
    public bool playerCerca;

    public bool reciendDentro;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Quaternion boxRotation = transform.rotation;
        //playerCerca = Physics.CheckBox(transform.position + offsetBox, detectionBoxSize / 2, boxRotation, playerLayer);

        /*
        if(playerCerca) 
        {
            reciendDentro = true;
            camaraFP.runSpeed = setspeed;
        }
        else
        {
            if (reciendDentro)
            {
                camaraFP.runSpeed = 1;
                reciendDentro = false;
            }
        }
        */

        if (playerCerca)
        {
            if (camaraFP.runing)
            {
                camaraFP.runSpeed = setRunspeed;
            }
            else
            {
                camaraFP.runSpeed = setspeed;
            }
        }

    }

    private void OnDrawGizmos()
    {
        /*
        // Guarda la matriz de transformación original de Gizmos
        Gizmos.matrix = Matrix4x4.TRS(transform.position + offsetBox, transform.rotation, Vector3.one);

        // Establece el color del Gizmo
        Gizmos.color = Color.green;

        // Dibuja el WireCube con la rotación aplicada
        Gizmos.DrawWireCube(Vector3.zero, detectionBoxSize);

        // Restaura la matriz de transformación de Gizmos a su estado original (opcional, pero recomendable)
        Gizmos.matrix = Matrix4x4.identity;
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            //camaraFP.runSpeed = setspeed;
            playerCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playerCerca = false;
            camaraFP.runSpeed = 1;
        }
    }
}
