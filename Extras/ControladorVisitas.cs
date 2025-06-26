using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorVisitas : MonoBehaviour
{

    public GameObject puntoSpawn;

    public GameObject prefabNPC_1;

    NpcDialogue npcDialogue;
    NPC_Visita npcVisita;
    GameObject npcActual;
    void Start()
    {
        //GenerarVisita(prefabNPC_1);
    }

    void Update()
    {
        
    }

    public void GenerarVisita(GameObject npc)
    {
        npcActual = Instantiate(npc, puntoSpawn.transform.position, Quaternion.identity);

        npcDialogue = npcActual.GetComponent<NpcDialogue>();
        npcVisita = npcActual.GetComponent<NPC_Visita>();     

    }
}
