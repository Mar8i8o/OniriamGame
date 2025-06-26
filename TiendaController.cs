using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiendaController : MonoBehaviour
{
    public int indice;

    GameObject dondeSpawnea;


    public GameObject hamburguesaPrefab;
    public GameObject botellaPrefab;

    public GameObject pedidoCheckPrefab;
    public GameObject pedidoCheckNoDinero;
    public GameObject pedidoCheckParent;

    public PedidoSpawnerController pedidoSpawnerController;

    public bool blockPedidos;

    void Start()
    {
        dondeSpawnea = GameObject.Find("SpawnTienda");

        //SpawnearItems();

        //PlayerPrefs.DeleteAll();

        //pedidoSpawnerController.SetSpawnItems("VHSItem_Gato", dondeSpawnea, 10, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public void SpawnearItems()
    {
        for (int i = 0; i < hamburguesas; i++)
        {
            GameObject instancia = Instantiate(hamburguesaPrefab, dondeSpawnea.transform.position, Quaternion.identity);
            instancia.name = hamburguesaPrefab.name + "_" + i;

            instancia.GetComponent<ItemAtributes>().GetPosition();

        }
    }
    */

    public void PedidoCheck()
    {
        print("CHECK");
        GameObject instancia = Instantiate(pedidoCheckPrefab, pedidoCheckParent.transform.position, Quaternion.identity);
        instancia.transform.SetParent(pedidoCheckParent.transform);
    }

    public void SinDineroCheck()
    {
        print("CHECK");
        GameObject instancia = Instantiate(pedidoCheckNoDinero, pedidoCheckParent.transform.position, Quaternion.identity);
        instancia.transform.SetParent(pedidoCheckParent.transform);
    }

}
