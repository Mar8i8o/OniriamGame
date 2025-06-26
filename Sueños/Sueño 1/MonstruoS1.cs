using UnityEngine;
using UnityEngine.AI;

public class MonstruoS1 : MonoBehaviour
{
    [Header("Componentes")]
    [HideInInspector]public NavMeshAgent agent;
    public GameObject mesh;
    GameObject player;
    CamaraFP camaraFP;
    CharacterController characterController;
    CapsuleCollider coliderPlayer;
    PlayerVida playerVida;
    PensamientoControler pensamientoControler;
    DreamController dreamController;

    //public TrailRenderer trailRenderer;

    public GameObject comiendoPoint;
    public GameObject comiendoSpawnPoint;

    public float distanciaSq;

    public Transform sonidos;

    public AudioSource sonidoComer;
    public AudioSource sonidoNormal;
    public AudioSource sonidoArrastrarse;

    public GramofonoController gramofonoController;

    [Header("Teletransporte")]
    public Transform[] positionPoints;
    public float distanciaMinima = 3f;
    public float radioVisibilidad = 1.5f;

    [Header("Detección")]
    public DetectarIsVisible detectarIsVisible;

    [Header("Teletransporte por tiempo")]
    public float tiempoMaxSinVer = 10f;
    public float tiempoSinSerVisto = 0f;

    private bool tpeado = false;
    private Camera cam;

    private float tiempoEntreActualizacionesDestino = 0.2f;
    private float tiempoDesdeUltimaActualizacion = 0f;

    bool puedeTeletransportarse;

    public bool comiendoPlayer;
    bool puedeComer;
    public bool acechandoPlayer;
    public int contadorEspacios;

    public bool bloquearLiberarse;

    public ParticleSystem particulas;

    private void Awake()
    {
        player = GameObject.Find("Player");
        dreamController = GameObject.Find("GameManager").GetComponent<DreamController>();
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();
        camaraFP = player.GetComponent<CamaraFP>();
        characterController = player.GetComponent<CharacterController>();
        playerVida = player.GetComponent<PlayerVida>();
        coliderPlayer = player.GetComponent<CapsuleCollider>();
        agent = gameObject.GetComponent<NavMeshAgent>();

        detectarIsVisible.enabled = false;

    }

    void Start()
    {
        cam = Camera.main;
        puedeComer = true;
    }

    void Update()
    {
        if (comiendoPlayer)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, comiendoPoint.transform.position, 0.5f * Time.deltaTime);
            agent.SetDestination(transform.position);
            playerVida.MantenerDanyo();

