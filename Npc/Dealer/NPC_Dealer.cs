using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Dealer : MonoBehaviour
{
    public ItemSpawnerController weedSpawner;
    public ItemSpawnerController pastisSpawner;

    GameObject player;

    public GameObject posicionSpawnPorro;
    public GameObject itemSpawneado;
    public GameObject dondeMira;
    public Collider colider;

    public bool esperandoRecojer;

    public Animator NPCanim;
    SceneDialogsController sceneDialogsController;

    public float existenciasPorros;

    public GameObject posicionDescanso;
    public NpcDialogue npcDialogue;
    public Animator npcAnim;

    public bool yendose;
    public bool blockAbrirPuertas;
    public bool volviendo;
    public float distanciaPuerta;
    public DoorController doorController;

    Vector3 posicionInicial;

    [HideInInspector]public NavMeshAgent agent;
    TimeController timeController;
    public GameObject panelDialogo;
    public LookAtObj lookAtObj;

    public bool rutinaNormal;
    void Start()
    {
        //Invoke(nameof(OfrecerPorro), 1);
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        sceneDialogsController = GameObject.Find("GameManager").GetComponent<SceneDialogsController>();
        timeController = GameObject.Find("GameManager").GetComponent<TimeController>(); 
        posicionInicial = transform.position;
        
    }

    public float tiempoEsperandoRecojer;
    public GameObject alijoDrogas;

    public bool freezePos;
    void Update()
    {

        if (esperandoRecojer)
        {
            if (itemSpawneado != null)
            {

                tiempoEsperandoRecojer += Time.deltaTime;

                if (itemSpawneado.GetComponent<ItemAtributes>().vendido) //CAMBIA A VENDIDO EN EL PICKUP DE ITEMATRIBUTES
                {
                    colider.enabled = true;
                    esperandoRecojer = false;
                    NPCanim.SetBool("Ofrecer", false);
                    npcDialogue.canTalk = true;
                }

                if (tiempoEsperandoRecojer > 120)
                {
                    itemSpawneado.transform.position = alijoDrogas.transform.position;

                    if (itemSpawneado.GetComponent<ItemAtributes>().isCigar) { itemSpawneado.GetComponent<ItemAtributes>().ResetearCigarro(); }
                    else if (itemSpawneado.GetComponent<ItemAtributes>().isPildoras) { itemSpawneado.GetComponent<ItemAtributes>().ResetearPastillas(); }

                    colider.enabled = true;
                    esperandoRecojer = false;
                    NPCanim.SetBool("Ofrecer", false);
                    npcDialogue.canTalk = true;
                }
            }
        }

        //print("Existencias porros: " + CalcularExistenciasPorros());
        ControladorAnim();
        if(rutinaNormal) ControladorRutina();
    }

    public Vector3 targetRotacion;

    public void ControladorAnim()
    {
        if (agent.velocity.magnitude < 0.12f)
        {
            npcAnim.SetBool("Walking", false);
            npcDialogue.canTalk = true;
            lookAtObj.enabled = true;
            //MirarEjeY(player.transform, 1);
            AjustarEulerAngles(targetRotacion);
        }
        else
        {
            npcAnim.SetBool("Walking", true);
            npcDialogue.canTalk = false;
            lookAtObj.enabled = false;
        }
    }

    void AjustarEulerAngles(Vector3 targetEuler)
    {
        float velocidad = 2f;
        Quaternion rotacionObjetivo = Quaternion.Euler(targetEuler);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidad);
    }


    public void MirarEjeY(Transform dondeMira, float speed)
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
    public void ControladorRutina()
    {
        /*
        if (timeController.hora == 21 && !yendose && !panelDialogo.activeSelf && !esperandoRecojer)
        {
            yendose = true;
            volviendo = false;
            blockAbrirPuertas = false;
        }

        if (timeController.hora == 12 && timeController.hora < 13 && !volviendo)
        {
            yendose = false;
            volviendo = true;
            blockAbrirPuertas = false;
        }
        */


        if (yendose || volviendo)
        {
            if (!freezePos)
            {
                distanciaPuerta = Vector3.Distance(doorController.gameObject.transform.position, transform.position);
                if (yendose) agent.SetDestination(posicionDescanso.transform.position);
                else if (volviendo) agent.SetDestination(posicionInicial);
            }
            else
            {
                agent.SetDestination(transform.position);
            }

        }

    }

    public void CerrarPuerta()
    {
        doorController.SetCerrarPuerta();
    }

    #region porros
    public void OfrecerPorro()
    {
        //GameObject.Find("GameManager").GetComponent<GastosManager>().nuevoGasto("Lechuga", 5, false);
        NPCanim.SetBool("Ofrecer", true);
        Invoke(nameof(SpawnearPorro), 0.5f);
    }

    public void SpawnearPorro()
    {

        colider.enabled = false;

        for(int i = 0; i < weedSpawner.items.Length; i++) 
        {

            //print("vuelta: " + i + "esta active: " + weedSpawner.items[i].GetComponent<ItemAtributes>().active);

            if (!weedSpawner.items[i].GetComponent<ItemAtributes>().vendido)
            {

                print("SpawnPorro");
                weedSpawner.items[i].SetActive(true);
                weedSpawner.items[i].GetComponent<ItemAtributes>().vendido = false;
                weedSpawner.items[i].GetComponent<ItemAtributes>().active = true;
                weedSpawner.items[i].GetComponent<ItemAtributes>().ActivarItem();
                weedSpawner.items[i].GetComponent<Rigidbody>().isKinematic = true;
                weedSpawner.items[i].transform.position = posicionSpawnPorro.transform.position;
                weedSpawner.items[i].transform.rotation = posicionSpawnPorro.transform.rotation;
                weedSpawner.items[i].transform.SetParent(posicionSpawnPorro.transform);

                itemSpawneado = weedSpawner.items[i];

                npcDialogue.canTalk = false;

                esperandoRecojer = true;
                tiempoEsperandoRecojer = 0;

                break;
            }
        }
    }

    public int CalcularExistenciasPorros()
    {

        int existencias = 0;

        for (int i = 0; i < weedSpawner.items.Length; i++)
        {

            //print("vuelta: " + i + "esta active: " + weedSpawner.items[i].GetComponent<ItemAtributes>().active);

            if (!weedSpawner.items[i].GetComponent<ItemAtributes>().vendido)
            {
                existencias++;
            }
        }

        return existencias;
    }

    #endregion


    #region Pastillas
    public void OfrecerPastillas()
    {
        //GameObject.Find("GameManager").GetComponent<GastosManager>().nuevoGasto("Lechuga", 5, false);
        NPCanim.SetBool("Ofrecer", true);
        Invoke(nameof(SpawnearPastillas), 0.5f);
    }

    public void SpawnearPastillas()
    {

        colider.enabled = false;

        for (int i = 0; i < pastisSpawner.items.Length; i++)
        {

            //print("vuelta: " + i + "esta active: " + weedSpawner.items[i].GetComponent<ItemAtributes>().active);

            if (!pastisSpawner.items[i].GetComponent<ItemAtributes>().vendido)
            {

                print("SpawnPastillas");
                pastisSpawner.items[i].GetComponent<ItemAtributes>().vendido = false;
                pastisSpawner.items[i].GetComponent<ItemAtributes>().active = true;
                pastisSpawner.items[i].GetComponent<ItemAtributes>().ActivarItem();
                pastisSpawner.items[i].GetComponent<Rigidbody>().isKinematic = true;
                pastisSpawner.items[i].transform.position = posicionSpawnPorro.transform.position;
                pastisSpawner.items[i].transform.rotation = posicionSpawnPorro.transform.rotation;
                pastisSpawner.items[i].transform.SetParent(posicionSpawnPorro.transform);

                itemSpawneado = pastisSpawner.items[i];

                npcDialogue.canTalk = false;

                esperandoRecojer = true;
                tiempoEsperandoRecojer = 0;

                break;
            }
        }
    }

    public int CalcularExistenciasPastillas()
    {

        int existencias = 0;

        for (int i = 0; i < pastisSpawner.items.Length; i++)
        {

            //print("vuelta: " + i + "esta active: " + weedSpawner.items[i].GetComponent<ItemAtributes>().active);

            if (!pastisSpawner.items[i].GetComponent<ItemAtributes>().vendido)
            {
                existencias++;
            }
        }

        return existencias;
    }

    #endregion

    public void AbrirTienda()
    {
        sceneDialogsController.IniciarDialogo("dealer_shop", dondeMira, false, 4 ,1, false, "");
        sceneDialogsController.estabaFreezed = false;
    }

    public string idDialogoDia;
    public void Hablar()
    {
        sceneDialogsController.IniciarDialogo(idDialogoDia, dondeMira, false,4, 1, false, "");
        sceneDialogsController.estabaFreezed = false;
    }

    public void SinDinero()
    {
        sceneDialogsController.IniciarDialogo("dealer_noMoney", dondeMira, false,4, 1, false, "");
        sceneDialogsController.estabaFreezed = false;
    }

    public void SinExistencias()
    {
        sceneDialogsController.IniciarDialogo("dealer_noExistencias", dondeMira, false, 4, 1, false, ""); ;
        sceneDialogsController.estabaFreezed = false;
    }

    public int hits;

    public void RecibirHit()
    {
        CancelInvoke(nameof(UnfrezePos));
        print("HitDealer");
        npcAnim.SetTrigger("Hit");
        freezePos = true;
        Invoke(nameof(UnfrezePos), 1.5f);

        if (hits > 2)
        {
            Invoke(nameof(QuejarseHit), 1.5f);
            hits = 0;
        }
        else
        {
            hits++;
        }

    }

    public void QuejarseHit()
    {
        CancelInvoke(nameof(QuejarseHit));
        sceneDialogsController.IniciarDialogo("dealer_hit1", dondeMira, false,4, 1, false, "");
    }

    public void UnfrezePos()
    {
        CancelInvoke(nameof(UnfrezePos));
        freezePos = false;
    }

}
