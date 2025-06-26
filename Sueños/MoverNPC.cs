using UnityEngine;
using UnityEngine.AI;

public class MoverNPC : MonoBehaviour
{

    public bool freeze;
    public bool freezePos;

    public Animator npcAnim;
    public NpcDialogue scriptNpcDialogo;
    public NavMeshAgent agent;

    public Transform destino;

    public bool persiguiendoPlayer;
    public bool playerOculto;

    public bool patrullando;
    public bool esperando;

    [HideInInspector]public TaquillaController taquillaController;
    public Transform posicionPatrulla; //DONDE SE VA SI ESTAS OCULTO
    public Transform posicionEspera; //DONDE ESPERA LOS PRIMEROS SEGUNDOS SI TE OCULTAS

    GameObject player;
    PlayerVida playerVida;

    void Start()
    {
        player = GameObject.Find("Player");
        playerVida = player.GetComponent<PlayerVida>();
    }

    void Update()
    {
        
        if(freeze) 
        {
            if(npcAnim != null)npcAnim.SetBool("Walking", false);
            return;
        }

        if (!freezePos) { agent.SetDestination(destino.position); }
        else if (freezePos) { agent.SetDestination(transform.position); }


        distanciaObjetivo = Vector3.Distance(destino.position, transform.position);

        if (agent.velocity.magnitude < 0.15f)
        {
            npcAnim.SetBool("Walking", false);
            scriptNpcDialogo.canTalk = true;

        }
        else
        {
            npcAnim.SetBool("Walking", true);
            scriptNpcDialogo.canTalk = false;
        }

        if (persiguiendoPlayer) { PersiguiendoPlayer(); }

        npcAnim.SetBool("Run", runing);

    }

    public float distanciaObjetivo;
    public bool runing;
    public bool sabeDondeEsta;
    public DetectarCampoVision detectarCampoVision;

    public float tiempoEsperando;

    public void PersiguiendoPlayer()
    {
        npcAnim.SetBool("Buscando", esperando);

        if(agent.remainingDistance > 20 && !runing && sabeDondeEsta && !freezeRun) //CORRER
        {
            runing = true;
            agent.speed = 6;
        }

        if(runing) //DEJAR DE CORRER
        {
            if(distanciaObjetivo < 4) 
            {
                runing = false;
                agent.speed = 2;
            }
        }

        if (freezeAtack) //MIRAR AL PLAYER CUANDO ATACA
        { 
            ForzarMiradaY(player.transform, 10f);
            print("ForzandoMiradaHaciaPlayer");
        }


        if (!playerOculto) //ATACAR
        {
            if (distanciaObjetivo < 1.7f)
            {
                if (!freezeAtack && sabeDondeEsta)
                {
                    Atacar();
                }
            }
        }
        else if(sabeDondeEsta && !freezeAtack && !taquillaController.abierto) //ABRIR PUERTA DE LA TAQUILLA Y ATACAR
        {
            if (distanciaObjetivo < 1)
            {
                if (!taquillaController.abierto)
                {
                    Invoke(nameof(Atacar), 1);
                    esperando = false;
                    taquillaController.SetAbrirTaquilla();
                }
            }
        }

        if(esperando && !sabeDondeEsta) //DEJAR DE ESPERAR Y PASAR A PATRULLAR
        {
            if (distanciaObjetivo < 1)
            {
                tiempoEsperando += Time.deltaTime;

                if (tiempoEsperando > 10)
                {
                    DejarDeEsperar();
                }
            }

        }

        if(playerOculto) 
        {
            if (!sabeDondeEsta) //NO SABE QUE EL PLAYER ESTA EN LA TAQUILLA
            {
                if (!esperando && !patrullando)
                {
                    esperando = true;
                    freezeAtack = false;
                    freezePos = false;
                    //Invoke(nameof(DejarDeEsperar), 10);
                    destino = posicionEspera;
                    agent.stoppingDistance = 0;
                    tiempoEsperando = 0;
                }
                else if (patrullando)
                {
                    destino = posicionPatrulla;
                }

                if (esperando && posicionEspera != null) { destino = posicionEspera; }
                else if (patrullando && posicionPatrulla != null) { destino = posicionPatrulla; }
            }
            else //SABE QUE EL PLAYER ESTA EN LA TAQUILLA
            {
                destino = taquillaController.posicionDelanteTaquilla.transform;
                agent.stoppingDistance = 0;
                esperando = true;
                ForzarMiradaY(player.transform, 3f);
            }

        }
        else if(!sabeDondeEsta) //DETECTAR AL PLAYER
        {
            if (detectarCampoVision.viendoPlayer)
            {
                patrullando = false;
                destino = player.transform;
                sabeDondeEsta = true;
                esperando = false;
            }
            else if (Vector3.Distance(transform.position, player.transform.position) < 3 && !playerOculto)
            {
                patrullando = false;
                destino = player.transform;
                sabeDondeEsta = true;
                esperando = false;
            }
        }
    }

    public void DejarDeEsperar()
    {
        CancelInvoke(nameof(DejarDeEsperar));
        esperando = false;
        patrullando = true;
        tiempoEsperando = 0;
        runing = false;
    }

    public void SalirTaquilla()
    {
        agent.stoppingDistance = 1.2f;
        esperando = false;

        if(detectarCampoVision.viendoPlayer || Vector3.Distance(transform.position, player.transform.position) < 3 || esperando)
        {
            sabeDondeEsta = true;
            destino = player.transform;
            patrullando = false;
        }
        else
        {
            patrullando = true;
            sabeDondeEsta = false;
        }

    }

    bool freezeAtack;
    public bool freezeRun;

    public void Atacar()
    {
        npcAnim.SetTrigger("Atack");
        //playerVida.RecibirDanyo();
        freezeAtack = true;
        freezePos = true;

        Invoke(nameof(UnFreezeAtack), 2);

    }

    public void RecibirHit()
    {
        npcAnim.SetTrigger("Hit");
        freezeAtack = true;
        freezePos = true;

        Invoke(nameof(UnFreezeAtack), 1);
    }

    public void UnFreezeAtack()
    {
        CancelInvoke(nameof(UnFreezeAtack));
        freezeAtack = false;
        freezePos = false;
    }

    public void ForzarMiradaY(Transform dondeMira, float speed)
    {
        // Obtén la dirección hacia el objetivo
        Vector3 targetDirection2 = dondeMira.transform.position - transform.position;
        targetDirection2.y = 0f; // Asegúrate de que no haya rotación vertical

        // Calcula la rotación necesaria para mirar hacia el objetivo solo en el eje Y
        Quaternion targetRotation2 = Quaternion.LookRotation(targetDirection2);

        // Ajusta la rotación para que solo afecte el eje Y
        targetRotation2 = Quaternion.Euler(0f, targetRotation2.eulerAngles.y, 0f);

        // Aplica la rotación de forma suavizada
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation2, speed * Time.deltaTime);
    }

    public bool ComprobarSabeDondeEsta()
    {
        if(detectarCampoVision.viendoPlayer || distanciaObjetivo < 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
