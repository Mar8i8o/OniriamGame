using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaculoJuego : MonoBehaviour
{

    //public float velocity;
    public float velocidad;
    Vector3 posicionInicial;

    public GameObject puntos;

    void Start()
    {
        //print("hola");
        //juegoController = GameObject.Find("GameManager").GetComponent<JuegoController>();
        //posicionInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        gameObject.transform.Translate(-velocidad * Time.deltaTime, 0, 0);
        //if (juegoController.gameOver) { Destroy(gameObject); }
    }

    private void FixedUpdate()
    {
       
    }

    public void ResetPosition()
    {
        //transform.position = juegoController.puntoSpawnObstaculo.transform.position;
        float random = Random.Range(-1.8f, 1.9f);
        transform.position = new Vector3(transform.position.x, transform.transform.position.y + random, transform.position.z);
        puntos.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("BordeJuegoFinal"))
        {
            //juegoController.puntos++;
            //Destroy(gameObject);
            gameObject.SetActive(false);

            //ResetPosition();

        }
    }
}
