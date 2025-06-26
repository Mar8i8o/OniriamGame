using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BordeCaerse : MonoBehaviour
{
    public ControlarCaerse2 controlarCaerse;
    public CharacterController characterController;
    public PensamientoControler pensamientoController;

    public float tiempoDentro;

    public float tiempoUltimoPensamiento;
    void Start()
    {
        tiempoUltimoPensamiento = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (tiempoUltimoPensamiento < 20)
        {
            tiempoUltimoPensamiento += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            if (tiempoUltimoPensamiento > 18)
            {
                controlarCaerse.Caerse();
                pensamientoController.MostrarPensamiento("No deberia seguir por aqui...", 2);
                tiempoUltimoPensamiento = 0;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (!controlarCaerse.estadoCaidaLevantarse && characterController.velocity.magnitude != 0)
            {
                tiempoDentro += Time.deltaTime;

                if (tiempoDentro > 2)
                {
                    tiempoDentro = 0;
                    controlarCaerse.Caerse();
                }
            }
            else
            {
                tiempoDentro = 0;
            }
        }
    }
}
