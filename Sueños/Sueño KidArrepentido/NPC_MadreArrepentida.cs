using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_MadreArrepentida : MonoBehaviour
{

    public GameObject dondeMira;
    public SceneDialogsController sceneDialogsController;
    public Animator anim;

    public Raycast ray;

    public GameObject accionKarmaPositivo;
    public GameObject accionKarmaNegativo;
    public GameObject accionDespertarseSusto;

    public MoverNPC moverNPCMadre;
    public GameObject player;

    void Start()
    {
        HablarMadre();
    }

    void Update()
    {
        moverNPCMadre.ForzarMiradaY(player.transform, 2);
    }

    public void HablarMadre()
    {

        anim.SetBool("mirandoAlFrente", true);

        if(ray.hasObject)
        {
            if(ray.itemAtributes.isCarta)
            {
                DialogoPositivo();
            }
            else if(ray.itemAtributes.isNota) 
            {
                DialogoPositivo();
            }

        }
        else
        {
            DialogoNegativo();
        }
    }

    public GameObject accionLlorarMadre;

    public void DialogoPositivo()
    {
        sceneDialogsController.numAcciones = 2;
        sceneDialogsController.accion1 = accionKarmaPositivo;
        sceneDialogsController.accion2 = accionKarmaNegativo;
        sceneDialogsController.IniciarDialogo("madre_s1_1", dondeMira, true,0.8f, 0.1f, false, "");
    }

    public void DialogoNegativo()
    {
        sceneDialogsController.numAcciones = 2;
        sceneDialogsController.accion1 = accionLlorarMadre;
        sceneDialogsController.accion2 = accionDespertarseSusto;
        sceneDialogsController.IniciarDialogo("madre_s1_2", dondeMira, true,0.8f, 0.1f, false, "");
    }
}
