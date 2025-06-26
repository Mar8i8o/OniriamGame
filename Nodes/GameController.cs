using UnityEngine;

public class GameController : MonoBehaviour
{
    public TimeController timeController;
    public DayGraph[] dayGraphs; // Array de grafos de días
    public EventosCorreos eventosCorreos;
    public LlamadasController eventosLlamadas;
    public NoticiasManager noticiasManager;
    public ControladorVisitas controladorVisitas;
    public EventosMensajes eventosMensajes;
    public EventosEntregarCartas eventosEntregarCartas;
    public GameObject chekCorreo;
    public GameObject chekCorreoPosition;
    public GuardarController guardarController;
    public GameObject panelLlamada; // Panel de llamada

    private DayGraph currentDayGraph;

    private void Awake()
    {

        LoadDayGraph();

        foreach (EventNode node in currentDayGraph.nodes)
        {
            // Inicializar los nodos con los componentes necesarios
            if (node is EmailEventNode emailNode)
            {
                emailNode.NodeAwake();
            }
            else if (node is PhoneCallEventNode phoneCallNode)
            {
                phoneCallNode.NodeAwake();
            }
            else if (node is NewsEventNode newsNode)
            {
                newsNode.NodeAwake();
            }
            else if (node is NPCEventNode npcNode)
            {
                npcNode.NodeAwake();
            }
            else if (node is StainEventNode stainNode)
            {
                stainNode.NodeAwake();
            }
            else if (node is LetterEventNode letterNode)
            {
                letterNode.NodeAwake();
            }
            else if (node is PedidoNode pedidoNode)
            {
                pedidoNode.NodeAwake();
            }

        }
    }

    void Start()
    {
        // Configurar la referencia de TimeController al GameController
        //timeController.gameController = this;


    }

    void Update()
    {
        float currentTime = timeController.totalSegundos;

        foreach (EventNode node in currentDayGraph.nodes)
        {
            node.CalculateEventTime();

            // Inicializar los nodos con los componentes necesarios
            if (node is EmailEventNode emailNode)
            {
                emailNode.Initialize(eventosCorreos, chekCorreo, chekCorreoPosition);
            }
            else if (node is PhoneCallEventNode phoneCallNode)
            {
                phoneCallNode.Initialize(eventosLlamadas, panelLlamada);
            }
            else if (node is NewsEventNode newsNode)
            {
                newsNode.Initialize(noticiasManager);
            }
            else if (node is NPCEventNode npcNode)
            {
                npcNode.Initialize(controladorVisitas);
            }
            else if (node is StainEventNode stainNode)
            {
                stainNode.Initialize(eventosMensajes);
            }
            else if (node is LetterEventNode letterNode)
            {
                letterNode.Initialize(eventosEntregarCartas);
            }
            else if (node is PedidoNode pedidoNode)
            {
                pedidoNode.Initialize();
            }

            // Ejecutar el nodo si es el tiempo adecuado
            if (currentTime >= node.eventTime && !node.processed)
            {
                node.Execute(); // Ejecuta el evento del nodo
                node.processed = true; // Marca el evento como procesado
            }
        }
    }

    void LoadDayGraph()
    {
        int currentDia = timeController.dia;
        if (currentDia > 0 && currentDia <= dayGraphs.Length)
        {
            currentDayGraph = dayGraphs[currentDia - 1]; // Ajusta el índice para que empiece por el día 1
        }
        else
        {
            Debug.LogWarning("Día fuera de rango. Asegúrate de que dayGraphs tiene suficiente longitud.");
        }
    }

    // Método para actualizar el grafo del día cuando cambia el día
    public void ActualizarDia()
    {
        LoadDayGraph();
    }

    // Método para guardar el estado de los nodos
    public void GuardarEstadoNodos()
    {
        foreach (DayGraph dayGraph in dayGraphs)
        {
            foreach (EventNode node in dayGraph.nodes)
            {
                node.GuardarEstado(); // Llama a GuardarEstado en todos los nodos
            }
        }
    }
}
