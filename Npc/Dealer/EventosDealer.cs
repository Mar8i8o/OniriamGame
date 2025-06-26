using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventosDealer : MonoBehaviour
{
    TimeController timeController;
    public NPC_Dealer npc_Dealer;
    PajarosManager pajarosManager;

    public bool eventoConoceDealerActivo;

    public GameObject posicionEscondido;
    public GameObject player;
    CamaraFP camaraFP;
    public GameObject panelDialogo;

    public GameObject triggerDialogoPrimeraInteraccion;

    public Vector3 destinoPos;

    public AudioSource audioMetal;

    public bool forzarMirada;
    public Transform dondeMira;

    PensamientoControler pensamientoControler;

    public DoorController puertaAtico;

    public GameObject chavolaPorContruir;
    public GameObject chavolaEnConstruccion1;
    public GameObject chavolaConstruida;

    void Start()
    {
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();
        pajarosManager = GameObject.Find("PajarosManager").GetComponent<PajarosManager>();
        timeController = GameObject.Find("GameManager").GetComponent<TimeController>();
        camaraFP = player.GetComponent<CamaraFP>();
        camaraFP = player.GetComponent<CamaraFP>();

        triggerDialogoPrimeraInteraccion.SetActive(false);

        //Invoke(nameof(EmpezarEventoConocerDealer), 10);

        if(timeController.dia == 1)
        {
            chavolaConstruida.SetActive(false);
            chavolaEnConstruccion1.SetActive(false);
            chavolaPorContruir.SetActive(true);
        }
        else if (timeController.dia == 2)
        {
            chavolaConstruida.SetActive(false);
            chavolaEnConstruccion1.SetActive(true);
            chavolaPorContruir.SetActive(false);
        }
        else if (timeController.dia >= 3) 
        {
            chavolaConstruida.SetActive(true);
            chavolaEnConstruccion1.SetActive(false);
            chavolaPorContruir.SetActive(false);
        }

    }


    bool hanHablado;
    void Update()
    {
        ControlarEventos();
        ControlarHorasEventos();

        if(forzarMirada)
        {
            camaraFP.ForzarMiradaX(dondeMira, 5);
            camaraFP.ForzarMiradaY(dondeMira, 5);
        }
    }
    public void ControlarHorasEventos()
    {
        if(timeController.dia == 1 && !eventoConoceDealerActivo && !hanHablado)
        {
            if(timeController.hora >= 15 && timeController.minutes >= 30)
            {
                if (!pajarosManager.playerCerca)
                {
                    EmpezarEventoConocerDealer();
                }
            }
            else
            {
                if (npc_Dealer.gameObject.activeSelf) { npc_Dealer.gameObject.SetActive(false); }
                //print("ApagarDealer");
            }
        }
    }

    public GameObject posicionHablarPlayer;

    public void ControlarEventos()
    {
        if (eventoConoceDealerActivo)
        {
            npc_Dealer.MirarEjeY(player.transform, 1);

            if (panelDialogo.activeSelf)
            {
                if (destinoPos == Vector3.zero) destinoPos = posicionHablarPlayer.transform.position;
                npc_Dealer.agent.SetDestination(destinoPos);
                hanHablado = true;
            }
            else
            {
                npc_Dealer.agent.SetDestination(posicionEscondido.transform.position);
                if (hanHablado) //FINALIZAR EVENTO
                {
                    npc_Dealer.rutinaNormal = true;
                    npc_Dealer.volviendo = true;
                    eventoConoceDealerActivo = false;
                    puertaAtico.generaPensamiento = true;
                }
            }

        }
    }

    public void EmpezarEventoConocerDealer()
    {

        npc_Dealer.gameObject.SetActive(true);
        npc_Dealer.rutinaNormal = false;
        npc_Dealer.npcDialogue.canTalk = false;

        npc_Dealer.gameObject.transform.position = posicionEscondido.transform.position;

        eventoConoceDealerActivo = true;
        audioMetal.Play();

        dondeMira = audioMetal.gameObject.transform;
        forzarMirada = true;

        triggerDialogoPrimeraInteraccion.SetActive(true);

        Invoke(nameof(DejarDeMirar), 1);
        LlamarPensamieto("He oído algo arriba, debería subir a ver qué ha pasado", 1, 4);

    }

    public void DejarDeMirar()
    {
        forzarMirada = false;
    }

    Vector3 GenerarPuntoDelante()
    {
        return player.transform.position + (player.transform.forward * 2);
    }

    string pensamientoAMostrar;
    float duracionPensamientoAMostrar;
    bool llamandoPensamiento;

    public void LlamarPensamieto(string pensamiento, float tiempo, float duracion)
    {
        pensamientoAMostrar = pensamiento;
        duracionPensamientoAMostrar = duracion;
        Invoke(nameof(MostrarPensamiento), tiempo);
        llamandoPensamiento = true;
    }

    void MostrarPensamiento()
    {
        pensamientoControler.MostrarPensamiento(pensamientoAMostrar, duracionPensamientoAMostrar);
        llamandoPensamiento = false;
    }
}
