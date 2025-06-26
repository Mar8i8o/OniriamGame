using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotCuadricula : MonoBehaviour
{
    public Image image;
    public ButtonScreen buttonScreen;

    public CuadriculaController controller;

    public ButtonScreen butonScreen;

    public bool selected;

    public TextMeshProUGUI txt;

    private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public string letra;

    public int numFilaVer;
    public int numFilaHor;

    public int indiceFilaVer;
    public int indiceFilaHor;

    public bool especial;

    void Awake()
    {
        LetraAleatoria();
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonScreen.inButton) 
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                CambiarColor();
            }
        }

        if (selected)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                RecuperarColor();
            }
        }

        txt.text = letra;
    }

    

    public void LetraAleatoria()
    {
        letra = GetRandomLetter().ToString();
        especial = false;
    }

    char GetRandomLetter()
    {
        // Obtiene una letra aleatoria del abecedario
        int randomIndex = Random.Range(0, alphabet.Length);
        return alphabet[randomIndex];
    }

    public void CambiarColor()
    {
        if (!selected)
        {
            image.color = Color.green;
            selected = true;
            controller.slotsSeleccionados++;

            controller.winComprobada = false;

            if (especial)
            {
                controller.correctSlots++;
            }
        }
    }

    public void RecuperarColor()
    {
        image.color = Color.white;
        selected = false;
        controller.ComprobarWin();
        //controller.slotsSeleccionados = 0;
    }

}
