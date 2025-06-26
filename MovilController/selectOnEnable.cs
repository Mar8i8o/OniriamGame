using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class selectOnEnable : MonoBehaviour
{
    public ButtonMovilController buttonMovilController;
    private void OnEnable()
    {

            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(gameObject);
            buttonMovilController.selecionado = true;

    }

}
