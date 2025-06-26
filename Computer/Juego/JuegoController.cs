using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JuegoController : MonoBehaviour
{
    public GameObject puntoSpawnObstaculo;
    public GameObject prefabObstaculo;
    public PersonajeJuegoController personajeJuegoController;

    public TextMeshProUGUI puntosTXT;
    public TextMeshProUGUI recordTXT;
    public float tiempoSpawn;
    public float frecuenciaSpawn = 5;

    public bool juegoActivo;
    public float margen;
    public bool gameOver;

    public float tiempoJugando;

    public int puntos;
    public int record;

    public float velocidadObstaculos = 5;

    GuardarController guardarController;

    public ObstaculoJuego[] obstaculos;

    void Start()
    {
        record = PlayerPrefs.GetInt("RecordJuego", record);
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();

    }

    // Update is called once per frame
    void Update()
    {

        if (!juegoActivo) return;

        if (!gameOver) 
        {
            tiempoSpawn += Time.deltaTime;

            if (tiempoSpawn > frecuenciaSpawn) 
            {
                float random = Random.Range(-1.8f, 1.9f);
                //GameObject instancia = Instantiate(prefabObstaculo, puntoSpawnObstaculo.transform.position, Quaternion.identity);
                //instancia.transform.position = new Vector3(instancia.transform.position.x, instancia.transform.transform.position.y + random, instancia.transform.position.z);
                //instancia.GetComponent<ObstaculoJuego>().velocidad = velocidadObstaculos;

                InstanciarObstaculo(new Vector3(puntoSpawnObstaculo.transform.position.x, puntoSpawnObstaculo.transform.transform.position.y + random, puntoSpawnObstaculo.transform.position.z), velocidadObstaculos);

                tiempoSpawn = 0;
            }

            tiempoJugando += Time.deltaTime;
            velocidadObstaculos = 5 + (tiempoJugando/10);
            if (frecuenciaSpawn > 0.2f) { frecuenciaSpawn = 2 - (tiempoJugando / 100); }
            else { frecuenciaSpawn = 0.2f; }

            if (puntos > record)
            {
                record = puntos;
            }
        }
        puntosTXT.text = "" + puntos;
        recordTXT.text = "" + record;

    }

    public int indiceObstaculos;
    public void InstanciarObstaculo(Vector3 posicion, float velocidad)
    {
        if (indiceObstaculos + 1 == obstaculos.Length)
        {
            indiceObstaculos = 0;
        }
        else
        {
            indiceObstaculos++;
        }

        obstaculos[indiceObstaculos].velocidad = velocidad;
        obstaculos[indiceObstaculos].gameObject.transform.position = posicion;
        obstaculos[indiceObstaculos].gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt("RecordJuego", record);
        }
    }

    public bool muerto;

    public void GameOver() //CUANDO TE MATAN
    {

        print("DEAD");

        //personajeJuegoController.gameObject.SetActive(false);
        personajeJuegoController.freezeControl = true;
        personajeJuegoController.rb.linearVelocity = Vector3.zero;
        personajeJuegoController.rb.AddForce(new Vector3(-8, 4, 0), ForceMode.Impulse);


        muerto = true;
        personajeJuegoController.anim.SetBool("dead", muerto);

        Invoke(nameof(StartGameOver), 1f);
        Invoke(nameof(CancellGameOver),1.3f);

        if (puntos > record)
        {
            record = puntos;
        }

        puntos = 0;
    }

    public void StartGameOver()
    {
        gameOver = true;

        GameObject[] obstaculos = GameObject.FindGameObjectsWithTag("ObstaculoJuego");

        
        for (int i = 0; i < obstaculos.Length; i++) 
        {
            //obstaculos[i].GetComponent<ObstaculoJuego>().ResetPosition();

            obstaculos[i].SetActive(false);
        }

        personajeJuegoController.gameObject.SetActive(true);
        personajeJuegoController.Spawn();
        personajeJuegoController.freezeControl = false;
        tiempoJugando = 0;
        muerto = false;
        personajeJuegoController.anim.SetBool("dead", muerto);
    }
    public void CancellGameOver()
    {
        gameOver = false;
        muerto = false;
        personajeJuegoController.anim.SetBool("dead", muerto);
        tiempoSpawn = 1;
        tiempoJugando = 0;
        
    }

    public void EmpezarJuego()
    {
        juegoActivo = true;
        StartGameOver();
        personajeJuegoController.gameObject.SetActive(true);
        personajeJuegoController.Spawn();
        gameOver = false;
        tiempoSpawn = 1;
    }

    public void SalirJuego()
    {
        juegoActivo = false;

        GameObject[] obstaculos = GameObject.FindGameObjectsWithTag("ObstaculoJuego");


        for (int i = 0; i < obstaculos.Length; i++)
        {
            //obstaculos[i].GetComponent<ObstaculoJuego>().ResetPosition();
            //Destroy(obstaculos[i]);
            obstaculos[i].SetActive(false);
        }
    }
}
