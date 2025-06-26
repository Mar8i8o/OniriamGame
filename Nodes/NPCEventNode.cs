using UnityEngine;
using XNode;

[CreateNodeMenu("Event/NPC Event")]
public class NPCEventNode : EventNode
{

    // ControladorVisitas y otros componentes necesarios
    private ControladorVisitas controladorVisitas;
    public GameObject npcEnviar;

    [System.NonSerialized] public bool eventoEnviado = false;

    // Método llamado cuando el script está siendo cargado
    void Awake()
    {
        // Buscar el GameObject del NPC utilizando su nombre
        //npcEnviar = GameObject.Find(npcEnviarName);
    }

    public void NodeAwake()
    {

    }

    // Método para ejecutar el evento del NPC
    public override void Execute()
    {
        base.Execute();

        if (!eventoEnviado)
        {
            Debug.Log("NPC enviado");

            // Lógica para generar la visita del NPC
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

    // Sobrescribir el método GuardarEstado para guardar el estado del nodo
    public override void GuardarEstado()
    {
        PlayerPrefs.SetInt(eventName + "NPCLlamado", eventoEnviado ? 1 : 0);
    }
}
