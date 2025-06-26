using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancinController : MonoBehaviour
{
    public SofaController sofaController;
    public bool balanceandose;
    public Animator anim;
    public Animator kidAnim;

    public NpcDialogue npcDialogue;

    bool recienSentado;

    void Start()
    {
        recienSentado = true;
        kidAnim.SetBool("Balancin", true);
    }

    void Update()
    {
        balanceandose = sofaController.sentadoSofa;
        anim.SetBool("Balanceandose", balanceandose);

        if(sofaController.sentadoSofa && recienSentado) 
        {
            recienSentado = false;
            npcDialogue.idDialogo = "nino_s1_2";
            npcDialogue.unicaVez = true;
            npcDialogue.tienePensamiento = true;
            npcDialogue.pensamiento = "Deberia ir a buscar esa carta";
        }

        if(npcDialogue.idDialogo == "nino_s1_2" && !sofaController.sentadoSofa)
        {
            npcDialogue.idDialogo = "nino_s1_1";
            npcDialogue.unicaVez = false;
            npcDialogue.tienePensamiento = false;
        }

    }
}
