using UnityEngine;
using UnityEngine.EventSystems;

public class SeleccionarOnEnable : MonoBehaviour
{

    public GameObject opcionSeleccionar;

    private void OnEnable()
    {
        print("Seleccionar on enable");
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(opcionSeleccionar);
    }
}
