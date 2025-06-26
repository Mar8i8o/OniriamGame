using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeJuegoController : MonoBehaviour
{
    public Rigidbody rb;
    JuegoController juegoController;
    Vector3 posicionInicial;

    public Animator anim;
    public Animator animObj;

    public bool freeze;

    public bool freezeControl;

    void Start()
    {
        juegoController = GameObject.Find("GameManager").GetComponent<JuegoController>();
        posicionInicial = transform.position;

        if (freeze) rb.isKinematic = true;
    }

    void Update()
    {

        if (freeze) rb.isKinematic = true;

        if (!freezeControl)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                freeze = false;
                rb.isKinematic = false;
                rb.AddForce(new Vector3(0, 8, 0), ForceMode.Impulse);
                anim.SetBool("fliying", false);
                anim.SetTrigger("fly");
                animObj.SetTrigger("fly");
            }
        }

        anim.SetBool("fliying", freeze);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("BordeJuego"))
        {
            //Spawn();
            juegoController.GameOver();
        }
        else if (collision.transform.CompareTag("ObstaculoJuego"))
        {
            //Spawn();
            juegoController.GameOver();
        }
        else if (collision.transform.CompareTag("PuntosJuego"))
        {
            //Spawn();
            juegoController.puntos++;
            //Destroy(collision.gameObject);
            print("SumarPuntos");
            //collision.gameObject.SetActive(false);
        }
        else if (collision.transform.CompareTag("Puntero"))
        {
            freeze = false;
            rb.isKinematic = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("BordeJuego"))
        {
            //Spawn();
            juegoController.GameOver();
        }
        else if (other.transform.CompareTag("ObstaculoJuego"))
        {
            //Spawn();
            juegoController.GameOver();
        }
        else if (other.transform.CompareTag("PuntosJuego"))
        {
            //Spawn();
            print("SumarPuntos");
            juegoController.puntos++;
            //Destroy(other.gameObject);
            //other.gameObject.SetActive(false);

            freeze = false;
            rb.isKinematic = false;
        }
    }

    public void Spawn()
    {
        transform.position = posicionInicial;
        rb.linearVelocity = Vector3.zero;
        freeze = true;
    }
}
