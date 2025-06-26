using UnityEngine;

public class BuscarObjetosPorLayer : MonoBehaviour
{
    public GameObject[] todos;
    void Start()
    {
        int layerId = LayerMask.NameToLayer("EspejoVolume");

        if (layerId == -1)
        {
            Debug.LogError("El layer 'EspejoVolum' no existe.");
            return;
        }

        GameObject[] todos = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject go in todos)
        {
            if (go.layer == layerId)
            {
                Debug.Log("Encontrado: " + go.name, go);
            }
        }
    }
}
