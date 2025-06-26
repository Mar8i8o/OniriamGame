using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavadoraController : MonoBehaviour
{
    public LayerMask capaJugador;
    public bool jugadorCerca;
    public float radio;

    public Animator parentAnim;
    public Animator lavadoraAnim;
    Raycast raycast;

    public bool encendida;
    public float tiempoLavando;
    public bool lavadoFinalizado;

    public ItemAtributes sabana;

    public GameObject sabanasLimpias;
    public GameObject sabanasSucias;

    public GameObject sabanasPoint;

    public BoxCollider colider;

    GuardarController guardarController;

    void Start()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        raycast = GameObject.Find("Main Camera").GetComponent<Raycast>();
        lavadoFinalizado = false;
        GetDatos();

        if(!encendida)
        {
            sabanasSucias.SetActive(false); 
            sabanasLimpias.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //jugadorCerca = Physics.CheckSphere(transform.position, radio, capaJugador);

        /*
        if (!lavadoFinalizado)
        {
            if (raycast.hasObject)
            {
                if (raycast.itemAtributes.isSabana)
                {
                    colider.enabled = true;
                    lavadoraAnim.SetBool("Open", jugadorCerca);
                }
                else
                {
                    colider.enabled = false;
                    lavadoraAnim.SetBool("Open", false);
                }

            }
            else
            {
                colider.enabled = false;
                lavadoraAnim.SetBool("Open", false);
            }
        }
        else
        {
            lavadoraAnim.SetBool("Open", true);

            if (raycast.hasObject) colider.enabled = false;
            else colider.enabled = true;
        }
        */

        if(apuntandoLavadora)
        {
            tiempoApuntandoLavadora += Time.deltaTime;

            if(!puertaAbierta && !lavadoFinalizado && !encendida && raycast.hasObject)
            {
                if (raycast.itemAtributes.isSabana && !raycast.itemAtributes.sabanaLimpia)
                {
                    lavadoraAnim.SetBool("Open", true);
                    puertaAbierta = true;
                }
            }

            if(tiempoApuntandoLavadora > 1)
            {
                apuntandoLavadora = false;
                tiempoApuntandoLavadora = 0;
                lavadoraAnim.SetBool("Open", false);
                puertaAbierta = false;
            }
        }

        if(lavadoFinalizado) 
        {
            if(!puertaAbierta)
            {
                lavadoraAnim.SetBool("Open", true);
                puertaAbierta = true;
            }
        }


        if (encendida && !lavadoFinalizado) 
        {
            tiempoLavando += Time.deltaTime;
            parentAnim.SetBool("Vibrando", true);

            if (tiempoLavando > 10)
            {
                sabana.sabanaLimpia = true;
                lavadoFinalizado = true;
                parentAnim.SetBool("Vibrando", false);
            }
        }
        else
        {
            parentAnim.SetBool("Vibrando", false);
        }


        if (encendida && !lavadoFinalizado) { sabanasSucias.SetActive(true); sabanasLimpias.SetActive(false); }
        else if (encendida && lavadoFinalizado) { sabanasSucias.SetActive(false); sabanasLimpias.SetActive(true); }

        //sabanasLimpias.SetActive(lavadoFinalizado);

        
        
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            GuardarDatos();
        }
    }

    public void UsarLavadora()
    {
        if (!encendida) 
        {
            encendida = true;
            lavadoFinalizado = false;
            sabana.active = false;
            sabana.DesactivarItem();
            raycast.ForzarSoltarObjeto();
            sabana.transform.position = Vector3.zero;
            tiempoLavando = 0;

            puertaAbierta = false;
            lavadoraAnim.SetBool("Open", false);

        }
        if (lavadoFinalizado)
        {
            sabana.active = true;
            sabana.sabanaLimpiaOBJ.SetActive(true);
            sabana.sabanaSuciaOBJ.SetActive(false);
            sabana.ActivarItem();
            sabana.transform.position = sabanasPoint.transform.position;
            raycast.CogerObjeto(sabana.gameObject);

            encendida = false;
            lavadoFinalizado = false;
            tiempoLavando = 0;

            sabanasSucias.SetActive(false); 
            sabanasLimpias.SetActive(false);

        }
    }

    public bool puertaAbierta;
    public float tiempoApuntandoLavadora;
    public bool apuntandoLavadora;

    public void ApuntandoLavadora()
    {
        tiempoApuntandoLavadora = 0;
        apuntandoLavadora = true;
    }

    public void GuardarDatos()
    {
        PlayerPrefs.SetInt(gameObject.name + "encendida", System.Convert.ToInt32(encendida));
        PlayerPrefs.SetFloat(gameObject.name + "tiempoLavando", tiempoLavando);

    }

    public void GetDatos()
    {
        if (PlayerPrefs.GetInt(gameObject.name + "encendida", System.Convert.ToInt32(encendida)) == 0) { encendida = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "encendida", System.Convert.ToInt32(encendida)) == 1) { encendida = true; }

        tiempoLavando = PlayerPrefs.GetFloat(gameObject.name + "tiempoLavando", tiempoLavando);

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}
