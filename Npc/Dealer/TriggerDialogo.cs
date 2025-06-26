using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogo : MonoBehaviour
{
    public string idDialogo;

    public NpcDialogue npcDialogue;

    public AudioSource audioSusto;
    public bool daSusto;

    public bool usado;

    public bool cambiaCanTalk;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (!usado)
            {
                usado = true;
                if(daSusto) { audioSusto.Play(); }
                if (cambiaCanTalk) { npcDialogue.canTalk = true; }
                npcDialogue.SetAbrirDialogo(idDialogo);
            }
        }
    }

}
