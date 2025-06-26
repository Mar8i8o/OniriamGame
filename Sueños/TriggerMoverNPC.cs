using UnityEngine;

public class TriggerMoverNPC : MonoBehaviour
{
    public MoverNPC moverNPC;
    public GameObject destinoNPC;

    public bool moviendoNPC;
    public bool usado;

    void Start()
    {
        usado = false;
        moverNPC.gameObject.SetActive(false);
        moverNPC.freeze = true;
    }

    void Update()
    {
        if(moviendoNPC)
        {
            if(Vector3.Distance(moverNPC.gameObject.transform.position, destinoNPC.transform.position) <= 1)
            {
                moverNPC.gameObject.SetActive(false);
                moviendoNPC = false;
                print("DespawnNPC");
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if(!usado)
            {

                usado = true;
                moverNPC.gameObject.SetActive(true);
                moviendoNPC = true;
                print("SpawnEnemigoCarretera");
                moverNPC.gameObject.SetActive(true);
                moverNPC.freeze = false;
                moverNPC.destino = destinoNPC.transform;
            }
        }
    }
}
