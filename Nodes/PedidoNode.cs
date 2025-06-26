using UnityEngine;
using XNode;

[CreateNodeMenu("Event/Pedido Event")]
public class PedidoNode : EventNode
{
    public string pedidoName; // Nombre del GameObject del pedido

    // Componentes necesarios
    private PedidoSpawnerController pedidoSpawnerController;
    private Transform dondeSpawnea;
    private ItemAtributes pedido;

    [System.NonSerialized] public bool pedidoEntregado = false;

    // Método llamado al cargar el script
    void Awake()
    {
        // Buscar el GameObject del pedido utilizando su nombre
        //pedido = GameObject.Find(pedidoName).GetComponent<ItemAtributes>();
        //dondeSpawnea = GameObject.Find("PuntosSpawnTienda").transform;
        //pedidoSpawnerController = GameObject.Find("PedidoController").GetComponent<PedidoSpawnerController>();
    }

    public void NodeAwake()
    {
        pedido = GameObject.Find(pedidoName).GetComponent<ItemAtributes>();
        dondeSpawnea = GameObject.Find("PuntosSpawnTienda").transform;
        pedidoSpawnerController = GameObject.Find("PedidoController").GetComponent<PedidoSpawnerController>();
    }

    // Método para ejecutar el evento del pedido
    public override void Execute()
    {
        base.Execute();

        if (!pedidoEntregado)
        {
            Debug.Log("Pedido enviado");
            NodeAwake();
            // Lógica para entregar el pedido
            pedidoSpawnerController.SetSpawnItems(pedidoName, dondeSpawnea.gameObject, 10, true);

            pedidoEntregado = true;
        }
    }

    // Inicializar el nodo con los componentes necesarios
    public void Initialize()
    {
        // Cargar el estado del pedido entregado desde PlayerPrefs
        pedidoEntregado = PlayerPrefs.GetInt(eventName + "pedidoEntregado", 0) == 1;
    }

    // Método para guardar el estado
    public override void GuardarEstado()
    {
        PlayerPrefs.SetInt(eventName + "pedidoEntregado", pedidoEntregado ? 1 : 0);
    }
}
