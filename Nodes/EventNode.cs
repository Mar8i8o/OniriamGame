using UnityEngine;
using XNode;

[CreateNodeMenu("Event/Basic Event")]
public class EventNode : Node
{
    public string eventName;
    //public string eventDescription;
    //public Sprite eventImage;
    public int eventHour; // Hora en la que debe ocurrir el evento (0-23)
    public int eventMinute; // Minuto en el que debe ocurrir el evento (0-59)
    [System.NonSerialized] public bool processed = false; // Indica si el evento ya ha sido procesado

    [System.NonSerialized] public float eventTime; // Tiempo en segundos en el que debe ocurrir el evento

    // Método para calcular eventTime basado en eventHour y eventMinute
    public void CalculateEventTime()
    {
        eventTime = eventHour * 3600 + eventMinute * 60;
    }

    // Método virtual para ejecutar el evento
    public virtual void Execute()
    {
        Debug.Log("Evento ejecutado: " + eventName);
    }

    // Método virtual para guardar el estado del nodo
    public virtual void GuardarEstado()
    {
        // Implementación por defecto (puede ser vacía o guardar un estado común)
    }
}
