using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonScreen : MonoBehaviour
{
    public Button button;
    public bool inButton;

    MouseController mouseController;
    Raycast playerScript;
    ComputerController computerController;

    public bool imputField;
    public GameObject parent;

    private void Awake()
    {
        mouseController = GameObject.Find("PunteroScreen").GetComponent<MouseController>();
        computerController = GameObject.Find("CanvasPantallaPc").GetComponent<ComputerController>();
        playerScript = GameObject.Find("Main Camera").GetComponent<Raycast>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inButton /*&& playerScript.tiempoUsandoPc > 0.2f*/) 
        {
            if (playerScript.usingComputer && computerController.tiempoPcEncendido > 0.2f)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!imputField)button.onClick.Invoke();
                    else
                    {
                        var eventSystem = EventSystem.current;
                        eventSystem.SetSelectedGameObject(parent);
                    }
                    inButton = false;
                    mouseController.NormalCursor();
                };
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Puntero"))
        {
            if (playerScript.usingComputer)
            {
                //print("select");
                if(!imputField)button.Select();
                mouseController.SelectCursor();

                inButton = true;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Puntero"))
        {
            if (playerScript.usingComputer)
            {
                if(!imputField)button.Select();
                mouseController.SelectCursor();

                inButton = true;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Puntero"))
        {
            if (playerScript.usingComputer)
            {
                Deselect();
            }
        }
    }

    public void Deselect()
    {
        if (playerScript.usingComputer)
        {
            //print("Error");
            inButton = false;

            mouseController.NormalCursor();
            if (!imputField)
            {
                GameObject myEventSystem = GameObject.Find("EventSystem");
                myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
            }
        }
    }
}
