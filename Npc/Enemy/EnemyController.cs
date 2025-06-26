using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public float distancia;

    PlayerStats playerStats;

    public Renderer render;

    public bool enemyRespawn;

    [HideInInspector]public CharacterController characterController;
    [HideInInspector]public CamaraFP camaraFP;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public GameObject canvas;
    [HideInInspector] public ParpadeoController parpadeoController;

    public DetectarPlayer detectarPlayer;

    BrazoController brazoConReloj;
    TimeController timeController;

    PoliciaController policiaController;

    NavMeshAgent agent;

    public GameObject puntoEsquina;
    public GameObject puntoPuerta;
    public GameObject puntoEscondido;

    public bool escondido;

    public GameObject puntoMirarPuerta;

    public Animator anim;
    bool walking;
    [HideInInspector]public bool playerNoqueado;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timeController = GameObject.Find("GameManager").GetComponent<TimeController>();
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
        brazoConReloj = GameObject.Find("BrazoConReloj").GetComponent<BrazoController>();
        policiaController = GameObject.Find("GameManager").GetComponent<PoliciaController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerNoqueado)
        {
            if (detectarPlayer.playerDentro)
            {

                distancia = Vector3.Distance(player.transform.position, transform.position);


                if (distancia < 15)
                {
                    playerStats.locuraMomentanea = (((distancia - 20) * -1) * 10);

                    if (distancia < 8)
                    {
                        NoquearPlayer();
                    }

                }
            }
        }
  
        /*
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            NoquearPlayer();
        }
        */        

        ControlarPosiciones();

        if (agent.velocity.magnitude < 0.18f)
        {
            anim.SetBool("Walking", false);
            walking = false;
            //sonidoPasos.Stop();

        }
        else
        {
            //if (!sonidoPasos.isPlaying) { sonidoPasos.Play(); }
            anim.SetBool("Walking", true);
            walking = true;
        }

    }

    bool randomDiario; //CONTROLA GENERAR UN NUMERO RANDOM AL DIA
    public void ControlarPosiciones()
    {
        if (timeController.hora >= 21 || timeController.hora < 6) //VA A LA PUERTA (NOCHE)
        {
            randomDiario = false;
            agent.SetDestination(puntoPuerta.transform.position);
            LookTo(puntoMirarPuerta.transform);
        }
        else // DE DIA
        {

            if(!randomDiario)
            {
                int aleatorio = Random.Range(1, 11);

                if (aleatorio >= 7) { escondido = true; }
                else { escondido = false; }

                randomDiario = true;

                if(timeController.dia == 1 ) { escondido = false; }

            }

            if (policiaController.policiaActivo) { escondido |= true; }

            if (!escondido)
            {
                agent.SetDestination(puntoEsquina.transform.position);
                LookTo(puntoMirarPuerta.transform);
            }
            else
            {
                agent.SetDestination(puntoEscondido.transform.position);
                if (!walking)LookTo(player.transform);
            }
            
        }

    }

    public void LookTo(Transform target)
    {
        // Obtener la dirección hacia el objetivo
        Vector3 targetDirection = target.position - transform.position;

        // Calcular la rotación necesaria para mirar hacia el objetivo solo en el eje X
        Quaternion targetRotationX = Quaternion.LookRotation(targetDirection, Vector3.up);

        // Solo rotar en el eje X manteniendo la rotación actual en los otros ejes
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotationX.eulerAngles.y, targetRotationX.eulerAngles.z);

        // Aplicar la rotación de manera suave solo en el eje X
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
    }

    Vector3 rotacionInicial;
    public GameObject posicionRespawn;
    public void NoquearPlayer()
    {
        playerNoqueado = true;
        rotacionInicial = player.transform.eulerAngles;

        camaraFP.enabled = false;
        characterController.enabled = false;
        rb.isKinematic = false;
        canvas.SetActive(false);

        rb.AddForce(transform.forward * 1, ForceMode.Impulse);
        brazoConReloj.puedeSacarBrazo = false;
        Invoke(nameof(CerrarOjos), 1f);
        Invoke(nameof(Despertarse), 5f);
    }

    public void CerrarOjos()
    {
        parpadeoController.beingKO = true;
        parpadeoController.SetCerrarOjos(200);
    }

    public void Despertarse()
    {
        parpadeoController.SetAbrirOjos(200);

        player.transform.eulerAngles = rotacionInicial;
        player.transform.position = posicionRespawn.transform.position;

        camaraFP.enabled = true;
        characterController.enabled = true;
        rb.isKinematic = true;
        canvas.SetActive(true);

        brazoConReloj.puedeSacarBrazo = true;

        Invoke(nameof(DesactivarBeingKO), 1f);
        playerNoqueado = false;
    }

    public void DesactivarBeingKO()
    {
        parpadeoController.beingKO = false;
    }

}
