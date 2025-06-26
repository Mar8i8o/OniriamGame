using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Coche : MonoBehaviour
{
    public GameObject poscionFinal;
    public float distance;
    public float distancePuerta;

    public GameObject madre;
    public MadreController madreController;
    public NavMeshAgent agent;

    public GameObject posicionSalidaPrincipal;
    public GameObject posicionSalidaPrincipal2;
    public GameObject posicionSalidaPrincipal3;

    public GameObject posicionPlayer;

    public GameObject player;

    public Animator animPuertaCochePrincipal;
    public Animator animPuertaCocheAtras;
    public Animator madreAnim;

    bool animationStart;
    bool madreSaliendo;
    bool recogiendoBebe;

    bool moviendoMadre;

    public SillaCoche sillaCoche;
    public CharacterController characterController;

    void Start()
    {
        player = GameObject.Find("Player");
        madreAnim.SetBool("Sit", true);
        agent.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector3.Distance(poscionFinal.transform.position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, poscionFinal.transform.position, 0.01f);



        if (madreSaliendo)
        {
            madre.transform.position = Vector3.MoveTowards(madre.transform.position, posicionSalidaPrincipal.transform.position, 0.006f);
            madreController.LookTo(posicionSalidaPrincipal.transform);
        }

        if (distance < 1 && !animationStart)
        {
            animationStart = true;
            Invoke(nameof(AbrirPuerta), 1);
            Invoke(nameof(AnimLevantarse), 1);
            Invoke(nameof(MoverMadre), 3f);
            //Invoke(nameof(RecogerBebe), 12.4f);

        }

        if (moviendoMadre)
        {
            distancePuerta = Vector3.Distance(posicionSalidaPrincipal2.transform.position, madre.transform.position);
            if(distancePuerta < 1)
            {
                print("HaLlegado");
                Invoke(nameof(RecogerBebe), 0.5f);
                moviendoMadre = false;
            }
        }
        if (recogiendoBebe)
        {
            madreController.LookTo(posicionPlayer.transform);
            //agent.SetDestination(posicionSalidaPrincipal3.transform.position);
        }

    }

    public void RecogerBebe()
    {
        recogiendoBebe = true;
        animPuertaCocheAtras.SetBool("Open", true);
        agent.SetDestination(posicionSalidaPrincipal3.transform.position);

        Invoke(nameof(AnimCogerBebe),1.5f);

    }

    public void AnimCogerBebe()
    {
        madreController.anim.SetBool("HasBaby", true);
        Invoke(nameof(CogerBebe), 0.2f);
    }

    public void CogerBebe()
    {
        agent.enabled = false;
        characterController.enabled = true;
        recogiendoBebe = false;
        sillaCoche.sentado = false;
        madreController.carringBaby = true;
        //player.transform.SetParent(madre.transform);
    }

    public void AbrirPuerta()
    {
        animPuertaCochePrincipal.SetBool("Open", true);
    }

    public void SetMadreSaliendo()
    {
        madreSaliendo = true;
    }

    public void AnimLevantarse()
    {
        madreAnim.SetBool("Sit", false);
        Invoke(nameof(SetMadreSaliendo), 0.1f);
    }

    public void MoverMadre()
    {
        madre.transform.SetParent(null);
        madreSaliendo = false;
        agent.enabled = true;
        moviendoMadre = true;
        agent.SetDestination(posicionSalidaPrincipal2.transform.position);
    }
}
