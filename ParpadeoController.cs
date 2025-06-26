using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParpadeoController : MonoBehaviour
{
    public GameObject parpadoArriba;
    public GameObject parpadoAbajo;

    MostrarControles mostrarControles;

    public float velocidad;

    Vector3 posicionParpadoArriba;
    Vector3 posicionParpadoAbajo;

    public Vector3 posicionMinParpadoArriba;
    public Vector3 posicionMinParpadoAbajo;

    public bool cerrandoOjos;
    public bool puedesAbrirlos;

    public bool abrirOjos;

    public bool beingKO;

    public bool spawnAbriendoOjos;
    
    DreamController dreamController;

    GuardarController guardarController;
    void Start()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        dreamController = GameObject.Find("GameManager").GetComponent<DreamController>();

        mostrarControles = GameObject.Find("MostrarControlesController").GetComponent<MostrarControles>();
        //mostrarControles.MostrarControlesText("Pulsa ESPACIO para mantenerte despierto", 5);

        posicionParpadoAbajo = parpadoAbajo.transform.position;
        posicionParpadoArriba = parpadoArriba.transform.position;

        if (!dreamController.inDream)
        {
            if (PlayerPrefs.GetInt("SpawnAbrirOjos", System.Convert.ToInt32(spawnAbriendoOjos)) == 0) { spawnAbriendoOjos = false; }
            else { spawnAbriendoOjos = true; }
        }

        if (spawnAbriendoOjos) 
        {
            parpadoAbajo.transform.position = new Vector3(parpadoAbajo.transform.position.x, 327, parpadoAbajo.transform.position.z);
            parpadoArriba.transform.position = new Vector3(parpadoArriba.transform.position.x, 696, parpadoArriba.transform.position.z);

            Invoke(nameof(AbrirOjosRetard), 0.5f);
            spawnAbriendoOjos = false;
        }
    }

    public void AbrirOjosRetard()
    {
        SetAbrirOjos(200);
    }

    // Update is called once per frame
    void Update()
    {           
        if (cerrandoOjos)
        {
            if (puedesAbrirlos)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (parpadoArriba.transform.position.y < posicionParpadoArriba.y)
                    {
                        //print("AbriendoOjos");
                        parpadoAbajo.transform.Translate(0, -velocidad * Time.deltaTime * 100, 0);
                        parpadoArriba.transform.Translate(0, velocidad * Time.deltaTime * 100, 0);
                    }
                }
                else
                {
                    CerrarOjos();
                }
            }
            else
            {
                CerrarOjos();
            }
        }
        else if (abrirOjos) 
        {
            if (parpadoArriba.transform.localPosition.y < 530)
            {
                parpadoAbajo.transform.Translate(0, velocidad * Time.deltaTime, 0);
                parpadoArriba.transform.Translate(0, -velocidad * Time.deltaTime, 0);
            }
            else
            {
                abrirOjos = false;
                spawnAbriendoOjos = false;
            }

        }

     //print(parpadoArriba.transform.localPosition.y);
    }

    void CerrarOjos()
    {
        if (parpadoArriba.transform.localPosition.y > -200)
        {
            parpadoAbajo.transform.Translate(0, velocidad * Time.deltaTime, 0);
            parpadoArriba.transform.Translate(0, -velocidad * Time.deltaTime, 0);
        }
        else
        {
            if (!beingKO)
            {
                //print("Dormido");
            }
        }
    }

    public void SetCerrarOjos(float parpadoVelocidad)
    {
        //ResetOjos();

        print("CerrarOjos");
        cerrandoOjos = true;
        abrirOjos = false;
        puedesAbrirlos = false;
        velocidad = parpadoVelocidad;
    }

    public void SetAbrirOjos(float parpadoVelocidad)
    {
        print("AbrirOjos");
        cerrandoOjos = false;
        puedesAbrirlos = false;
        abrirOjos = true;
        velocidad = -parpadoVelocidad;
    }

    public void ResetOjos()
    {
        parpadoAbajo.transform.position = posicionParpadoAbajo;
        parpadoArriba.transform.position = posicionParpadoArriba;
    }

    public void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt("SpawnAbrirOjos", System.Convert.ToInt32(spawnAbriendoOjos));
        }
    }
}