            if (!bloquearLiberarse)
            {
                if (Input.GetKeyDown(KeyCode.Space)) contadorEspacios++;
                if (contadorEspacios > 10) SalirComerPlayer();
            }
        }

        if (!puedeComer && !comiendoPlayer)
            sonidoComer.volume -= Time.deltaTime;

        if (!acechandoPlayer) return; //ACECHANDO

        //trailRenderer.gameObject.transform.position = new Vector3(transform.position.x, trailRenderer.gameObject.transform.position.y, transform.position.z);
        particulas.transform.position = new Vector3(transform.position.x, particulas.gameObject.transform.position.y, transform.position.z);

        tiempoDesdeUltimaActualizacion += Time.deltaTime;
        Vector3 posJugador = player.transform.position;

        MoverSonido();

        if (!gramofonoController.sonando)
        {
            if (detectarIsVisible.isVisible)
            {
                tiempoSinSerVisto = 0f;
                tpeado = false;

                if (!agent.enabled) agent.enabled = true;

                if (tiempoDesdeUltimaActualizacion >= tiempoEntreActualizacionesDestino)
                {
                    agent.SetDestination(posJugador);
                    //trailRenderer.emitting = true;
                    tiempoDesdeUltimaActualizacion = 0f;
                }
            }
            else
            {
                tiempoSinSerVisto += Time.deltaTime;

                distanciaSq = (transform.position - player.transform.position).sqrMagnitude;

                if ((!tpeado || tiempoSinSerVisto >= tiempoMaxSinVer) && puedeTeletransportarse && distanciaSq >= 120) // 8^2 = 64
                {
                    IniciarTP();
                }
                else
                {
                    if (!agent.enabled) agent.enabled = true;

                    if (tiempoDesdeUltimaActualizacion >= tiempoEntreActualizacionesDestino)
                    {
                        agent.SetDestination(posJugador);
                        tiempoDesdeUltimaActualizacion = 0f;
                    }
                }
            }
        }
        else //CUANDO ESTA SONANDO EL GRAMOFONO
        {
            agent.SetDestination(gramofonoController.gameObject.transform.position);
            ForzarMiradaY(gramofonoController.gameObject.transform, 1);
        }

        //CONTROLAR SONIDO ARRASTRARSE

        if (agent.velocity.magnitude < 0.15f)
        {
            if (sonidoArrastrarse.isPlaying)sonidoArrastrarse.Stop();
        }
        else
        {
            if(!sonidoArrastrarse.isPlaying)sonidoArrastrarse.Play();
        }

    }

    private void IniciarTP()
    {
        agent.enabled = false;
        mesh.SetActive(false);
        tpeado = true;
        tiempoSinSerVisto = 0f;
        //trailRenderer.emitting = false;
        TPPositionPointMasCercano();
    }

    private void TPPositionPointMasCercano()
    {
        Vector3 posPlayer = player.transform.position;
        float distanciaMinimaSq = distanciaMinima * distanciaMinima;

        Transform mejorPunto = null;
        float mejorDistanciaSq = Mathf.Infinity;

        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);

        foreach (Transform pt in positionPoints)
        {
            Vector3 ptPos = pt.position;
            float distSq = (ptPos - posPlayer).sqrMagnitude;

            if (distSq < distanciaMinimaSq || distSq >= mejorDistanciaSq)
                continue;

            if (EsVisibleParaJugador(ptPos, radioVisibilidad, frustumPlanes))
                continue;

            mejorDistanciaSq = distSq;
            mejorPunto = pt;
        }

        if (mejorPunto == null)
        {
            Debug.LogWarning("No se encontró punto válido para TP.");
            IniciarTP();
            //mesh.SetActive(true);
            return;
        }

        transform.position = mejorPunto.position;

        Vector3 dir = posPlayer - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0f)
            transform.rotation = Quaternion.LookRotation(dir);

        if (detectarIsVisible.isVisible)
            Debug.LogWarning("El punto elegido es visible tras el TP.");

        mesh.SetActive(true);
        //trailRenderer.emitting = true;
    }
    public LayerMask layerMask = ~0;  // Por defecto todas las capas
    private bool EsVisibleParaJugador(Vector3 centro, float radio, Plane[] frustumPlanes)
    {
        if (!GeometryUtility.TestPlanesAABB(frustumPlanes, new Bounds(centro, Vector3.one * radio * 2f)))
            return false;

        Vector3 origen = cam.transform.position;
        Vector3 dir = (centro - origen).normalized;
        float distancia = Vector3.Distance(origen, centro);

        if (Physics.Raycast(origen, dir, distancia, layerMask))
            return false;

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !comiendoPlayer && puedeComer)
        {
            IniciarComer();
        }
    }

    public void MoverSonido()
    {
        sonidos.position = Vector3.MoveTowards(sonidos.position, transform.position, 8 * Time.deltaTime);
    }
    public void IniciarComer()
    {
        comiendoPlayer = true;
        camaraFP.freeze = true;
        characterController.enabled = false;
        coliderPlayer.enabled = false;
        acechandoPlayer = false;
        puedeComer = false;

        contadorEspacios = 0;

        sonidoComer.Play();
        sonidoNormal.Stop();
        sonidoComer.volume = 1.0f;

        playerVida.vidas--;
        if (playerVida.vidas <= 0)
        {
            Invoke(nameof(LlamarDespertarse), 3);
            bloquearLiberarse = true;
        }
        else
        {
            pensamientoControler.MostrarPensamiento("Pulsa [ESPACIO] para soltarte", 99);
        }

    }

    public void LlamarDespertarse()
    {
        dreamController.negro.SetActive(true);
        dreamController.Despertarse();
    }

    public void SalirComerPlayer()
    {
        player.transform.position = comiendoSpawnPoint.transform.position;
        comiendoPlayer = false;
        camaraFP.freeze = false;
        characterController.enabled = true;
        coliderPlayer.enabled = true;
        acechandoPlayer = true;

        playerVida.SoltarDanyo();
        //playerVida.particulasSangre.Play();

        pensamientoControler.DejarDeMostrarPensamiento();
        sonidoNormal.Play();
        Invoke(nameof(ActivarPuedeComer), 2);
        contadorEspacios = 0;
    }

    public void ActivarPuedeComer()
    {
        puedeComer = true;
        sonidoComer.Stop();
    }

    public void IniciarPersecucion()
    {
        acechandoPlayer = true;
        Invoke(nameof(ActivarTP), 4);
        detectarIsVisible.enabled = true;
    }

    public void DesactivarPersecucion()
    {
        acechandoPlayer = false;
        puedeComer = false;
        detectarIsVisible.enabled = false;
        sonidoArrastrarse.enabled = false;
    }

    public void ActivarTP()
    {
        puedeTeletransportarse = true;
    }

    public void ForzarMiradaY(Transform dondeMira, float speed)
    {
        // Obtén la dirección hacia el objetivo
        Vector3 targetDirection2 = dondeMira.transform.position - transform.position;
        targetDirection2.y = 0f; // Asegúrate de que no haya rotación vertical

        // Calcula la rotación necesaria para mirar hacia el objetivo solo en el eje Y
        Quaternion targetRotation2 = Quaternion.LookRotation(targetDirection2);

        // Ajusta la rotación para que solo afecte el eje Y
        targetRotation2 = Quaternion.Euler(0f, targetRotation2.eulerAngles.y, 0f);

        // Aplica la rotación de forma suavizada
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation2, speed * Time.deltaTime);
    }
}
