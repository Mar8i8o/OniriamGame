using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedidoSpawnerController : MonoBehaviour
{
    public GameObject prefabPedido;

    public float tiempo;

    public GameObject[] items;

    public int max;

    public int itemsRestantes;

    public int itesmActivos;

    public TutorialController tutorialController;

    void Start()
    {

        items = new GameObject[max];

        for (int i = 0; i < max; i++)
        {
            GameObject instancia = Instantiate(prefabPedido, transform.position, Quaternion.identity);
            instancia.name = prefabPedido.name + "_" + i;
            //instancia.GetComponent<PedidoController>().GetPosition();
            items[i] = instancia;
        }


    }

    public PedidoController ultimoPedido;

    public void SpawnearPedido(ItemSpawnerController itemPedido , GameObject dondeSpawnea, float tiempoPedido, bool tieneCaja, string idCaja, bool empaquetado, string idEspecial)
    {

        RecuentoItems();

        for (int i = 0; i < max; i++)
        {
            if (!items[i].GetComponent<PedidoController>().active)
            {
                items[i].transform.position = transform.position;
                items[i].GetComponent<PedidoController>().gameObject.SetActive(true);
                items[i].GetComponent<PedidoController>().nameItemASpawnear = itemPedido.gameObject.name;
                items[i].SetActive(true);
                items[i].GetComponent<PedidoController>().active = true;
                items[i].GetComponent<PedidoController>().usaName = false;
                items[i].GetComponent<PedidoController>().scriptItemASpawnear = itemPedido;
                items[i].GetComponent<PedidoController>().retraso = tiempoPedido;
                items[i].GetComponent<PedidoController>().dondeSpawnea = dondeSpawnea;
                items[i].GetComponent<PedidoController>().tieneCaja = tieneCaja;
                items[i].GetComponent<PedidoController>().idCaja = idCaja;
                items[i].GetComponent<PedidoController>().idEspecial = idEspecial;
                items[i].GetComponent<PedidoController>().tienePaquete = empaquetado;
                ultimoPedido = items[i].GetComponent<PedidoController>();

                break;
            }
        }

        RecuentoItems();

    }

    public void SetSpawnItems(string itemPedidoName, GameObject dondeSpawnea, float tiempoPedido, bool tieneCaja) //PEDIDO PERSONALIZADO PARA EVENTOS
    {
        RecuentoItems();

        ItemAtributes itemPedido = GameObject.Find(itemPedidoName).GetComponent<ItemAtributes>();

        for (int i = 0; i < max; i++)
        {
            if (!items[i].GetComponent<PedidoController>().active)
            {
                items[i].transform.position = transform.position;
                items[i].GetComponent<PedidoController>().gameObject.SetActive(true);
                items[i].GetComponent<PedidoController>().nameItemASpawnear = itemPedidoName;
                items[i].GetComponent<PedidoController>().usaName = true;
                items[i].SetActive(true);
                items[i].GetComponent<PedidoController>().active = true;
                //items[i].GetComponent<PedidoController>().scriptItemASpawnear = itemPedido;
                items[i].GetComponent<PedidoController>().retraso = tiempoPedido;
                items[i].GetComponent<PedidoController>().dondeSpawnea = dondeSpawnea;
                items[i].GetComponent<PedidoController>().tieneCaja = tieneCaja;
                ultimoPedido = items[i].GetComponent<PedidoController>();

                break;
            }
        }

        RecuentoItems();
    }

    public void RecuentoItems()
    {
        itemsRestantes = 0;

        for (int i = 0; i < max; i++)
        {
            if (!items[i].GetComponent<PedidoController>().active)
            {
                itemsRestantes++;
            }
        }
        itesmActivos = max - itemsRestantes;
    }

    void Update()
    {
        
    }
}
