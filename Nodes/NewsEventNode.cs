using UnityEngine;
using XNode;

[CreateNodeMenu("Event/News Event")]
public class NewsEventNode : EventNode
{
    public string noticiaParaEnviarName; // Nombre del GameObject de la noticia

    // NoticiasManager y otros componentes necesarios
    private NoticiasManager noticiasManager;
    private NoticiasController noticiaParaEnviar;
    private GameObject noticiaParaEnviarGO;

    [System.NonSerialized] public bool noticiaEnviada = false;

    // Método llamado cuando el script está siendo cargado
    void Awake()
    {
        // Buscar el GameObject de la noticia utilizando su nombre
        noticiaParaEnviarGO = GameObject.Find(noticiaParaEnviarName);
        Debug.Log("Noticia encontrada" + noticiaParaEnviarGO);
    }

    public void NodeAwake()
    {
        // Buscar el GameObject de la noticia utilizando su nombre
        noticiaParaEnviarGO = GameObject.Find(noticiaParaEnviarName);
        Debug.Log("Noticia encontrada" + noticiaParaEnviarGO);
    }

    // Método para ejecutar el evento de noticia
    public override void Execute()
    {
        base.Execute();

        if (!noticiaEnviada)
        {
            Debug.Log("Noticia enviada");


            noticiaParaEnviar = noticiaParaEnviarGO.GetComponent<NoticiasController>();

            // Lógica para enviar la noticia
            if (noticiaParaEnviar != null && noticiasManager != null)
            {
                noticiasManager.SetNoticia(noticiaParaEnviar);
            }

            noticiaEnviada = true;
        }
    }

    // Inicializar el nodo con los componentes necesarios
    public void Initialize(NoticiasManager noticias)
    {
        noticiasManager = noticias;

        // Cargar el estado de la noticia enviada desde PlayerPrefs
        noticiaEnviada = PlayerPrefs.GetInt(eventName + "noticiaEnviada", 0) == 1;
    }

    // Sobrescribir el método GuardarEstado para guardar el estado del nodo
    public override void GuardarEstado()
    {
        PlayerPrefs.SetInt(eventName + "noticiaEnviada", noticiaEnviada ? 1 : 0);
    }
}
