using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{

    #region variables

    public float distancia;
    public LayerMask mask;

    public Image puntero;
    public Vector3 maxPuntero;
    public Vector3 minPuntero;

    public GameObject pickUpObjectPosition;
    public GameObject basuraPosition;
    public GameObject eatPosition;
    public GameObject drinkPosition;
    public GameObject movilPosition;
    public GameObject usingMovilPosition;
    public GameObject fregonaPosition;
    public GameObject sabanaPosition;
    public GameObject linternaPosition;
    public GameObject monitorPosition;
    public GameObject velaPosition;
    public GameObject caballetePosition;
    public GameObject cigarPosition;
    public GameObject pildorasPosition;
    public GameObject tazaPosition;
    public GameObject llavePosition;
    public GameObject notaPosition;

    public Animator eatAnimation;
    public Animator drinkAnimation;
    public Animator fregonaAnimation;
    public Animator smokeAnimation;
    public Animator pildorasAnimation;
    [HideInInspector]public GameObject camara;

    public bool hasObject;
    public bool itemGuardado;
    [HideInInspector] public bool eating;
    [HideInInspector] public bool drinking;
    public bool usingComputer;
    [HideInInspector] public bool usingMovil;
    public bool agachado;

    public MovilController movilController;
    [HideInInspector] public PlayerStats playerStats;
    public Transform posicionCamaraInicial;
    public Transform posicionCamaraAgachado;
    public Transform posicionCamaraSuelo;
    [HideInInspector]public GameObject player;
    ElectricidadController electricidadController;

    public CamaraFP camaraScript;

    LlamadasController llamadasController;
    PensamientoControler pensamientoControler;
    DreamController dreamController;
    SacarMovilController sacarMovilController;
    [HideInInspector] public bool canPickUp;

    GameObject computerScreen;
    GameObject pickUpObject;
    Rigidbody pickUpObjectRb;
    public ItemAtributes itemAtributes;

    public float tiempoConObjeto;
    public float tiempoSinObjeto;
    float tiempoComiendo;
    float tiempoBebiendo;

    public float tiempoUsandoPc;
    [HideInInspector] public bool movimientoDespacio;

    public float tiempoSinUsarItem;

    public CapsuleCollider capsule;
    public CharacterController characterController;

    DetectarObjetoCabeza detectarObjetoCabeza;

    GameObject puntoCerradura;

    public bool enElSuelo;

    ComputerController computerController;

    public CorregirPickUp corregirPickUp;


    public GameObject icoLanzarItem;
    public GameObject icoComer;
    public GameObject icoBeber;
    public GameObject icoUsar;
    public GameObject icoAbrir;
    public GameObject icoLeer;
    public GameObject icoDejarDeLeer;
    public GameObject icoRotar;
    public GameObject icoGuardarItem;
    public GameObject icoSacarItem;
    public GameObject icoFumar;
    public GameObject icoTomarPastillas;

    #endregion

    private void Awake()
    {
        minPuntero = puntero.transform.localScale;
        maxPuntero = puntero.transform.localScale * 2;
    }
    void Start()
    {
        
        camara = GameObject.Find("CameraParent");
        player = GameObject.Find("Player");
        dreamController = GameObject.Find("GameManager").GetComponent<DreamController>();
        detectarObjetoCabeza = GameObject.Find("DetectarObjetoCabeza").GetComponent<DetectarObjetoCabeza>();
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();
        if (!dreamController.inDream)
        { 
            computerController = GameObject.Find("CanvasPantallaPc").GetComponent<ComputerController>();
            sonidoComer = GameObject.Find("SonidoComer").GetComponent<AudioSource>();
            sacarMovilController = player.GetComponent<SacarMovilController>();
            llamadasController = GameObject.Find("GameManager").GetComponent<LlamadasController>();
            playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
            electricidadController = GameObject.Find("ElectricidadControler").GetComponent<ElectricidadController>();
        }

        canPickUp = true;

    }

    void Update()
    {
        if (!usingMovil) RayCast();     

        if (hasObject) 
        {
            if (!itemAtributes.comiendoTrozoPizza && !itemAtributes.abriendoPuerta) SoltarObjeto();        
            //if (itemAtributes.isMovil) UsarMovilController();
            if (!drinking && !eating) tiempoSinUsarItem += Time.deltaTime;
            if (itemAtributes.isFood || itemAtributes.isPizza) EatController();
            if (itemAtributes.isWater) DrinkController();
            else if (itemAtributes.isVela) UsarVela();
            else if (itemAtributes.isCigar) SmokeController();
            else if (itemAtributes.isPildoras) PildorasController();
            else if (itemAtributes.isMando) MandoController();
            else if (itemAtributes.isTaza) ControlarTazaCafe();
            

            if (itemAtributes.isFregona)
            {
                LimpiarController();
            }

            tiempoConObjeto += Time.deltaTime;
            tiempoSinObjeto = 0;
        }
        else
        {
            tiempoSinObjeto += Time.deltaTime;
        }

        if (!enElSuelo)
        {
            if (agachado || usandoTobogan)
            {
                EstadoAgachadoUpdate();
            }
            else
            {
                EstadoDepieUpdate();
            }
        }

        ControladorTeclas();

    }

    private void LateUpdate()
    {
        if (hasObject)
        {
            AjustarPuntoCojerObjeto();
        }
    }

    public void EstadoAgachadoUpdate()
    {
        camara.transform.localPosition = Vector3.MoveTowards(camara.transform.localPosition, posicionCamaraAgachado.localPosition, 3f * Time.deltaTime);
        capsule.center = new Vector3(0, -0.62f, 0);
        capsule.height = 0.7f;
        characterController.height = 0.7f;
        characterController.center = new Vector3(0, -0.62f, 0);
    }

    public void EstadoDepieUpdate()
    {
        camara.transform.localPosition = Vector3.MoveTowards(camara.transform.localPosition, posicionCamaraInicial.localPosition, 3f * Time.deltaTime);
        capsule.height = 2;
        characterController.height = 2;
        capsule.center = Vector3.zero;
        characterController.center = Vector3.zero;
    }

    public void AjustarPuntoCojerObjeto()
    {

        if (!eating)
        {
            if (!itemAtributes.isMovil) //NO ES MOVIL
            {
                if (itemAtributes.isFregona)
                {
                    pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, fregonaPosition.transform.position, 9 * Time.deltaTime);
                    //else pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, fregonaPosition.transform.position, 0.005f);
                    pickUpObject.transform.rotation = fregonaPosition.transform.rotation;
                }
                else if (itemAtributes.isNota)
                {
                    pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, notaPosition.transform.position, 9 * Time.deltaTime);
                    pickUpObject.transform.rotation = notaPosition.transform.rotation;
                }
                else if (itemAtributes.isVela)
                {
                    pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, velaPosition.transform.position, 9 * Time.deltaTime);
                }
                else if (itemAtributes.isWater)
                {
                    pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, drinkPosition.transform.position, 9 * Time.deltaTime);
                }
                else if (itemAtributes.isPildoras)
                {
                    pickUpObject.transform.position = Vector3.MoveTowards(pildorasAnimation.transform.position, pildorasAnimation.transform.position, 9 * Time.deltaTime);
                    pickUpObject.transform.rotation = pildorasPosition.transform.rotation;
                }
                else if (itemAtributes.isTaza)
                {
                    pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, tazaPosition.transform.position, 9 * Time.deltaTime);
                    pickUpObject.transform.rotation = tazaPosition.transform.rotation;
                }
                else if (itemAtributes.isCigar)
                {
                    pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, cigarPosition.transform.position, 9 * Time.deltaTime);
                    pickUpObject.transform.rotation = cigarPosition.transform.rotation;
                }
                else if (itemAtributes.isFood)
                {
                    pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, pickUpObjectPosition.transform.position, 9 * Time.deltaTime);
                    pickUpObject.transform.rotation = pickUpObject.transform.rotation;
                }
                else if (itemAtributes.isLlave)
                {
                    if (itemAtributes.abriendoPuerta)
                    {
                        pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, puntoCerradura.transform.position, 9 * Time.deltaTime);
                        pickUpObject.transform.rotation = puntoCerradura.transform.rotation;
                    }
                    else
                    {
                        pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, llavePosition.transform.position, 9 * Time.deltaTime);
                        pickUpObject.transform.rotation = llavePosition.transform.rotation;
                    }
                }
                //else if (tiempoConObjeto < 2) pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, pickUpObjectPosition.transform.position, 0.09f);
                else
                {
                    //pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, pickUpObjectPosition.transform.position, 0.005f);
                    float moveSpeed = tiempoConObjeto < 2 ? 9f : 0.5f;

                    // Calcula la posición objetivo basada en el offset relativo a la cámara
                    Vector3 targetPosition = pickUpObjectPosition.transform.position + pickUpObjectPosition.transform.rotation * itemAtributes.Offset;

                    // Mueve el objeto hacia la posición objetivo
                    pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, targetPosition, moveSpeed * Time.deltaTime);

                    // Aplica la rotación del objeto en relación a la cámara
                    pickUpObject.transform.eulerAngles = pickUpObjectPosition.transform.eulerAngles + itemAtributes.Rotation;
                }
            }
            else if (itemAtributes.isMovil)//ES MOVIL
            {
                if (!usingMovil)
                {
                    if (tiempoConObjeto < 2 && !movimientoDespacio) pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, movilPosition.transform.position, 9 * Time.deltaTime);
                    else pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, movilPosition.transform.position, 0.5f * Time.deltaTime);
                }
                else
                {
                    pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, usingMovilPosition.transform.position, 0.5f * Time.deltaTime);
                }
            }

        }
    }

    public void DejarDeUsarOrdenador()
    {
        Invoke(nameof(DesactivarUsingComputer), 0.1f);
        puntero.enabled = true;
        tiempoUsandoPc = 0;
        screenController.mouseController.punteroActivo = false;
        player.GetComponent<CamaraFP>().freezeCamera = false;
        GameObject.Find("CanvasPantallaPc").GetComponent<ComputerController>().ApagarOrdenador();
    }
    public void DesactivarUsingComputer()
    {
        usingComputer = false;
    }

    public bool blockAgacharse;

    [HideInInspector] public bool pulsandoAgachado;
    [HideInInspector] public bool usandoTobogan;
    public void ControladorTeclas()
    {
        if (!blockAgacharse) ////AGACHARSE
        {
            if (!sentado && !camaraScript.freeze)
            {
                if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.C))
                {
                    agachado = true;
                    pulsandoAgachado = true;
                }
                if (Input.GetKeyUp(KeyCode.C)) 
                {
                    //print("levantarse");
                    pulsandoAgachado = false;
                    if (!detectarObjetoCabeza.objetoEncima) { agachado = false; } 
                }
            }
            else
            {
                pulsandoAgachado = false;
            }
        }
        else
        {
            pulsandoAgachado = false;
        }

        if (usingComputer)
        {
            //camara.transform.LookAt(computerScreen.transform.position);
            //camaraScript.ForzarMirada(computerScreen.transform, 4);
            camaraScript.ForzarMiradaX(computerScreen.transform, 4);
            camaraScript.ForzarMiradaY(computerScreen.transform, 4);

            tiempoUsandoPc += Time.deltaTime;

            if (!electricidadController.electricidad)
            {
                DejarDeUsarOrdenador();
            }

            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                DejarDeUsarOrdenador();
            }
        }

        if(hasObject)
        {
            if(Input.GetKeyDown(KeyCode.G))
            {
                if(itemGuardado)
                {
                    SacarItem();
                }
                else
                {
                    GuardarItem();
                }
            }
        }
        
    }

    public void GuardarItem()
    {
        itemGuardado = true;

        OcultarIconos();

        icoGuardarItem.SetActive(false);
        icoSacarItem.SetActive(true);

        if (itemAtributes.mostrandoTexto) { itemAtributes.DejarDeMostrarTexto(); }
    }

    public void SacarItem()
    {
        itemGuardado = false;
        icoGuardarItem.SetActive(true);
        icoSacarItem.SetActive(false);

        if (usingMovil)
        {
            movilController.DejarDeUsarMovil();
        }
        if (sacarMovilController.movilSacado)
        {
            sacarMovilController.GuardarMovil();
        }

        MostrarIconos();
    }

    #region UsosItems

    public Animator animVela;
    public void UsarVela()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && tiempoConObjeto > 0.1f && !seleccionando)
        {
            itemAtributes.UsarVela();
            animVela.SetTrigger("Usar");
        }
    }

    [HideInInspector] public bool limpiando;
    public void LimpiarController()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && tiempoConObjeto > 0.1f)
        {
            if (!limpiando)
            {
                fregonaAnimation.SetTrigger("Limpiar");
                limpiando = true;
                Invoke(nameof(CancelarLimpiar), 0.7f);
            }
        }
    }

    public void CancelarLimpiar()
    {
        limpiando = false;
    }

    bool pressDrink;
    float tiempoBebiendoLoop;
    public void DrinkController()
    {
        if (!drinking)
        {

            pickUpObject.transform.rotation = drinkPosition.transform.rotation;

            if (Input.GetKeyDown(KeyCode.Mouse0) && tiempoConObjeto > 0.2f && itemAtributes.scriptLiquido.usos > 0 && tiempoSinUsarItem > 0.15f && !seleccionando)
            {
                drinking = true;
                tiempoBebiendo = 0;
                tiempoSinUsarItem = 0;
                //itemAtributes.scriptLiquido.usos--;
                drinkAnimation.SetBool("Drinking", true);
            }

        }

        if (Input.GetKey(KeyCode.Mouse0) && tiempoConObjeto > 0.2f)
        {
            pressDrink = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && tiempoConObjeto > 0.2f)
        {
            pressDrink = false;
        }

        if (drinking)
        {

            tiempoBebiendo += Time.deltaTime;
            tiempoBebiendoLoop += Time.deltaTime;

            if (tiempoBebiendoLoop > 2) 
            {
                itemAtributes.scriptLiquido.usos--;
                tiempoBebiendoLoop = 0;
            }

            if (itemAtributes.scriptLiquido.usos <= 0)
            {
                pressDrink = false;
                drinking = false;
                drinkAnimation.SetBool("Drinking", false);
                icoBeber.SetActive(false);
            }

            itemAtributes.scriptLiquido.cantidadDeAgua -= Time.deltaTime * itemAtributes.scriptLiquido.WobbleSpeed;
            playerStats.agua += Time.deltaTime * 2;
            playerStats.pis += Time.deltaTime;
            if (itemAtributes.isLata) playerStats.cansancio -= Time.deltaTime;

            //pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, drinkPosition.transform.position, 0.001f);
            pickUpObject.transform.rotation = drinkPosition.transform.rotation;
            //pickUpObject.transform.LookAt(camara.transform.position);

            if (tiempoBebiendo > 2 && !pressDrink)
            {
                drinking = false;
                tiempoBebiendo = 0;
                //Destroy(pickUpObject);

                drinkAnimation.SetBool("Drinking", false);
            }

        }
    }

    public AudioSource sonidoComer;
    public void EatController()
    {
        if (!itemAtributes.isPizza)
        {
            if (!eating && !seleccionando)
            {

                //tiempoConObjeto += Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.Mouse0) && tiempoConObjeto > 0.2f && tiempoSinUsarItem > 0.15f && !itemGuardado)
                {
                    if (playerStats.hambre < 90)
                    {
                        eating = true;
                        tiempoComiendo = 0;
                        tiempoSinUsarItem = 0;
                        eatAnimation.SetBool("Eating", true);
                        sonidoComer.Play();
                        icoComer.SetActive(false);
                        icoGuardarItem.SetActive(false);
                    }
                    else
                    {
                        pensamientoControler.MostrarPensamiento("No puedo comer mas, estoy lleno", 1);
                    }
                    
                }

            }

            if (eating)
            {

                tiempoComiendo += Time.deltaTime;

                pickUpObject.transform.position = Vector3.MoveTowards(pickUpObject.transform.position, eatPosition.transform.position, 5 * Time.deltaTime);
                //pickUpObject.transform.LookAt(camara.transform.position);

                if (tiempoComiendo > itemAtributes.cuantoTardaEnComer)
                {
                    hasObject = false;
                    eating = false;
                    tiempoComiendo = 0;


                    //Destroy(pickUpObject);
                    
                    Transform[] children = itemAtributes.mesh.GetComponentsInChildren<Transform>();

                    for (int i = 0; i < children.Length; i++)
                    {
                       children[i].gameObject.layer = 3;
                    }
                    

                    ForzarSoltarObjeto();

                    itemAtributes.active = false;
                    itemAtributes.DesactivarItem();
                    itemAtributes.transform.position = Vector3.zero;
                    pickUpObject.transform.SetParent(null);
                    //pickUpObject.GetComponentInChildren<ItemAtributes>().GuardarPosicion();


                    playerStats.hambre += itemAtributes.cuantoLlena;

                    eatAnimation.SetBool("Eating", false);
                }

            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && tiempoConObjeto > 0.2f && !seleccionando && !itemGuardado)
            {
                if (playerStats.hambre < 90)
                {
                    itemAtributes.ComerTrozoPizza();
                    sonidoComer.Play();

                    if(itemAtributes.indiceTrozosPizza >= 7)
                    {
                        icoComer.SetActive(false);
                    }

                }
                else
                {
                    pensamientoControler.MostrarPensamiento("No puedo comer mas, estoy lleno", 1);
                }
            }
        }
    }
    public void UsarMovilController() //OBSOLETO
    {
        if ((Input.GetKeyDown(KeyCode.Mouse0) && tiempoConObjeto > 0.2f))
        {
            if (!usingMovil && !seleccionando)
            {
                //player.GetComponent<CamaraFP>().freezeCamera = true;
                print("usarMovil");
                player.GetComponent<CamaraFP>().freeze = true;
                movilController.AbrirMovil();

                movilController.sacarMovilController.icoDejarDeUsarMovil.SetActive(true);
                movilController.sacarMovilController.icoUsarMovil.SetActive(false);
                movilController.sacarMovilController.icoGuardarMovil.SetActive(false);

                movilController.EncenderPantalla();
                movilController.encendido = true;
                puntero.enabled = false;
                usingMovil = true;
            }
        }
    }

    [HideInInspector]public bool bebiendoCafe;
    public Animator tazaAnim;

    float tiempoBebiendoTaza;
    public void ControlarTazaCafe()
    {
        
        if ((Input.GetKeyDown(KeyCode.Mouse0) && tiempoConObjeto > 0.2f) && !seleccionando && !bebiendoCafe && !itemGuardado)
        {
            if (itemAtributes.cafeController.cantidadCafe > 0)
            {
                tiempoBebiendoTaza = 0;
                bebiendoCafe = true;
                tazaAnim.SetBool("Bebiendo", true);
                print("bebiendo");
            }
        }
        if ((Input.GetKey(KeyCode.Mouse0) && tiempoConObjeto > 0.2f) && !seleccionando && !itemGuardado)
        {
            tiempoBebiendoTaza += Time.deltaTime;

            if (itemAtributes.cafeController.cantidadCafe <= 0)
            {
                TerminarDeBeber();
                icoBeber.SetActive(false);
            }
        }
        if ((Input.GetKeyUp(KeyCode.Mouse0) && tiempoConObjeto > 0.2f) && !seleccionando)
        {
            if (tiempoBebiendoTaza > 1f)
            {
                TerminarDeBeber();
            }
            else
            {
                Invoke(nameof(TerminarDeBeber), 1f - tiempoBebiendoTaza);
            }
        }

        if (bebiendoCafe) 
        {
            itemAtributes.cafeController.cantidadCafe -= Time.deltaTime * 2f;
            playerStats.cansancio -= Time.deltaTime * 0.25f;
        }

    }
    public void TerminarDeBeber()
    {
        tazaAnim.SetBool("Bebiendo", false);
        bebiendoCafe = false;
    }

    public void MandoController()
    {
        if ((Input.GetKeyDown(KeyCode.Mouse0) && tiempoConObjeto > 0.2f) && !seleccionando && !itemGuardado)
        {
            itemAtributes.PulsarMando();
            GameObject.Find("TriggerTVPrincipal").GetComponent<ControlTV>().InteractuarTV();
        }
    }

    bool tomandoPildoras;
    float tiempoTomandoPildoras;
    public void PildorasController()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && tiempoConObjeto > 0.2f)
        {
            if (!itemAtributes.boteVacio)
            {
                pildorasAnimation.SetBool("Drinking", true);
                tomandoPildoras = true;
                tiempoTomandoPildoras = 0;

                itemAtributes.AbrirTapa();
            }
        }

        if (tomandoPildoras)
        {
            tiempoTomandoPildoras += Time.deltaTime;

            if (tiempoTomandoPildoras > 1)
            {
                print("consumir pildoras");
                itemAtributes.ConsumirPildoras();
                tomandoPildoras = false;
                pildorasAnimation.SetBool("Drinking", false);
                itemAtributes.CerrarTapa();

                playerStats.cansancio -= 5;
                playerStats.locura -= 10;
                playerStats.pastillas += 10;
            }
        }
    }

    float tiempoFumando;
    float tiempoFumandoReal;
    bool fumando;
    public void SmokeController()
    {
        if (!itemAtributes.cigarroConsumido)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && tiempoConObjeto > 0.2f && !seleccionando && !fumando)
            {
                //tiempoSinUsarItem = 0;
                CancelInvoke(nameof(DejarDeFumar));
                smokeAnimation.SetBool("Smoke", true);
                itemAtributes.ConsumirCigarro();
                tiempoFumando = 0;
                tiempoFumandoReal = 0;

                fumando = true;

            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                tiempoFumando += Time.deltaTime;
                tiempoFumandoReal += Time.deltaTime;

                if (tiempoFumando > 1f)
                {
                    itemAtributes.DarCalada();
                    tiempoFumando = 0;
                }

            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (tiempoFumandoReal > 1) { smokeAnimation.SetBool("Smoke", false); DejarDeFumar(); }
                else { Invoke(nameof(DejarDeFumar), 1 - tiempoFumandoReal); }
                
            }
        }
        else
        {
            smokeAnimation.SetBool("Smoke", false);
            Invoke(nameof(DejarDeFumar), 1);
            itemAtributes.ApagarCigarro();
        }

        if (fumando) 
        {
            playerStats.fumada += Time.deltaTime * 3;
            playerStats.cansancio += Time.deltaTime * 0.1f;
        }

    }

    public void DejarDeFumar()
    {
        fumando = false;
        smokeAnimation.SetBool("Smoke", false);
    }

    [HideInInspector]public bool sentado;
    public bool tumbado;
    [HideInInspector]public GameObject posicionSilla;

    ScreenController screenController;

    #endregion

    [HideInInspector] public bool blockRaycast;

    public void RayCast()
    {

        if(blockRaycast) { return; }

        Debug.DrawRay(transform.position, transform.forward, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distancia, mask))
        {

            if (hit.collider != null && hit.collider.CompareTag("PickableObject"))
            {
                if (canPickUp)
                {
                    ItemAtributes itemAtributeshit;
                    itemAtributeshit = hit.collider.GetComponent<ItemAtributes>();
                    if (!itemAtributeshit.isCaballete)
                    {
                        if (!hasObject)
                        {
                            CursorSeleccion();

                            if (Input.GetKeyDown(KeyCode.Mouse0) && hasObject == false)
                            {
                                CogerObjeto(hit.collider.gameObject);
                            }
                        }
                    }
                    else
                    {
                        if (!hasObject)
                        {
                            CursorSeleccion();

                            if (Input.GetKeyDown(KeyCode.Mouse0) && hasObject == false)
                            {
                                CogerObjeto(hit.collider.gameObject);
                            }
                        }
                        else
                        {
                            if (itemAtributes.isLienzo)
                            {
                                CursorSeleccion();

                                if (Input.GetKeyDown(KeyCode.Mouse0))
                                {
                                    hit.collider.GetComponent<CaballeteController>().ColocarLienzo(itemAtributes.gameObject);
                                }
                            }
                        }
                    }
                }
            }

            else if (hit.collider != null && hit.collider.CompareTag("Silla"))
            {
                CursorSeleccion();

                if (Input.GetKeyDown(KeyCode.Mouse0) )
                {
                    if (!sentado)
                    {
                        ChairController chairScript;
                        chairScript = hit.collider.GetComponent<ChairController>();
                        chairScript.SentarseSilla();
                        if (!chairScript.generaPensamiento)
                        {
                            //posicionSilla = chairScript.posicionSilla;
                            player.GetComponent<CapsuleCollider>().enabled = false;
                            player.GetComponent<CamaraFP>().freeze = true;
                            player.GetComponent<CharacterController>().enabled = false;
                            //sentado = true;
                        }
                    }
                    else if (sentado) 
                    {
                        sentado = false;
                        player.GetComponent<CapsuleCollider>().enabled = true;
                        player.GetComponent<CamaraFP>().freeze = false;
                        player.GetComponent<CharacterController>().enabled = true;
                    }
                }
            }

            else if (hit.collider != null && hit.collider.CompareTag("Computer"))
            {

                if (sentado && !hasObject)
                {
                    CursorSeleccion();
                    if (Input.GetKeyDown(KeyCode.Mouse0) && hasObject == false)
                    {
                        if (electricidadController.electricidad)
                        {

                            if (!computerController.pantallaEncendida.activeSelf)
                            {
                                computerController.EncenderOrdenador();
                                puntero.enabled = false;
                                screenController = hit.collider.GetComponent<ScreenController>();
                                screenController.mouseController.punteroActivo = true;
                                camaraScript.freezeCamera = true;
                                computerScreen = hit.collider.gameObject;
                                usingComputer = true;
                            }
                        }
                        else
                        {
                            pensamientoControler.MostrarPensamiento("No puedo usar el ordenador sin electricidad", 1);
                        }
                    }
                }
            }

            else if (hit.collider != null && hit.collider.CompareTag("NPC"))
            {
                NpcDialogue npcDialogue;
                npcDialogue = hit.collider.gameObject.GetComponent<NpcDialogue>();
                CursorSeleccion();
                if (npcDialogue.canTalk)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        npcDialogue.AbrirDialogo();
                    }
                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("Accion"))
            {
                TriggerAccion triggerAccion;
                triggerAccion = hit.collider.gameObject.GetComponent<TriggerAccion>();
                CursorSeleccion();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    triggerAccion.IniciarAccion();
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("Obstaculo"))
            {
                InterectuarObstaculo interectuarObstaculo;
                interectuarObstaculo = hit.collider.gameObject.GetComponent<InterectuarObstaculo>();
                CursorSeleccion();
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    interectuarObstaculo.Interactuar();
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("Taquilla"))
            {
                TaquillaController taquillaController;
                taquillaController = hit.collider.gameObject.GetComponent<TaquillaController>();
                CursorSeleccion();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    taquillaController.Interactuar();
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("Sofa"))
            {
                SofaController sofaController;
                sofaController = hit.collider.gameObject.GetComponent<SofaController>();
                if (!camaraScript.freeze)
                {
                    CursorSeleccion();

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        sofaController.Usar();
                    }
                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("Mirilla"))
            {     
                MirillaController mirillaController;
                mirillaController = hit.collider.gameObject.gameObject.GetComponent<MirillaController>();

                //mirillaController.AbrirTapaMirilla();

                if (!hasObject)
                {
                    CursorSeleccion();
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        mirillaController.AbrirMirilla();
                    }
                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("PomoPuerta"))
            {
                CursorSeleccion();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    DoorController doorController;
                    doorController = hit.collider.gameObject.GetComponent<DoorController>();

                    if (hasObject)
                    {
                        if (itemAtributes.isLlave && doorController.tieneCerradura && !doorController.sePuedeAbrir)
                        {

                            puntoCerradura = doorController.puntoCerradura;
                            doorController.UsarCerradura(itemAtributes.idLlave, itemAtributes);
                            itemAtributes.abriendoPuerta = true;

                            pickUpObject.layer = 3;
                            itemAtributes.mesh.layer = 3;
                            Transform[] children = itemAtributes.mesh.GetComponentsInChildren<Transform>();

                            for (int i = 0; i < children.Length; i++)
                            {
                               children[i].gameObject.layer = 3;
                            }

                        }
                        else
                        {
                            doorController.AbrirPuerta();
                        }
                    }
                    else
                    {
                        doorController.AbrirPuerta();
                    }

                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("PuertaArmario"))
            {
                CursorSeleccion();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    TriggerPuertaArmario doorController;
                    doorController = hit.collider.gameObject.GetComponent<TriggerPuertaArmario>();
                    doorController.AbrirPuerta();
                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("Pestillo"))
            {
                CursorSeleccion();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    PestilloController pestilloController;
                    pestilloController = hit.collider.gameObject.GetComponent<PestilloController>();
                    pestilloController.AbrirPestillo();
                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("Radio"))
            {
                CursorSeleccion();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Radio radio;
                    radio = hit.collider.gameObject.GetComponent<Radio>();
                    radio.InteractuarRadio();
                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("TapaCaja"))
            {
                CursorSeleccion();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    TapaCaja tapaCaja;
                    tapaCaja = hit.collider.gameObject.GetComponent<TapaCaja>();
                    tapaCaja.Interactuar();
                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("BeberAgua"))
            {
                CursorSeleccion();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    TriggerBeberAgua beberAgua;
                    beberAgua = hit.collider.gameObject.GetComponent<TriggerBeberAgua>();
                    beberAgua.Interactuar();
                }
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    TriggerBeberAgua beberAgua;
                    beberAgua = hit.collider.gameObject.GetComponent<TriggerBeberAgua>();
                    beberAgua.MantenerPulsado();
                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("Alarm"))
            {

                if (!hasObject)
                {
                    AlarmController alarmController;
                    alarmController = hit.collider.gameObject.gameObject.GetComponent<AlarmController>();

                    if (alarmController.sonandoAlarma)
                    {
                        CursorSeleccion();
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            alarmController.UsarAlarma();
                        }
                    }
                    /*
                    else if (!tumbado)
                    {

                        CursorSeleccion();
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            alarmController.UsarAlarma();
                        }
                    }
                    */
                    
                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("Bater"))
            {
                    CursorSeleccion();
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        BaterController baterController;
                        baterController = hit.collider.gameObject.gameObject.GetComponent<BaterController>();
                        baterController.HacerPis();
                    }
            }
            else if (hit.collider != null && hit.collider.CompareTag("TapaBater"))
            {
                    CursorSeleccion();
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        TapaBaterController tapaBaterController;
                        tapaBaterController = hit.collider.gameObject.gameObject.GetComponent<TapaBaterController>();
                        tapaBaterController.Open();
                    }
            }
            else if (hit.collider != null && hit.collider.CompareTag("CadenaBater"))
            {
                CursorSeleccion();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    CadenaController cadenaController;
                    cadenaController = hit.collider.gameObject.gameObject.GetComponent<CadenaController>();
                    cadenaController.TirarCadena();
                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("CharcoPis"))
            {
                if (hasObject)
                {
                    if (itemAtributes.isFregona)
                    { 
                    CursorSeleccion();
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        CharcosPisController charcoPis;
                        charcoPis = hit.collider.gameObject.gameObject.GetComponent<CharcosPisController>();
                        charcoPis.Limpiar();
                    }
                    }
                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("Mancha"))
            {
                if (hasObject)
                {
                    if (itemAtributes.isFregona)
                    {
                        CursorSeleccion();
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            ManchaParedController manchaPared;
                            manchaPared = hit.collider.gameObject.gameObject.GetComponent<ManchaParedController>();
                            manchaPared.Limpiar();
                        }
                    }
                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("Cama"))
            {
                CamaController camaController;
                camaController = hit.collider.gameObject.gameObject.GetComponent<CamaController>();

                if(camaController.tieneSabanas)
                {
                    CursorSeleccion();
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        camaController.InteractuarCama();
                    }
                }
                else if (!camaController.tieneSabanas && hasObject)
                {
                    if (itemAtributes.isSabana && itemAtributes.sabanaLimpia)
                    {
                        CursorSeleccion();
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            camaController.InteractuarCama();
                        }
                    }
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("Lavadora"))
            {

                LavadoraController lavadoraController;
                lavadoraController = hit.collider.gameObject.gameObject.GetComponent<LavadoraController>();
                lavadoraController.ApuntandoLavadora();

                if (lavadoraController.puertaAbierta)
                {
                    CursorSeleccion();

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        lavadoraController.UsarLavadora();
                    }
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("PalancaElectricidad"))
            {

                CursorSeleccion();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    PalancasElectricidad palancasElectricidad;
                    palancasElectricidad = hit.collider.gameObject.gameObject.GetComponent<PalancasElectricidad>();
                    palancasElectricidad.Usar();
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("PuertaElectricidad"))
            {

                CursorSeleccion();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    ElectricidadController electricidadController;
                    electricidadController = hit.collider.gameObject.gameObject.GetComponent<ElectricidadController>();
                    electricidadController.Usar();
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("Cajon"))
            {

                CursorSeleccion();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    CajonController cajonController;
                    cajonController = hit.collider.gameObject.gameObject.GetComponent<CajonController>();
                    cajonController.InteractuarCajon();
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("ControlTV"))
            {

                CursorSeleccion();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    ControlTV controlTV;
                    controlTV = hit.collider.gameObject.gameObject.GetComponent<ControlTV>();
                    controlTV.InteractuarTV();
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("Ducha"))
            {

                CursorSeleccion();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Ducha ducha;
                    ducha = hit.collider.gameObject.gameObject.GetComponent<Ducha>();
                    ducha.Usar();
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("Pensamiento"))
            {

                CursorSeleccion();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    PensamientoTrigger pensamientoTrigger;
                    pensamientoTrigger = hit.collider.gameObject.gameObject.GetComponent<PensamientoTrigger>();
                    pensamientoTrigger.Interactuar();
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("Interruptor"))
            {

                CursorSeleccion();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    TriggerInterruptor interruptor;
                    interruptor = hit.collider.gameObject.gameObject.GetComponent<TriggerInterruptor>();
                    interruptor.Interactuar();
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("Horno"))
            {

                CursorSeleccion();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    HornoController hornoController;
                    hornoController = hit.collider.gameObject.gameObject.GetComponent<HornoController>();
                    hornoController.Interactuar();
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("Gramofono"))
            {

                CursorSeleccion();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    GramofonoController gramofonoController;
                    gramofonoController = hit.collider.gameObject.gameObject.GetComponent<GramofonoController>();
                    gramofonoController.Interactuar();
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("TapaCafe"))
            {
                TapaCafe tapaCafe;
                tapaCafe = hit.collider.gameObject.gameObject.GetComponent<TapaCafe>();
                CursorSeleccion();
                tapaCafe.ApuntarTapa();
                if (hasObject)
                {
                    if (itemAtributes.isCafe)
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            if (tapaCafe.triggerCafetera.cantidadCafe < 9.9f)
                            {
                                tapaCafe.LlenarCafe();
                                ForzarSoltarObjeto();
                                itemAtributes.active = false;
                                itemAtributes.DesactivarItem();
                                itemAtributes.transform.position = Vector3.zero;
                            }
                            else
                            {
                                pensamientoControler.MostrarPensamiento("Ya esta lleno", 1);
                            }
                        }
                    }
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("Cafetera"))
            {
                TriggerCafetera triggerCafetera;
                triggerCafetera = hit.collider.gameObject.gameObject.GetComponent<TriggerCafetera>();
                CursorSeleccion();
                if (triggerCafetera.canUse)
                {

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        triggerCafetera.InteractuarCafetera();
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        pensamientoControler.MostrarPensamiento("La taza ya esta llena", 1);
                    }
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("ColocarCafe"))
            {

                if (hasObject)
                {
                    if (itemAtributes.isTaza)
                    {
                        CursorSeleccion();

                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                itemAtributes.inCafetera = true;
                                ForzarSoltarObjeto();
                                TriggerColocarCafe triggerCafetera;
                                triggerCafetera = hit.collider.gameObject.gameObject.GetComponent<TriggerColocarCafe>();
                                triggerCafetera.cafeInterior = itemAtributes;
                                triggerCafetera.ColocarCafe();
                            }
                    }
                }

            }
            else if (hit.collider != null && hit.collider.CompareTag("VCR"))
            {
                VCR_Controller controlVCR;
                controlVCR = hit.collider.gameObject.gameObject.GetComponent<VCR_Controller>();
                controlVCR.ApuntandoTapa();
                CursorSeleccion();

                if (!controlVCR.VHSDentro)
                {
                    if (hasObject)
                    {
                        if (itemAtributes.isVHS)
                        {
                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {

                                controlVCR.vhsInterior = itemAtributes;

                                itemAtributes.MeterVHS();
                                controlVCR.MeterVHS();
                                ForzarSoltarObjeto();
                            }
                        }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        controlVCR.SoltarVHS();
                    }
                }

            }
            else if (hit.collider.tag == "ParedAtrapasuenos")
            {
                CursorSeleccion();
            }
            else
            {
                Deseleccionar();
            }
        }
        else
        {
            Deseleccionar();
        }

        
    }

    #region Selecciones

    public bool seleccionando;
    public void CursorSeleccion()
    {
        puntero.transform.localScale = maxPuntero;
        seleccionando = true;
    }

    public void Deseleccionar()
    {
        puntero.transform.localScale = minPuntero;
        seleccionando = false;
    }

    #endregion

    float fuerza;
    bool pressLefrClick;
    public void SoltarObjeto() 
    {
        if (!usingMovil && tiempoConObjeto > 0.1f && !itemGuardado)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && hasObject == true)
            {
                fuerza = 0.5f;
                pressLefrClick = true;
            }

            if (pressLefrClick)
            {
                if (fuerza < 4) fuerza += Time.deltaTime * 3;
            }

            if (Input.GetKeyUp(KeyCode.Mouse1) && hasObject == true) //     TIRAR OBJETO
            {
                itemAtributes.TirarObjeto();
                if (usingMovil)
                {
                    //movilController.CerrarMovil();
                    //movilController.ApagarPantalla();
                    if (!movilController.llamadasController.esperandoLlamada)
                    {
                        var eventSystem = EventSystem.current;
                        eventSystem.SetSelectedGameObject(null);
                    }

                    player.GetComponent<CamaraFP>().freeze = false;
                    puntero.enabled = true;

                }

                if(itemAtributes.isMovil) { movilController.ApagarPantalla(); }
                else if(itemAtributes.isCigar) { DejarDeFumar(); }
                else if(itemAtributes.isWater) 
                {
                    drinking = false;
                    drinkAnimation.SetBool("Drinking", false);
                    tiempoBebiendo = 0;
                }

                itemAtributes.pickUp = false;
                hasObject = false;
                pressLefrClick = false;

                pickUpObject.layer = 3;
                itemAtributes.mesh.layer = 3;


                Transform[] children = itemAtributes.mesh.GetComponentsInChildren<Transform>();

                if (!itemAtributes.isFregona)
                {
                    for (int i = 0; i < children.Length; i++)
                    {
                        children[i].gameObject.layer = 3;
                    }
                    if (itemAtributes.isMovil) { itemAtributes.movilController.pantallaEncendido.layer = 3; }
                }
                else
                {
                    meshFregona = itemAtributes.mesh;
                    Invoke(nameof(CambiarLayerFregona), 0.1f);
                }

                if (itemAtributes.isCigar)
                {
                    for (int i = 0; i < itemAtributes.partesCigarro.Length; i++)
                    {
                        itemAtributes.partesCigarro[i].gameObject.layer = 3;
                    }
                }
                else if (itemAtributes.isVela)
                {
                    itemAtributes.velaEncendidaOBJ.layer = 3;
                    itemAtributes.velaApagadaOBJ.layer = 3;
                }

                eatAnimation.SetBool("Eating", false);
                if (sonidoComer != null) { sonidoComer.Stop(); }
                eating = false;
                pickUpObjectRb.isKinematic = false;
                itemAtributes.col.enabled = true;
                pickUpObject.transform.SetParent(null);
                movimientoDespacio = false;

                if(corregirPickUp.algoDentro) 
                {
                    fuerza = 0;
                    pickUpObject.transform.position = player.transform.position;
                }

                pickUpObjectRb.AddForce(transform.forward * 2f * fuerza, ForceMode.Impulse);

                if (itemAtributes.isLlave) { itemAtributes.colisionesLlave.SoltarLlave(); }
                if(itemAtributes.notaFreezeCamera) camaraScript.freezeCamera = false;


                itemAtributes.col.isTrigger = false;

                if (itemAtributes.isCaja) 
                {
                    pickUpObject.layer = 17;

                    itemAtributes.cajaController.contenidoCajon.layer = 2;
                    itemAtributes.cajaController.contenidoCajonCol.enabled = true;

                }

                if (!corregirPickUp.algoDentro) { CorregirUbicacion(pickUpObject); }

                itemGuardado = false;

                OcultarIconos();

            }
        }


    }

    GameObject meshFregona;
    public void ForzarSoltarObjeto()
    {
        itemAtributes.TirarObjeto();
        if (usingMovil)
        {
            //movilController.CerrarMovil();
            //movilController.ApagarPantalla();
            if (!movilController.llamadasController.esperandoLlamada)
            {
                var eventSystem = EventSystem.current;
                eventSystem.SetSelectedGameObject(null);
            }

            player.GetComponent<CamaraFP>().freeze = false;
            puntero.enabled = true;

        }

        if(itemAtributes.notaOpening) { itemAtributes.notaOpening = false; }

        itemAtributes.pickUp = false;
        hasObject = false;
        pressLefrClick = false;

        pickUpObject.layer = 3;
        itemAtributes.mesh.layer = 3;


        Transform[] children = itemAtributes.mesh.GetComponentsInChildren<Transform>();

        if (!itemAtributes.isFregona)
        {
            for (int i = 0; i < children.Length; i++)
            {
                children[i].gameObject.layer = 3;
            }
            if (itemAtributes.isMovil) { itemAtributes.movilController.pantallaEncendido.layer = 3; }
        }
        else
        {
            meshFregona = itemAtributes.mesh;
            Invoke(nameof(CambiarLayerFregona), 0.1f);
        }


        if (itemAtributes.isCigar)
        {
            for (int i = 0; i < itemAtributes.partesCigarro.Length; i++)
            {
                itemAtributes.partesCigarro[i].gameObject.layer = 3;
            }
        }
        else if (itemAtributes.isVela)
        {
            itemAtributes.velaEncendidaOBJ.layer = 3;
            itemAtributes.velaApagadaOBJ.layer = 3;
        }

        eatAnimation.SetBool("Eating", false);
        eating = false;
        if(sonidoComer != null) { sonidoComer.Stop(); }
        pickUpObjectRb.isKinematic = false;
        itemAtributes.col.enabled = true;
        pickUpObject.transform.SetParent(null);
        movimientoDespacio = false;
        tiempoConObjeto = 0;

        pickUpObjectRb.AddForce(transform.forward * 2f * 0.5f, ForceMode.Impulse);

        if (itemAtributes.isLlave) { itemAtributes.colisionesLlave.SoltarLlave(); }
        if (itemAtributes.notaFreezeCamera) camaraScript.freezeCamera = false;

        itemAtributes.col.isTrigger = false;

        if (itemAtributes.isCaja)
        {
            pickUpObject.layer = 17;
            itemAtributes.cajaController.contenidoCajonCol.enabled = true;
            itemAtributes.cajaController.contenidoCajon.layer = 2;

        }

        itemGuardado = false;

        OcultarIconos();

    }

    public LayerMask capasCorregirUbicacion;
    public void CorregirUbicacion(GameObject objeto)
    {
        // Radio estrecho para la anchura y altura
        float radio = itemAtributes.radiusCorreccion;
        Vector3 puntoInferior = objeto.transform.position - Vector3.up * radio; // Base
        Vector3 puntoSuperior = objeto.transform.position + Vector3.up * radio; // Parte superior

        Vector3 desplazamiento = Vector3.up * 0.1f; // Ajuste hacia arriba

        // Verifica si hay intersección con otros colliders en la cápsula
        while (Physics.CheckCapsule(puntoInferior, puntoSuperior, radio, capasCorregirUbicacion))
        {
            objeto.transform.position += desplazamiento;

            // Actualiza los puntos de la cápsula para la nueva posición
            puntoInferior = objeto.transform.position - Vector3.up * radio;
            puntoSuperior = objeto.transform.position + Vector3.up * radio;
        }

    }

    public void CambiarLayerFregona()
    {
        Transform[] children = meshFregona.GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            children[i].gameObject.layer = 3;
        }
    }

    public void CogerObjeto(GameObject cual)
    {
        itemAtributes = cual.GetComponent<ItemAtributes>();

        itemGuardado = false;

        itemAtributes.pickUp = true;

        hasObject = true;

        tiempoConObjeto = 0;
        tiempoSinUsarItem = 0;

        pickUpObject = cual.gameObject;
        pickUpObjectRb = cual.gameObject.GetComponent<Rigidbody>();
        pickUpObject.layer = 6;
        itemAtributes.mesh.layer = 6;
        itemAtributes.mesh.SetActive(true);

        //if(itemAtributes.isCaja) { itemAtributes.cajaController.objetosCajon.SetActive(false); }

        if (itemAtributes.inCafetera) { itemAtributes.SoltarTaza(); }
        if (itemAtributes.isLlave) { itemAtributes.colisionesLlave.PickUpLlave(); }
        if(itemAtributes.isAtrapasuenos && itemAtributes.clavado)
        {
            if(itemAtributes.idEspecial == "Vida")
            {
                itemAtributes.indicadorAtrapasuenos.atrapasuenosVida--;
            }
            else if (itemAtributes.idEspecial == "Resistencia")
            {
                itemAtributes.indicadorAtrapasuenos.atrapasuenosResistencia--;
            }
        }

        if (itemAtributes.empaquetado) { itemAtributes.Desempaquetar(); }
        
        if (!itemAtributes.isFregona)
        {

                Transform[] children = itemAtributes.mesh.GetComponentsInChildren<Transform>();

                for (int i = 0; i < children.Length; i++)
                {
                    children[i].gameObject.layer = 6;
                }

                if (itemAtributes.isMovil) { itemAtributes.movilController.pantallaEncendido.layer = 6; }

        }
        else
        {
            Transform[] children = itemAtributes.mesh.GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++)
            {
                children[i].gameObject.layer = 10;
            }

        }

        if (itemAtributes.isCigar)
        {
            for (int i = 0; i < itemAtributes.partesCigarro.Length; i++)
            {
                itemAtributes.partesCigarro[i].gameObject.layer = 6;
            }
        }
        else if (itemAtributes.isVela)
        {
            itemAtributes.velaEncendidaOBJ.layer = 6;
            itemAtributes.velaApagadaOBJ.layer = 6;
        }
        

        if (itemAtributes.isTaza) { itemAtributes.inCafetera = false; }

        pickUpObjectRb.isKinematic = true;
        itemAtributes.col.enabled = false;

        if (itemAtributes.isMovil)
        {
            pickUpObject.transform.rotation = movilPosition.transform.rotation;
            movilController = cual.gameObject.GetComponent<MovilController>();
        }
        else if (itemAtributes.isFregona)
        {
            pickUpObject.transform.rotation = fregonaPosition.transform.rotation;
        }
        else
        {
            pickUpObject.transform.rotation = pickUpObjectPosition.transform.rotation;
        }

        if (itemAtributes.isCaja) 
        {
            itemAtributes.cajaController.PickUp();
            itemAtributes.cajaController.contenidoCajonCol.enabled = false;
        }


        pickUpObject.transform.SetParent(gameObject.transform);
        notaPosition.transform.rotation = pickUpObject.transform.rotation;
        itemAtributes.inCajon = false;
        itemAtributes.PickUp();

        if(itemAtributes.pensamientoAlCojer != "" && !pensamientoControler.mostrandoPensamiento)
        {
            pensamientoControler.MostrarPensamiento(itemAtributes.pensamientoAlCojer, 2);
        }

        //ICONOS TUTORIAL

        MostrarIconos();

    }

    #region ControlIconos

    public void OcultarIconos()
    {
        icoLanzarItem.SetActive(false);
        icoComer.SetActive(false);
        icoBeber.SetActive(false);
        icoUsar.SetActive(false);
        icoAbrir.SetActive(false);
        icoLeer.SetActive(false);
        icoDejarDeLeer.SetActive(false);
        icoRotar.SetActive(false);
        icoSacarItem.SetActive(false);
        icoGuardarItem.SetActive(false);
        icoFumar.SetActive(false);
        icoTomarPastillas.SetActive(false);
    }

    public void MostrarIconos()
    {
        icoLanzarItem.SetActive(true);
        if (itemAtributes.isFood)
        {
            if (itemAtributes.isPizza)
            {
                if (itemAtributes.indiceTrozosPizza < 7)
                {
                    icoComer.SetActive(true);
                }
            }
            else
            {
                icoComer.SetActive(true);
            }
        }
        else if (itemAtributes.isWater)
        {
            if (itemAtributes.scriptLiquido.usos > 0)
            {
                icoBeber.SetActive(true);
            }
        }
        else if (itemAtributes.isTaza)
        {
            if (itemAtributes.cafeController.cantidadCafe > 0)
            {
                icoBeber.SetActive(true);
            }
        }
        else if (itemAtributes.isVela || itemAtributes.isFregona || itemAtributes.isLinterna || itemAtributes.isMando)
        {
            icoUsar.SetActive(true);
        }
        else if (itemAtributes.isCarta && !itemAtributes.cartaAbierta)
        {
            icoAbrir.SetActive(true);
        }
        else if (itemAtributes.isNota)
        {
            icoRotar.SetActive(true);
        }
        else if (itemAtributes.isCigar)
        {
            if(!itemAtributes.cigarroConsumido)
            {
                icoFumar.SetActive(true);
            }
        }
        else if(itemAtributes.isPildoras)
        {
            if(itemAtributes.indicePastillas < itemAtributes.cantidadPastillas.Length)
            {
                icoTomarPastillas.SetActive(true);
            }
        }
        icoGuardarItem.SetActive(true);
    }

    #endregion


}
