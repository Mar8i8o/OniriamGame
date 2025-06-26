using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MovimientoEnemy : MonoBehaviour
{
    public GameObject destino;
    CamaController camaController;
    public NavMeshAgent agent;
    public GameObject finalPasillo;
    public GameObject player;
    public Animator npcAnim;
    public AudioSource sonidoPasos;
    public float distance = 10;
    public bool desapareceAlLlegar;
    public bool teMira;
    bool yendoHaciTi;

    void Start()
    {
        player = GameObject.Find("Player");
        camaController = GameObject.Find("CamaTrigger").GetComponent<CamaController>();
        yendoHaciTi = true;     
    }

    float tiempoMirandote;
    void Update()
    {
        agent.SetDestination(destino.transform.position);
        
        if (agent.velocity.magnitude < 0.18f)
        {
            npcAnim.SetBool("Walking", false);

            sonidoPasos.Stop();

            if (teMira)
            {
                distance = Vector3.Distance(transform.position, destino.transform.position);
                LookTo(player.transform);
                tiempoMirandote += 1 * Time.deltaTime;

                if (tiempoMirandote > 10) { destino = finalPasillo; yendoHaciTi = false; sonidoPasos.Play(); }
            }

        }
        else
        {
            if (!sonidoPasos.isPlaying) { sonidoPasos.Play(); }
            npcAnim.SetBool("Walking", true);
        }


        /////////////////

        if (desapareceAlLlegar || (teMira && !yendoHaciTi)) 
        {
            distance = Vector3.Distance(transform.position, destino.transform.position);
            
            if (distance < 2) { Destruir(); }
        }

        if(teMira)
        {
            if (yendoHaciTi)
            {
                //LookTo(player.transform);
            }
        }

       
    }



    public void Destruir()
    {
        camaController.alguienEnLaCasa = false;
        Destroy(gameObject);
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
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3 * Time.deltaTime);
    }

}
