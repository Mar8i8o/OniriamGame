using UnityEngine;
using UnityEngine.AI;

public class GuardianLaberinto : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    Animator npcAnim;
    PlayerStats playerStats;
    DreamController dreamController;

    public bool enfadado;
    public bool temblando;

    bool sinMascara;

    public GameObject mascara;
    public GameObject cuerdaMascara;

    public Rigidbody rbMascara;

    LaberintoController laberintoController;
    ParpadeoController parpadeoController;

    void Start()
    {
        player = GameObject.Find("Player");
        parpadeoController = GameObject.Find("GameManager").GetComponent<ParpadeoController>();
        dreamController = GameObject.Find("GameManager").GetComponent<DreamController>();
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
        laberintoController = GameObject.Find("LaberintoParent").GetComponent<LaberintoController>();
        agent = GetComponent<NavMeshAgent>();
        npcAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);

        if (laberintoController.caminoErroneo) { enfadado = true; }

        if (agent.velocity.magnitude < 0.15f)
        {
            npcAnim.SetBool("Walking", false);

            if (!temblando && enfadado)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < 10)
                {
                    temblando = true;
                }
            }

        }
        else
        {
            npcAnim.SetBool("Walking", true);
            npcAnim.SetBool("Temblar", false);
            temblando = false;
        }

        if(enfadado)
        {
            if(!sinMascara)
            {
                QuitarMascara();
            }

            if(temblando)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < 4)
                {
                    //print("PlayerEliminado");
                    //playerStats.NoquearPlayer();
                    if (!parpadeoController.cerrandoOjos)
                    {
                        parpadeoController.SetCerrarOjos(300);
                        dreamController.LlamarDespertarseApartamento(5);
                    }
                }
            }

            npcAnim.SetBool("Temblar", temblando);

        }

        
    }

    public void QuitarMascara()
    {
        Destroy(cuerdaMascara);
        mascara.transform.SetParent(null);
        mascara.transform.tag = "GuardianLaberinto";
        rbMascara.isKinematic = false;
    }
}
