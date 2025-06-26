using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotClima : MonoBehaviour
{
    public int dia;
    public int indice;

    public string climaId;

    public string diaSemana;

    public GameObject icoSoleado;
    public GameObject icoLluvia;
    public GameObject icoTormenta;
    public GameObject icoNublado;

    TimeController timeController;
    LluviaController lluviaController;

    public TextMeshProUGUI diaTXT;
    public TextMeshProUGUI diaSemanaTXT;

    public bool abreviaTXT;

    public bool cambiaFondo;

    public GameObject fondoDespejado;
    public GameObject fondoNublado;

    private void Awake()
    {
        timeController = GameObject.Find("GameManager").GetComponent<TimeController>();
        lluviaController = GameObject.Find("ClimaController").GetComponent<LluviaController>();
    }

    void Start()
    {

        dia = indice + timeController.dia; 

        if(dia > 30) 
        {
            float diaAux = dia = indice + timeController.dia; ;

            diaTXT.text = "" + diaAux + "/" + timeController.mes + 1;
        }
        else 
        {
            diaTXT.text = "" + dia + "/" + timeController.mes;
        }

        if (abreviaTXT) { SetDiaSemana(); }
        else { diaSemana = timeController.diasSemana[dia]; }
        diaSemanaTXT.text = diaSemana;
        climaId = lluviaController.climaID[dia-1];
        SetImage();

    }


    private void OnEnable()
    {
        dia = indice + timeController.dia;

        if (dia > 30)
        {
            float diaAux = dia = indice + timeController.dia; ;

            diaTXT.text = "" + diaAux + "/" + timeController.mes + 1;
        }
        else
        {
            diaTXT.text = "" + dia + "/" + timeController.mes;
        }

        if (abreviaTXT) { SetDiaSemana(); }
        else { diaSemana = timeController.diasSemana[dia]; }
        diaSemanaTXT.text = diaSemana;
        climaId = lluviaController.climaID[dia-1];
        SetImage();
    }

    void Update()
    {
        //SetImage();
    }

    public void SetDiaSemana()
    {
        if (timeController.diasSemana[dia] == "Lunes") { diaSemana = "Lun"; }
        else if (timeController.diasSemana[dia] == "Martes") { diaSemana = "Mar"; }
        else if (timeController.diasSemana[dia] == "Miercoles") { diaSemana = "Mie"; }
        else if (timeController.diasSemana[dia] == "Jueves") { diaSemana = "Jue"; }
        else if (timeController.diasSemana[dia] == "Viernes") { diaSemana = "Vie"; }
        else if (timeController.diasSemana[dia] == "Sabado") { diaSemana = "Sab"; }
        else if (timeController.diasSemana[dia] == "Domingo") { diaSemana = "Dom"; }
    }

    public void SetImage()
    {
        icoLluvia.SetActive(climaId == "Lluvia");
        icoSoleado.SetActive(climaId == "Soleado");
        icoTormenta.SetActive(climaId == "Tormenta");
        icoNublado.SetActive(climaId == "Nublado");

        if(cambiaFondo)
        {
            fondoDespejado.SetActive(climaId == "Soleado");
            fondoNublado.SetActive(climaId != "Soleado");
        }

    }
}
