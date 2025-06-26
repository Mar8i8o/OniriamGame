using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public bool punteroActivo;
    public Rigidbody rb;

    public float sensibilidad;


    public GameObject normalCursor;
    public GameObject selectCursor;

    public GameObject pantallaEncendida;

    public float limiteX_Left;
    public float limiteX_Right;
    public float limiteY_Top;
    public float limiteY_Down;

    void Start()
    {
        NormalCursor();
    }

    // Update is called once per frame
    void Update()
    {

        if (punteroActivo)
        {

            //print(transform.position.y);

            if (transform.position.x > limiteX_Left)
            {
                transform.Translate(Input.GetAxis("Mouse X") * sensibilidad * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.position = new Vector3(limiteX_Left + 0.1f, transform.position.y, transform.position.z);
            }

            if (transform.position.x < limiteX_Right)
            {
                transform.Translate(Input.GetAxis("Mouse X") * sensibilidad * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.position = new Vector3(limiteX_Right - 0.1f, transform.position.y, transform.position.z);
            }

            if (transform.position.y < limiteY_Top)
            {
                transform.Translate(0, Input.GetAxis("Mouse Y") * sensibilidad * Time.deltaTime, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, limiteY_Top - 0.1f, transform.position.z);
            }

            if (transform.position.y > limiteY_Down)
            {
                transform.Translate(0, Input.GetAxis("Mouse Y") * sensibilidad * Time.deltaTime, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, limiteY_Down + 0.1f, transform.position.z);
            }



            //rb.velocity = new Vector3(Input.GetAxis("Mouse X") * sensibilidad, Input.GetAxis("Mouse Y") * sensibilidad, 0);



        }



    }


    private void FixedUpdate()
    {
        
    }

    public void SelectCursor()
    {
        if (pantallaEncendida.activeSelf)
        {
            //print("Seleccionar");
            normalCursor.SetActive(false);
            selectCursor.SetActive(true);
        }
    }

    public void NormalCursor()
    {
        if (pantallaEncendida.activeSelf)
        {
            normalCursor.SetActive(true);
            selectCursor.SetActive(false);
        }
    }
}
