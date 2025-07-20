using Unity.VisualScripting;
using UnityEngine;

public class Dia2_Controller : MonoBehaviour
{
     LlamadasController llamadasController;
    DreamController dreamController;
    public int indiceHistoria;
    public int inicioSuperado; //1 SI HAS PASADO LA PARTE DE LA ESCALERA, 0 SI AUN NO
    public float contador;

    public DoorController puertaEscalera;
    public DoorController puertaPasilloFinal;
    public NpcDialogue abuelitaNPC;
    public GameObject panelLlamada;
    public PensamientoControler pensamientoControler;

    public EscalerasController escalerasController;
    public GameObject escaleras;

    PlayerStats playerStats;

    public GameObject alterK;
    public Animator alterKAnim;

    public bool playerDentro;
    public Vector3 offsetBoxEntrada;
    public Vector3 detectionBoxSizeEntrada;
    Quaternion boxRotation;
    public LayerMask playerLayer;
    public bool drawGizmos;

    public Transform posicionAlterK;
    public Transform posicionAlterKInicio;

    public CameraShake cameraShake;

    public AudioSource sonidoPuerta;

    public GameObject laberinto;

    public GameObject elementosFase1;
    public GameObject elementosFase2;

    public ChairController chairController;

    private void Awake()
    {
        abuelitaNPC.gameObject.SetActive(false);
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
    }

    void Start()
    {
        llamadasController = GameObject.Find("GameManager").GetComponent<LlamadasController>();
        dreamController = GameObject.Find("GameManager").GetComponent<DreamController>();

        inicioSuperado = PlayerPrefs.GetInt("InicioSuperadoEscaleras", inicioSuperado);

        if(inicioSuperado == 1) //DESPERTARSE DESPUES
        {
            indiceHistoria = 10;
            laberinto.SetActive(true);

            elementosFase1.SetActive(false);
            elementosFase2.SetActive(true);
        }
        else //INICIO
        {
            laberinto.SetActive(false);

            elementosFase1.SetActive(true);
            elementosFase2.SetActive(false);

            chairController.blockSentarseSilla = true;
            chairController.generaPensamiento = true;
            chairController.pensamiento = "Hoy tengo que ir a trabajar presencialmente";
            playerStats.actualizaStats = false;
        }

    }

    void Update()
    {
        if (indiceHistoria == 0)
        {
            pensamientoControler.MostrarPensamiento("¿Quien era ella?", 2);
            indiceHistoria++;
        }
        if (indiceHistoria == 1 )
        {
            contador += Time.deltaTime;

            if( contador > 6 ) 
            {
                pensamientoControler.MostrarPensamiento("Hoy tengo que ir a trabajar presencialmente", 2);
                indiceHistoria++;
                contador = 0;
            }

        }
        else if( indiceHistoria == 2 ) 
        {
            contador += Time.deltaTime;

            if (contador > 6)
            {
                llamadasController.tienePensamiento = true;
                llamadasController.pensamiento = "Tengo que ir al trabajo";
                llamadasController.GenerarRecibirLlamada("Jefe", "jefe_d1_1");
                llamadasController.bloquearColgar = true;
                puertaEscalera.sePuedeAbrir = false;
                indiceHistoria++;
                contador = 0;
            }
        }
        else if (indiceHistoria == 3)
        {
            if (llamadasController.esperandoLlamada && !panelLlamada.activeSelf)
            {
                puertaEscalera.pensamiento = "Está cerrada, deberia ir a buscar algo para abrir la puerta, tengo que ir a trabajar";
                indiceHistoria++;
            }
        }
        else if (indiceHistoria == 4)
        {
            if (puertaEscalera.blockPensamiento)
            {
                abuelitaNPC.gameObject.SetActive(true);
                indiceHistoria++;
            }
        }
        else if((indiceHistoria == 5))
        {
            if(abuelitaNPC.hablado && !abuelitaNPC.dialogoActivo)
            {
                escaleras.SetActive(true);
                puertaEscalera.generaPensamiento = false;
                puertaEscalera.sePuedeAbrir = true;
                indiceHistoria++;
            }
        }
        else if(indiceHistoria == 6) 
        {
            playerDentro = Physics.CheckBox(transform.position + offsetBoxEntrada, detectionBoxSizeEntrada / 2, boxRotation, playerLayer);
            if(playerDentro)
            {
                alterK.SetActive(true);
                alterKAnim.SetTrigger("Patada");
                contador = 0;
                indiceHistoria++;
            }
        }
        else if (indiceHistoria == 7)
        {
            contador += Time.deltaTime;
            alterK.transform.position = Vector3.MoveTowards(alterK.transform.position, posicionAlterKInicio.position, 0.5f * Time.deltaTime);
            if (contador > 1.2)
            {
                alterKAnim.SetBool("Walking", true);
                contador = 0;
                indiceHistoria++;
            }
            else if(contador > 0.5)
            {
                if (!puertaRota)
                {
                    puertaPasilloFinal.RomperPuerta();
                    print("CameraShake");
                    cameraShake.CameraShakeMomentaneo(1);
                    sonidoPuerta.Play();
                    puertaRota = true;
                }
            }
        }
        else if (indiceHistoria == 8)
        {

            alterK.transform.position = Vector3.MoveTowards(alterK.transform.position, posicionAlterK.position, 1 * Time.deltaTime);

            contador += Time.deltaTime;
            if (contador > 1.5)
            {
                PlayerPrefs.SetInt("InicioSuperadoEscaleras", 1);
                dreamController.ForzarDespertarseApartamento();
                escalerasController.luzTerrado.SetActive(true);
                escalerasController.luzDelSol.SetActive(true);
                indiceHistoria++;
            }
        }
        else if (indiceHistoria == 10) //DESPERTARSE 2
        {
            pensamientoControler.MostrarPensamiento("¿Que acaba de pasar?", 5);
            indiceHistoria++;
        }
    }

    bool puertaRota;

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        // Guarda la matriz de transformación original de Gizmos
        Matrix4x4 originalMatrix = Gizmos.matrix;

        // Establece el color del Gizmo
        Gizmos.color = Color.green;

        // Aplica la primera transformación y dibuja el primer WireCube
        Gizmos.matrix = originalMatrix * Matrix4x4.TRS(transform.position + offsetBoxEntrada, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, detectionBoxSizeEntrada);

        // Restaura la matriz de transformación de Gizmos a su estado original
        Gizmos.matrix = originalMatrix;
    }

    [ContextMenu(itemName: "Delete_DayPrefs")]

    public void deleteDayPrefs()
    {
        PlayerPrefs.DeleteKey("InicioSuperadoEscaleras");
        Debug.Log("All player prefs deleted");
    }
}
