using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlarCaerse : MonoBehaviour
{
    public GameObject puntoCaerse;
    public GameObject camara;

    public CamaraFP camaraFP;
    public Raycast ray;
    public GameObject player;
    public CharacterController characterController;
    public Rigidbody rb;

    public BrazoController brazoConReloj;


    public bool caerse;
    public bool levantandose;
    public bool enElSuelo;

    public float speed;
    public float tiempo;

    public Vector3 rotacionInicial;
    public Vector3 rotacionInicialCamara;
    public Vector3 posicionInicial;


    public Vector3 rotacionCaido;
    void Start()
    {
        
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Q)) 
        {
            if (!levantandose)
            {
                if (!caerse)
                {
                    Caerse();
                }
                else
                {
                    Levantarse();
                }
            }
        }

        if (levantandose)
        {
            if (camaraFP.runSpeed < 1) { camaraFP.runSpeed += Time.deltaTime; }
        }
        if(enElSuelo) 
        {
            if(Input.GetAxis("Vertical") > 0.5)
            {
                Levantarse();
            }
        }
    }


    private void FixedUpdate()
    {
        if (levantandose)
        {
            MoverYRotarHaciaObjetivo(posicionInicial, new Vector3(0, rotacionInicial.y, 0), player.transform);
            //RotarHaciaObjetivo(new Vector3(rotacionInicialCamara.x, rotacionInicialCamara.y,0), camara.transform);

            //camara.transform.LookAt(new Vector3(camara.transform.position.x, camara.transform.position.y, camara.transform.position.z));
            LookAt(puntoObjetivo, camara.transform, 10);

        }

        if (caerse)
        {
            //camara.transform.position = Vector3.MoveTowards(camara.transform.position, puntoCaerse.transform.position, speed * Time.deltaTime);
            //RotarHaciaObjetivo(new Vector3(0,rotacionInicialCamara.y, 0), camara.transform);
            LookAt(puntoObjetivo, camara.transform, 10);
        }
    }

    public void Caerse()
    {
        ray.enElSuelo = true;
        caerse = true;
        camaraFP.runSpeed = 0.1f;

        camaraFP.freezeCamera = true;

        rotacionInicial = player.transform.eulerAngles;
        posicionInicial = player.transform.position;
        rotacionInicialCamara = camara.transform.eulerAngles;

        camaraFP.freeze = true;
        characterController.enabled = false;
        rb.isKinematic = false;

        rb.AddForce(transform.forward * 1.4f, ForceMode.Impulse);
        brazoConReloj.puedeSacarBrazo = false;

        rb.constraints = RigidbodyConstraints.FreezeRotationY;

        puntoObjetivo = GenerarPuntoDelante();

        Invoke(nameof(SetEnElSuelo), 2);

    }

    public Vector3 puntoObjetivo;
    public GameObject prefabReference;
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

    public void SetEnElSuelo()
    {
        enElSuelo = true;
        //caerse = false;
        //camaraFP.freezeCamera = false;
    }

    public void Levantarse()
    {
        caerse = false;
        levantandose = true;
        enElSuelo = false;

        characterController.enabled = true;
        rb.isKinematic = true;
        camaraFP.freeze = false;
        camaraFP.freezeCamera = false;

        Invoke(nameof(DejarDeLevantarse),tiempo);
    }

    public void DejarDeLevantarse()
    {
        camaraFP.runSpeed = 1;
        camaraFP.freezeCamera= false;
        levantandose = false;

        player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0);
    }

    public float umbralRotacion = 0.1f;
    public float umbralPosicion = 0.1f;
    public bool movimientoFinalizado;

    void MoverYRotarHaciaObjetivo(Vector3 posicionObjetivo, Vector3 rotacionObjetivo, Transform target)
    {
        /*
        // Mover el target hacia la posición objetivo de forma fluida
        target.position = Vector3.Lerp(target.position, posicionObjetivo, Time.deltaTime * speed);

        // Convertir los ángulos de Euler objetivo a una Quaternion
        Quaternion rotacionObjetivoQuat = Quaternion.Euler(rotacionObjetivo);

        // Rotar el target hacia la rotación objetivo de forma fluida
        target.rotation = Quaternion.Slerp(target.rotation, rotacionObjetivoQuat, Time.deltaTime * speed);
        */

        ////////////////////////


        // Mantiene las posiciones actuales en X y Z, y mueve solo en Y
        Vector3 nuevaPosicion = new Vector3(
            target.position.x,
            Mathf.MoveTowards(target.position.y, posicionObjetivo.y, Time.deltaTime * speed),
            target.position.z
        );

        target.position = nuevaPosicion;

        // Convertir los ángulos de Euler objetivo a una Quaternion
        Quaternion rotacionObjetivoQuat = Quaternion.Euler(rotacionObjetivo);

        // Rotar el target hacia la rotación objetivo de forma fluida
        target.rotation = Quaternion.Slerp(target.rotation, rotacionObjetivoQuat, Time.deltaTime * speed);


    }

    void RotarHaciaObjetivo(Vector3 rotacionObjetivo, Transform target)
    {
        // Convertir los ángulos de Euler objetivo a una Quaternion
        Quaternion rotacionObjetivoQuat = Quaternion.Euler(rotacionObjetivo);

        // Rotar el target hacia la rotación objetivo de forma fluida
        target.rotation = Quaternion.Slerp(target.rotation, rotacionObjetivoQuat, Time.deltaTime * speed);

    }

    }

