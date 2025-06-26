using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpujaPelotas : MonoBehaviour
{
    GameObject player;

    public float distanceSqr;

    public DoorController doorController;

    float tiempoPuertaAbierta;

    bool active;

    public bool seDestruye;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

        distanceSqr = (player.transform.position - transform.position).sqrMagnitude;       
        active = distanceSqr < 36;


        if (seDestruye)
        {

            if (doorController.puertaAbierta)
            {
                tiempoPuertaAbierta += Time.deltaTime;

                if (tiempoPuertaAbierta > 1)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (active)
        {
            if (other.transform.CompareTag("Pelota"))
            {
                print("EmpujarPelota");
                other.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
