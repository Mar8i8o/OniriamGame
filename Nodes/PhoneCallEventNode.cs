using UnityEngine;
using XNode;

[CreateNodeMenu("Event/Phone Call Event")]
public class PhoneCallEventNode : EventNode
{
    public string nombreUsuarioLlamada;
    public string idDialogo;
    public bool blockColgar;

    // Componentes necesarios
    private LlamadasController eventosLlamadas;
    private GameObject panelLlamada;

    private bool llamadaEnviada = false;

    public void NodeAwake()
    {

    }

    // M�todo para ejecutar el evento de llamada
    public override void Execute()
    {
        base.Execute();

        if (!llamadaEnviada && !panelLlamada.activeSelf)
        {
            Debug.Log("Llamada enviada");
            Debug.Log($"Llamada de {nombreUsuarioLlamada} con di�logo {idDialogo}");

            // L�gica para manejar la llamada
            if (blockColgar) eventosLlamadas.bloquearColgar = true;
            else eventosLlamadas.bloquearColgar = false;

            eventosLlamadas.GenerarRecibirLlamada(nombreUsuarioLlamada, idDialogo);
            llamadaEnviada = true;
        }
    }

    // Inicializar el nodo con los componentes necesarios
    public void Initialize(LlamadasController eventos, GameObject panel)
    {
        eventosLlamadas = eventos;
        panelLlamada = panel;

        // Cargar el estado de la llamada enviada desde PlayerPrefs
        llamadaEnviada = PlayerPrefs.GetInt(eventName + "llamadaRealizada", 0) == 1;


        //Debug.Log("La llamada ha sido enviada?:" + llamadaEnviada);

    }

    // Sobrescribir el m�todo GuardarEstado para guardar el estado del nodo
    public override void GuardarEstado()
    {
        PlayerPrefs.SetInt(eventName + "llamadaRealizada", llamadaEnviada ? 1 : 0);
    }
}
