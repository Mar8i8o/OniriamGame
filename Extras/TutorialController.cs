using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public bool tutorial;

    public bool blockTutorial;

    public TimeController timeController;

    public PensamientoControler pensamientoControler;
    public PlayerStats playerStats;
    public CamaController camaController;
    public AlarmController alarmController;
    public BrazoController brazoController;
    public ComputerController computerController;
    public PedidoSpawnerController pedidoSpawnerController;
    public ChairController chairController;
    public LlamadasController llamadasController;
    public CamaraFP camaraFP;
    public GameObject puntoEnemigo;
    public EnemyController enemyController;
    public AudioSource audioSusto;
    public AudioSource audioTimbre;

    public DetectarPlayer detectarPlayerPasillo;

    public ControlarEventoLlamada llamadaAmigo;

    string pensamientoAMostrar;
    float duracionPensamientoAMostrar;

    public int indiceTutorial;

    public float tiempoIndice;

    public GameObject[] triggers;
    public DoorController triggerCocina;
    public DoorController triggerEntrada;

    public GameObject sillaPC;

    ItemAtributes burgerItem;

    GameObject itemPedido;
    GameObject cajaItemPedido;

    public GameObject blockTrigger;

    public TiendaController tiendaController;

    public Animator enemigoEsquinaAnim;

    float contador;

    public GameObject correoAmigo;
    public EventosCorreos eventosCorreos;


    void Start()
    {
        if(timeController.dia == 1 && !blockTutorial) { tutorial = true; }

        if (tutorial) { IniciarTutorial(); blockTrigger.SetActive(true); }
    }

    void Update()
    {

        print("Puerta entrada: " + triggerEntrada.sePuedeAbrir);

        if (tutorial)
        {

            if(SeHaPasado(12,30))
            {
                timeController.freezeTime = true;
                print("se ha pasado");
            }

            tiempoIndice += Time.deltaTime;

            if (indiceTutorial == 0)
            {
                //pensamientoControler.MostrarPensamiento("Otra vez el mismo sueño", 2);
                LlamarPensamieto("Otra vez el mismo sueño", 2, 3);
                //LlamarPensamieto("Pulsa [TAB] para abrir el reloj", 2, 4);
                indiceTutorial++;
                tiempoIndice = 0;
                contador = 0;
                if(tiempoIndice > 5)
                {
                    indiceTutorial++;
                    pensamientoControler.DejarDeMostrarPensamiento();
                }
            }
            if (indiceTutorial == 1)
            {
                if (!alarmController.sonandoAlarma)
                {
                    contador += Time.deltaTime; ;
                    if (contador > 1)
                    {

                        brazoController.puedeSacarBrazo = true;
                        if (!pensamientoControler.mostrandoPensamiento) pensamientoControler.MostrarPensamiento("Manten pulsado [TAB] para sacar el reloj", 5);
                        else
                        {
                            pensamientoControler.DejarDeMostrarPensamiento();
                            LlamarPensamieto("Manten pulsado [TAB] para sacar el reloj", 5, 1);
                            ;
                        }
                        indiceTutorial++;
                        tiempoIndice = 0;

                    }
                }

            }
            else if (indiceTutorial == 2) 
            {

                if(tiempoIndice > 10)
                {
                    if (!pensamientoControler.mostrandoPensamiento) pensamientoControler.MostrarPensamiento("Manten pulsado [TAB] para sacar el reloj", 5);
                }

                if (brazoController.brazoExtendido)
                {
                    indiceTutorial++;
                    tiempoIndice = 0;
                    pensamientoControler.DejarDeMostrarPensamiento();
                    LlamarPensamieto("Tengo ganas de ir al baño", 1, 1);
                }
            }
            else if (indiceTutorial == 3)
            {
                if(tiempoIndice > 1 && camaController.usandoCama && !pensamientoControler.mostrandoPensamiento && !llamandoPensamiento)
                {
                    if (!pensamientoControler.mostrandoPensamiento && !llamandoPensamiento)
                    {
                        LlamarPensamieto("[C] para levantarse", 1, 3);
                    }
                    camaController.puedeLevantarse = true;
                }
                if (!camaController.usandoCama)
                {
                    indiceTutorial++;
                    tiempoIndice = 0;
                    pensamientoControler.DejarDeMostrarPensamiento();
                    CancelInvoke(nameof(LlamarPensamieto));

                    chairController.blockSentarseSilla = true;
                    chairController.generaPensamiento = true;
                    chairController.pensamiento = "Deberia ir al baño primero";

                    pensamientoControler.DejarDeMostrarPensamiento();
                    LlamarPensamieto("Deberia ir al baño", 2, 2);
                }
            }
            else if(indiceTutorial == 4)
            {
                if(tiempoIndice > 40)
                {
                    pensamientoControler.MostrarPensamiento("Deberia ir al baño", 2);
                    tiempoIndice = 0;
                }
                if (playerStats.pis < 10)
                {
                    LlamarPensamieto("Tengo hambre, deberia ir a comer algo a la cocina", 4, 3);
                    chairController.pensamiento = "Deberia comer algo antes";
                    triggerCocina.sePuedeAbrir = true;
                    triggerCocina.generaPensamiento = false;
                    triggerEntrada.pensamiento = "Deberia ir a comer algo a la cocina ";
                    playerStats.hambre = 9;
                    indiceTutorial++;
                    tiempoIndice = 0;
                }
                else
                {
                    //pensamientoControler.DejarDeMostrarPensamiento();
                }
            }
            else if (indiceTutorial == 5)
            {
                if (burgerItem == null) { burgerItem = GameObject.Find("BurgerItem_0").GetComponent<ItemAtributes>(); }

                if (tiempoIndice > 40 && !pensamientoControler.mostrandoPensamiento)
                {
                    pensamientoControler.MostrarPensamiento("Deberia comer algo", 2);
                    tiempoIndice = 0;
                }

                if (!burgerItem.active)
                {
                    LlamarPensamieto("Ya no queda comida, deberia pedir algo por internet", 1, 3);
                    triggerEntrada.pensamiento = "Deberia pedir algo por el ordenador";
                    chairController.blockSentarseSilla = false;
                    chairController.generaPensamiento = false;
                    sillaPC.layer = 3; //REACTIVA EL RAYCAST DE LA SILLA
                    indiceTutorial++;
                    tiempoIndice = 0;
                }
            }
            else if (indiceTutorial == 6)
            {
                if(computerController.pantallaEncendida.activeSelf)
                {
                    LlamarPensamieto("La contraseña es mi nombre y mi año de nacimiento", 1, 5);
                    indiceTutorial++;
                    tiempoIndice = 0;
                }
            }
            else if (indiceTutorial == 7)
            {
                if (tiempoIndice > 120)
                {
                    //pensamientoControler.MostrarPensamiento("La contraseña era mi nombre y mi año de nacimiento", 3);
                    tiempoIndice = 0;
                }
                if (computerController.pantallaEncendida.activeSelf && !computerController.panelLogin.activeSelf)
                {
                    pensamientoControler.DejarDeMostrarPensamiento();
                    LlamarPensamieto("Deberia pedir algo en JustFood", 1, 5);
                    indiceTutorial++;
                    tiempoIndice = 0;
                }
            }
            else if (indiceTutorial == 8)
            {
                if (tiempoIndice > 50)
                {
                    pensamientoControler.MostrarPensamiento("Deberia pedir algo en JustFood desde internet", 3);
                    tiempoIndice = 0;
                }
                if (pedidoSpawnerController.itesmActivos >= 1)
                {
                    LlamarPensamieto("Tengo que esperar a que llegue", 1, 3);
                    triggerEntrada.pensamiento = "Aún no ha llegado la comida ";
                    indiceTutorial++;
                    tiempoIndice = 0;
                    //enemyController.gameObject.SetActive(true);
                    enemigoEsquinaAnim.gameObject.SetActive(true);
                }
            }
            else if (indiceTutorial == 9)
            {
                if(tiempoIndice > 10)
                {
                    //LlamarPensamieto("[C] para salir", 1, 1);
                    pensamientoControler.MostrarPensamiento("[C] para salir", 5);
                    tiempoIndice = 0;
                }
                if(!chairController.sentadoEnSilla)
                {
                    pensamientoControler.DejarDeMostrarPensamiento();
                    indiceTutorial++;
                    tiempoIndice = 0;
                }
            }
            else if (indiceTutorial == 10)
            {
                if (computerController.pantallaEncendida.activeSelf) { tiempoIndice = 0; }
                if (tiempoIndice > 4)
                {
                    //llamadaAmigo.EnviarLlamada();
                    eventosCorreos.EnviarCorreo(correoAmigo);
                    llamadasController.GenerarRecibirLlamada("Noah", "amigo_d1_1");
                    indiceTutorial++;
                    tiempoIndice = 0;
                }
            }
            else if (indiceTutorial == 11)
            {
                if(!llamadasController.sonidoLlamada.isPlaying && !llamadasController.llamadaActiva)
                {
                    LlamarPensamieto("Ya ha llegado el pedido", 1, 3);
                    pedidoSpawnerController.ultimoPedido.SpawnearItem();
                    itemPedido = pedidoSpawnerController.ultimoPedido.scriptItemASpawnear.itemRecienSpawneado.gameObject;
                    itemPedido.layer = 2; //HACE QUE EL ITEM NO SEA INTERACTUABLE

                    if(pedidoSpawnerController.ultimoPedido.tieneCaja)
                    {
                        cajaItemPedido = pedidoSpawnerController.ultimoPedido.itemcajaActual.itemRecienSpawneado.gameObject;
                        cajaItemPedido.layer = 2;
                    }

                    pedidoSpawnerController.ultimoPedido.gameObject.layer = 1;
                    audioTimbre.Play();
                    //triggerEntrada.SetActive(true);
                    triggerEntrada.sePuedeAbrir = true;
                    triggerEntrada.generaPensamiento = false;
                    indiceTutorial++;
                    tiempoIndice = 0;
                }
            }
            else if (indiceTutorial == 12)
            {
                if (detectarPlayerPasillo.playerDentroUnico) //ENTRA AL PASILLO
                {
                    audioSusto.Play();
                    itemPedido.layer = 3;
                    if(cajaItemPedido != null) { cajaItemPedido.layer = 3; }
                    indiceTutorial++;
                    tiempoIndice = 0;
                }
            }
            else if (indiceTutorial == 13)
            {
                camaraFP.ForzarMiradaX(puntoEnemigo.transform, 10);
                camaraFP.ForzarMiradaY(puntoEnemigo.transform, 10);

                if (tiempoIndice > 0.5f)
                {
                    indiceTutorial++;
                    //enemyController.escondido = true;
                    enemigoEsquinaAnim.SetBool("Escondido", true);
                    tutorial = false;
                    SalirTutorial();
                    Invoke(nameof(OcultarEnemigo), 2);
                    tiempoIndice = 0;
                    LlamarPensamieto("¿Será el repartidor?", 2, 3);
                }
            }
        }
    }

    public void OcultarEnemigo()
    {
        enemyController.gameObject.SetActive(false);
        enemigoEsquinaAnim.gameObject.SetActive(false);
    }

    public void IniciarTutorial()
    {

        playerStats.pis = 70;
        playerStats.hambre = 10;


        playerStats.muetraPensamientos = false;
        camaController.muestraPensamientos = false;
        playerStats.actualizaStats = false;
        alarmController.SonarAlarma();
        alarmController.alarmaEventsManager.alarmaActiva = true;
        alarmController.alarmaEventsManager.blockComprobarAlarma = true;
        brazoController.puedeSacarBrazo = false;
        camaController.puedeLevantarse = false;
        llamadasController.esperaLlamadaInfinita = true;

        tiendaController.blockPedidos = true;

        //triggerEntrada.SetActive(false);
        //triggerCocina.SetActive(false);

        triggerEntrada.sePuedeAbrir = false;
        print("Cerrar puerta entrada: " + triggerEntrada.sePuedeAbrir);
        triggerEntrada.generaPensamiento = true;
        triggerEntrada.pensamiento = " Deberia ir al baño primero ";

        triggerCocina.sePuedeAbrir = false;
        triggerCocina.generaPensamiento = true;
        triggerCocina.pensamiento = " Deberia ir al baño primero ";


        sillaPC.layer = 2; //PONE LA LAYER IGNORERAYCAST A LA SILLA

        for (int i = 0; triggers.Length > i; i++) 
        {
            triggers[i].SetActive(false);
        }

        //Invoke(nameof(MostrarPensamiento), 5);
    }

    public void SalirTutorial()
    {
        playerStats.muetraPensamientos = true;
        tiendaController.blockPedidos = false;
        camaController.muestraPensamientos = true;
        playerStats.actualizaStats = true;
        alarmController.alarmaEventsManager.blockComprobarAlarma = false;
        //brazoController.puedeSacarBrazo = true;
        //camaController.puedeLevantarse = true;
        llamadasController.esperaLlamadaInfinita = false;
        timeController.freezeTime = false;

        //triggerEntrada.SetActive(true);
        //triggerCocina.SetActive(true);

        for (int i = 0; triggers.Length > i; i++)
        {
            triggers[i].SetActive(true);
        }

        //Invoke(nameof(MostrarPensamiento), 5);
        blockTrigger.SetActive(false);
    }

    public bool SeHaPasado(int horas, int minutos)
    {
        TimeSpan objetivo = new TimeSpan(horas, minutos, 0);
        TimeSpan actual = TimeSpan.FromSeconds(timeController.totalSegundos);
        return actual > objetivo;
    }

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
