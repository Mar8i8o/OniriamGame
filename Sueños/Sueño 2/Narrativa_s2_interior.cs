using UnityEngine;

public class Narrativa_s2_interior : MonoBehaviour
{

    public bool drawGizmos;

    public int indiceHistoria;

    public bool playerDentro;

    public Vector3 offsetBoxEntrada;
    public Vector3 detectionBoxSizeEntrada;
    Quaternion boxRotation;
    public LayerMask playerLayer;

    public Vector3 offsetBoxFinalEntrada;
    public Vector3 detectionBoxSizeFinalEntrada;

    public Vector3 offsetBoxEntradaFiesta;
    public Vector3 detectionBoxSizeEntradaFiesta;

    public float contador;

    public GameObject luzFinalPasillo;
    public GameObject enemigoPasillo;
    public GameObject mundoNormal;

    public DoorController puertaEntrada;

    public GameObject mejorAmigo;
    public NpcDialogue mejorAmigo_dialogue;
    public MoverNPC mejorAmigoMovimiento;
    public LookAtObj2 mejorAmigoMirada;

    public GameObject player;


    public DoorController puertaEntradaFiesta;

    public AudioSource sonidoSusto;

    public Material materialChica;
    public Texture texturaChica;


    void Start()
    {

        materialChica.SetTexture("_BaseColorMap", texturaChica);

        luzFinalPasillo.SetActive(false);
        enemigoPasillo.SetActive(false);
        mejorAmigo.SetActive(false);
        mundoNormal.SetActive(false);
    }

    void Update()
    {

        if (indiceHistoria == 0)
        {
            playerDentro = Physics.CheckBox(transform.position + offsetBoxEntrada, detectionBoxSizeEntrada / 2, boxRotation, playerLayer);

            if(playerDentro) 
            {
                contador += Time.deltaTime;


                if (contador > 0 && contador < 0.1)
                {
                    //enemigoPasillo.SetActive(true);
                    //luzFinalPasillo.SetActive(true);

                    puertaEntrada.SetCerrarPuerta();
                    puertaEntrada.sePuedeAbrir = false;
                    puertaEntrada.pensamiento = "Algo esta bloqueando la puerta";
                    puertaEntrada.generaPensamiento = true;

                }
                else if (contador > 0.1 && contador < 2) 
                {
                    luzFinalPasillo.SetActive(false);
                    enemigoPasillo.SetActive(false);
                }
                else if (contador > 2 && contador < 10)
                {
                    luzFinalPasillo.SetActive(true);
                    contador = 0;
                    indiceHistoria++;
                    playerDentro = false;
                }
            }

        }

        if (indiceHistoria == 1)
        {
            playerDentro = Physics.CheckBox(transform.position + offsetBoxFinalEntrada, detectionBoxSizeFinalEntrada / 2, boxRotation, playerLayer);

            if (playerDentro && EstaMirandoHacia(rotacionObjetivo))
            {
                mejorAmigo.SetActive(true);
                mundoNormal.SetActive(true);
                mejorAmigo_dialogue.speedY = 5;
                mejorAmigo_dialogue.SetAbrirDialogo("amigo_s2_4");
                mejorAmigo_dialogue.speedY = 1;
                sonidoSusto.Play();
                //mejorAmigo_dialogue.canTalk = false;

                puertaEntradaFiesta.sePuedeAbrir = true;

                indiceHistoria++;
            }

        }
        if(indiceHistoria == 2) 
        {

            playerDentro = Physics.CheckBox(transform.position + offsetBoxEntradaFiesta, detectionBoxSizeEntradaFiesta / 2, boxRotation, playerLayer);

            if (puertaEntradaFiesta.puertaAbierta)
            {
                mejorAmigoMovimiento.freeze = false;
                mejorAmigoMirada.maxRotationAngleYMin = 10;
                mejorAmigoMirada.maxRotationAngleYPos = 10;
                puertaEntradaFiesta.sePuedeAbrir = false;
            }

            if(playerDentro)
            {
                Invoke(nameof(CerrarPuertaEntradaFiesta), 1.5f);
                indiceHistoria++;
            }
        }

    }
    public void CerrarPuertaEntradaFiesta()
    {
        puertaEntradaFiesta.SetCerrarPuerta();
    }

    public Vector3 rotacionObjetivo; // Ángulos en los que el jugador debe mirar (en grados)
    public float anguloTolerancia = 10f; // Margen de error en grados

    bool EstaMirandoHacia(Vector3 rotacionDeseada)
    {
        // Convertir la rotación deseada a una dirección en espacio mundial
        Quaternion rotacionObjetivoQuat = Quaternion.Euler(rotacionDeseada);
        Vector3 direccionObjetivo = rotacionObjetivoQuat * Vector3.forward;

        // Dirección actual del jugador
        Vector3 forwardJugador = player.transform.forward;

        // Calcular el ángulo entre ambas direcciones
        float angulo = Vector3.Angle(forwardJugador, direccionObjetivo);

        return angulo <= anguloTolerancia;
    }

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

        // Restaura la matriz de transformación original antes de aplicar la segunda transformación
        Gizmos.matrix = originalMatrix;

        // Aplica la segunda transformación y dibuja el segundo WireCube
        Gizmos.matrix = originalMatrix * Matrix4x4.TRS(transform.position + offsetBoxFinalEntrada, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, detectionBoxSizeFinalEntrada);

        // Restaura la matriz de transformación original antes de aplicar la tercera transformación
        Gizmos.matrix = originalMatrix;

        // Establece un nuevo color para diferenciar el tercer WireCube
        Gizmos.color = Color.blue;

        // Aplica la tercera transformación y dibuja el tercer WireCube
        Gizmos.matrix = originalMatrix * Matrix4x4.TRS(transform.position + offsetBoxEntradaFiesta, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, detectionBoxSizeEntradaFiesta);

        // Restaura la matriz de transformación de Gizmos a su estado original
        Gizmos.matrix = originalMatrix;
    }

}
