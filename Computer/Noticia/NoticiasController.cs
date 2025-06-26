using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoticiasController : MonoBehaviour
{
    public Sprite thisImage;
    public bool hasImage;


    //public string tituloNoticia;
    //[TextArea(15, 20)]
    //public string contenidoNoticia;
    //public string fechaNoticia;

    NoticiasManager noticiasManager;
    TimeController timeController;

    public GameObject content;

    public int indiceNoticia;

    //public bool hasEnlace;
    //public Vector3 enlaceOffset;
    //public string enlaceId;

    public float totalSegundosNoticia;
    public float diaNoticia;
    public float mesNoticia;

    public bool active;

    public GameObject panelContenidoNoticia;
    //public TextMeshProUGUI noticiaTituloTXT;

    ComputerController computerController;

    public OpenPanelOniriam openPanelOniriam;
    public ScrollRect scrollRect;


    private void Awake()
    {

        computerController = GameObject.Find("CanvasPantallaPc").GetComponent<ComputerController>();

        //noticiaTituloTXT.text = tituloNoticia;

        if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(active)) == 0) { active = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(active)) == 1) { active = true; }

        diaNoticia = PlayerPrefs.GetFloat(gameObject.name + "dia", diaNoticia);
        mesNoticia = PlayerPrefs.GetFloat(gameObject.name + "mes", mesNoticia);
        totalSegundosNoticia = PlayerPrefs.GetFloat(gameObject.name + "totalSegundos", totalSegundosNoticia);

        if (active) { gameObject.SetActive(true); }
        else { gameObject.SetActive(false); }

        noticiasManager = GameObject.Find("GameManager").gameObject.GetComponent<NoticiasManager>();
        timeController = GameObject.Find("GameManager").gameObject.GetComponent<TimeController>();

        //if (enlaceId == "") hasEnlace = false;
        //else hasEnlace = true;

        //fechaNoticia = diaNoticia+"/"+mesNoticia+"/08";

        //ComprobarPrimeraPosicion();

    }

    private void Start()
    {
        //ComprobarPrimeraPosicion();
    }

    public void IndicarIndice(int indice)
    {
        indiceNoticia = indice;
        if(indiceNoticia == 0)
        {
            AbrirNoticia();
        }
        else
        {
            panelContenidoNoticia.SetActive(false);
        }
    }

    public void ComprobarPrimeraPosicion()
    {

            print("Posicion noticia: " + gameObject.transform.GetSiblingIndex());
            if (gameObject.transform.GetSiblingIndex() == 0)
            {
                AbrirNoticia();
            }
            else
            {
                panelContenidoNoticia.SetActive(false);
            }

    }

    public void DesactivarNoticia()
    {
        active = false;
        gameObject.SetActive(false);
    }

    public void AbrirNoticia()
    {
        //noticiasManager.AbrirNoticia(tituloNoticia, contenidoNoticia, hasImage, thisImage, fechaNoticia, hasEnlace, enlaceOffset, enlaceId);
        openPanelOniriam.CambiarScrollBar(scrollRect);
        panelContenidoNoticia.transform.SetAsLastSibling();
        noticiasManager.CerrarContenidos(panelContenidoNoticia);


        panelContenidoNoticia.SetActive(true);

        //print("abrirNoticia");
    }

    public void GuardarDatos()
    {
        PlayerPrefs.SetFloat(gameObject.name + "totalSegundos", totalSegundosNoticia);
        PlayerPrefs.SetFloat(gameObject.name + "dia", diaNoticia);
        PlayerPrefs.SetFloat(gameObject.name + "mes", mesNoticia);
        PlayerPrefs.SetInt(gameObject.name + "active", System.Convert.ToInt32(active));
    }


}
