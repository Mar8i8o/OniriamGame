using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMovilController : MonoBehaviour
{
    public bool selecionado;
    public float tiempoSeleccionado;
    public Button button;

    public Raycast playerScript;
    LlamadasController controller;

    public bool menuInicio;
    public MovilController controllerInicio;

    void Start()
    {
        playerScript = GameObject.Find("Main Camera").GetComponent<Raycast>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.usingMovil) button.interactable = true;
        else button.interactable = false;

        if (selecionado) 
        {
            tiempoSeleccionado += Time.deltaTime;

            if (tiempoSeleccionado > 0.1f)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    button.onClick.Invoke();
                }
            }
        }

        if(EventSystem.current.currentSelectedGameObject == button.gameObject)
        {
            selecionado = true;
        }
    }

    private void OnDisable()
    {
        selecionado = false;
    }

    public void Seleccionar()
    {
        selecionado = true;
        tiempoSeleccionado = 0;

        if (menuInicio) { controllerInicio.seleccionar = gameObject; }
    }

    public void Deseleccionar()
    {
        Invoke(nameof(DeseleccionarDelay), 0.1f);
    }

    public void DeseleccionarDelay()
    {
        selecionado = false;
        tiempoSeleccionado = 0;
    }

    public void Click()
    {
        print("click");

        if (menuInicio) { controllerInicio.seleccionar = gameObject; }
    }
}
