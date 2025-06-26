using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CorreosManager : MonoBehaviour
{
    public string asuntoPrincipal;
    public float horaPrincipal;

    public string usuarioPrincipal;

    //[TextArea(15, 20)]
    public string correo;

    public TextMeshProUGUI asuntoTXT;
    public TextMeshProUGUI horaTXT;
    public TextMeshProUGUI fechaTXT;
    public TextMeshProUGUI usuarioTXT;
    public TextMeshProUGUI correoTXT;

    public Image logoUsuario;

    int cuantasImagenes = 0;
    public GameObject Image1GO;
    public GameObject Image2GO;
    public Image image1;
    public Image image2;

    public Image imageGrande;

    public bool correoAbierto;
    public GameObject contenidoCorreo;
    public GameObject welcomeObj;

    public GameObject panel;
    public GameObject layoutGroup;
    public VerticalLayoutGroup verLayoutGroup;
    public GameObject[] correos;
    public float offsetPanel;
    public float offsetPos;
    Vector3 posicionlayoutGroup;

    public float offsetEnlace;
    bool tieneEnlace;
    public GameObject enlace;
    public TextMeshProUGUI enlaceTXT;

    [HideInInspector]public string enlaceId;

    public GameObject correoOBJ;

    public GameObject panelJustFood;
    public GameObject panelCorreos;

    private void Awake()
    {
        posicionlayoutGroup = layoutGroup.transform.localPosition;

    }
    void Start()
    {
        //triggerButtons = GameObject.FindGameObjectsWithTag("TriggerButton");
    }

    // Update is called once per frame
    void Update()
    {
        //if (correoAbierto) { contenidoCorreo.SetActive(true); welcomeObj.SetActive(false); }
        //else { contenidoCorreo.SetActive(false); welcomeObj.SetActive(true); return; }
    }

    public void MostrarCorreo(string usuario, string asunto, float totalSegundos, string correo, Sprite sprite, int cuantas, bool hasLink, GameObject queCorreo, int mes, int dia)
    {

        usuarioTXT.text = usuario;
        asuntoTXT.text = asunto;
        correoTXT.text = correo;
        logoUsuario.sprite = sprite;
        tieneEnlace = hasLink;
        correoAbierto = true;
        cuantasImagenes = cuantas;
        correoOBJ = queCorreo;

        if (dia >= 10)
        {
            fechaTXT.text = (dia + 1) + "/" + mes + "/08";
        }
        else
        {
            fechaTXT.text = "0" + (dia + 1) + "/" + mes + "/08";
        }

        float hora = Mathf.FloorToInt(totalSegundos / 3600);
        float minutes = Mathf.FloorToInt(totalSegundos / 60);
        minutes = Mathf.FloorToInt(minutes % 60);
        float seconds = Mathf.FloorToInt(totalSegundos % 60);

        horaTXT.text = string.Format("{0:00}:{1:00}", hora, minutes);


        if (cuantasImagenes == 0) { Image1GO.SetActive(false); Image2GO.SetActive(false); }
        else if (cuantasImagenes == 1) { Image1GO.SetActive(true); Image2GO.SetActive(false); }
        else if (cuantasImagenes == 2) { Image1GO.SetActive(true); Image2GO.SetActive(true); }

        if (tieneEnlace) { enlace.SetActive(true); enlace.transform.localPosition = layoutGroup.transform.position + new Vector3(600, offsetEnlace, 0); }
        else { enlace.SetActive(false); }

    }

    public void RecibirImagenes(Sprite sprite1, Sprite sprite2)
    {
        if (cuantasImagenes == 1)
        {
            image1.sprite = sprite1;
        }
        else if (cuantasImagenes == 2)
        {
            image1.sprite = sprite1;
            image2.sprite = sprite2;
        }
    }

    public GameObject[] triggerButtons;

    public int enQueImagenEsta;
    public void MostrarImagen(Sprite imagen)
    {
        /*
        enQueImagenEsta = cual;
        if (cual == 1) 
        {
            imageGrande.sprite = image1.sprite;
        }
        else if (cual == 2)
        {
            imageGrande.sprite = image2.sprite;
        }
        else if (cual == 3) //PROFILE PIC
        {
            imageGrande.sprite = logoUsuario.sprite;
        }
        */

        imageGrande.sprite = imagen;

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

    #region

    public ImpresoraController ImpresoraController;
    public void Imprimir()
    {
        if (!ImpresoraController.imprimiendo)
        {
            /*
            if (enQueImagenEsta == 1)
            {
                ImpresoraController.Imprimir(image1);
            }
            else if (enQueImagenEsta == 2)
            {
                ImpresoraController.Imprimir(image2);
            }
            else if (enQueImagenEsta == 3)
            {
                ImpresoraController.Imprimir(logoUsuario);
            }
            */

            ImpresoraController.Imprimir(imageGrande);
        }
        else
        {
            GameObject instancia = Instantiate(waitPrefab, decargaCheckParent.transform.position, Quaternion.identity);
            instancia.transform.SetParent(decargaCheckParent.transform);
        }
    }

    public void ImpressNotification()
    {
        GameObject instancia = Instantiate(ImpressPrefab, decargaCheckParent.transform.position, Quaternion.identity);
        instancia.transform.SetParent(decargaCheckParent.transform);
    }

    public GameObject cantImpressPrefab;
    public GameObject ImpressPrefab;
    public GameObject waitPrefab;

    public void CantImpress()
    {
        GameObject instancia = Instantiate(cantImpressPrefab, decargaCheckParent.transform.position, Quaternion.identity);
        instancia.transform.SetParent(decargaCheckParent.transform);
    }

    #endregion

    public void MostrarEnlace(string enlaceStr, float offset, string enlaceIdStr)
    {
        enlaceTXT.text = enlaceStr;
        offsetEnlace = offset;
        enlaceId = enlaceIdStr;
    }

    public GameObject decargaCheckPrefab;
    public GameObject decargaCheckParent;

    public PlaySoundsMovil sonidoDescarga;
    public ComputerController computerController;
    public GameObject panelOniriam;

    public void UsarEnlace()
    {
        print("usarenlace");
        if (enlaceId == "justfood") 
        {
            panelCorreos.gameObject.SetActive(false);
            panelJustFood.gameObject.SetActive(true);
        }
        if (enlaceId == "openOniriam")
        {
            panelCorreos.gameObject.SetActive(false);
            panelOniriam.gameObject.SetActive(true);
            computerController.OniriamDesbloqueado = true;
            
        }
        if (enlaceId == "descargaAudioSecreto")
        {
            sonidoDescarga.gameObject.SetActive(true);
            sonidoDescarga.active = true;
            //sonidoDescarga.GuardarDatos();


            print("CHECK");
            GameObject instancia = Instantiate(decargaCheckPrefab, decargaCheckParent.transform.position, Quaternion.identity);
            instancia.transform.SetParent(decargaCheckParent.transform);
        }
    }

    public void BorrarCorreo(GameObject queCorreo)
    {
        queCorreo.GetComponent<CorreosController>().active = false;
        queCorreo.GetComponent<CorreosController>().contenidoCorreo.SetActive(false);
        //correoOBJ.GetComponent<CorreosController>().GuardarDatos();
        queCorreo.SetActive(false);
        correoAbierto = false;
    }
}

