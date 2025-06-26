using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class NoticiasManager : MonoBehaviour
{

    public Image imageGrande;

    public GameObject[] triggerButtons;

    public string nombreNoticiaAct;
    public string contenidoNoticiaAct;
    public TextMeshProUGUI contenidoNoticiaTXT;
    public TextMeshProUGUI nombreNoticiaTXT;
    public TextMeshProUGUI fechaNoticiaTXT;
    public Image imagenActual;
    public bool hasImageAct;

    public GameObject[] noticias;

    Vector3 enlacePos;

    TimeController timeController;
    GuardarController guardarController;

    public GameObject[] contenidosNoticias;

    //public ComputerController computerController;

    public string noticiaTitular;

    private void Awake()
    {
        //enlacePos = enlace.transform.position;
        timeController = GameObject.Find("GameManager").GetComponent<TimeController>();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        //computerController = GameObject.Find("GameManager").GetComponent<ComputerController>();

        noticias = GameObject.FindGameObjectsWithTag("PanelNoticia");
        contenidosNoticias = GameObject.FindGameObjectsWithTag("ContenidoNoticia");
        CerrarContenidos(gameObject);
    }
    void Start()
    {

        /*
        int indiceMasAlto = 0;
        int noticiaMasAlta = 0;

        for(int i = 0; i < noticias.Length; i++)
        {
            if ((noticias[i].GetComponent<NoticiasController>().indiceNoticia >= indiceMasAlto) && noticias[i].GetComponent<NoticiasController>().content.activeSelf)
            {
                noticiaMasAlta = i;
                indiceMasAlto = noticias[i].GetComponent<NoticiasController>().indiceNoticia;
            }
        }

        if (noticias.Length > 0)
        {
            NoticiasController noticiaActual = noticias[noticiaMasAlta].GetComponent<NoticiasController>();

            AbrirNoticia(noticiaActual.tituloNoticia, noticiaActual.contenidoNoticia, noticiaActual.hasImage, noticiaActual.thisImage, noticiaActual.fechaNoticia, noticiaActual.hasEnlace, noticiaActual.enlaceOffset, noticiaActual.enlaceId);
        }
        */

        OrdenarNoticia();

        //NoticiasController noticiaActual = noticiasOrdenadas[0].GetComponent<NoticiasController>();

        //AbrirNoticia(noticiaActual.tituloNoticia, noticiaActual.contenidoNoticia, noticiaActual.hasImage, noticiaActual.thisImage, noticiaActual.fechaNoticia, noticiaActual.hasEnlace, noticiaActual.enlaceOffset, noticiaActual.enlaceId);

    }

    public GameObject[] noticiasOrdenar;
    public GameObject[] noticiasOrdenadas;
    public float[] horas;
    public List<double> horasList;
    public void OrdenarNoticia()
    {
        horasList.Clear();

        noticiasOrdenar = GameObject.FindGameObjectsWithTag("PanelNoticia");
        noticiasOrdenadas = new GameObject[noticiasOrdenar.Length];

        if (noticiasOrdenar.Length > 0)
        {
            List<double> horasList = new List<double>();
            for (int i = 0; i < noticiasOrdenar.Length; i++)
            {

                    double totalSegundos = noticiasOrdenar[i].GetComponent<NoticiasController>().totalSegundosNoticia;
                    double dia = noticiasOrdenar[i].GetComponent<NoticiasController>().diaNoticia;
                    double mes = noticiasOrdenar[i].GetComponent<NoticiasController>().mesNoticia;
                    horasList.Add(totalSegundos / 600 + dia * 100 + mes * 100000);

            }

            horasList.Sort();
            horasList.Reverse();

            for (int j = 0; j < noticiasOrdenar.Length; j++)
            {
                for (int i = 0; i < noticiasOrdenar.Length; i++)
                {
                    double totalSegundos = noticiasOrdenar[i].GetComponent<NoticiasController>().totalSegundosNoticia;
                    double dia = noticiasOrdenar[i].GetComponent<NoticiasController>().diaNoticia;
                    double mes = noticiasOrdenar[i].GetComponent<NoticiasController>().mesNoticia;
                    double tiempoNoticia = totalSegundos / 600 + dia * 100 + mes * 100000;

                    if (tiempoNoticia == horasList[j])
                    {
                        //print(horasList[j] + noticiasOrdenar[i].gameObject.name);
                        noticiasOrdenar[i].transform.SetSiblingIndex(j);
                        noticiasOrdenar[i].GetComponent<NoticiasController>().IndicarIndice(j);
                        noticiasOrdenadas[j] = noticiasOrdenar[i];
                    }
                }
            }

        }

        //print(horasList.ToArray()[1]);
    }

    public void CerrarContenidos(GameObject excepcion)
    {

        for (int i = 0; i < contenidosNoticias.Length; i++) 
        {
            //print(contenidosNoticias[i].name + " y " + excepcion.name);
            if(contenidosNoticias[i].name == excepcion.name)
            {
                contenidosNoticias[i].SetActive(true);
            }
            else
            {
                contenidosNoticias[i].SetActive(false);
            }
        }
    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            for(int i = 0; i < noticias.Length;  i++) 
            {
                noticias[i].GetComponent<NoticiasController>().GuardarDatos();
            }
        }
    }

    public string enlaceId;
    public GameObject enlace;
    public TextMeshProUGUI enlaceTXT;

    /*
    public void AbrirNoticia(string nombreNoticia, string contenidoNoticia, bool hasImage, Sprite image, string fechaNoticia, bool hasEnlace, Vector3 enlaceOfsset, string thisenlaceId)
    {
        nombreNoticiaAct = nombreNoticia;
        nombreNoticiaTXT.text = nombreNoticia;

        contenidoNoticiaAct = contenidoNoticia;
        contenidoNoticiaTXT.text = contenidoNoticia;

        fechaNoticiaTXT.text = fechaNoticia;

        hasImageAct = hasImage;

        enlaceId = thisenlaceId;

        /*
        if (hasImage) 
        {
            imagenActual.gameObject.SetActive(true);
            imagenActual.sprite = image;
        }
        else
        {
            imagenActual.gameObject.SetActive(false);
        }
        //

        //////////////////
        
        if (hasEnlace) 
        {
            enlace.SetActive(true);
            enlace.transform.position = enlacePos + enlaceOfsset;
            enlaceTXT.text = "www." + enlaceId + ".com";

        }
        else
        {
            enlace.SetActive(false);
        }

    }

     */
    public void SetNoticia(NoticiasController queNoticia)
    {
        print("setNoticia");

        queNoticia.gameObject.SetActive(true);
        queNoticia.content.gameObject.SetActive(true);
        queNoticia.active = true;
        queNoticia.totalSegundosNoticia = timeController.totalSegundos;
        queNoticia.diaNoticia = timeController.dia+1;
        queNoticia.mesNoticia = timeController.mes;

        //queNoticia.fechaNoticia = (timeController.dia + 1) + "/"+queNoticia.mesNoticia+ "/08";

        OrdenarNoticia();

        if (noticiasOrdenar.Length >= 14)
        {
            noticiasOrdenadas[noticiasOrdenar.Length - 1].GetComponent<NoticiasController>().DesactivarNoticia();
        }

        for (int i = 0; i < noticiasOrdenar.Length; i++)
        {
            noticiasOrdenar[i].GetComponent<NoticiasController>().ComprobarPrimeraPosicion();
        }

    }

    public void MostrarImagen()
    {
        imageGrande.sprite = imagenActual.sprite;

        print("MostrarImagen");

        triggerButtons = GameObject.FindGameObjectsWithTag("TriggerButton");

        for (int i = 0; i < triggerButtons.Length; i++)
        {
            triggerButtons[i].gameObject.SetActive(false);
        }

    }

    public void CerrarImagen()
    {
        for (int i = 0; i < triggerButtons.Length; i++)
        {
            triggerButtons[i].gameObject.SetActive(true);
        }
    }

    public GameObject panelNoticias;
    public GameObject panelJustFood;
    public void UsarEnlace()
    {
        print("usarenlace");
        if (enlaceId == "justfood")
        {
            panelNoticias.gameObject.SetActive(false);
            panelJustFood.gameObject.SetActive(true);
        }
    }

    #region

    public ImpresoraController ImpresoraController;
    public GameObject decargaCheckParent;

    public void Imprimir()
    {
       ImpresoraController.Imprimir(imageGrande);
    }

    /*
    public void ImpressNotification()
    {
        print("notificacion");
        GameObject instancia = Instantiate(ImpressPrefab, decargaCheckParent.transform.position, Quaternion.identity);
        instancia.transform.SetParent(decargaCheckParent.transform);
    }


    public GameObject cantImpressPrefab;
    public GameObject ImpressPrefab;
    public GameObject waitPrefab;

    public void CantImpress()
    {
        //GameObject instancia = Instantiate(cantImpressPrefab, decargaCheckParent.transform.position, Quaternion.identity);
        //instancia.transform.SetParent(decargaCheckParent.transform);
    }
    */

    #endregion
}
