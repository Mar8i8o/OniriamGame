using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Video;

public class ItemAtributes : MonoBehaviour
{

    #region Variables

    [Header("GENERAL OPTIONS")]

    public Vector3 Offset;
    public Vector3 Rotation;

    public bool active;

    public bool sePuedeTirar;
    public bool siempreActivo;
    public bool test;
    public bool itemInicio;
    public GameObject mesh;

    public float radiusCorreccion;

    public bool siempreHor;

    [Header("COMIDA")]

    public bool isFood;
    public bool isWater;
    public bool isPizza;
    public bool isLata;

    public float cuantoLlena = 30;
    public float cuantoTardaEnComer = 3.9f;

    public Animator pizzaAnim;
    public GameObject[] trozosPizza;
    bool pizzaOpen;

    public GameObject lataNormal;
    public GameObject lataAbierta;
    public GameObject lataAplastada;
    public Wobble scriptLiquido;

    [Header("MOVIL")]

    public bool isMovil;
    public MovilController movilController;

    [Header("NOTA/CARTA")]

    public bool isCarta;
    public bool isNota;

    public int intentosCarta;
    [HideInInspector] public bool notaOpening;
    public Animator cartaAnim;

    public bool cartaAbierta;
    public GameObject contenidoCarta;
    public ItemAtributes itemAtributesContenidoCarta;

    [TextArea(15, 20)]
    public string textoNotaDelante;

    [TextArea(15, 20)]
    public string textoNotaDetras;

    public bool detras;

    [HideInInspector]public bool mostrandoTexto;
    [HideInInspector]public LeerTextoController leerTextoController;

    [Header("FREGONA")]

    public bool isFregona;
    public GameObject[] pelosFregona;

    public Rigidbody peloFregonaRotation;

    [Header("SABANA")]

    public bool isSabana;
    public bool sabanaLimpia;

    public GameObject sabanaLimpiaOBJ;
    public GameObject sabanaSuciaOBJ;

    [Header("ATRAPASUEÑOS")]

    public bool isAtrapasuenos;
    public bool clavado;

    public string idEspecial;

    public bool isPrint;

    [Header("LINTERNA")]

    public bool isLinterna;
    public bool linternaEncendida;

    public bool monitor;

    [Header("VELA")]
    public bool isVela;
    public bool velaEncendida;
    public ParticleSystem particulasFuego;

    public GameObject velaEncendidaOBJ;
    public GameObject velaApagadaOBJ;

    public bool isCaballete;

    public bool isLienzo;
    public string lienzoId;
    [HideInInspector] public bool enCaballete;
    [HideInInspector] public CaballeteController caballeteController;

    public GameObject luz;
    public GameObject luzApagada;

    public bool isBasura;

    [Header("CIGARRO")]

    public bool isCigar;
    public GameObject[] partesCigarro;
    public GameObject[] posicionesHumo;
    public ParticleSystem humoConstante;
    public ParticleSystem humoCalada;

    [HideInInspector]public bool vendido;

    [Header("PILDORAS")]

    public bool isPildoras;
    public GameObject[] cantidadPastillas;
    public Animator pildorasAnim;

    public bool isMando;
    public Animator mandoAnim;

    //HIDE

    [HideInInspector]public bool pickUp;
    public int primeraVez;
    TiendaController tiendaController;
    GameObject camara;
    CamaraFP camaraFP;
    GameObject player;
    Raycast scriptPlayer;
    SceneDialogsController sceneDialogsController;
    PensamientoControler pensamientoControler;
    GuardarController guardarController;
    PlayerStats playerStats;

    [HideInInspector] public Rigidbody rb;

    public bool isVHS;
    public VideoClip videoVHS;
    public bool inVCR;

    public bool isTaza;
    public TriggerCafetera triggerCAFetera;
    public bool inCafetera;
    public CafeController cafeController;

    public bool isCafe;

    [HideInInspector]public Vector3 posicionInicial;
    [HideInInspector]public Vector3 rotacionInicial;

    public bool isLlave;
    public int idLlave;
    public bool abriendoPuerta;
    [HideInInspector]public ColisionesLlave colisionesLlave;

    //bool canPickUp;

    public Collider col;

    public bool isCaja;
    public CajaController cajaController;
    public bool inCajon;
    public string cajonName;
    [HideInInspector] public ContenidoCajonController contenidoCajon;
    public AudioSource itemHitSound;

    [HideInInspector]public IndicadorAtrapasuenos indicadorAtrapasuenos;

    #endregion


