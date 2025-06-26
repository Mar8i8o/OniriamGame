using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class accionIrse : MonoBehaviour
{
    public NPC_Visita npc_Visita;
    void Start()
    {
        npc_Visita.SetMarcharse();
    }

}
