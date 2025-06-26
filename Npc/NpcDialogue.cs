using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDialogue : MonoBehaviour
{   
    SceneDialogsController sceneDialogsController;

    public string idDialogo;
    public GameObject dondeMira;

    public bool canTalk;
    public bool miraPlayer;
    public MoverNPC moverNPC;

    public int vecesHablado;

    public bool isPolicia;
    public NpcPolicia npcPolicia;

    public bool tieneAccion;
    public int acciones;

    public GameObject accion1;
    public GameObject accion2;

    public bool unicaVez;
    public string idDialogo2;

    [HideInInspector]public float speedY;
    [HideInInspector]public float speedX;
    [HideInInspector] public bool hablado;

    public bool dialogoActivo;

    GameObject player;




    void Awake()
    {
        sceneDialogsController = GameObject.Find("GameManager").GetComponent<SceneDialogsController>();

        if (isPolicia ) {npcPolicia = GetComponent<NpcPolicia>(); }

        speedY = 1;
        speedX = 4;

        player = GameObject.Find("Player");

    }

    void Update()
    {
        if(dialogoActivo && miraPlayer)
        {
            moverNPC.ForzarMiradaY(player.transform, 1);
        }
    }

    public void AbrirDialogo()
    {
        if (canTalk)
        {
            if (!sceneDialogsController.dialogueActive)
            {
                if(tieneAccion) 
                {
                    SetAcciones();
                }

                sceneDialogsController.IniciarDialogo(idDialogo, dondeMira, tieneAccion, speedX, speedY, tienePensamiento, pensamiento);
                tienePensamiento = false;
                sceneDialogsController.npcDialogue = this;
                vecesHablado++;
                hablado = true;

                if (isPolicia) { npcPolicia.SetMarcharse(); }
                if (unicaVez) { idDialogo = idDialogo2; }

                dialogoActivo = true;

            }
        }
    }

    public bool tienePensamiento;
    public string pensamiento;

    public void SetAbrirDialogo(string dialogo)
    {
        if (canTalk)
        {
            if (!sceneDialogsController.dialogueActive)
            {
                sceneDialogsController.IniciarDialogo(dialogo, dondeMira, tieneAccion,speedX, speedY, tienePensamiento, pensamiento);
                sceneDialogsController.npcDialogue = this;
                vecesHablado++;

                if (isPolicia) { npcPolicia.SetMarcharse(); }

                dialogoActivo = true;

            }
        }
    }

    void SetAcciones()
    {

        sceneDialogsController.numAcciones = acciones;

        if (acciones == 1)
        {
            sceneDialogsController.accion1 = accion1;
        }
        else if (acciones == 2)
        {
            sceneDialogsController.accion1 = accion1;
            sceneDialogsController.accion2 = accion2;
        }
    }
}
