using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBasura : MonoBehaviour
{
    public int cantidadBasura;

    public GameObject[] nivelesBasura;

    public ItemSpawnerController spawnerItemController;

    public GameObject puntoSpawn;

    [HideInInspector]public GameObject ultimaBasura;
    GuardarController guardarController;
    GameObject alijoDrogas;

    public bool isVacio;

    void Start()
    {
        cantidadBasura = PlayerPrefs.GetInt("CANTIDADBASURA", 0);

        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();

        alijoDrogas = GameObject.Find("AlijoDrogas");

        for (int i = 0; i < nivelesBasura.Length; i++)
        {
            if (i == cantidadBasura) nivelesBasura[i].SetActive(true);
            else { nivelesBasura[i].SetActive(false); }
        }

        //print("SpawnBasura");
        //spawnerItemController.SpawnearItem(puntoSpawn);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt("CANTIDADBASURA", cantidadBasura);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("PickableObject"))
        {        

            ItemAtributes itemAtributes;

            itemAtributes = other.gameObject.GetComponent<ItemAtributes>();

            if (itemAtributes.sePuedeTirar)
            {

                if (!isVacio)
                {
                    cantidadBasura++;

                    for (int i = 0; i < nivelesBasura.Length; i++)
                    {
                        if (i == cantidadBasura) nivelesBasura[i].SetActive(true);
                        else { nivelesBasura[i].SetActive(false); }
                    }

                    if (cantidadBasura == 8)
                    {
                        print("SpawnBasura");
                        spawnerItemController.SpawnearItem(puntoSpawn);
                        ultimaBasura.GetComponent<Collider>().isTrigger = true;
                        //ultimaBasura.GetComponent<Rigidbody>().AddForce(0, 50, -50);

                        Invoke(nameof(ActivarCollider), 0.2f);

                        cantidadBasura = 0;

                        for (int i = 0; i < nivelesBasura.Length; i++)
                        {
                            if (i == cantidadBasura) nivelesBasura[i].SetActive(true);
                            else { nivelesBasura[i].SetActive(false); }
                        }
                    }
                }

                if (!itemAtributes.isCigar && !itemAtributes.isPildoras)
                {
                    itemAtributes.active = false;
                    itemAtributes.DesactivarItem();
                    itemAtributes.transform.position = Vector3.zero;
                    //itemAtributes.GuardarPosicion();

                    if (itemAtributes.isWater) { itemAtributes.RellenarAgua(); }
                    else if (itemAtributes.isPizza) { itemAtributes.ResetearPizza(); }
                }
                else
                {

                    other.gameObject.transform.position = alijoDrogas.transform.position;

                    if (itemAtributes.isCigar) { itemAtributes.ResetearCigarro(); }
                    else if (itemAtributes.isPildoras) { itemAtributes.ResetearPastillas(); }
                }
                
            }
            else //SE REPELE LA BASURA CON OTRO SCRIPT
            {
                if (isVacio) 
                {
                    itemAtributes.gameObject.transform.position = itemAtributes.posicionInicial;
                    itemAtributes.gameObject.transform.eulerAngles = itemAtributes.rotacionInicial;
                }
            }
        }
    }

    public void ActivarCollider()
    {
        ultimaBasura.GetComponent<Collider>().isTrigger = false;
    }

}
