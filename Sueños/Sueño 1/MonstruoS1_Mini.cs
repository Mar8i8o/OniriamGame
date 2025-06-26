using UnityEngine;
using UnityEngine.AI;

public class MonstruoS1_Mini : MonoBehaviour
{

    NavMeshAgent agent;
    GameObject player;

    public Transform trailRender;

    public bool aplastado;

    public GameObject meshNormal;
    public GameObject meshAplastado;

    public AudioSource sonidoArrastrarse;
    public AudioSource sonidoSplash;


    private void Awake()
    {
        player = GameObject.Find("Player");
        agent = gameObject.GetComponent<NavMeshAgent>();


        meshAplastado.SetActive(false);
        meshNormal.SetActive(true);
    }

    public void Update()
    {
        if (!aplastado)
        {
            agent.SetDestination(player.transform.position);
            trailRender.transform.position = transform.position;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!aplastado)
        {
            if (other.transform.CompareTag("Player"))
            {
                sonidoArrastrarse.Stop();
                sonidoSplash.Play();
                aplastado = true;

                meshNormal.SetActive(false);
                meshAplastado.SetActive(true);

                agent.enabled = false;
            }
        }

    }
}