    private void Awake()
    {
        if (isLlave) { colisionesLlave = GetComponent<ColisionesLlave>(); }
        col = GetComponent<Collider>();
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();
        sceneDialogsController = GameObject.Find("GameManager").GetComponent<SceneDialogsController>();
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody>();
        //PlayerPrefs.DeleteAll();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        camara = GameObject.Find("Main Camera");
        if(isAtrapasuenos)indicadorAtrapasuenos = camara.GetComponent<IndicadorAtrapasuenos>();
        player = GameObject.Find("Player");
        camaraFP = player.GetComponent<CamaraFP>();
        scriptPlayer = camara.GetComponent<Raycast>();
        tiendaController = GameObject.Find("GameManager").GetComponent<TiendaController>();
        leerTextoController = GameObject.Find("LeerTextoController").GetComponent<LeerTextoController>();

        blockSonidosInicio = true;

        if (isTaza)
        {
            triggerCAFetera = GameObject.Find("TriggerCafetera").GetComponent<TriggerCafetera>();
        }

        if(isVHS)positionVCR = GameObject.Find("VHS_Position");

        if (!test)
        {

            primeraVez = PlayerPrefs.GetInt(gameObject.name + "itemInicio", 1);

            if (itemInicio)
            {
                if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(active)) == 0) { active = false; }
                else { active = true; }

                if(active == false) { DesactivarItem(); }

                if (primeraVez == 1) GuardarPosicion();
                //GetPosition();
                string name = gameObject.name;
                gameObject.name = "delete";

                primeraVez = 0;
                //print("Item inicio "+ name);
                PlayerPrefs.SetInt(name + "itemInicio", 0);
                //print("Delete:" + name);
                if (!siempreActivo) { Destroy(this.gameObject); }
            }

        }

        //if (isCarta) { contenidoCarta.name = contenidoCarta.name + "_" + gameObject.name; }

    }

    void Start()
    {

            if (PlayerPrefs.GetInt(gameObject.name + "clavado", System.Convert.ToInt32(clavado)) == 0) { clavado = false; }
            else { clavado = true; }
            if (clavado) { rb.isKinematic = true; }

            idEspecial = PlayerPrefs.GetString(gameObject.name + "idEspecial", idEspecial);

            if (isCigar || isPildoras) 
            {

                if (PlayerPrefs.GetInt(gameObject.name + "vendido", System.Convert.ToInt32(active)) == 0) { vendido = false; }
                else { vendido = true; }

            }

            if (isCigar) 
            {
                 indiceCigarro = PlayerPrefs.GetInt(gameObject.name + "indiceCigarro", indiceCigarro);
                 ActualizarParteCigarro();

                 if (indiceCigarro + 1 < partesCigarro.Length) { }
                 else //CIGARRO CONSUMIDO
                 {
                    GameObject alijoDrogas = GameObject.Find("AlijoDrogas");

                    cigarroConsumido = true;
                    transform.position = alijoDrogas.transform.position;
                    ResetearCigarro();
                 }
            }
            else if (isPildoras) 
            {

                indicePastillas = PlayerPrefs.GetInt(gameObject.name + "indicePastillas", indicePastillas);

                if (indicePastillas == cantidadPastillas.Length)
                {
                    boteVacio = true;
                }
                
                ActualizarPastillas();
            }
        
            

        if (!test)
        {
            if (!siempreActivo)
            {
                if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(active)) == 0) { active = false; }
                else { active = true; }

                if (active) { ActivarItem(); }
                else DesactivarItem();
            }
            else { active = true; }

            
        }
        if (active) GetPosition();
        if (isPizza) { 
            indiceTrozosPizza = PlayerPrefs.GetInt(gameObject.name + "indicePizza", indiceTrozosPizza);
            CalcularPizza();

            ActivarAnim();
            pizzaAnim.SetBool("Open", false);
            pizzaOpen = false;
            Invoke(nameof(DesactivarAnim), 2);
        }

        if (isLienzo) { lienzoId = PlayerPrefs.GetString(gameObject.name + "lienzoId", lienzoId); }

        if (isCarta)
        {
            if (PlayerPrefs.GetInt(gameObject.name + "cartaAbierta", System.Convert.ToInt32(cartaAbierta)) == 0) { cartaAbierta = false; }
            else { cartaAbierta = true; }
            cartaAnim.SetBool("Open", cartaAbierta);
        }

        if (isSabana)
        {
            if (PlayerPrefs.GetInt(gameObject.name + "sabanaLimpia", System.Convert.ToInt32(sabanaLimpia)) == 0) { sabanaLimpia = false; }
            else { sabanaLimpia = true; }
        }
        if (isVHS)
        {
            if (PlayerPrefs.GetInt(gameObject.name + "DentroVCR", System.Convert.ToInt32(inVCR)) == 0) { inVCR = false; }
            else { inVCR = true; GameObject.Find("TriggerVCR").GetComponent<VCR_Controller>().vhsInterior = gameObject.GetComponent<ItemAtributes>(); }
        }
        else if(isTaza)
        {
            if (PlayerPrefs.GetInt(gameObject.name + "inCafetera", System.Convert.ToInt32(inCafetera)) == 0) { inCafetera = false; }
            else { inCafetera = true; triggerCAFetera.cafeInterior = gameObject.GetComponent<ItemAtributes>(); }
        }

        if (isLinterna) { luz.SetActive(false); luzApagada.SetActive(true); }
        if (isVela)
        {
            if (PlayerPrefs.GetInt(gameObject.name + "velaEncendida", System.Convert.ToInt32(velaEncendida)) == 0) { velaEncendida = false; }
            else { velaEncendida = true; }

            if (velaEncendida)
            {
                luz.SetActive(true);
                particulasFuego.Play();
                //particulasFuego.emission = false;
            }
            else
            {
                luz.SetActive(false);
                particulasFuego.Stop();
            }
        }

        posicionInicial = transform.position;
        rotacionInicial = transform.eulerAngles;

        if (PlayerPrefs.GetInt(gameObject.name + "inCajon", System.Convert.ToInt32(inCajon)) == 0) { inCajon = false; }
        else { inCajon = true;}
        cajonName = PlayerPrefs.GetString(gameObject.name + "cajonName", cajonName);

        if(inCajon && cajonName != "")
        {
            //if (GameObject.Find(cajonName).GetComponent<ContenidoCajonController>() == null) { print(gameObject.name); }
            //print(gameObject.name);
            contenidoCajon = GameObject.Find(cajonName).GetComponent<ContenidoCajonController>();
            gameObject.transform.SetParent(contenidoCajon.cajon.transform, true);
            contenidoCajon.objetos++;

            if(contenidoCajon.isCaja) 
            { 
              rb.isKinematic = false;
              col.isTrigger = false;
              if (!contenidoCajon.cajaController.abierto) { contenidoCajon.cajaController.ActivarColiders(); }
            }
        }

        if (PlayerPrefs.GetInt(gameObject.name + "empaquetado", System.Convert.ToInt32(inCajon)) == 0) { empaquetado = false; }
        else { empaquetado = true; }
        if (empaquetado) { Empaquetar(); }
        else if (paquete != null) { Desempaquetar(); }

        if (!active) { DesactivarAnim(); gameObject.SetActive(false); }
        else { Invoke(nameof(DesactivarAnim), 2); }

        Invoke(nameof(UnblockSonidosInicio), 1);

    }

    public void LiberarItem()
    {
        //rb.isKinematic = true; col.isTrigger = true;
    }

    void Update()
    {

        if (!active) gameObject.SetActive(false);

        #region LogicaUpdate

        if (notaOpening) 
        {
            transform.position = Vector3.MoveTowards(transform.position, scriptPlayer.notaPosition.transform.position, 3 * Time.deltaTime);
            transform.rotation = scriptPlayer.notaPosition.transform.rotation;
        };

        if (isPizza)
        {
            if (pickUp) {
                if (!pizzaOpen)
                {
                    ActivarAnim();
                    pizzaAnim.SetBool("Open", true);
                    pizzaOpen = true;
                    Invoke(nameof(DesactivarAnim), 2);
                }
            }
            else 
            {
                if (pizzaOpen)
                {
                    ActivarAnim();
                    pizzaAnim.SetBool("Open", false);
                    pizzaOpen = false;
                    Invoke(nameof(DesactivarAnim), 2);
                }
            }
        }
        if (comiendoTrozoPizza) { ActualizarPizza(); }
        if (isLata) //ESTADOS LATA
        {
            if (scriptLiquido.usos == 4) { lataNormal.SetActive(true); lataAplastada.SetActive(false); lataAbierta.SetActive(false); }
            else if (scriptLiquido.usos > 0) { lataAbierta.SetActive(true); lataNormal.SetActive(false); lataAplastada.SetActive(false); }
            else if (!lataAplastada.activeSelf) { Invoke(nameof(AplastarLata), 0.1f); }
        }
        else if (isCigar)
        {
            ControlarCigarro();
        }

        if (isVela)
        {
            if (velaEncendida)
            {
                velaEncendidaOBJ.SetActive(true);
                velaApagadaOBJ.SetActive(false);
            }
            else
            {
                velaEncendidaOBJ.SetActive(false);
                velaApagadaOBJ.SetActive(true);
            }
        }

        ControlesLeerNota();

        if (isVHS) { ControlarVHS(); }

        /*
        if (isSabana)
        {
            if (Time.frameCount % 10 == 0)
            {
                if (sabanaLimpia)
                {
                    sabanaSuciaOBJ.SetActive(false);
                    sabanaLimpiaOBJ.SetActive(true);
                }
                else
                {
                    sabanaSuciaOBJ.SetActive(true);
                    sabanaLimpiaOBJ.SetActive(false);
                }
            }
        }
        */

        if (sceneDialogsController.dialogueActive || scriptPlayer.seleccionando) { return; }

        if (isCarta) ///////////////////////////////////////////////////CARTA
        {
            if (pickUp)
            {
                if (scriptPlayer.tiempoConObjeto > 0.1f && Input.GetKeyDown(KeyCode.Mouse0) && !scriptPlayer.seleccionando) { AbrirCarta(); print("Abrir carta"); }
            }
        }

        if (isLinterna && !scriptPlayer.seleccionando) 
        {
            if (Input.GetKeyDown (KeyCode.Mouse0) && pickUp && scriptPlayer.tiempoConObjeto > 0.1f) 
            {
                if (linternaEncendida) 
                {
                    linternaEncendida = false;
                    luz.SetActive(false);
                    luzApagada.SetActive(true);
                }
                else
                {
                    linternaEncendida = true;
                    luz.SetActive(true);
                    luzApagada.SetActive(false);
                }
            }
        }
        if(inCajon)
        {
            if (contenidoCajon.isCaja)
            {
                if (!contenidoCajon.cajaController.abierto) { } //{ col.isTrigger = true; } //CERRADO
                else { col.isTrigger = false; rb.isKinematic = false; print("puertaAbierta"); }
            }
        }

        

        #endregion

    }

    public void PickUp()
    {

        if (isCigar || isPildoras) vendido = true;

        if (notaOpening) { Invoke(nameof(CancelarNotaOpening), 0.5f); }
        clavado = false;

        if (isSabana && !sabanaLimpia && !pensamientoControler.mostrandoPensamiento) { pensamientoControler.MostrarPensamiento("Deberia llevar esto a la lavadora", 1); }

        if (isLienzo)
        {
            if (enCaballete)
            {
                caballeteController.QuitarLienzo();
            }
        }

        if(isFregona)
        {
            peloFregonaRotation.isKinematic = true;
            peloFregonaRotation.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;

        }

    }


    public void TirarObjeto()
    {

        if (mostrandoTexto)
        {
            mostrandoTexto = false;
            leerTextoController.DejarDeMostrarTexto();
            scriptPlayer.puntero.enabled = true;

        }

        if (isFregona)
        {

            peloFregonaRotation.constraints &= ~RigidbodyConstraints.FreezePositionX;
            peloFregonaRotation.constraints &= ~RigidbodyConstraints.FreezePositionY;
            peloFregonaRotation.constraints &= ~RigidbodyConstraints.FreezePositionZ;

            peloFregonaRotation.isKinematic = false;
        }

        if (siempreHor) { transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, 0); }
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            GuardarPosicion();
        }
    }

    public void DesactivarItem()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        transform.position = Vector3.zero;
        mesh.SetActive(false);
        active = false;
        gameObject.SetActive(false);
    }

    public void ActivarItem()
    {
        gameObject.SetActive(true);
        mesh.SetActive(true);
        active = true;
        if(!clavado)gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        //gameObject.layer = 3;

        if (isPizza)
        {
            ActivarAnim();
            pizzaAnim.SetBool("Open", false);
            pizzaOpen = false;
            Invoke(nameof(DesactivarAnim), 2);
        }

        if(isVela)
        {
            if (velaEncendida)
            {
                velaEncendidaOBJ.SetActive(true);
                velaApagadaOBJ.SetActive(false);
                luz.SetActive(true);
                particulasFuego.Play();
            }
            else
            {
                velaEncendidaOBJ.SetActive(false);
                velaApagadaOBJ.SetActive(true);
                luz.SetActive(false);
                particulasFuego.Stop();
            }

        }


    }

    float posX;
    float posY;
    float posZ;

    float rotX;
    float rotY;
    float rotZ;

    public void GuardarPosicion()
    {

        print("GuardarPosicion " + gameObject.name);

            PlayerPrefs.SetFloat(gameObject.name + "posX", transform.position.x);
            PlayerPrefs.SetFloat(gameObject.name + "posZ", transform.position.z);
            PlayerPrefs.SetFloat(gameObject.name + "posY", transform.position.y);

            PlayerPrefs.SetFloat(gameObject.name + "rotX", transform.eulerAngles.x);
            PlayerPrefs.SetFloat(gameObject.name + "rotZ", transform.eulerAngles.z);
            PlayerPrefs.SetFloat(gameObject.name + "rotY", transform.eulerAngles.y);

            PlayerPrefs.SetInt(gameObject.name + "active", System.Convert.ToInt32(active));

            if (isCigar || isPildoras) PlayerPrefs.SetInt(gameObject.name + "vendido", System.Convert.ToInt32(vendido));

            if (isCarta) PlayerPrefs.SetInt(gameObject.name + "cartaAbierta", System.Convert.ToInt32(cartaAbierta));
            else if (isSabana) PlayerPrefs.SetInt(gameObject.name + "sabanaLimpia", System.Convert.ToInt32(sabanaLimpia));
            else if (isVela) PlayerPrefs.SetInt(gameObject.name + "velaEncendida", System.Convert.ToInt32(velaEncendida));
            else if (isLienzo) PlayerPrefs.SetString(gameObject.name + "lienzoId", lienzoId);
            else if (isPizza) PlayerPrefs.SetInt(gameObject.name + "indicePizza", indiceTrozosPizza);
            else if (isCigar) PlayerPrefs.SetInt(gameObject.name + "indiceCigarro", indiceCigarro);
            else if (isPildoras) PlayerPrefs.SetInt(gameObject.name + "indicePastillas", indicePastillas);
            else if (isVHS) PlayerPrefs.SetInt(gameObject.name + "DentroVCR", System.Convert.ToInt32(inVCR));
            else if (isTaza) PlayerPrefs.SetInt(gameObject.name + "inCafetera", System.Convert.ToInt32(inCafetera));
            PlayerPrefs.SetInt(gameObject.name + "inCajon", System.Convert.ToInt32(inCajon));
            PlayerPrefs.SetString(gameObject.name + "cajonName", cajonName);
            PlayerPrefs.SetInt(gameObject.name + "clavado", System.Convert.ToInt32(clavado));

            PlayerPrefs.SetString(gameObject.name + "idEspecial", idEspecial);

    }

    public void GetPosition()
    {
        posX = PlayerPrefs.GetFloat(gameObject.name + "posX", transform.position.x);
        posY = PlayerPrefs.GetFloat(gameObject.name + "posY", transform.position.y);
        posZ = PlayerPrefs.GetFloat(gameObject.name + "posZ", transform.position.z);

        rotX = PlayerPrefs.GetFloat(gameObject.name + "rotX", transform.eulerAngles.x);
        rotY = PlayerPrefs.GetFloat(gameObject.name + "rotY", transform.eulerAngles.y);
        rotZ = PlayerPrefs.GetFloat(gameObject.name + "rotZ", transform.eulerAngles.z);

        transform.position = new Vector3(posX, posY, posZ);
        transform.eulerAngles = new Vector3(rotX, rotY, rotZ);

    }

    public void SoltarObjeto()
    {
        scriptPlayer.ForzarSoltarObjeto();
    }

    #region Pastillas

    public int indicePastillas;
    [HideInInspector] public bool boteVacio;
    public void ConsumirPildoras()
    {
        if (indicePastillas < cantidadPastillas.Length)
        {
            indicePastillas++;

            ActualizarPastillas();
            
        }
    }
    public void ActualizarPastillas()
    {
        for (int i = 0; i < cantidadPastillas.Length; i++)
        {
            if (indicePastillas == i) { cantidadPastillas[i].SetActive(true); }
            else { cantidadPastillas[i].SetActive(false); }
        }
    }
    public void AbrirTapa()
    {
        ActivarAnim();
        pildorasAnim.SetBool("TapaAbierta", true);
        Invoke(nameof(DesactivarAnim), 2);
    }
    public void CerrarTapa()
    {
        ActivarAnim();
        pildorasAnim.SetBool("TapaAbierta", false);
        Invoke(nameof(DesactivarAnim), 2);

        //playerStats.cansancio -= 10;

        if (indicePastillas == cantidadPastillas.Length)
        {
            boteVacio = true;
        }
    }

    public void ResetearPastillas()
    {
        indicePastillas = 0;
        boteVacio = false;
        vendido = false;
        ActualizarPastillas();

        gameObject.SetActive(false);

    }

    #endregion

    #region Cigarro

    [HideInInspector]public int indiceCigarro;
    bool cigarroEncendido;

    public float tiempoCigarrEncendido;

    [HideInInspector]public bool cigarroConsumido;

    public void ControlarCigarro()
    {
        if (cigarroEncendido) 
        {
            tiempoCigarrEncendido += Time.deltaTime;

            if (tiempoCigarrEncendido > 10)
            {
                humoConstante.Stop();
            }
        }
    }
    public void ConsumirCigarro()
    {
        Invoke(nameof(DarCalada), 0.5f);

        if (!cigarroEncendido) 
        {
            cigarroEncendido = true;
            humoConstante.Play();
        }
    }

    public void DarCalada()
    {

        if (!cigarroConsumido)
        {
            if (indiceCigarro + 1 < partesCigarro.Length)
            {
                print("calada");

                tiempoCigarrEncendido = 0;
                humoCalada.Play();
                indiceCigarro++;
                ActualizarParteCigarro();
            }
            else
            {
                print("cigarro acabado");
                cigarroConsumido = true;
            }
        }

    }

    public void ApagarCigarro()
    {
        //humoConstante.Stop();
        if (tiempoCigarrEncendido < 7) 
        {
            tiempoCigarrEncendido = 7;
        }
       
    }

    public void ActualizarParteCigarro()
    {
        for (int i = 0; i < partesCigarro.Length; i++)
        {
            if (indiceCigarro == i)
            {
                partesCigarro[i].SetActive(true);
                humoCalada.gameObject.transform.position = posicionesHumo[i].transform.position;
                humoConstante.gameObject.transform.position = posicionesHumo[i].transform.position;

            }
            else
            {
                partesCigarro[i].SetActive(false);
            }
        }
    }

    public void ResetearCigarro()
    {
        cigarroConsumido = false;
        indiceCigarro = 0;
        vendido = false;
        ActualizarParteCigarro();
        gameObject.SetActive(false);
    }

    #endregion

    #region Pizza

    [HideInInspector] public int indiceTrozosPizza = -1;
    [HideInInspector] public bool comiendoTrozoPizza;
    Vector3 posicionTrozoPizza;
    Vector3 rotTrozoPizza;
    public void ComerTrozoPizza()
    {
        if (!comiendoTrozoPizza && indiceTrozosPizza < 7)
        {
            indiceTrozosPizza++;
            posicionTrozoPizza = trozosPizza[indiceTrozosPizza].transform.localPosition;
            rotTrozoPizza = trozosPizza[indiceTrozosPizza].transform.localEulerAngles;
            comiendoTrozoPizza = true;
            Invoke(nameof(AcabarDeComerTrozoDePiza), 0.4f);
        }
        //Destroy(trozosPizza[indiceTrozosPizza]);

    }

    public void ActualizarPizza()
    {
        trozosPizza[indiceTrozosPizza].transform.LookAt(new Vector3(camara.transform.position.x, camara.transform.position.y, camara.transform.position.z));
        trozosPizza[indiceTrozosPizza].transform.position = Vector3.MoveTowards(trozosPizza[indiceTrozosPizza].transform.position, player.transform.position + new Vector3(0,0.5f,0), 1f * Time.deltaTime);
    }

    public void AcabarDeComerTrozoDePiza()
    {
        trozosPizza[indiceTrozosPizza].transform.localPosition = posicionTrozoPizza;
        trozosPizza[indiceTrozosPizza].transform.localEulerAngles = rotTrozoPizza;
        trozosPizza[indiceTrozosPizza].SetActive(false);
        comiendoTrozoPizza = false;

        

        playerStats.hambre += 10;

    }

    public void CalcularPizza()
    {
        for (int i = 0; i < trozosPizza.Length; i++)
        {
            if(i <= indiceTrozosPizza) { trozosPizza[i].SetActive(false); }
            else { trozosPizza[i].SetActive(true); }
            
        }
    }

    public void ResetearPizza()
    {
        for (int i = 0; i < trozosPizza.Length; i++)
        {
            trozosPizza[i].SetActive(true); 
        }
        indiceTrozosPizza = -1;
        PlayerPrefs.SetInt(gameObject.name + "indicePizza", indiceTrozosPizza);
    }

    #endregion

    #region Carta
    public void AbrirCarta()
    {
        if (!cartaAbierta)
        {
            ActivarAnim();
            if (intentosCarta <= 0)
            {
                cartaAbierta = true;
                cartaAnim.SetBool("Open", cartaAbierta);
                TransportarObjetoCarta();
                //Invoke(nameof(TransportarObjetoCarta), 0.1f);
                Invoke(nameof(SoltarObjeto), 0.3f);
                Invoke(nameof(CogerObjetoCarta), 0.5f);
            }
            else
            {
                intentosCarta--;
                cartaAnim.SetTrigger("Temblar");
            }
            Invoke(nameof(DesactivarAnim), 2);
        }
    }
    public void TransportarObjetoCarta()
    {
        //contenidoCarta.name = contenidoCarta.name + "_" + gameObject.name;
        itemAtributesContenidoCarta.ActivarItem();
        contenidoCarta.transform.SetParent(camara.transform);
        itemAtributesContenidoCarta.notaOpening = true;
        //itemAtributesContenidoCarta.GuardarPosicion();
        itemAtributesContenidoCarta.active = true;
        contenidoCarta.SetActive(true);
        itemAtributesContenidoCarta.notaOpening = true;
        itemAtributesContenidoCarta.active = true;
        itemAtributesContenidoCarta.col.enabled = false;
        itemAtributesContenidoCarta.rb.isKinematic = true;
        contenidoCarta.transform.position = player.transform.position + new Vector3(0, -0.5f, 0);
        //itemAtributesContenidoCarta.GuardarPosicion();
    }
    public void CogerObjetoCarta()
    {
        CancelInvoke(nameof(CogerObjetoCarta));

        itemAtributesContenidoCarta.notaOpening = false;
        scriptPlayer.CogerObjeto(contenidoCarta);
    }

    public void CancelarNotaOpening()
    {
        notaOpening = false;
    }

    [HideInInspector]public bool notaFreezeCamera;

    bool mantenerLeerTexto;
    float tiempoManteniendoNota;
    string textoActual;
    public void ControlesLeerNota() //CONECTADO AL UPDATE
    {
        if (pickUp && isNota && scriptPlayer.tiempoConObjeto > 0.1f && !notaOpening) 
        {



            // Rotación mundial del objeto
            Quaternion worldRotation = transform.rotation;

            // Rotación mundial del padre
            Quaternion parentWorldRotation = transform.parent.rotation;

            // Calcula la rotación relativa
            Quaternion localRotationRelative = Quaternion.Inverse(parentWorldRotation) * worldRotation;

            // Convertir a ángulos de Euler si es necesario
            Vector3 localEulerAnglesRelative = localRotationRelative.eulerAngles;

            // Mostrar la rotación local relativa
            //Debug.Log("Rotación local relativa: " + localEulerAnglesRelative);


            if (localEulerAnglesRelative.y > 70 && localEulerAnglesRelative.y < 260)
            {
                detras = true;
                textoActual = textoNotaDetras;
            }
            else
            {
                detras = false;
                textoActual = textoNotaDelante;
            }

            // Actualizar el texto mostrado
            leerTextoController.textoTXT.text = textoActual;


            if (Input.GetKeyDown(KeyCode.Mouse0) && !scriptPlayer.seleccionando)  //PULSAR
            {
                tiempoManteniendoNota = 0;
                mantenerLeerTexto = false;
                
            }

            if (Input.GetKey(KeyCode.Mouse0) && !scriptPlayer.seleccionando && !sceneDialogsController.dialogueActive) //MANTENER
            {

                tiempoManteniendoNota += Time.deltaTime;

                if(tiempoManteniendoNota > 0.4f) { mantenerLeerTexto = true; }

                /*
                if(!mostrandoTexto)
                {
                    mostrandoTexto = true;
                    leerTextoController.MostrarTexto(textoActual);
                    scriptPlayer.puntero.enabled = false;
                }
                */

                notaFreezeCamera = true;
                camaraFP.freezeCamera = true;

                if (Input.GetAxis("Mouse X") > 0.1) { scriptPlayer.notaPosition.transform.Rotate(0, 6, 0);}
                if (Input.GetAxis("Mouse X") < -0.1) { scriptPlayer.notaPosition.transform.Rotate(0, -6, 0);}

            }
            else if(notaFreezeCamera && !sceneDialogsController.dialogueActive)
            {
                notaFreezeCamera = false;
                camaraFP.freezeCamera = false;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && !scriptPlayer.seleccionando && !sceneDialogsController.dialogueActive) //LEVANTAR
            {
                if(!mantenerLeerTexto)
                {
                    if (mostrandoTexto)
                    {
                        mostrandoTexto = false;
                        leerTextoController.DejarDeMostrarTexto();
                        scriptPlayer.puntero.enabled = true;
                    }
                    else //MOSTRAR TEXTO
                    {
                        mostrandoTexto = true;
                        leerTextoController.MostrarTexto(textoActual);
                        scriptPlayer.puntero.enabled = false;
                    }
                }
            }

            if(mostrandoTexto && sceneDialogsController.dialogueActive) //DEJA DE MOSTRAR EL TEXTO SI ESTAS EN UN DIALOGO
            {
                mostrandoTexto = false;
                leerTextoController.DejarDeMostrarTexto();
            }

        }
    }

    #endregion

    #region VHS

    public GameObject positionVCR;

    public void MeterVHS()
    {
        inVCR = true;
    }

    public void SoltarVHS()
    {
        gameObject.layer = 3;
    }

    public void ControlarVHS()
    {
        if (inVCR)
        {
            //rb.isKinematic = true;
            //gameObject.GetComponent<Collider>().enabled = false;
            transform.position = Vector3.MoveTowards(transform.position, positionVCR.transform.position, 10 * Time.deltaTime);
            transform.rotation = positionVCR.transform.rotation;

            gameObject.layer = 1;
        }
        else
        {
            //rb.isKinematic = false;
            gameObject.GetComponent<Collider>().enabled = true;
        }
    }

    #endregion

    #region Vela

    public void UsarVela()
    {
        Invoke(nameof(CambiarFuegoVela), 0.2f);
    }

    public void CambiarFuegoVela()
    {
        if (velaEncendida)
        {
            velaEncendida = false;
            luz.SetActive(false);
            particulasFuego.gameObject.SetActive(false);
        }
        else
        {
            velaEncendida = true;
            luz.SetActive(true);
            particulasFuego.gameObject.SetActive(true);
            particulasFuego.Play();
        }
    }

    #endregion

    #region Otros
    public void AplastarLata()
    {
        lataNormal.SetActive(false); lataAplastada.SetActive(true); lataAbierta.SetActive(false);
        CancelInvoke(nameof(AplastarLata));
    }

    public GameObject pelosStatic;
    public GameObject pelosActive;

    public void SoltarTaza()
    {
        triggerCAFetera.SoltarTaza();
    }

    public void RellenarAgua()
    {
        scriptLiquido.cantidadDeAgua = scriptLiquido.maxAgua;
    }

    public void PulsarMando()
    {
        ActivarAnim();
        mandoAnim.SetTrigger("Pulsar");
        Invoke(nameof(DesactivarAnim), 2);
    }

    #endregion

    public void DesactivarAnim()
    {
        CancelInvoke(nameof(DesactivarAnim));
        if(mandoAnim != null) { mandoAnim.enabled = false; }
        else if(pildorasAnim != null) { pildorasAnim.enabled = false; }
        else if(pizzaAnim != null) { pizzaAnim.enabled = false; }
        else if(cartaAnim != null) { cartaAnim.enabled = false; }
    }

    public void ActivarAnim()
    {
        CancelInvoke(nameof(DesactivarAnim));
        if (mandoAnim != null) { mandoAnim.enabled = true; }
        else if (pildorasAnim != null) { pildorasAnim.enabled = true; }
        else if (pizzaAnim != null) { pizzaAnim.enabled = true; }
        else if (cartaAnim != null) { cartaAnim.enabled = true; }
    }

    public GameObject paquete;
    public bool empaquetado;

    public void Empaquetar()
    {
        if (paquete != null)
        {
            empaquetado = true;
            paquete.SetActive(true);
        }
    }

    public void Desempaquetar()
    {
        //print("Intentar desempaquetar");
        if (paquete != null)
        {
            //print("Desempaquetar");
            empaquetado = false;
            paquete.SetActive(false);
        }
    }

    bool blockSonidosInicio = true;

    public void UnblockSonidosInicio()
    {
        blockSonidosInicio = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Solo continuar si el objeto colisionado está en la capa Default
        if (collision.gameObject.layer != LayerMask.NameToLayer("Default"))
            return;

        if (itemHitSound != null && !blockSonidosInicio)
        {
            itemHitSound.Play();
        }
    }


}
