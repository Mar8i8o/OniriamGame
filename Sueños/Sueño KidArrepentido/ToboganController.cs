using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToboganController : MonoBehaviour
{
    public GameObject player;
    public DetectarObjetoCabeza detectarObjetoCabeza;
    public Raycast ray;

    ToboganFollowPoints toboganFollowPoints;
    public GameObject target;
    public GameObject terreno;
    public Rigidbody playerRb;
    public Collider playerCol;
    public CharacterController playerCC;
    public CamaraFP camaraFP;

    public bool usandoTobogan;

    public Light luzSolLight;



    void Start()
    {
        toboganFollowPoints = target.GetComponent<ToboganFollowPoints>();
    }

    // Update is called once per frame
    void Update()
    {
        if (usandoTobogan)
        {
            //player.transform.position = Vector3.MoveTowards(player.transform.position, target.transform.position, 50 * Time.deltaTime);
            player.transform.position = target.transform.position;
            detectarObjetoCabeza.objetoEncima = true;

            if (luzSolLight.intensity >= 0) luzSolLight.intensity -= Time.deltaTime * 20;

        }
    }

    public void MontarTobogan()
    {
        usandoTobogan = true;
        ray.usandoTobogan = true;
        terreno.SetActive(false);
        //luzSol.SetActive(false);

        camaraFP.freeze = true;
        //playerRb.isKinematic = true;
        playerCol.isTrigger = true;
        //playerCC.enabled = false;

        target.transform.position = player.transform.position;

        toboganFollowPoints.IniciarRecorrido();
    }

    public GameObject coliderTobogan;
    public void DesmontarTobogan()
    {
        usandoTobogan = false;
        ray.usandoTobogan = false;
        terreno.SetActive(false);
        detectarObjetoCabeza.objetoEncima = false;

        camaraFP.freeze = false;
        //playerRb.isKinematic = false;
        playerCol.isTrigger = false;
        //playerCC.enabled = false;
        coliderTobogan.SetActive(true);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            MontarTobogan();
        }
    }
}
