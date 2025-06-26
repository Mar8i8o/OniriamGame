using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajonController : MonoBehaviour
{
    public bool cajonAbierto;

    public GameObject abiertoPoint;

    Vector3 posicionInicial;
    public GameObject cajon;

    public bool horizontal;
    public bool isNevera;
    public bool isPersiana;

    public Animator anim;

    public CajonController frigo;

    GuardarController guardarController;

    public bool tieneLuz;
    public GameObject luz;
    public GameObject luz2;
    public float tiempoEnApagarse;

    public AudioSource sonidoAbrir;
    public AudioSource sonidoCerrar;

    private void Awake()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        if (isPersiana) { horizontal = true; }
    }
    void Start()
    { 

        if (PlayerPrefs.GetInt(gameObject.name + "CajonAbierto", System.Convert.ToInt32(cajonAbierto)) == 0) { cajonAbierto = false; }
        else { cajonAbierto = true; }

        if (!horizontal)
        {
            posicionInicial = cajon.transform.position;

            if (cajonAbierto)
            {
                cajon.transform.position = abiertoPoint.transform.position;
            }
            else
            {
                cajon.transform.position = posicionInicial;
            }
        }
        else
        {
            if (!cajonAbierto) { 
                ApagarAnim();

                if(tieneLuz)
                {
                    luz.SetActive(false);
                    if (luz2 != null) luz2.SetActive(false);
                }

            }
            else 
            { 
                AbrirCajon(); 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //animCajon.SetBool("Open", cajonAbierto);

        if (!horizontal)
        {
            if (cajonAbierto)
            {
                cajon.transform.position = Vector3.Lerp(cajon.transform.position, abiertoPoint.transform.position, 6f * Time.deltaTime);
            }
            else
            {
                cajon.transform.position = Vector3.Lerp(cajon.transform.position, posicionInicial, 6f * Time.deltaTime);
            }
        }
    }

    public void InteractuarCajon()
    {
        if (cajonAbierto)
        {         
            CerrarCajon();          
        }
        else
        {
            AbrirCajon();
        }
    }

    public void CerrarCajon()
    {
        cajonAbierto = false;
        if (horizontal) 
        { 
            EncenderAnim();
            anim.SetBool("Open", false);
            Invoke(nameof(ApagarAnim), 2);

            if(tieneLuz)
            {
                Invoke(nameof(ApagarLuz), tiempoEnApagarse);
            }

        }
        if (isNevera) frigo.CerrarCajon();

        if (sonidoCerrar != null) { sonidoCerrar.Play(); }
    }

    public void AbrirCajon()
    {
        cajonAbierto = true;
        if (horizontal) 
        {
            EncenderAnim();
            anim.SetBool("Open", true);
            Invoke(nameof(ApagarAnim), 2);

            if (tieneLuz)
            {
                CancelInvoke(nameof(ApagarLuz));
                luz.SetActive(true);
                if(luz2 != null)luz2.SetActive(true);
            }

        }

        if(sonidoAbrir != null) { sonidoAbrir.Play(); }

    }

    public void ApagarLuz()
    {
        luz.SetActive(false);
        if (luz2 != null) luz2.SetActive(false);
    }

    private void LateUpdate()
    {

        if (guardarController.guardando)
        {
            //cajonAbierto = false;
            //if (horizontal) { anim.SetBool("Open", false); }
            PlayerPrefs.SetInt(gameObject.name + "CajonAbierto", System.Convert.ToInt32(cajonAbierto));
        }

    }

    void ApagarAnim()
    {
        anim.enabled = false;
    }

    void EncenderAnim()
    {
        CancelInvoke(nameof(ApagarAnim));
        anim.enabled = true;
    }
}
