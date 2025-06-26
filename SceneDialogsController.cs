using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SceneDialogsController : MonoBehaviour
{
    public GameObject panelDialogos;
    public JSONConverter dialogScript;
    public DialogeController dialogeController;

    public GameObject camara;
    public CamaraFP camaraScript;
    public Raycast raycast;

    public bool dialogueActive;
    public GameObject npc;
    GameObject player;

    public GameObject puntero;


    public bool hasAction;
    public int numAcciones;
    public GameObject accion1;
    public GameObject accion2;

    public bool estabaFreezed;
    
    void Start()
    {
        player = GameObject.Find("Player");
        raycast = GameObject.Find("Main Camera").GetComponent<Raycast>();
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();
    }

    // Update is called once per frame

    public float speedY;
    public float speedX;

    void LateUpdate()
    {

        if (dialogueActive)
        {

            //////////////////////////

            camaraScript.ForzarMiradaX(npc.transform, speedX);

            // Obtén la dirección hacia el objetivo
            Vector3 targetDirection2 = npc.transform.position - player.transform.position;
            targetDirection2.y = 0f; // Asegúrate de que no haya rotación vertical

            // Calcula la rotación necesaria para mirar hacia el objetivo solo en el eje Y
            Quaternion targetRotation2 = Quaternion.LookRotation(targetDirection2);

            // Ajusta la rotación para que solo afecte el eje Y
            targetRotation2 = Quaternion.Euler(0f, targetRotation2.eulerAngles.y, 0f);

            // Aplica la rotación de forma suavizada
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation2, speedY * Time.deltaTime);

        }
    }

    [HideInInspector] public NpcDialogue npcDialogue;

    bool tienePensamientoActual;
    string pensamientoActual;

    public void IniciarDialogo(string idDialogo, GameObject who, bool tieneAccion, float setSpeedX ,float setSpeedY, bool tienePensamiento, string pensamiento)
    {
        dialogScript.idDialogo = idDialogo;
        dialogScript.EmpezarDialogo();

        speedY = setSpeedY;
        speedX = setSpeedX;

        hasAction = tieneAccion;

        estabaFreezed = camaraScript.freeze;
        panelDialogos.SetActive(true);
        camaraScript.freezeCamera = true;
        camaraScript.freeze = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        npc = who;

        tienePensamientoActual = tienePensamiento;
        pensamientoActual = pensamiento;

        dialogueActive = true;

        if (hasAction) { SetAcciones();  }

        dialogeController.EmpezarDialogos();
        puntero.SetActive(false);
        raycast.blockRaycast = true;
    }

    void SetAcciones()
    {
        if (numAcciones == 1)
        {
            dialogeController.accion1 = accion1;
        }
        else if(numAcciones == 2)
        {
            dialogeController.accion1 = accion1;
            dialogeController.accion2 = accion2;
        }
    }

    PensamientoControler pensamientoControler;

    public void ReactivarMovimiento()
    {
        puntero.SetActive(true);
        camaraScript.freezeCamera = false;
        if(!estabaFreezed)camaraScript.freeze = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        dialogueActive = false;

        raycast.blockRaycast = false;

        if(npcDialogue != null) { npcDialogue.dialogoActivo = false; }

        if(tienePensamientoActual)
        {
            tienePensamientoActual = false;
            pensamientoControler.MostrarPensamiento(pensamientoActual, 2);
        }


    }
}
