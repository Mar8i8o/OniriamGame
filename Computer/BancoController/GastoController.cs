using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GastoController : MonoBehaviour
{
    public GastoController siguienteGasto;

    public TextMeshProUGUI nombreGastoTXT;
    public TextMeshProUGUI cantidadGastoTXT;

    public string nombreGastoActual;
    public float cantidadGastoActual;

    public float indiceGasto;
    public bool ultimo;

    public bool ingreso;

    public GameObject fondoGasto;
    public GameObject fondoIngreso;

    void Start()
    {
        GetDatos();
    }

    // Update is called once per frame
    void Update()
    {
        nombreGastoTXT.text = nombreGastoActual;
        if (!ingreso)cantidadGastoTXT.text = "-" + cantidadGastoActual + "€";
        else cantidadGastoTXT.text = "+" + cantidadGastoActual + "€";

        fondoGasto.SetActive(!ingreso);
        fondoIngreso.SetActive(ingreso);
    }

    public void EstablecerGasto(string nombreGasto, float cantidadGasto, bool esIngreso)
    {

        if (!ultimo) { siguienteGasto.EstablecerGasto(nombreGastoActual, cantidadGastoActual, ingreso); }

        nombreGastoActual = nombreGasto;
        cantidadGastoActual = cantidadGasto;
        ingreso = esIngreso;
    }

    public void GuardarDatos()
    {
        PlayerPrefs.SetFloat(gameObject.name + "cantidadGasto", cantidadGastoActual);
        PlayerPrefs.SetString(gameObject.name + "nombreGasto", nombreGastoActual);
        PlayerPrefs.SetInt(gameObject.name + "esIngreso", System.Convert.ToInt32(ingreso));
    }

    public void GetDatos()
    {
        if (PlayerPrefs.GetInt(gameObject.name + "esIngreso", System.Convert.ToInt32(ingreso)) == 0) { ingreso = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "esIngreso", System.Convert.ToInt32(ingreso)) == 1) { ingreso = true; }

        cantidadGastoActual = PlayerPrefs.GetFloat(gameObject.name + "cantidadGasto", cantidadGastoActual);
        nombreGastoActual = PlayerPrefs.GetString(gameObject.name + "nombreGasto", nombreGastoActual);

    }
}
