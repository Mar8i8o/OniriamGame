using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicadorAtrapasuenos : MonoBehaviour
{
    public float distancia;

    public GameObject bulletHole;

    public LayerMask mask;

    public GameObject indicadorAtrapaSuenos;
    public GameObject indicadorNota;
    public GameObject indicadorLienzo;
    public GameObject lienzoPoint;
    //public GameObject atrapasuenosObj;

    PensamientoControler pensamientoControler;

    Raycast raycast;
    GuardarController guardarController;

    public int atrapasuenosVida;
    public int atrapasuenosResistencia;

    public bool puedeColocar;

    private void Awake()
    {
        //render.material.SetTexture("_BaseMap", texture);

        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>(); 
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>(); 

        raycast = GameObject.Find("Main Camera").GetComponent<Raycast>();
        //indicadorAtrapaSuenos.SetActive(false);

        atrapasuenosVida = PlayerPrefs.GetInt("atrapasuenosVida", atrapasuenosVida);
        atrapasuenosResistencia = PlayerPrefs.GetInt("atrapasuenosResistencia", atrapasuenosResistencia);

    }
    void Update()
    {
        if (puedeColocar)
        {
            ColocarObjeto();
            if (!raycast.hasObject)
            {
                indicadorAtrapaSuenos.SetActive(false);
                indicadorNota.SetActive(false);
                indicadorLienzo.SetActive(false);
            }
        }

        //print("Esta guardando: " + guardarController.guardando);
        if(guardarController.guardando)
        {
            GuardarDatos();
        }

    }

    public void GuardarDatos()
    {
        print("Guardar datos atrapasueños");
        PlayerPrefs.SetInt("atrapasuenosVida", atrapasuenosVida);
        PlayerPrefs.SetInt("atrapasuenosResistencia", atrapasuenosResistencia);
    }

    public void ReactivarCanPickUp()
    {
        raycast.canPickUp = true;
    }

    public void ColocarObjeto()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distancia, mask))
        {
            if (hit.collider.tag == "ParedAtrapasuenos")
            {
                raycast.CursorSeleccion();

                if (raycast.hasObject)
                {
                    if (raycast.itemAtributes.isAtrapasuenos || raycast.itemAtributes.isPrint || raycast.itemAtributes.isNota || raycast.itemAtributes.isLienzo)
                    {

                        if (raycast.itemAtributes.isAtrapasuenos) indicadorAtrapaSuenos.SetActive(true);
                        else if (raycast.itemAtributes.isPrint || raycast.itemAtributes.isNota) indicadorNota.SetActive(true);
                        else if (raycast.itemAtributes.isLienzo) indicadorLienzo.SetActive(true);
                        //indicadorAtrapaSueños.transform.position = Vector3.MoveTowards(indicadorAtrapaSueños.transform.position, hit.point, 0.1f);
                        indicadorAtrapaSuenos.transform.position = hit.point;
                        indicadorNota.transform.position = hit.point;
                        indicadorLienzo.transform.position = hit.point;

                        if (Input.GetKeyDown(KeyCode.Mouse0) && raycast.tiempoConObjeto > 0.2f) //COLOCAR OBJETO PARED
                        {
                            if (raycast.itemAtributes.isLienzo)
                            {
                                raycast.itemAtributes.gameObject.transform.position = lienzoPoint.transform.position + new Vector3(0, -0.178f, 0);
                                raycast.itemAtributes.gameObject.transform.eulerAngles = indicadorLienzo.transform.eulerAngles + new Vector3(-90, 0, 0);
                            }
                            else
                            {
                                raycast.itemAtributes.gameObject.transform.position = hit.point + new Vector3(0, -0.178f, 0);

                                if (raycast.itemAtributes.isAtrapasuenos) //COLOCAR ATRAPASUEÑOS
                                {
                                    raycast.itemAtributes.gameObject.transform.rotation = indicadorAtrapaSuenos.transform.rotation;

                                }
                                else
                                {
                                    raycast.itemAtributes.gameObject.transform.rotation = indicadorLienzo.transform.rotation;
                                }

                            }
                            raycast.canPickUp = false;
                            Invoke(nameof(ReactivarCanPickUp), 0.3f);
                            raycast.ForzarSoltarObjeto();
                            raycast.itemAtributes.rb.isKinematic = true;
                            raycast.itemAtributes.clavado = true;

                            if (raycast.itemAtributes.isAtrapasuenos) //APLICAR ID ATRAPASUEÑOS (DESPUES DE SOLTRARLO)
                            {
                                if (raycast.itemAtributes.idEspecial == "Vida")
                                {
                                    atrapasuenosVida++;
                                    pensamientoControler.MostrarPensamiento("Esto me dará mas salud en mis sueños", 2);
                                }
                                else if (raycast.itemAtributes.idEspecial == "Resistencia")
                                {
                                    atrapasuenosResistencia++;
                                    pensamientoControler.MostrarPensamiento("Esto me dará mas resistencia en mis sueños", 2);
                                }
                            }
                        }
                    }
                }
                else //SI NO TIENES NADA EN LA MANO, DICE QUE PUEDE PONER ALGO
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if(!pensamientoControler.mostrandoPensamiento)pensamientoControler.MostrarPensamiento("En esta pared podría colgar algo", 1);
                    }
                }

            }
            else
            {
                indicadorAtrapaSuenos.SetActive(false);
                indicadorNota.SetActive(false);
                indicadorLienzo.SetActive(false);
            }
        }
        else
        {
            indicadorAtrapaSuenos.SetActive(false);
            indicadorNota.SetActive(false);
            indicadorLienzo.SetActive(false);
        }
    }
}
