using UnityEngine;
using XNode;

[CreateNodeMenu("Event/NPC Event")]
public class NPCEventNode : EventNode
{

    // ControladorVisitas y otros componentes necesarios
    private ControladorVisitas controladorVisitas;
    public GameObject npcEnviar;

    [System.NonSerialized] public bool eventoEnviado = false;

    // M�todo llamado cuando el script est� siendo cargado
    void Awake()
    {
        // Buscar el GameObject del NPC utilizando su nombre
        //npcEnviar = GameObject.Find(npcEnviarName);
    }

    public void NodeAwake()
    {

    }

    // M�todo para ejecutar el evento del NPC
    public override void Execute()
    {
        base.Execute();

        if (!eventoEnviado)
        {
            Debug.Log("NPC enviado");

            // L�gica para generar la visita del NPC
            if (npcEnviar != null && controladorVisitas != null)
            {
                controladorVisitas.GenerarVisita(npcEnviar);
            }

            eventoEnviado = true;
        }
    }

    // Inicializar el nodo con los componentes necesarios
    public void Initialize(ControladorVisitas visitas)
    {
        controladorVisitas = visitas;

        // Cargar el estado del evento enviado desde PlayerPrefs
        eventoEnviado = PlayerPrefs.GetInt(eventName + "NPCLlamado", 0) == 1;
    }

    // Sobrescribir el m�todo GuardarEstado para guardar el estado del nodo
    public override void GuardarEstado()
    {
        PlayerPrefs.SetInt(eventName + "NPCLlamado", eventoEnviado ? 1 : 0);
    }
}
