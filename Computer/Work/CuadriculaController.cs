
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CuadriculaController : MonoBehaviour
{

    public string palabra;

    public string[] palabras;

    int indicePalabra;

    public int wins;

    public SlotCuadricula[] slots;

    public int numHorizontal;
    public int numVertical;

    public TextMeshProUGUI txtIndicador;
    public TextMeshProUGUI txtFinal;
    public TextMeshProUGUI txtIndicadorRestamtes;

    public SlotCuadricula[] slotsARellenarHor;
    public SlotCuadricula[] slotsARellenarVer;

    public int slotsSeleccionados;

    public GameObject panelCuadricula;
    public GameObject panelEscribirTxt;

    public MouseController mouseController;

    public GameObject panelCompilar;

    public GameObject panelAcabado;

    public bool finalizado;
    public bool compilando;

    Vector3 posicionInicial;

    PlayerStats playerStats;
    GastosManager gastosManager;
    GuardarController guardarController;
    TimeController timeController;

    public int diaWin;

    private void Awake()
    {
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
        gastosManager = GameObject.Find("GameManager").GetComponent<GastosManager>();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        timeController = GameObject.Find("GameManager").GetComponent<TimeController>();

        indicePalabra = 0;

        posicionInicial = txtFondo.transform.position;

        slotsARellenarHor = new SlotCuadricula[numHorizontal];
        slotsARellenarVer = new SlotCuadricula[numVertical];

        wins = PlayerPrefs.GetInt("Wins", 0);
        diaWin = PlayerPrefs.GetInt("DiaWin", 0);
        indicePalabra = PlayerPrefs.GetInt("indicePalabra", 0);
        //timeController.dia 

        //RellenarCuadricula();

        int contadorHor = 0;
        int contadorFilaHor = 1;

        for (int i = 0; i < slots.Length; i++)
        {
            contadorHor++;
            slots[i].indiceFilaHor = contadorHor;
            slots[i].numFilaHor = contadorFilaHor;

            if (contadorHor == 11) { contadorHor = 0; contadorFilaHor++; }
        }

        int contadorVer = 1;
        int contadorFilaVer = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            contadorFilaVer++;
            slots[i].indiceFilaVer = contadorVer;
            slots[i].numFilaVer = contadorFilaVer;

            if (contadorFilaVer == 11) { contadorFilaVer = 0; contadorVer++; }
        }

        //RellenarCuadriculaHor();
        //RellenarCuadriculaVer();


    }
    void Start()
    {

        GenerarNivel();
    }

    public GameObject txtFondo;
    float tiempoAnimacion;

    public Image barraCompilar;
    float cantidadCompilada;
    void FixedUpdate()
    {

        if (finalizado)
        {
            if (diaWin != timeController.dia)
            {
                finalizado = false;
                compilando = false;
                panelCompilar.SetActive(false);
                panelCuadricula.SetActive(true);
                panelAcabado.SetActive(false);

                wins = 0;

                GenerarNivel();
            }
        }

        if (animacionFondo)
        {
            txtFondo.transform.Translate(0, -1, 0);

            tiempoAnimacion += Time.deltaTime;

            if (tiempoAnimacion > 1.5f)
            {
                txtFondo.transform.position = posicionInicial;
                animacionFondo = false;

                if (wins >= 5) ///////////WIN FINAL////////////
                {
                    animacionFondo = false;
                    panelCompilar.SetActive(true);
                    panelEscribirTxt.SetActive(false);

                    diaWin = timeController.dia;

                    compilando = true;
                    finalizado = true;

                    gastosManager.nuevoGasto("Trabajo", 100, true);
                    playerStats.dinero += 100;


                    cantidadCompilada = 0;

                }
                else
                {
                    animacionFondo = false;
                    panelCuadricula.SetActive(true);
                    panelEscribirTxt.SetActive(false);
                    GenerarNivel();
                }
            }
        }

        if (compilando)
        {
            cantidadCompilada += Time.deltaTime;

            barraCompilar.fillAmount = cantidadCompilada / 5;

            if (cantidadCompilada > 5)
            {
                compilando = false;
                MostrarVuelveOtroDia();
            }
        }

        txtIndicadorRestamtes.text = wins + " / 5";


        if (slotsSeleccionados > 20)
        {
            LimpiarCuadricula();
        }

        txtIndicador.text = palabra + ";";
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt("Wins", wins);
            PlayerPrefs.SetInt("DiaWin", diaWin);
            PlayerPrefs.SetInt("indicePalabra", indicePalabra);

        }
    }

    void MostrarVuelveOtroDia()
    {
        panelCompilar.SetActive(false);
        panelCuadricula.SetActive(false);
        panelAcabado.SetActive(true);
    }

    void LimpiarCuadricula()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RecuperarColor();
            slots[i].butonScreen.Deselect();
        }

        correctSlots = 0;
        slotsSeleccionados = 0;
    }

    public int correctSlots;

    public bool winComprobada;
    public void ComprobarWin()
    {

        if (slotsSeleccionados == palabra.Length)
        {
            if (correctSlots == palabra.Length)
            {
                PasarNivel();
                wins++;
                correctSlots = 0;
                slotsSeleccionados = 0;
            }
            else
            {
                correctSlots = 0;
                slotsSeleccionados = 0;
            }
        }
        else
        {
            correctSlots = 0;
            slotsSeleccionados = 0;
        }

    }

    bool animacionFondo;
    public void PasarNivel()
    {

        mouseController.NormalCursor();

        animacionFondo = false;
        panelCuadricula.SetActive(false);
        panelEscribirTxt.SetActive(true);

        txtFinal.text = "";
        //GenerarNivel();
        tiempoAnimacion = 0;
        StopAllCoroutines();
        StartCoroutine(AparecerTextoGradualmente());
    }

    IEnumerator AparecerTextoGradualmente()
    {
        string mensaje = palabra;

        for (int i = 0; i < mensaje.Length; i++)
        {
            txtFinal.text += mensaje[i];
            yield return new WaitForSeconds(0.2f);
        }

        animacionFondo = true;
    }

    public void GenerarNivel()
    {

        if (indicePalabra + 1 <= palabras.Length)
        {
            indicePalabra++;
            palabra = palabras[indicePalabra - 1];
        }
        else
        {
            indicePalabra = 0;
            indicePalabra++;
            palabra = palabras[indicePalabra - 1];
        }

        LimpiarCuadricula();
        winComprobada = false;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].LetraAleatoria();
        }

        //RellenarCuadriculaHor();

        int aleatorioNivel = Random.Range(0, 2);

        if (aleatorioNivel == 0) { RellenarCuadriculaVer(); }
        else if (aleatorioNivel == 1) { RellenarCuadriculaHor(); }

    }

    public float aleatorio;
    public void RellenarCuadriculaHor()
    {
        aleatorio = Random.Range(1, numHorizontal + 1);

        int repeticion = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].numFilaHor == aleatorio)
            {
                slotsARellenarHor[repeticion] = slots[i];
                repeticion++;

                if (repeticion == slotsARellenarHor.Length) break;
            }
        }

        int baseNum = numHorizontal - palabra.Length;

        int aleatorio2 = Random.Range(0, baseNum);

        if (aleatorio2 + palabra.Length > numHorizontal) { aleatorio2--; }

        for (int n = 0; n < palabra.Length; n++)
        {
            if (n + aleatorio2 < slotsARellenarHor.Length && slotsARellenarHor[n + aleatorio2] != null)
            {
                slotsARellenarHor[n + aleatorio2].letra = "" + palabra[n];
                slotsARellenarHor[n + aleatorio2].especial = true;
            }
            else
            {
                GenerarNivel();
                return;
            }
        }
    }

    public void RellenarCuadriculaVer()
    {
        aleatorio = Random.Range(1, numVertical);

        int repeticion = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].numFilaVer == aleatorio)
            {
                slotsARellenarVer[repeticion] = slots[i];
                repeticion++;

                if (repeticion == slotsARellenarVer.Length) break;
            }
        }

        int baseNum = numVertical - palabra.Length;

        int aleatorio2 = Random.Range(0, baseNum);

        if (aleatorio2 + palabra.Length > numVertical) { aleatorio2--; }

        for (int n = 0; n < palabra.Length; n++)
        {
            if (n + aleatorio2 < slotsARellenarVer.Length && slotsARellenarVer[n + aleatorio2] != null)
            {
                slotsARellenarVer[n + aleatorio2].letra = "" + palabra[n];
                slotsARellenarVer[n + aleatorio2].especial = true;
            }
            else
            {
                GenerarNivel();
                return;
            }
        }
    }


}
