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

    // M�todo para calcular eventTime basado en eventHour y eventMinute
    public void CalculateEventTime()
    {
        eventTime = eventHour * 3600 + eventMinute * 60;
    }

    // M�todo virtual para ejecutar el evento
    public virtual void Execute()
    {
        Debug.Log("Evento ejecutado: " + eventName);
    }

    // M�todo virtual para guardar el estado del nodo
    public virtual void GuardarEstado()
    {
        // Implementaci�n por defecto (puede ser vac�a o guardar un estado com�n)
    }
}
