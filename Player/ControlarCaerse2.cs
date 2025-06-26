using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlarCaerse2 : MonoBehaviour
{

    public bool estadoCaidaLevantarse;
    public bool cayendose;
    public bool levantandose;
    public bool moviendoseCaida;
    public bool puedeLevantarse;


    public Raycast ray;
    public CamaraFP camaraFP;

    public GameObject player;
    public Collider colider;
    public GameObject puntoCaerse;

    public CharacterController characterController;

    public GameObject camara;

    public Animator anim;

    public float caidaSpeed = 5;
    float caidaSpeedInicial;

    Vector3 forwardCaida;

    public float levantarseSpeed;

    public bool desmayado;


    void Start()
    {
        //forwardCaida = transform.forward;

    }

    void Update()
    {


        if(estadoCaidaLevantarse) 
        {
            camara.transform.position = Vector3.MoveTowards(camara.transform.position, puntoCaerse.transform.position,8 * Time.deltaTime);
            if (moviendoseCaida && !puedeLevantarse)
            {
                characterController.Move(forwardCaida * caidaSpeed * Time.deltaTime);

                if (caidaSpeed > 0) { caidaSpeed -= Time.deltaTime * 5; }
                else
                {
                    puedeLevantarse = true;
                    levantandose = false;
                }
            }
        }
        else
        {
                ControlarCaida();
        }

        anim.SetBool("Cayendose", cayendose);

        /*
        if(Input.GetKeyDown(KeyCode.Q)) 
        {
            if (!estadoCaidaLevantarse)
            {
                Caerse();
            }
        }
        */

        if(puedeLevantarse && !desmayado) 
        {
            if (Input.GetAxis("Vertical") > 0.5)
            {
                if (!levantandose)
                {
                    Levantarse();
                }
            }
        }

        if(levantandose)
        {
            if(camaraFP.runSpeed < 1) camaraFP.runSpeed += Time.deltaTime * levantarseSpeed;
            else { FinalizarLevantarse(); }
        }

    }

    public Vector3 puntoDelante;

    private void FixedUpdate()
    {
        if (estadoCaidaLevantarse)
        {
            if (cayendose && !puedeLevantarse)
            {
                LookAt(puntoDelante, camara.transform, 10);
                camaraFP.freezeCamera = true;
            }
            else
            {
                camaraFP.freezeCamera = false;
            }
        }
    }

    Vector3 GenerarPuntoDelante()
    {
        // Calcular el punto frente a la cámara utilizando su dirección hacia adelante
        //return Instantiate(prefabReference,transform.position + transform.forward * 10, Quaternion.identity);
        return transform.position + (transform.forward * 30) + (transform.up * 1.5f);
    }

    public void LookAt(Vector3 puntoObjetivo, Transform target, float rotSpeed)
    {
        // Calcular la rotación objetivo para mirar hacia el punto objetivo
        Quaternion rotacionObjetivo = Quaternion.LookRotation(puntoObjetivo - target.position);

        // Rotar suavemente hacia la rotación objetivo usando Slerp
        target.rotation = Quaternion.Slerp(target.rotation, rotacionObjetivo, Time.deltaTime * rotSpeed);
    }


    public float timeRandom;

    public float posibilidades;

    public float runSpeed;

    public void ControlarCaida()
    {
        if(camaraFP.runing)
        {
            if (Time.frameCount % timeRandom == 0)
            {
                int aleatorio = Random.Range(0, 101);

                if (aleatorio < posibilidades) 
                {
                    runSpeed = camaraFP.runSpeed;
                    //if(runSpeed > 1) runSpeed = 1;
                    //if(runSpeed == 0.6f) runSpeed = 0.4f;

                    Caerse();
                    camaraFP.runing = false;

                }
            }
        }
    }

    public void Caerse()
    {
        ray.enElSuelo = true;
        cayendose = true;
        levantandose = false;
        estadoCaidaLevantarse = true;
        moviendoseCaida = true;
        forwardCaida = transform.forward;
        puedeLevantarse = false;

        camaraFP.runSpeed = 0.01f;
        camaraFP.freezeRun = true;

        caidaSpeedInicial = caidaSpeed;

        puntoDelante = GenerarPuntoDelante();

        //Invoke(nameof(DetenerMoverseCaida), 1);

    }

    public void Desmayarse()
    {
        ray.enElSuelo = true;
        cayendose = true;
        levantandose = false;
        estadoCaidaLevantarse = true;
        moviendoseCaida = true;
        forwardCaida = transform.forward;
        puedeLevantarse = false;
        desmayado = true;

        camaraFP.runSpeed = 0.01f;
        camaraFP.freezeRun = true;

        caidaSpeedInicial = caidaSpeed;

        puntoDelante = GenerarPuntoDelante();

        //Invoke(nameof(DetenerMoverseCaida), 1);

    }

    public void DetenerMoverseCaida()
    {
        moviendoseCaida = false;
    }

    public void Levantarse()
    {
        levantandose = true;
        cayendose = false;
        puedeLevantarse = false;
    }

    public void FinalizarLevantarse()
    {
        camaraFP.runSpeed = 1;
        estadoCaidaLevantarse = false;
        levantandose = false;
        puedeLevantarse = false;
        cayendose = false;

        ray.enElSuelo = false;
        camaraFP.freezeRun = false;

        caidaSpeed = caidaSpeedInicial;
         
    }
}
