using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyScript : MonoBehaviour
{
    //public GameObject prefabItemEnVenta;
    public PedidoSpawnerController pedidoController;
    public ItemSpawnerController itemSpawnerController;
    GameObject dondeSpawnea;

    public float precio;
    public float retraso;

    public bool empaquetado;
    public bool tieneCaja;
    public string idCaja;

    public string nombreGasto;

    public string idEspecial;

    PlayerStats playerStats;
    public TiendaController tiendaController;

    public TextMeshProUGUI textoPrecio;
    public GameObject buyButton;
    public GameObject outOfStockButton;

    public GameObject posicionPedidoCheck;

    GastosManager gastosManager;

    void Start()
    {
        gastosManager = GameObject.Find("GameManager").GetComponent<GastosManager>(); 
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
        tiendaController = GameObject.Find("GameManager").GetComponent<TiendaController>();
        dondeSpawnea = GameObject.Find("SpawnTienda");

    }


    public void RecuentoDeItems()
    {
        itemSpawnerController.RecuentoItems();

        itemsRestantes = itemSpawnerController.itemsRestantes - itemSpawnerController.pedidosActivos;

        if (itemsRestantes > 0) { buyButton.SetActive(true); outOfStockButton.SetActive(false); }
        else { buyButton.SetActive(false); outOfStockButton.SetActive(true); }
    }

    int itemsRestantes;

    private void LateUpdate()
    {
        RecuentoDeItems();
        textoPrecio.text = "Pedir por " + precio + "$";
    }

    public void Comprar()
    {
        RecuentoDeItems();

        if (itemsRestantes > 0)
        {
            if ((playerStats.dinero - precio) >= 0)
            {
                playerStats.dinero -= precio;

                gastosManager.nuevoGasto(nombreGasto, precio, false);

                itemSpawnerController.pedidosActivos++;

                pedidoController.SpawnearPedido(itemSpawnerController, dondeSpawnea, retraso, tieneCaja, idCaja, empaquetado, idEspecial);

                tiendaController.pedidoCheckParent = posicionPedidoCheck;
                tiendaController.PedidoCheck();
            }
            else
            {
                tiendaController.pedidoCheckParent = posicionPedidoCheck;
                tiendaController.SinDineroCheck();
            }

        }

        RecuentoDeItems();

    }
}
