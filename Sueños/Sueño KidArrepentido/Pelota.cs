using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pelota : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject player;

    //public LayerMask playerLayer;
    //public float detectionRadius;
    bool playerCerca;

    float distance;

    bool active;

    public Renderer objectRenderer1;
    public Renderer objectRenderer2;
    public Material[] materiales;

    public bool modeloAlto;

    public GameObject lowModel;
    public GameObject highModel;

    public bool debug;

    public float distanceSqr;

    void Start()
    {

        int aleatorio = Random.Range(0, materiales.Length);

        //objectRenderer1.material = materiales[aleatorio];
        objectRenderer2.material = materiales[aleatorio];
        player = GameObject.Find("Player");

        rb.isKinematic = true;
        //rb.gameObject.isStatic = true;
    }

    public void FixedUpdate()
    {

        if (Time.frameCount % 20 == 0)
        {
            distanceSqr = (player.transform.position - transform.position).sqrMagnitude;
            if(rb.linearVelocity.magnitude < 0.2f)rb.isKinematic = distanceSqr > 4f;
        }

    }
    /*
    [ContextMenu(itemName: "NewPosition")]

    public void NewPosition()
    {
        transform.position = Vector3.zero;
        Debug.Log("NewPosition");

        transform.position = new Vector3(PlayerPrefs.GetFloat(gameObject.name + "X", transform.position.x), transform.position.y, transform.position.z);
        transform.position = new Vector3(transform.position.x, PlayerPrefs.GetFloat(gameObject.name + "Y", transform.position.y), transform.position.z);
        transform.position = new Vector3(transform.position.x, transform.position.y, PlayerPrefs.GetFloat(gameObject.name + "Z", transform.position.z));

    }

    [ContextMenu(itemName: "SavePosition")]

    public void SavePosition()
    {
        PlayerPrefs.SetFloat(gameObject.name + "X", transform.position.x);
        PlayerPrefs.SetFloat(gameObject.name + "Y", transform.position.y);
        PlayerPrefs.SetFloat(gameObject.name + "Z", transform.position.z);

    }
    */
}
