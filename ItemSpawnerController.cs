using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerController : MonoBehaviour
{

    public GuardarController guardarController;

    public GameObject prefabItem;
    public GameObject[] items;

    public int max;

    public int itemsRestantes;

    public int pedidosActivos;

    public bool isLienzo;
    public bool isBasura;

    PintarLienzos pintarLienzos;

    public TriggerBasura basuraScript;

    public bool debug;

    void Awake()
    {
        pintarLienzos = GameObject.Find("GameManager").GetComponent<PintarLienzos>();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();

        pedidosActivos = PlayerPrefs.GetInt(gameObject.name + "PEDIDOSACTIVOS", pedidosActivos);

        items = new GameObject[max];

        if (isLienzo) pintarLienzos.lienzos = new GameObject[max];

        for (int i = 0; i < max; i++) 
        {
            GameObject instancia = Instantiate(prefabItem, transform.position, Quaternion.identity);
            instancia.name = prefabItem.name + "_" + i;
            instancia.GetComponent<ItemAtributes>().GetPosition();


            items[i] = instancia;
            if (isLienzo) pintarLienzos.lienzos[i] = instancia;
            
        }

    }

    public ItemAtributes itemRecienSpawneado;

    public void SpawnearItem(GameObject donde)
    {

        RecuentoItems();

        for (int i = 0;i < max;i++) 
        {
            if (!items[i].GetComponent<ItemAtributes>().active) 
            {
                items[i].SetActive(true);
                items[i].transform.position = donde.transform.position;
                items[i].transform.rotation = donde.transform.rotation;
                items[i].GetComponent<ItemAtributes>().active = true;
                items[i].GetComponent<ItemAtributes>().ActivarItem();
                itemRecienSpawneado = items[i].GetComponent<ItemAtributes>();

                if (isBasura) { basuraScript.ultimaBasura = items[i].gameObject; }

                break;

            }
        }

        if (isBasura && itemsRestantes == 0) 
        {
            for (int i = 0; i < max; i++)
            {
                if (items[i].GetComponent<ItemAtributes>().active)
                {
                    items[i].transform.position = donde.transform.position;
                    items[i].GetComponent<ItemAtributes>().active = true;
                    items[i].GetComponent<ItemAtributes>().ActivarItem();

                    if (isBasura) { basuraScript.ultimaBasura = items[i].gameObject; }

                    break;

                }
            }
        }

    }

    public void RecuentoItems()
    {
        itemsRestantes = 0;

        for (int i = 0; i < max; i++)
        {
            if (!items[i].GetComponent<ItemAtributes>().active)
            {
                itemsRestantes++;
            }
        }
    }

    void Update()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt(gameObject.name + "PEDIDOSACTIVOS", pedidosActivos);
            if (debug) { print("ForzarGuardar"); }
            for (int i = 0; i < max; i++)
            {
                items[i].GetComponent<ItemAtributes>().GuardarPosicion();
                if (debug) { print("ForzarGuardarPosicion" + items[i].gameObject.name); }
            }

        }
    }

    private void LateUpdate()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}
