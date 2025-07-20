using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator pomoAnim;
    public Animator puertaAnim;

    public bool puertaAbierta;
    public bool entreAbierta;
    public bool abreLaEntreAbierta;

    public bool sePuedeAbrir;

    GuardarController guardarController;
    PensamientoControler pensamientoControler;

    public AudioSource sonidoTimbre;

    public bool generaPensamiento;
    public string pensamiento;
    [HideInInspector] public bool blockPensamiento;

    public bool tieneCerradura;
    public GameObject puntoCerradura;
    public int idPuerta;

    public Animator cerraduraAnim;

    public bool iniciaDialogo;
    public string idDialogo;
    public NpcDialogue npcDialogue;

    public bool activaAlgo;
    public GameObject queActiva;

    public Transform puntoMirarPuerta;

    public GameObject navMeshObstacle;

    public AudioSource sonidoAbrirPuerta;
    public AudioSource sonidoCerrarPuerta;
    public AudioSource sonidoGolpearPuerta;

    public bool guardaDatos;

    void Start()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();

        if (guardaDatos)
        {
            if (PlayerPrefs.GetInt(gameObject.name + "puertaAbierta", System.Convert.ToInt32(puertaAbierta)) == 0) { puertaAbierta = false; }
            else { puertaAbierta = true; }

            if (PlayerPrefs.GetInt(gameObject.name + "entreAbierta", System.Convert.ToInt32(entreAbierta)) == 0) { entreAbierta = false; }
            else { entreAbierta = true; }
        }

        //Invoke(nameof(EntreAbrirPuerta), 1);

        if(!puertaAbierta && !entreAbierta)
        {
            ApagarAnim();
        }
        else
        {

            EncenderAnim();

            puertaAnim.SetBool("open", puertaAbierta);
            puertaAnim.SetBool("EntreAbierta", entreAbierta);

            Invoke(nameof(ApagarAnim),3);

        }


        puedeTocarTimbre = true;
    }

    private void FixedUpdate()
    {
        if (guardarController.guardando && guardaDatos)
        {
            GuardarDatos();
        }
    }

    private void Update()
    {
        if(navMeshObstacle != null)
        {
            navMeshObstacle.SetActive((!sePuedeAbrir && !puertaAbierta));
        }
    }

    public void GuardarDatos()
    {
        PlayerPrefs.SetInt(gameObject.name + "entreAbierta", System.Convert.ToInt32(entreAbierta));
        PlayerPrefs.SetInt(gameObject.name + "puertaAbierta", System.Convert.ToInt32(puertaAbierta));
    }

    public void EntreAbrirPuerta()
    {
        pomoAnim.SetTrigger("open");

        if (sePuedeAbrir && !puertaAbierta)
        {
            puertaAbierta = true;
            entreAbierta = true;

            EncenderAnim();

            puertaAnim.SetBool("EntreAbierta", entreAbierta);

            Invoke(nameof(ApagarAnim), 3);
            //puertaAnim.SetBool("open", puertaAbierta);
        }
    }

    public void AbrirPuerta()
    {
        //CancelInvoke(nameof(ApagarAnim));
        EncenderAnim();
        pomoAnim.SetTrigger("open");  

        if (!iniciaDialogo)
        {
            if (sePuedeAbrir)
            {
                if (entreAbierta)
                {
                    if (abreLaEntreAbierta) { puertaAbierta = true; if (sonidoAbrirPuerta != null) { sonidoAbrirPuerta.Play(); } }
                    else { puertaAbierta = false; if (sonidoCerrarPuerta != null) { sonidoCerrarPuerta.Play(); } }
                }
                else
                {
                    if (puertaAbierta) //CERRAR PUERTA
                    {
                        puertaAbierta = false;
                        if (sonidoCerrarPuerta != null) { sonidoCerrarPuerta.Play(); }
                        tiempoTocando = 9;
                    }
                    else //ABRIR PUERTA
                    {
                        puertaAbierta = true;
                        if (sonidoAbrirPuerta != null) { sonidoAbrirPuerta.Play(); }
                        if(sonidoGolpearPuerta != null) { sonidoGolpearPuerta.Stop(); }
                        if (activaAlgo) { queActiva.SetActive(true); }
                    }
                }

                entreAbierta = false;
                puertaAnim.SetBool("EntreAbierta", entreAbierta);
                puertaAnim.SetBool("open", puertaAbierta);


            }
            else if(!sePuedeAbrir && puertaAbierta)
            {
                puertaAbierta = false;
                if (sonidoCerrarPuerta != null) { sonidoCerrarPuerta.Play(); }
                tiempoTocando = 9;

                puertaAnim.SetBool("open", puertaAbierta);

            }
            else if (generaPensamiento && !blockPensamiento)
            {
                pensamientoControler.MostrarPensamiento(pensamiento, 2.5f);
                blockPensamiento = true;
                Invoke(nameof(UnblockPensamiento), 3);
            }
        }
        else
        {
            iniciaDialogo = false;
            print("abrirDialogo");
            npcDialogue.SetAbrirDialogo(idDialogo);
            hablado = true;
        }

        Invoke(nameof(ApagarAnim), 3);

    }

    [HideInInspector] public bool hablado;

    void UnblockPensamiento()
    {
        blockPensamiento = false;
    }    

    public void SetPuertaAbierta()
    {

        EncenderAnim();
        //CancelInvoke(nameof(ApagarAnim));

        pomoAnim.SetTrigger("open");
        puertaAbierta = true;
        if (sonidoAbrirPuerta != null) { sonidoAbrirPuerta.Play(); }
        puertaAnim.SetBool("EntreAbierta", entreAbierta);
        puertaAnim.SetBool("open", puertaAbierta);

        Invoke(nameof(ApagarAnim), 3);

    }

    public void SetCerrarPuerta()
    {
        print("Set_CerrarPuerta" + gameObject.name);
        EncenderAnim();
        //CancelInvoke(nameof(ApagarAnim));

        if (sonidoCerrarPuerta != null) { sonidoCerrarPuerta.Play(); }

        pomoAnim.SetTrigger("open");
        puertaAbierta = false;
        puertaAnim.SetBool("EntreAbierta", entreAbierta);
        puertaAnim.SetBool("open", puertaAbierta);

        Invoke(nameof(ApagarAnim), 3);

    }

    public void TocarTimbre()
    {
        if (puedeTocarTimbre)
        {
            sonidoTimbre.Play();
            puedeTocarTimbre = false;
            Invoke(nameof(DesbloquearTocarTimbre), 5);
        }
    }

    public void DesbloquearTocarTimbre()
    {
        puedeTocarTimbre = false;
    }

    bool puedeTocarTimbre;

    public float tiempoTocando;
    public void SpamTocarTombre()
    {
        tiempoTocando += Time.deltaTime;

        if (tiempoTocando > 10)
        {
            sonidoTimbre.Play();
            tiempoTocando = 0;
        }

    }

    ItemAtributes llave;

    bool usandoCerradura;
    public void UsarCerradura(int llaveId, ItemAtributes thisLlave)
    {

        Invoke(nameof(AnimacionCerradura),0.2f);
        usandoCerradura = true;
        llave = thisLlave;

        if (llaveId == idPuerta)
        {
            sePuedeAbrir = true;
            Invoke(nameof(SetPuertaAbierta), 1.2f);
        }
        else
        {
            pensamientoControler.MostrarPensamiento("Esta llave no encaja", 1 );
        }

        Invoke(nameof(RecuperarLlave), 1.2f);
        Invoke(nameof(ApagarAnim), 2);

    }

    public void AnimacionCerradura()
    {
        EncenderAnim();
        cerraduraAnim.SetTrigger("UsarCerradura");
    }

    public void RecuperarLlave()
    {

        usandoCerradura = false;

        llave.abriendoPuerta = false;

        llave.gameObject.layer = 6;
        llave.mesh.layer = 6;
        Transform[] children = llave.mesh.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            children[i].gameObject.layer = 6;
        }
    }

    void EncenderAnim()
    {

        CancelInvoke(nameof(ApagarAnim));

        pomoAnim.enabled = true;
        puertaAnim.enabled = true;

        if(tieneCerradura) { cerraduraAnim.enabled = true; }

    }

    void ApagarAnim()
    {
        if (!usandoCerradura)
        {
            pomoAnim.enabled = false;
            puertaAnim.enabled = false;

            if (cerraduraAnim != null) { cerraduraAnim.enabled = false; }
        }

    }

    public void RomperPuerta()
    {
        print("RomperPuerta");
        EncenderAnim();
        puertaAnim.SetTrigger("RomperPuerta");
        sePuedeAbrir = false;
    }


}
