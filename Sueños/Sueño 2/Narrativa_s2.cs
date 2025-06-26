using UnityEngine;

public class Narrativa_s2 : MonoBehaviour
{

    public bool saltarseIntro;
    public bool spawnearCoche;

    public LookAtObj2 lookAtObj;
    public NpcDialogue npcDialogue;

    public SceneDialogsController sceneDialogsController;

    string idDialogo;

    public bool conversacionActiva;

    public GameObject enemigoCarretera;
    public LoopCarretera loopCarretera;
    public CocheController cocheController;

    public int indiceHistoria;
    public GameObject modeloCiudad;
    public Animator cochePosAnim;
    public Transform posicionAparcar;

    public Animator mejorAmigoAnim;

    void Start()
    {

        mejorAmigoAnim.SetBool("InCar", true);

        if (spawnearCoche) //ELIJE DONDE SPAWNEAR
        {

            cocheController.SetSubirCoche();

            if (!saltarseIntro)
            {
                idDialogo = "amigo_s2_1";
                Invoke(nameof(EmpezarDialogo), 4);
                enemigoCarretera.SetActive(false);
            }
            else
            {
                Invoke(nameof(DetenerCarretera), 4);
                indiceHistoria = 2;
            }

            lookAtObj.maxRotationAngleYPos = 15;

        }
        else
        {
            indiceHistoria = 10;
            loopCarretera.gameObject.SetActive(false);
        }
    }

    float contador;

    void Update()
    {
        if(conversacionActiva) //DETECTA CUANDO SE HA ACABADO UNA CONVERSACION
        {
            if (!sceneDialogsController.dialogueActive)
            {
                SalirDialogo();
                conversacionActiva = false;
            }
        }

        if (enemigoCarretera.activeSelf)
        {
            enemigoCarretera.transform.Translate(loopCarretera.velocidad * Time.deltaTime);
        }

        // INDICES HISTORIA //

        if(indiceHistoria == 2) //ESPERA A QUE ACABE EL LOOP Y ACTIVA LA ANIMACION DE APARCAR
        {
            if(!loopCarretera.avanzando) cochePosAnim.SetBool("Aparcar", true);

            float distancia = Vector3.Distance(cocheController.gameObject.transform.position, posicionAparcar.transform.position);

            //print(distancia);

            if (distancia <= 0.1f)
            {
                AparcarCoche();

                idDialogo = "amigo_s2_3";
                EmpezarDialogo();
            }
        }
        else if (indiceHistoria == 3) //CUANDO ACABAS DE HABLAR CON EL AMIGO Y TE ABRE LA PUERTA
        {
            contador += Time.deltaTime;

            if(contador > 1)
            {
                cocheController.SalirCoche();
            }
            if (contador > 1.5)
            {
                cocheController.CerrarPuerta("delanteDer");
            }
            if (contador > 2)
            {
                cochePosAnim.SetBool("Irse", true);
                ActualizarIndice();
            }

        }

    }

    public void EmpezarDialogo()
    {
        npcDialogue.idDialogo = idDialogo;
        npcDialogue.AbrirDialogo();

        lookAtObj.maxRotationAngleYPos = 55;

        conversacionActiva = true;
    }

    public void SalirDialogo()
    {
        lookAtObj.maxRotationAngleYPos = 15;
        ActualizarIndice();

        if (indiceHistoria == 1)
        {
            SpawnearEnemigoCarretera();

            idDialogo = "amigo_s2_2";
            Invoke(nameof(EmpezarDialogo), 10);

        }
        else if (indiceHistoria == 2)
        {
            DetenerCarretera();
        }
        else if (indiceHistoria == 3)
        {
            //print("Abrir puerta coche");
            cocheController.AbrirPuerta("delanteDer");
        }
    }

    public void ActualizarIndice()
    {
        indiceHistoria++;
    }

    public void SpawnearEnemigoCarretera()
    {
        print("SpawnEnemigo");
        enemigoCarretera.SetActive(true);
    }

    public void DetenerCarretera()
    {
        loopCarretera.DetenerCarretera();
        modeloCiudad.SetActive(true);
    }

    public void AparcarCoche()
    {
        //print("CocheAparcado");
    }
}

