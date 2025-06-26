using UnityEngine;
using XNode;

[CreateNodeMenu("Event/Letter Event")]
public class LetterEventNode : EventNode
{
    public string cartaAEntregarName; // Nombre del GameObject de la carta

    // EventosEntregarCartas y otros componentes necesarios
    private EventosEntregarCartas eventosEntregarCartas;
    private GameObject cartaAEntregar;

    [System.NonSerialized] public bool cartaEnviada = false;

    // M�todo llamado cuando el script est� siendo cargado
    void Awake()
    {
        // Buscar el GameObject de la carta utilizando su nombre
        cartaAEntregar = GameObject.Find(cartaAEntregarName);
    }

    public void NodeAwake()
    {
        cartaAEntregar = GameObject.Find(cartaAEntregarName);
    }

    // M�todo para ejecutar el evento de carta
    public override void Execute()
    {
        base.Execute();

        if (!cartaEnviada)
        {
            Debug.Log("Carta enviada");

            // L�gica para entregar la carta
            if (cartaAEntregar != null && eventosEntregarCartas != null)
            {
                eventosEntregarCartas.EntregarCarta(cartaAEntregar.GetComponent<ItemAtributes>());
            }

            cartaEnviada = true;
        }
    }

    // Inicializar el nodo con los componentes necesarios
    public void Initialize(EventosEntregarCartas eventos)
    {
        eventosEntregarCartas = eventos;

        // Cargar el estado de la carta enviada desde PlayerPrefs
        cartaEnviada = PlayerPrefs.GetInt(eventName + "cartaEnviada", 0) == 1;
    }

    // Sobrescribir el m�todo GuardarEstado para guardar el estado del nodo
    public override void GuardarEstado()
    {
        PlayerPrefs.SetInt(eventName + "cartaEnviada", cartaEnviada ? 1 : 0);
    }
}
