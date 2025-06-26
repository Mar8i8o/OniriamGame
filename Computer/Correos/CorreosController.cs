using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CorreosController : MonoBehaviour
{
    //public string asunto;
    public float totalSegundos;
    public int dia = 0;
    public int mes;

    public TextMeshProUGUI horaTXT;
    public CorreosManager correosManager;

    public bool active;
    public bool activeInicio;

    public bool leido;

    public GameObject barra;
    public ScrollBarScreen scrollBarScreen;
    public GameObject contenidosCorreos;

    public GameObject contenidoCorreo;

    public GameObject puntoSinLeer;
    public GameObject asuntoSinLeer;

    void Start()
    {
        //PlayerPrefs.DeleteAll();

        correosManager = GameObject.Find("GameManager").GetComponent<CorreosManager>();
        
        GetDatos();
        if (!active)gameObject.SetActive(false);

        GameObject[] contenidoCorreos = GameObject.FindGameObjectsWithTag("ContenidoCorreo");

        for (int i = 0; i < contenidoCorreos.Length; i++)
        {
            contenidoCorreos[i].SetActive(false);
        }
        barra.SetActive(false);

        puntoSinLeer.SetActive(!leido);
        asuntoSinLeer.SetActive(!leido);

    }

    void Update()
    {
        //asuntoTXT.text = asunto;

        float hora = Mathf.FloorToInt(totalSegundos / 3600);
        float minutes = Mathf.FloorToInt(totalSegundos / 60);
        minutes = Mathf.FloorToInt(minutes % 60);
        float seconds = Mathf.FloorToInt(totalSegundos % 60);

        horaTXT.text = string.Format("{0:00}:{1:00}", hora, minutes);

        //usuarioTXT.text = usuario;
    }

    private void FixedUpdate()
    {
        //GuardarDatos();
    }

    public void GuardarDatos()
    {
         PlayerPrefs.SetFloat(gameObject.name + "totalSegundos", totalSegundos);
         PlayerPrefs.SetInt(gameObject.name + "active", System.Convert.ToInt32(active));
         PlayerPrefs.SetInt(gameObject.name + "leido", System.Convert.ToInt32(leido));
    }

    public void GetDatos()
    {
        //asunto = PlayerPrefs.GetString(gameObject.name + "asunto", asunto);
        //correo = PlayerPrefs.GetString(gameObject.name + "correo", correo);
        //if (PlayerPrefs.GetInt(gameObject.name + "tieneEnlace", System.Convert.ToInt32(tieneEnlace)) == 0) { tieneEnlace = false; }
        //else if (PlayerPrefs.GetInt(gameObject.name + "tieneEnlace", System.Convert.ToInt32(tieneEnlace)) == 1) { tieneEnlace = true; }

        totalSegundos = PlayerPrefs.GetFloat(gameObject.name + "totalSegundos", totalSegundos);
        if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(activeInicio)) == 0) { active = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(activeInicio)) == 1) { active = true; }

        if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(leido)) == 0) { leido = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(leido)) == 1) { leido = true; }
    }

    public void AbrirCorreo()
    {
        leido = true;
        puntoSinLeer.SetActive(false);
        asuntoSinLeer.SetActive(false);

        /*
        if (tieneEnlace) { correosManager.MostrarEnlace(enlace, offsetEnlace, enlaceId); }
        correosManager.MostrarCorreo(usuario, asunto, totalSegundos, correo, spriteUsuario, cuantasImagenes, tieneEnlace, gameObject, dia, mes);

        if (cuantasImagenes > 0)
        {
            if (cuantasImagenes == 1) { correosManager.RecibirImagenes(imagen1, imagen1); }
            else if (cuantasImagenes == 2) { correosManager.RecibirImagenes(imagen1, imagen2); }
        }
        */
        GameObject[] contenidoCorreos = GameObject.FindGameObjectsWithTag("ContenidoCorreo");

        for(int i = 0; i < contenidoCorreos.Length; i++) 
        {
            contenidoCorreos[i].SetActive(false);
        }

        if(!barra.activeSelf)barra.SetActive(true);

        contenidoCorreo.SetActive(true);

        ScrollRect scrollRect = contenidoCorreo.GetComponentInChildren<ScrollRect>();
        scrollBarScreen.scrollRect = scrollRect;
        scrollBarScreen.scrollRect.normalizedPosition = new Vector2(0, 1);

    }
}
