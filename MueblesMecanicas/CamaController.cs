using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamaController : MonoBehaviour
{
    public bool estaMeada;
    public bool tieneSabanas;

    public GameObject sabanaLimpia;
    public GameObject sabanaSucia;

    public GameObject sabanaAbultada1;
    public GameObject sabanaAbultada2;

    public GameObject sabanaParent;
    public BoxCollider colider;

    public ItemAtributes sabana;
    public GameObject sabanaPosition;

    Raycast raycast;
    CamaraFP camaraFP;
    GuardarController guardarController;
    GameObject camara;
    GameObject player;
    ParpadeoController parpadeController;
    PensamientoControler pensamientoControler;
    RespiracionController respiracionController;
    PlayerStats playerStats;
    DreamController dreamController;
    SacarMovilController sacarMovilController;


    public GameObject posicionCamaPlayerActual;
    public GameObject posicionCamaPlayerBaja;
    public GameObject posicionCamaPlayerAlta;
    public GameObject interfazPlayer;
    public GameObject levantarseCamaPoint;

    public bool usandoCama;
    public float tiempoUsandoCama;
    public bool puedeMoverse;
    public bool paralisis;
    public bool alguienMirandote;

    public bool muestraPensamientos;

    public bool puedeLevantarse;

    [HideInInspector] public bool alguienEnLaCasa;
    BrazoController brazoConReloj;

    public GameObject icoLevantarse;

    private void Awake()
    {
        usandoCama = true;
    }

    void Start()
    {
        dreamController = GameObject.Find("GameManager").GetComponent<DreamController>();
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();
        parpadeController = GameObject.Find("GameManager").GetComponent<ParpadeoController>();
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
        camara = GameObject.Find("CameraParent");
        player = GameObject.Find("Player");
        camaraFP = GameObject.Find("Player").GetComponent<CamaraFP>();
        sacarMovilController = GameObject.Find("Player").GetComponent<SacarMovilController>();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        respiracionController = GameObject.Find("GameManager").GetComponent<RespiracionController>();
        brazoConReloj = GameObject.Find("BrazoConReloj").GetComponent<BrazoController>();

        raycast = GameObject.Find("Main Camera").GetComponent<Raycast>();

        GetDatos();

        if (tieneSabanas) sabana.gameObject.transform.position = sabanaPosition.transform.position;
        if (usandoCama) { Tumbarse();}

        sabanaParent.SetActive(tieneSabanas);

    }

    // Update is called once per frame
    void Update()
    {
        if (estaMeada) { sabanaSucia.SetActive(true); sabanaLimpia.SetActive(false); }
        else { sabanaSucia.SetActive(false); sabanaLimpia.SetActive(true); }

        //colider.enabled = tieneSabanas;

        /*
        if (tieneSabanas) 
        {
            if(!usandoCama)colider.enabled = true;
        }
        else
        {
            if (raycast.hasObject)
            {
                if (raycast.itemAtributes.isSabana && raycast.itemAtributes.sabanaLimpia) 
                {
                    colider.enabled = true;
                }
                else
                {
                    colider.enabled = false;
                }
            }
            else
            { 
                colider.enabled = false;
            }
        }
        */

        //////////
        
        if (usandoCama)
        {
            colider.enabled = false;
            if (tiempoUsandoCama < 0.5f) player.transform.position = Vector3.MoveTowards(player.transform.position ,posicionCamaPlayerActual.transform.position, 10f * Time.deltaTime);
            else 
            {
                float imput;
                if (puedeMoverse && !raycast.usingMovil) imput = Input.GetAxis("Horizontal") / 8;
                else imput = 0;

                player.transform.position = Vector3.MoveTowards(player.transform.position, posicionCamaPlayerActual.transform.position + new Vector3(0, 0, imput), 0.4f * Time.deltaTime);

                if (puedeMoverse && !raycast.usingMovil)
                {
                    if (Input.GetAxis("Vertical") <= 0) posicionCamaPlayerActual = posicionCamaPlayerBaja;
                    else posicionCamaPlayerActual = posicionCamaPlayerAlta;
                }
                else
                {
                    posicionCamaPlayerActual = posicionCamaPlayerBaja;
                }
            }

            sabanaAbultada1.SetActive(true);
            sabanaAbultada2.SetActive(true);

            tiempoUsandoCama += Time.deltaTime;

            

            if (tiempoUsandoCama > 0.1)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (!durmiendose)
                    {
                        if (!paralisis)
                        {
                            if (!parpadeController.abrirOjos && puedeLevantarse)
                            {
                                Levantarse();
                            }
                        }
                        else
                        {
                            pensamientoControler.MostrarPensamiento("Una presion en el pecho no me deja moverme", 1);
                        }
                    }
                }
                if (!raycast.hasObject && !raycast.seleccionando && !sacarMovilController.movilSacado)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (!paralisis)
                        {

                            if (playerStats.cansancio > 70)  //DORMIRSE
                            {
                                EmpezarDormirse();
                                icoLevantarse.SetActive(false);
                                sacarMovilController.enabled = false;
                            }
                            else
                            {
                                if(muestraPensamientos)pensamientoControler.MostrarPensamiento("No puedo dormir ahora, no estoy cansado", 1);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            sabanaAbultada1.SetActive(false);
            sabanaAbultada2.SetActive(false);
        }

    }

    public bool durmiendose;

    public void EmpezarDormirse()
    {
        durmiendose = true;
        //parpadeController.cerrandoOjos = true;
        //parpadeController.puedesAbrirlos = false;
        //parpadeController.velocidad = 100;
        puedeMoverse = false;
        parpadeController.SetCerrarOjos(100);
        interfazPlayer.SetActive(false);
        brazoConReloj.puedeSacarBrazo = false;

        camaraFP.GuardarController();

        parpadeController.spawnAbriendoOjos = true;
        Invoke(nameof(ActivarGuardar), 7);
        Invoke(nameof(Dormirse), 10);
    }

    public void ActivarGuardar()
    {
        guardarController.Guardar();
    }

    public void Dormirse()
    {
        //SceneManager.LoadScene("DreamScene");
        dreamController.Dormirse();
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            GuardarDatos();
        }
    }

    public void GuardarDatos()
    {
        PlayerPrefs.SetInt("camaPrincipalSucia", System.Convert.ToInt32(estaMeada));
        PlayerPrefs.SetInt(gameObject.name + "tieneSabanas", System.Convert.ToInt32(tieneSabanas));
        PlayerPrefs.SetInt(gameObject.name + "usandoCama", System.Convert.ToInt32(usandoCama));
        PlayerPrefs.SetInt(gameObject.name + "paralisis", System.Convert.ToInt32(paralisis));
        PlayerPrefs.SetInt(gameObject.name + "alguienTeMira", System.Convert.ToInt32(alguienMirandote));
    }

    public void GetDatos()
    {
        if (PlayerPrefs.GetInt("camaPrincipalSucia", System.Convert.ToInt32(estaMeada)) == 0) { estaMeada = false; }
        else if (PlayerPrefs.GetInt("camaPrincipalSucia", System.Convert.ToInt32(estaMeada)) == 1) { estaMeada = true; }

        if (PlayerPrefs.GetInt(gameObject.name + "tieneSabanas", System.Convert.ToInt32(tieneSabanas)) == 0) { tieneSabanas = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "tieneSabanas", System.Convert.ToInt32(tieneSabanas)) == 1) { tieneSabanas = true; }

        //if (PlayerPrefs.GetInt(gameObject.name + "usandoCama", System.Convert.ToInt32(usandoCama)) == 0) { usandoCama = false; }
        //else if (PlayerPrefs.GetInt(gameObject.name + "usandoCama", System.Convert.ToInt32(usandoCama)) == 1) { usandoCama = true; }

        if (PlayerPrefs.GetInt(gameObject.name + "paralisis", System.Convert.ToInt32(paralisis)) == 0) { paralisis = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "paralisis", System.Convert.ToInt32(paralisis)) == 1) { paralisis = true; }

        if (PlayerPrefs.GetInt(gameObject.name + "alguienTeMira", System.Convert.ToInt32(alguienMirandote)) == 0) { alguienMirandote = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "alguienTeMira", System.Convert.ToInt32(alguienMirandote)) == 1) { alguienMirandote = true; }
    }

    public void QuitarSabanas()
    {
        tieneSabanas = false;
        sabanaParent.SetActive(false);

        sabana.gameObject.transform.position = sabanaPosition.transform.position;
        sabana.active = true;
        sabana.gameObject.SetActive(true);
        sabana.sabanaLimpia = false;
        sabana.ActivarItem();
        sabana.sabanaSuciaOBJ.SetActive(true);
        sabana.sabanaLimpiaOBJ.SetActive(false);
        raycast.CogerObjeto(sabana.gameObject);

        //colider.enabled = false;
    }

    public void ColocarSabana()
    {
        raycast.ForzarSoltarObjeto();
        tieneSabanas = true;
        estaMeada = false;
        sabana.active = false;
        sabana.DesactivarItem();   
        sabana.transform.position = Vector3.zero;
        sabanaParent.SetActive(true);
    }

    public void InteractuarCama()
    {
        if (tieneSabanas)
        {
            if (estaMeada)
            {
                QuitarSabanas();
                sabanaParent.SetActive(tieneSabanas);
            }
            else
            {
                Tumbarse();
            }
        }
        else
        {
            if (raycast.hasObject) 
            {
                if (raycast.itemAtributes.isSabana && raycast.itemAtributes.sabanaLimpia) 
                {
                    ColocarSabana();
                    sabanaParent.SetActive(tieneSabanas);
                }
            }
        }   

    }

    public void Tumbarse()
    {

        icoLevantarse.SetActive(true);

        tiempoUsandoCama = 0;
        camaraFP.freeze = true;
        raycast.tumbado = true;
        raycast.blockAgacharse = true;
        player.GetComponent<CapsuleCollider>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        //camara.transform.SetParent(null);
        usandoCama = true;

        posicionCamaPlayerActual = posicionCamaPlayerBaja;
        puedeMoverse = true;

        if (paralisis) 
        {
            IniciarParalisis();
        }
        else if (alguienMirandote) 
        {
            AlguienTeMira();          
        }

        colider.enabled = false;

    }

    public void Levantarse()
    {

        icoLevantarse.SetActive(false);

        if (!raycast.usingMovil) { camaraFP.freeze = false; }
        raycast.tumbado = false;
        player.transform.position = levantarseCamaPoint.transform.position;
        player.GetComponent<CapsuleCollider>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        //camara.transform.SetParent(null);
        usandoCama = false;
        colider.enabled = true;

        interfazPlayer.SetActive(true);
        //parpadeController.ResetOjos();
        //parpadeController.velocidad = 50;
        //parpadeController.cerrandoOjos = false;

        Invoke(nameof(UnblockAgacharse), 0.4f);

        colider.enabled = true;

    }

    public void UnblockAgacharse()
    {
        raycast.blockAgacharse = false;
    }

    #region Paralisis


    public DoorController doorController;
    public PestilloController pestilloController;

    public AudioSource sonidoBebe;
    public AudioSource sonidoAranazos;
    public AudioSource sonidoAbrirPuerta;

    public GameObject prefabEnemy;

    public GameObject puntoPasillo1;
    public GameObject puntoPasillo2;

    public GameObject puntoPuerta;

    int numParalisis;
    public void IniciarParalisis()
    {

        //numParalisis = Random.Range(1, 3); 
        numParalisis = 1;

        puedeMoverse = false;
        playerStats.cantidadParalisis = 100;
        playerStats.paralisis = true;

        pestilloController.SetAbrirPestillo();
        doorController.EntreAbrirPuerta();


        respiracionController.seEscucha = true;
        respiracionController.saleVaho = true;

        //Invoke(nameof(SpawnearEnemigoAndante), 1);

        if (numParalisis == 1) //ENEMIGO TE MIRA
        {
            Invoke(nameof(SpawnearEnemigoTeMira), 15f);
            Invoke(nameof(SonidoAbrirPuerta), 10f);

            sonidoAranazos.time = 0;
            sonidoAranazos.Play();
        }
        else //ENEMIGO ANDA
        {
            Invoke(nameof(SpawnearEnemigoAndante), 15f);
            Invoke(nameof(SonidoAbrirPuerta), 10f);         

            sonidoBebe.time = 0;
            sonidoBebe.Play();
        }

        Invoke(nameof(DesactivarParalisis), 60);
        Invoke(nameof(DesactivarGrainParalisis), 50);

    }

    GameObject enemigoInstancia;
    public void AlguienTeMira()
    {
        pestilloController.SetAbrirPestillo();
        doorController.EntreAbrirPuerta();

        print("SpawnEnemy");

        /*
        enemigoInstancia = Instantiate(prefabEnemy, puntoPuerta.transform.position, puntoPuerta.transform.rotation);
        enemigoInstancia.GetComponent<MovimientoEnemy>().destino = puntoPuerta;
        //enemigoInstancia.GetComponent<MovimientoEnemy>().finalPasillo = puntoPasillo2;
        //instancia.GetComponent<MovimientoEnemy>().desapareceAlLlegar = true;

        Invoke(nameof(SonidoAbrirPuerta), 0.1f);
        Invoke(nameof(MoverAlguienTeMira), 0.2f);
        */

        SpawnearEnemigoTeMira();
    }

    public void DesaparecerEnemy()
    {
        Destroy(enemigoInstancia);
    }


    public void SonidoAbrirPuerta()
    {
        sonidoAranazos.Stop();
        sonidoAbrirPuerta.Play();
    }

    public void SpawnearEnemigoAndante()
    {
        print("SpawnEnemy");
        enemigoInstancia = Instantiate(prefabEnemy, puntoPasillo1.transform.position , puntoPasillo1.transform.rotation);
        enemigoInstancia.GetComponent<MovimientoEnemy>().destino = puntoPasillo2;
        enemigoInstancia.GetComponent<MovimientoEnemy>().desapareceAlLlegar = true;

        alguienEnLaCasa = true;
    }

    public void SpawnearEnemigoTeMira()
    {
        print("SpawnEnemy");
        enemigoInstancia = Instantiate(prefabEnemy, puntoPasillo1.transform.position, puntoPasillo1.transform.rotation);
        enemigoInstancia.GetComponent<MovimientoEnemy>().destino = puntoPuerta;
        enemigoInstancia.GetComponent<MovimientoEnemy>().desapareceAlLlegar = false;
        enemigoInstancia.GetComponent<MovimientoEnemy>().teMira = true;
        enemigoInstancia.GetComponent<MovimientoEnemy>().finalPasillo = puntoPasillo2;

        alguienEnLaCasa = true;
    }

    public void DesactivarGrainParalisis()
    {
        playerStats.paralisis = false;
    }

    public void DesactivarParalisis()
    {
        paralisis = false;
        alguienMirandote = false;
        sonidoBebe.Stop();
        sonidoAranazos.Stop();
        respiracionController.seEscucha = false;
        respiracionController.saleVaho = false;
    }

    #endregion

}
