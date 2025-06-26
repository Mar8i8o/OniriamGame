using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarScreen : MonoBehaviour
{
    public bool inButton;
    public bool click;

    public bool movilBar;
    MouseController mouseController;

    //public GameObject area;

    public ScrollRect scrollRect;
    Raycast playerScript;


    public ScrollBarScreen scrollBarContigua;

    public bool tieneScrollBarContigua;
    public bool principal;
    public bool active;


    void Start()
    {
        mouseController = GameObject.Find("PunteroScreen").GetComponent<MouseController>();
        playerScript = GameObject.Find("Main Camera").GetComponent<Raycast>();

        //inButton = true;


    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.usingComputer)
        {

            if (inButton)
            {
                if (tieneScrollBarContigua)
                {
                    active = true;
                    scrollBarContigua.active = false;
                }


                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (tieneScrollBarContigua && !active) { return; }
                    scrollRect.normalizedPosition = new Vector2(0, scrollRect.normalizedPosition.y + Input.GetAxis("Mouse Y") * 50 * Time.deltaTime);
                }
            }

            if(tieneScrollBarContigua && !active) { return; }
            if (scrollRect != null)
            {
                scrollRect.normalizedPosition = new Vector2(0, scrollRect.normalizedPosition.y + Input.GetAxis("Vertical") * 2 * Time.deltaTime);
                scrollRect.normalizedPosition = new Vector2(0, scrollRect.normalizedPosition.y + Input.GetAxis("Mouse ScrollWheel") * 50 * Time.deltaTime);
            }
        }
        else if (playerScript.usingMovil && movilBar)
        {

            float inputV = Input.GetAxis("Vertical");
            if (inputV > 0.5) inputV = 0.5f;

            scrollRect.normalizedPosition = new Vector2(0, scrollRect.normalizedPosition.y + inputV * 5 * Time.deltaTime);

        }

        if (up)
        {
            //scrollRect.normalizedPosition = new Vector2(0, scrollRect.normalizedPosition.y + 100);
            //scrollRect.normalizedPosition = new Vector2(0, scrollRect.normalizedPosition.y + 50 * 5 * Time.deltaTime);
            if(scrollRect != null)scrollRect.normalizedPosition = new Vector2(0, 1);
            Invoke(nameof(DisableUp), 0.1f);
        }

        if(scrollRect != null)debugPanel = scrollRect.normalizedPosition;
    }

    public Vector2 debugPanel;

    public void DisableUp()
    {
        up = false;
        //active = false;
    }

    public bool up;
    public void OnEnable()
    {
        if (movilBar)
        {
            
        }
        if (principal && tieneScrollBarContigua) { active = true; scrollBarContigua.active = false; }
        //print("OPEN");
        //scrollRect.normalizedPosition = new Vector2(0, 0);
        if(scrollRect != null)scrollRect.verticalNormalizedPosition = 1f;
        up = true;
        //scrollRect.normalizedPosition = new Vector2(0, scrollRect.normalizedPosition.y + 100);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Puntero"))
        {
            //print("select");
            mouseController.SelectCursor();

            inButton = true;

            GameObject myEventSystem = GameObject.Find("EventSystem");
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Puntero"))
        {
            mouseController.SelectCursor();
            inButton = true;
        }

    }
        private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Puntero"))
        {

            inButton = false;

            mouseController.NormalCursor();

            GameObject myEventSystem = GameObject.Find("EventSystem");
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        }
    }
}
