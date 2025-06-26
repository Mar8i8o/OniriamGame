using UnityEngine;
using XNode;

[CreateNodeMenu("Event/Email Event")]
public class EmailEventNode : EventNode
{
    //public string sender;
    //public string subject;
    //public string body;
    public string correoParaEnviarName; // Nombre del GameObject del correo

    // EventosCorreos y otros componentes necesarios
    private EventosCorreos eventosCorreos;
    private GameObject chekCorreo;
    private GameObject chekCorreoPosition;
    private GameObject correoParaEnviar;

    [System.NonSerialized] public bool mensajeEnviado = false;

    // M�todo llamado cuando el script est� siendo cargado
    void Awake()
    {
        // Buscar el GameObject del correo utilizando su nombre
        correoParaEnviar = GameObject.Find(correoParaEnviarName);
        Debug.Log("CORREO ENCONTRADO: " +correoParaEnviar);
    }

    public void NodeAwake()
    {
        correoParaEnviar = GameObject.Find(correoParaEnviarName);
        Debug.Log("CORREO ENCONTRADO: " + correoParaEnviar);
    }

    // M�todo para ejecutar el evento de correo
    public override void Execute()
    {
        base.Execute();

        if (!mensajeEnviado)
        {
            Debug.Log("Email enviado: " + eventName);
            //Debug.Log($"Correo de {sender} con asunto {subject}");
            //Debug.Log($"Cuerpo del correo: {body}");

            // L�gica para enviar el correo
            if (correoParaEnviar != null && eventosCorreos != null)
            {
                eventosCorreos.EnviarCorreo(correoParaEnviar);
            }

            // Instanciar el objeto de check correo
            if (chekCorreo != null && chekCorreoPosition != null)
            {
                GameObject instancia = GameObject.Instantiate(chekCorreo, chekCorreoPosition.transform.position, Quaternion.identity);
                instancia.transform.SetParent(chekCorreoPosition.transform);
            }

            mensajeEnviado = true;
        }
    }

    // Inicializar el nodo con los componentes necesarios
    public void Initialize(EventosCorreos eventos, GameObject check, GameObject checkPosition)
    {
        eventosCorreos = eventos;
        chekCorreo = check;
        chekCorreoPosition = checkPosition;

        // Cargar el estado del mensaje enviado desde PlayerPrefs
        mensajeEnviado = PlayerPrefs.GetInt(eventName + "correoEnviado", 0) == 1;
    }

    // Sobrescribir el m�todo GuardarEstado para guardar el estado del nodo
    public override void GuardarEstado()
    {
        PlayerPrefs.SetInt(eventName + "correoEnviado", mensajeEnviado ? 1 : 0);
    }
}
