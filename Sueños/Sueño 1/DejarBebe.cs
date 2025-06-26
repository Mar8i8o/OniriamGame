using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DejarBebe : MonoBehaviour
{
    GameObject player;
    public GameObject puntoBebe;
    public GameObject puntoFuera;

    public bool dejarBebe;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (dejarBebe)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, puntoBebe.transform.position, 0.01f);
        }
    }
}
