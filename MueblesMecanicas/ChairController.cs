using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairController : MonoBehaviour
{
    public GameObject posicionSilla;
    public MeshCollider sillaMesh;
    public Collider colider;
    public Collider coliderTrigger;
    public Raycast ray;

    GameObject player;

    public bool sillaUsada;
    public float distancia;

    public bool sentadoEnSilla;

    public GameObject pisPosition;
    public Vector3 offsetPis;
    Vector3 pisPositionPosicionInicial;

    public GameObject icoLevantarse;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (sillaUsada) 
        {
            distancia = Vector3.Distance(player.transform.position, transform.position);

            if (distancia > 1) { ActivarColisiones(); }

        }

        if (sentadoEnSilla)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, posicionSilla.transform.position, 5f * Time.deltaTime);
            pisPosition.transform.localPosition = offsetPis;
        }
    }

    private void LateUpdate()
    {
        if (sentadoEnSilla)
        {
            if (!ray.usingComputer)
            {
                if (ray.tiempoSinObjeto > 0.1f)
                {
                    if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        LevantaraeSilla();
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        LevantaraeSilla();
                    }
                }
            }

        }
    }

    public bool blockSentarseSilla;
    public bool generaPensamiento;
    public string pensamiento;

    public PensamientoControler pensamientoControler;

    public void SentarseSilla()
    {
        if (!blockSentarseSilla)
        {
            colider.isTrigger = true;
            coliderTrigger.enabled = false;
            sillaMesh.isTrigger = true;
            sillaUsada = true;
            ray.sentado = true;
            sentadoEnSilla = true;

            pisPositionPosicionInicial = pisPosition.transform.localPosition;

            icoLevantarse.SetActive(true);

        }

        if(generaPensamiento && !pensamientoControler.mostrandoPensamiento)
        {
            pensamientoControler.MostrarPensamiento(pensamiento, 2);
        }

    }

    public void LevantaraeSilla()
    {
        ray.sentado = false;
        coliderTrigger.enabled = true;
        player.GetComponent<CapsuleCollider>().enabled = true;
        player.GetComponent<CamaraFP>().freeze = false;
        player.GetComponent<CharacterController>().enabled = true;
        sentadoEnSilla = false;

        pisPosition.transform.localPosition = pisPositionPosicionInicial;

        icoLevantarse.SetActive(false);

    }
    public void ActivarColisiones()
    {
        colider.isTrigger = false;
        sillaMesh.isTrigger= false;
    }
}
