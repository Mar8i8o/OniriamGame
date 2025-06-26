using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class GastosManager : MonoBehaviour
{

    public TextMeshProUGUI dineroTXT;
    public GastoController primerGasto;

    public GastoController[] todosLosGastos;

    GuardarController guardarController;
    PlayerStats playerStats;

    public GameObject prueba;

    

    void Start()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            for (int i = 0; i < todosLosGastos.Length; i++)
            {
                todosLosGastos[i].GuardarDatos();
            }
        }

        dineroTXT.text = playerStats.dinero + "€";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            //nuevoGasto("sexyvoce", 100, false);
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(prueba);
        }
    }

    public void nuevoGasto(string nombre, float cantidad, bool esIngreso)
    {
        primerGasto.EstablecerGasto(nombre, cantidad, esIngreso);
    }

    public string numeroDeCuentaTransferencia;
    public float cantidadTransferencia;

    public TMP_InputField cantidadInput;
    public TMP_InputField cuentaInput;

    public GameObject posicionNotificacionTransferencia;
    public GameObject prefabNotificacionTransferencia;
    public GameObject prefabCantNotificacionTransferencia;
    public GameObject prefabInvalidData;
    public void HacerTransferencia()
    {
        numeroDeCuentaTransferencia = cuentaInput.text;
        if (cantidadInput.text != "") { cantidadTransferencia = Convert.ToInt32(cantidadInput.text); }
        else { cantidadTransferencia = 0; }

        if (cantidadTransferencia != 0 && numeroDeCuentaTransferencia != "")
        {
            if (playerStats.dinero > cantidadTransferencia)
            {
                nuevoGasto("Transferencia", cantidadTransferencia, false);
                playerStats.dinero -= cantidadTransferencia;

                GameObject instancia = Instantiate(prefabNotificacionTransferencia, posicionNotificacionTransferencia.transform.position, Quaternion.identity);
                instancia.transform.SetParent(posicionNotificacionTransferencia.transform);

                cuentaInput.text = "";
                cantidadInput.text = "";
            }
            else
            {
                GameObject instancia = Instantiate(prefabCantNotificacionTransferencia, posicionNotificacionTransferencia.transform.position, Quaternion.identity);
                instancia.transform.SetParent(posicionNotificacionTransferencia.transform);
            }
        }
        else
        {
            GameObject instancia = Instantiate(prefabInvalidData, posicionNotificacionTransferencia.transform.position, Quaternion.identity);
            instancia.transform.SetParent(posicionNotificacionTransferencia.transform);
        }
    }

    public void AbrirTransferencia()
    {
        cuentaInput.text = "";
        cantidadInput.text = "";
    }
}
