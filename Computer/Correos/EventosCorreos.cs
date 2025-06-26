using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventosCorreos : MonoBehaviour
{
    public GameObject[] correosSpam;

    public TimeController timeController;

    public float tiempoCorreosSpam;

    int tiempoEntreAnuncios;

    public GameObject[] todosLosCorreos;
    GuardarController guardarController;
    private void Awake()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        todosLosCorreos = GameObject.FindGameObjectsWithTag("Correo");
        correosOrdenar = GameObject.FindGameObjectsWithTag("Correo");


    }
    void Start()
    {
        tiempoEntreAnuncios = 10;
        OrdenarCorreos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 60 == 0)
        {
            //ControlarCorreosSpam();
        }
        
    }

    private void FixedUpdate()
    {
        if (guardarController.guardando)
        {
            for (int i = 0; i < todosLosCorreos.Length; i++) 
            {
                todosLosCorreos[i].GetComponent<CorreosController>().GuardarDatos();
            }
        }
    }

    private void LateUpdate()
    {
        //OrdenarCorreos();
    }

    public void ControlarCorreosSpam()
    {
        tiempoCorreosSpam += Time.deltaTime;

        if (tiempoCorreosSpam > tiempoEntreAnuncios)
        {
            int aleatorio = Random.Range(0, correosSpam.Length);

            for (int i = 0; i < correosSpam.Length; i++)
            {
                if (!correosSpam[i].GetComponent<CorreosController>().active)
                {
                    CorreosController correoControler = correosSpam[i].GetComponent<CorreosController>();
                    correoControler.totalSegundos = timeController.totalSegundos;
                    correoControler.dia = timeController.dia;
                    correoControler.active = true;
                    //correoControler.GuardarDatos();
                    //correosSpam[i].transform.SetAsFirstSibling();
                    correosSpam[i].SetActive(true);

                    OrdenarCorreos();

                    break;
                }
            }

            tiempoCorreosSpam = 0;

        }
    }

    public GameObject chekCorreo;
    public GameObject chekCorreoPosition;

    public void EnviarCorreo(GameObject queCorreo)
    {
        CorreosController correoControler = queCorreo.GetComponent<CorreosController>();
        correoControler.totalSegundos = timeController.totalSegundos;
        correoControler.dia = timeController.dia;
        correoControler.mes = timeController.mes;
        correoControler.active = true;
        //correoControler.GuardarDatos();
        //correosSpam[i].transform.SetAsFirstSibling();
        queCorreo.SetActive(true);

        print("check");
        GameObject instancia = Instantiate(chekCorreo, chekCorreoPosition.transform.position, Quaternion.identity);
        instancia.transform.SetParent(chekCorreoPosition.transform);

        OrdenarCorreos();
    }

    public GameObject[] correosOrdenar;
    public GameObject[] correosOrdenados;
    int horaMasGrande;
    public float[] horas;
    public List<float> horasList;

    public float correosSinLeer;

    public void ContarCorreosSinLeer()
    {
        //print("ContarCorreos");
        correosSinLeer = 0;
        for (int i = 0; i < correosOrdenar.Length; i++)
        {
            CorreosController cor = correosOrdenar[i].GetComponent<CorreosController>();
            if (cor.active && !cor.leido)
            {
                correosSinLeer++;
            }
        }
    }

    public void OrdenarCorreos()
    {

        horasList.Clear();

        ContarCorreosSinLeer();
        //correosOrdenar = GameObject.FindGameObjectsWithTag("Correo");
        correosOrdenados = new GameObject[correosOrdenar.Length];

        if (correosOrdenar.Length > 0)
        {
            List<double> horasList = new List<double>();
            for (int i = 0; i < correosOrdenar.Length; i++)
            {

                double totalSegundos = correosOrdenar[i].GetComponent<CorreosController>().totalSegundos;
                double dia = correosOrdenar[i].GetComponent<CorreosController>().dia;
                double mes = correosOrdenar[i].GetComponent<CorreosController>().mes;
                horasList.Add(totalSegundos / 600 + dia * 100 + mes * 100000);

            }

            horasList.Sort();
            horasList.Reverse();

            for (int j = 0; j < correosOrdenar.Length; j++)
            {
                for (int i = 0; i < correosOrdenar.Length; i++)
                {
                    double totalSegundos = correosOrdenar[i].GetComponent<CorreosController>().totalSegundos;
                    double dia = correosOrdenar[i].GetComponent<CorreosController>().dia;
                    double mes = correosOrdenar[i].GetComponent<CorreosController>().mes;
                    double tiempoNoticia = totalSegundos / 600 + dia * 100 + mes * 100000;

                    if (tiempoNoticia == horasList[j])
                    {
                        //print(horasList[j] + correosOrdenar[i].gameObject.name);
                        correosOrdenar[i].transform.SetSiblingIndex(j);
                        correosOrdenados[j] = correosOrdenar[i];
                    }
                }
            }
        }

        //print(horasList.ToArray()[1]);

    }
}
