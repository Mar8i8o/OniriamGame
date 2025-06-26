using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class MadreController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator anim;
    public bool carringBaby;
    public GameObject puntoBebe;
    GameObject player;
    public GameObject parent;
    public GameObject camara;
    public CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    public float speed;
    public float gravity = 9.87F;
    bool walking;
    float tiempoCarringBaby = 0;
    public GameObject centroPoint;
    public GameObject puntoCasa;
    public DejarBebe dejarBebe;

    public CamaraFP camaraFP;

    bool madreYendose;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (!carringBaby)
        {
            if (agent.velocity.magnitude < 0.1f)
            {
                anim.SetBool("Walking", false);
            }
            else
            {
                anim.SetBool("Walking", true);
            }
        }

        if (carringBaby)
        {
            tiempoCarringBaby += Time.deltaTime;

            if (tiempoCarringBaby > 1) 
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, puntoBebe.transform.position, 1f);
                if (tiempoCarringBaby < 4) { LookTo(puntoCasa.transform); print("mirandoCasa"); }
            }
            else
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, puntoBebe.transform.position, 0.01f);
            }

            MovimientoPersonaje();
            anim.SetBool("HasBaby", true);

            float VelX = Input.GetAxis("Horizontal");
            if (bordeJuego)
            {
                if (VelX < 0) VelX = 0;
            }
            float VelY = Input.GetAxis("Vertical");

            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) walking = false;
            else walking = true;


            anim.SetFloat("VelX", VelX);
            anim.SetFloat("VelY", VelY);
            anim.SetBool("Walking", walking);
        }
        else
        {
            anim.SetBool("HasBaby", false);
        }

        if (bordeJuego)
        {
            tiempoBordeJuego += Time.deltaTime;
            LookTo(centroPoint.transform);

            if (tiempoBordeJuego > 1)
            {
                bordeJuego = false;
            }
        }

        if(madreYendose) 
        {
            camaraFP.ForzarMirada(transform, 1);
            camaraFP.freezeCamera = true;
        }
    }

    public void MovimientoPersonaje()
    {
        //if (walking) { parent.transform.eulerAngles = new Vector3(parent.transform.eulerAngles.x, camara.transform.eulerAngles.y, parent.transform.eulerAngles.z); }
        if (!bordeJuego)parent.transform.Rotate(0, Input.GetAxis("Horizontal"), 0);

        if (controller.isGrounded)
        {
            float imputVertical = Input.GetAxis("Vertical");
            if (bordeJuego)
            {
                if (imputVertical < 0) imputVertical = 0;
            }
            moveDirection = new Vector3(0, 0, imputVertical);
            moveDirection = parent.transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                //moveDirection.y = jumpSpeed;
            }

        }
        else
        {
            //moveDirection.x = Input.GetAxis("Horizontal") * speed;
            //moveDirection.z = Input.GetAxis("Vertical") * speed;
            //moveDirection = parent.transform.TransformDirection(moveDirection);
        }
        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);
    }

    public void LookTo(Transform target)
    {
        // Obtener la dirección hacia el objetivo
        Vector3 targetDirection = target.position - parent.transform.position;

        // Calcular la rotación necesaria para mirar hacia el objetivo solo en el eje X
        Quaternion targetRotationX = Quaternion.LookRotation(targetDirection, Vector3.up);

        // Solo rotar en el eje X manteniendo la rotación actual en los otros ejes
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotationX.eulerAngles.y, targetRotationX.eulerAngles.z);

        // Aplicar la rotación de manera suave solo en el eje X
        parent.transform.rotation = Quaternion.Slerp(parent.transform.rotation, targetRotation, 1 * Time.deltaTime);
    }

    public void IrseMadre()
    {
        print("Irse");
        agent.SetDestination(dejarBebe.puntoFuera.transform.position);
        Invoke(nameof(CerrrarPuerta), 4);

        madreYendose = true;
    }

    public DoorController doorController;
    public void CerrrarPuerta()
    {
        doorController.SetCerrarPuerta();
    }

    bool bordeJuego;
    float tiempoBordeJuego;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("BordeJuego"))
        {
            if (carringBaby)
            {
                bordeJuego = true;
                tiempoBordeJuego = 0;
            }
        }
        if (other.transform.CompareTag("BordeJuegoFinal"))
        {
            dejarBebe.dejarBebe = true;
            carringBaby = false;
            controller.enabled = false;
            agent.enabled = true;
            agent.SetDestination(transform.position);
            Invoke(nameof(IrseMadre), 2);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        print("borde");
        if (other.transform.CompareTag("BordeJuego"))
        {
            if (carringBaby)
            {
                bordeJuego = true;
                tiempoBordeJuego = 0;
            }
        }
    }
}
