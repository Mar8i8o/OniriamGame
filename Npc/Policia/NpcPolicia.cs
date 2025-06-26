using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class NpcPolicia : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject puntoPosicionPuertaPrincipal;
    public GameObject puntoPosicionSalida;
    public NpcDialogue scriptNpcDialogo;
    SceneDialogsController sceneDialogsController;
    PoliciaController policiaController;

    public Animator npcAnim;

    public bool seVa;
    public bool llegando;

    public DoorController puertaPrincipal;

    public float tiempoEsperando;

    public bool teArresta;

    public Collider col;

    public GameObject player;


    void Start()
    {
        player = GameObject.Find("Player");
        puertaPrincipal = GameObject.Find("TriggerPomoPrincipal").GetComponent<DoorController>();
        puntoPosicionPuertaPrincipal = GameObject.Find("PuntoPosicionPuertaPrincipal");
        puntoPosicionSalida = GameObject.Find("PuntoSpawnNPC");
        sceneDialogsController = GameObject.Find("GameManager").GetComponent<SceneDialogsController>();
        policiaController = GameObject.Find("GameManager").GetComponent<PoliciaController>();

        if (policiaController.numVecesLlamadoPolicia == 1) scriptNpcDialogo.idDialogo = "policia_1";
        if (policiaController.numVecesLlamadoPolicia > 1) scriptNpcDialogo.idDialogo = "policia_2";

        llegando = true;

        puertaPrincipal.tiempoTocando = 8;
        tiempoEsperando = 0;

        if(policiaController.numVecesLlamadoPolicia >= 3) { teArresta = true; col.enabled = false; }

    }

    // Update is called once per frame
    void Update()
    {

        if(corriendoArrestado || arrestado) { return; }

        if (!seVa) agent.SetDestination(puntoPosicionPuertaPrincipal.transform.position);
        if (seVa && !sceneDialogsController.dialogueActive) Marcharse();

        if (agent.velocity.magnitude < 0.15f)
        {
            npcAnim.SetBool("Walking", false);
            scriptNpcDialogo.canTalk = true;

            LookTo(puertaPrincipal.puntoMirarPuerta);

            if (llegando && !puertaPrincipal.puertaAbierta) //ESPERANDO
            {
                print("LamarTimbre");
                puertaPrincipal.SpamTocarTombre();
                tiempoEsperando += Time.deltaTime;

                if (tiempoEsperando > 120)
                {
                    SetMarcharse();
                }
            }
        }
        else
        {
            npcAnim.SetBool("Walking", true);
            scriptNpcDialogo.canTalk = false;
        }

    }

    private void LateUpdate()
    {
        if (teArresta) { ControlarArrestar(); }
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

    public void Marcharse()
    {
        agent.SetDestination(puntoPosicionSalida.transform.position);

        float distancia = Vector3.Distance(gameObject.transform.position, puntoPosicionSalida.transform.position);

        if (distancia < 1) 
        {
            policiaController.policiaActivo = false;
            policiaController.puedesLlamarEmergencias = true;
            Destroy(gameObject);
        }
    }

    public void SetMarcharse()
    {
        seVa = true;
        llegando = false;
    }

    #region LOGICA_ARRESTAR


    public AudioSource sonidoEsposas;

    bool corriendoArrestado;
    bool arrestado;
    Vector3 posicionObjetivo;
    Vector3 puntoDelante;

    public void ControlarArrestar()
    {
        if (policiaController.detectarPlayer.playerDentro)
        {
            
            if (!corriendoArrestado)
            {
                corriendoArrestado = true;
                //agent.enabled = false;
                npcAnim.SetBool("Runing", true);
                puntoDelante = GenerarPuntoDelante();
                posicionObjetivo = new Vector3(puntoDelante.x, transform.position.y, puntoDelante.z);

                agent.speed = 10;
                agent.angularSpeed = 700;
                agent.acceleration = 150;

                policiaController.sonidoSusto.Play();

            }

        }
        else
        {
            
            if (!policiaController.puertaPrincipal.puertaAbierta)
            {
                corriendoArrestado = false;
                npcAnim.SetBool("Runing", false);
            }
            
        }

        if(corriendoArrestado)
        {
            //transform.position = Vector3.MoveTowards(transform.position, posicionObjetivo, 20 * Time.deltaTime);
            agent.SetDestination(GenerarPuntoDelante());
            agent.acceleration = 1000;
            //LookTo(player.transform);

            float distancia = Vector3.Distance(gameObject.transform.position, player.transform.position);

            if(distancia < 1 && !arrestado) 
            {
                Invoke(nameof(Arrestar),0.25f);
                arrestado = true;
            }

        }
        else
        {
            agent.angularSpeed = 180;
        }

    }

    public void Arrestar()
    {
        policiaController.pantallaNegro.SetActive(true);
        sonidoEsposas.Play();

        Invoke(nameof(CambiarEscena),3);
    }

    public void CambiarEscena()
    {
        SceneManager.LoadScene("ManicomioScene");
    }

    Vector3 GenerarPuntoDelante()
    {
        return player.transform.position + (player.transform.forward * 0.6f);
    }

    #endregion

}
