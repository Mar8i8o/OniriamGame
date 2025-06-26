using Unity.VisualScripting;
using UnityEngine;

public class Narrativa_s2_final : MonoBehaviour
{

    public bool playerDentro;

    public bool drawGizmos = true;

    public Animator amigoAnim;
    public NpcDialogue npcDialogue;

    public int indiceHistoria;

    void Start()
    {
        amigoAnim.SetBool("InCar", true);
        alterK.SetActive(true);
        alterKAnim.SetBool("InCar", true);
        alterK.SetActive(false);
        //cocheController.AbrirPuerta("detrasIzq");
    }


    public Vector3 offsetBox;
    public Vector3 detectionBoxSize;
    Quaternion boxRotation;
    public LayerMask playerLayer;

    public CocheController cocheController;
    public Animator cocheAnim;
    public LoopCarretera loopCarretera;

    public float contador;

    public LookAtObj2 lookAtObj2;

    public PlayerVida playerVida;

    public SceneDialogsController sceneDialogsController;
    public DetectarIsVisible detectarIsVisible;


    public Animator alterKAnim;
    public GameObject mejorAmigo;
    public GameObject alterK;

    public DreamController dreamController;

    public MoverNPC moverNPC;

    public DoorController doorController1;
    public DoorController doorController2;

    void Update()
    {

        //DETECTAR FINAL MALO

        if(!finalMalo)
        {
            if(playerVida.vidas <= 0)
            {
                DesbloquearFinalMalo();
            }
        }

        //LOGICA HISTORIA

        playerDentro = Physics.CheckBox(transform.position + offsetBox, detectionBoxSize / 2, boxRotation, playerLayer);

        if (indiceHistoria == 0)
        {
            if (playerDentro)
            {
                cocheController.AbrirPuerta("delanteDer");
                npcDialogue.speedY = 2;
                npcDialogue.SetAbrirDialogo("amigo_s2_1");

                doorController1.SetCerrarPuerta();
                doorController2.SetCerrarPuerta();

                doorController1.sePuedeAbrir = false;
                doorController2.sePuedeAbrir = false;

                moverNPC.persiguiendoPlayer = false;
                moverNPC.destino = moverNPC.posicionPatrulla;

                indiceHistoria++;

            }
        }
        if(indiceHistoria == 1) 
        {
            if(cocheController.sentado)
            {
                lookAtObj2.maxRotationAngleYPos = 0;
                lookAtObj2.maxRotationAngleYMin = 0;

                contador += Time.deltaTime;

                if (contador > 1)
                {
                    cocheAnim.SetBool("irseCoche", true);
                    contador = 0;
                    indiceHistoria++;

                    lookAtObj2.enabled = false;
                }
            }
        }
        if(indiceHistoria == 2)
        {
            contador += Time.deltaTime;
            lookAtObj2.maxRotationAngleYPos = -100;
            lookAtObj2.maxRotationAngleYMin = -20;

            if (contador > 3.5)
            {

                lookAtObj2.maxRotationAngleYPos = -150;
                lookAtObj2.maxRotationAngleYMin = -20;

                loopCarretera.AcelerarCarretera();
                contador = 0;
                indiceHistoria++;
            }
        }
        if (indiceHistoria == 3)  //DONDE VA AL FINAL MALO
        {
            contador += Time.deltaTime;
            lookAtObj2.maxRotationAngleYPos = -150;
            lookAtObj2.maxRotationAngleYMin = -20;

            if (contador > 6)
            {
                npcDialogue.SetAbrirDialogo("amigo_s2_1");
                contador = 0;
                indiceHistoria++;
                if(!finalMalo)
                {
                    lookAtObj2.maxRotationAngleYPos = 0;
                    lookAtObj2.maxRotationAngleYMin = 58;
                }
                else
                {
                    lookAtObj2.maxRotationAngleYPos = 0;
                    lookAtObj2.maxRotationAngleYMin = -163;
                }


                lookAtObj2.enabled = true;
  
            }
        }
        if(indiceHistoria == 4)
        {
            if (!sceneDialogsController.dialogueActive)
            {
                lookAtObj2.maxRotationAngleYPos = -150;
                lookAtObj2.maxRotationAngleYMin = -20;
                lookAtObj2.rotationSpeed = 2;
                contador += Time.deltaTime;
                if (contador > 5)
                {
                    if (!detectarIsVisible.isVisible)
                    {
                        contador = 0;
                        mejorAmigo.SetActive(false);
                        alterK.SetActive(true);
                        alterKAnim.SetBool("InCar", true);
                        indiceHistoria++;
                    }
                }
            }
        }
        if(indiceHistoria == 5)
        {
            contador += Time.deltaTime;

            if(contador > 3)
            {
                negro.SetActive(true);
                dreamController.Despertarse();
            }
        }

    }

    public GameObject negro;

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        // Guarda la matriz de transformación original de Gizmos
        Gizmos.matrix = Matrix4x4.TRS(transform.position + offsetBox, transform.rotation, Vector3.one);

        // Establece el color del Gizmo
        Gizmos.color = Color.green;

        // Dibuja el WireCube con la rotación aplicada
        Gizmos.DrawWireCube(Vector3.zero, detectionBoxSize);

        // Restaura la matriz de transformación de Gizmos a su estado original (opcional, pero recomendable)
        Gizmos.matrix = Matrix4x4.identity;
    }

    public GameObject posicionCocheFinal;

    public GameObject carreteraFinal;
    public GameObject escenarioFinal;
    public GameObject concierto;
    public ParpadeoController parpadeoController;
    public bool finalMalo;

    public void DesbloquearFinalMalo()
    {
        finalMalo = true;
        Invoke(nameof(EmpezarFinalMalo),5);
        parpadeoController.SetCerrarOjos(300);

    }

    public void EmpezarFinalMalo()
    {

        moverNPC.persiguiendoPlayer = false;
        moverNPC.destino = moverNPC.posicionPatrulla;

        carreteraFinal.SetActive(true);
        escenarioFinal.SetActive(true);
        concierto.SetActive(false);

        lookAtObj2.maxRotationAngleYPos = -150;
        lookAtObj2.maxRotationAngleYMin = -20;

        cocheAnim.enabled = false;
        cocheAnim.gameObject.transform.position = posicionCocheFinal.transform.position;
        cocheAnim.gameObject.transform.rotation = posicionCocheFinal.transform.rotation;

        loopCarretera.avanzando = true;

        cocheController.SetSubirCoche();

        indiceHistoria = 3;

        parpadeoController.SetAbrirOjos(300);

    }

}
