using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SillaCoche : MonoBehaviour
{
    public GameObject player;

    public GameObject positionSilla;
    public GameObject positionSillaUp;

    public GameObject posicionPlayerActual;

    public bool sentado;

    public bool puedeMoverse;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        float imput;
        if (puedeMoverse) imput = Input.GetAxis("Horizontal") / 8;
        else imput = 0;

        player.transform.position = Vector3.MoveTowards(player.transform.position, posicionPlayerActual.transform.position + new Vector3(imput, 0, 0), 0.004f);

        if (puedeMoverse)
        {
            if (Input.GetAxis("Vertical") <= 0) { posicionPlayerActual = positionSilla; print("posicionBaja"); }
            else posicionPlayerActual = positionSillaUp;
        }
        else
        {
            posicionPlayerActual = positionSilla;
        }


    }
}
