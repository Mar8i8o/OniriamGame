using UnityEngine;
using UnityEngine.AI;

public class PerseguirPlayer : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
    }
}
