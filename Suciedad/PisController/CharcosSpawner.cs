using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharcosSpawner : MonoBehaviour
{
    public GameObject prefabPedido;

    public float tiempo;

    public GameObject[] items;

    public int max;

    public int charcosRestantes;

    public GameObject pisPosition;

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

    public void SpawnearCharco()
    {

        RecuentoItems();

        for (int i = 0; i < max; i++)
        {
            if (!items[i].GetComponent<CharcosPisController>().active)
            {
                items[i].gameObject.SetActive(true);
                items[i].transform.position = pisPosition.transform.position;
                items[i].GetComponent<CharcosPisController>().active = true;
                items[i].GetComponent<CharcosPisController>().suciedad = 0.8f;
                items[i].GetComponent<CharcosPisController>().suciedadScale = 1f;
                items[i].GetComponent<CharcosPisController>().RandomRotation();
                //items[i].GetComponent<CharcosPisController>().GuardarDatos();
                items[i].SetActive(true);
                items[i].GetComponent<CharcosPisController>().IniciarAnimacion();
                items[i].GetComponent<CharcosPisController>().Activar();

                break;
            }
        }

    }

    public void RecuentoItems()
    {
        charcosRestantes = 0;

        for (int i = 0; i < max; i++)
        {
            if (!items[i].GetComponent<CharcosPisController>().active)
            {
                charcosRestantes++;
            }
        }
    }

    void Update()
    {

    }
}
