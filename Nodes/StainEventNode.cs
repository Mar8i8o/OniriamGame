using UnityEngine;
using XNode;

[CreateNodeMenu("Event/Stain Event")]
public class StainEventNode : EventNode
{
    public string manchaParaEnviarName; // Nombre del GameObject de la mancha

    // EventosMensajes y otros componentes necesarios
    private EventosMensajes eventosMensajes;
    private GameObject manchaParaEnviar;

    [System.NonSerialized] public bool manchaSpawneada = false;

    // Método llamado cuando el script está siendo cargado
    void Awake()
    {
        // Buscar el GameObject de la mancha utilizando su nombre
        manchaParaEnviar = GameObject.Find(manchaParaEnviarName);
    }

    public void NodeAwake()
    {
        manchaParaEnviar = GameObject.Find(manchaParaEnviarName);
    }

    // Método para ejecutar el evento de mancha
    public override void Execute()
    {
        base.Execute();

        if (!manchaSpawneada)
        {
            Debug.Log("Mancha enviada");

            // Lógica para activar la mancha
            if (manchaParaEnviar != null)
            {
                ManchaParedController manchaController = manchaParaEnviar.GetComponent<ManchaParedController>();
                if (manchaController != null)
                {
                    manchaController.Activar();
                }
            }

            manchaSpawneada = true;
        }
    }

    // Inicializar el nodo con los componentes necesarios
    public void Initialize(EventosMensajes eventos)
    {
        eventosMensajes = eventos;

        // Cargar el estado de la mancha enviada desde PlayerPrefs
        manchaSpawneada = PlayerPrefs.GetInt(eventName + "manchaSpawned", 0) == 1;
    }

    // Sobrescribir el método GuardarEstado para guardar el estado del nodo
    public override void GuardarEstado()
    {
        PlayerPrefs.SetInt(eventName + "manchaSpawned", manchaSpawneada ? 1 : 0);
    }
}
