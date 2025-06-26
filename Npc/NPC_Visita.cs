using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Visita : MonoBehaviour
{
    public NavMeshAgent agent;
    GameObject puntoPosicionPuertaPrincipal;
    GameObject puntoPosicionSalida;
    public NpcDialogue scriptNpcDialogo;
    SceneDialogsController sceneDialogsController;

    public Animator npcAnim;

    public bool seVa;
    public bool llegando;

    DoorController puertaPrincipal;
    public float tiempoEsperando;


    public Collider col;

    GameObject player;


    void Start()
    {
        player = GameObject.Find("Player");
        puertaPrincipal = GameObject.Find("TriggerPomoPrincipal").GetComponent<DoorController>();
        puntoPosicionPuertaPrincipal = GameObject.Find("PuntoPosicionPuertaPrincipal");
        puntoPosicionSalida = GameObject.Find("PuntoSpawnNPC");
        sceneDialogsController = GameObject.Find("GameManager").GetComponent<SceneDialogsController>();

        llegando = true;

        puertaPrincipal.tiempoTocando = 8;
        tiempoEsperando = 0;

    }

    // Update is called once per frame
    void Update()
    {

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
            Destroy(gameObject);
        }
    }

    public void SetMarcharse()
    {
        seVa = true;
        llegando = false;
    }
}
