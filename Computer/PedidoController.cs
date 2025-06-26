using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedidoController : MonoBehaviour
{
    public GameObject dondeSpawnea;
    Transform[] puntosSpawn;
    //public GameObject prefabPedido;
    public float retraso;
    public bool tieneCaja;
    public bool tienePaquete;
    public string idCaja;
    public string idEspecial;

    ItemSpawnerController spawnerCajasPeque;
    ItemSpawnerController spawnerCajasMed;
    ItemSpawnerController spawnerCajasGran;

    public TiendaController tiendaController;

    public ItemSpawnerController scriptItemASpawnear;
    public string nameItemASpawnear;

    public bool active;

    GuardarController guardarController;
    DoorController doorController;
    DetectarPlayer detectarPlayer;

    public bool usaName;

    private void OnEnable()
    {
    }
    private void Awake()
    {
        detectarPlayer = GameObject.Find("TrigerDetectarPlayerFuera").GetComponent<DetectarPlayer>();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        doorController = GameObject.Find("TriggerPomoPrincipal").GetComponent<DoorController>();
        spawnerCajasPeque = GameObject.Find("CajasPeqController").GetComponent<ItemSpawnerController>();
        spawnerCajasMed = GameObject.Find("CajasMedianasController").GetComponent<ItemSpawnerController>();
        spawnerCajasGran = GameObject.Find("CajasController").GetComponent<ItemSpawnerController>();

        dondeSpawnea = GameObject.Find("PuntosSpawnTienda");
        puntosSpawn = dondeSpawnea.GetComponentsInChildren<Transform>();

    }

    private void Start()
    {
        tiendaController = GameObject.Find("GameManager").GetComponent<TiendaController>();
        GetDatos();
    }

    void Update()
    {
        if (active)
        {
            if(!tiendaController.blockPedidos)retraso -= Time.deltaTime;

            if (retraso <= 0 && !detectarPlayer.playerDentro) //SPAWNEA EL ITEM
            {
                SpawnearItem();
            }

            GuardarDatos();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public ItemSpawnerController itemcajaActual;
    public void SpawnearItem()
    {

        int random = Random.Range(0, puntosSpawn.Length);

        if (tieneCaja)
        {

            if(idCaja == "cajaPeq")
            {
                itemcajaActual = spawnerCajasPeque;
            }
            if(idCaja == "cajaMed")
            {
                itemcajaActual = spawnerCajasMed;
            }
            if (idCaja == "cajaGran")
            {
                itemcajaActual = spawnerCajasGran;
            }

            itemcajaActual.SpawnearItem(puntosSpawn[random].gameObject);

            if (!usaName) //SPAWNEA EL ITEM DE FORMA NORMAL
            {
                scriptItemASpawnear.SpawnearItem(itemcajaActual.itemRecienSpawneado.cajaController.puntoSpawnItemsCaja);
                scriptItemASpawnear.pedidosActivos--;
                scriptItemASpawnear.itemRecienSpawneado.idEspecial = idEspecial;

                if (tienePaquete)
                {
                    scriptItemASpawnear.itemRecienSpawneado.empaquetado = true;
                    scriptItemASpawnear.itemRecienSpawneado.Empaquetar();
                }

                itemcajaActual.itemRecienSpawneado.cajaController.contenidoCajonController.anyadirObjetoCajon(scriptItemASpawnear.itemRecienSpawneado);


            }
            else //SPAWNEA EL ITEM DE FORMA FORZADA, SIN USAR EL ITEMSPAWNERCONTROLLER
            {
                ItemAtributes item = GameObject.Find(nameItemASpawnear).GetComponent<ItemAtributes>();
                item.active = true;
                item.gameObject.transform.position = itemcajaActual.itemRecienSpawneado.cajaController.puntoSpawnItemsCaja.transform.position;

                item.idEspecial = idEspecial;

                if (tienePaquete)
                {
                    item.empaquetado = true;
                    item.Empaquetar();
                }

                itemcajaActual.itemRecienSpawneado.cajaController.contenidoCajonController.anyadirObjetoCajon(item);

            }

            
            itemcajaActual.itemRecienSpawneado.cajaController.DesactivarColiders();

        }
        else
        {
            if (!usaName) //SPAWNEA EL ITEM DE FORMA NORMAL
            {
                scriptItemASpawnear.SpawnearItem(puntosSpawn[random].gameObject);
                scriptItemASpawnear.pedidosActivos--;

                scriptItemASpawnear.itemRecienSpawneado.idEspecial = idEspecial;

                if (tienePaquete)
                {
                    scriptItemASpawnear.itemRecienSpawneado.empaquetado = true;
                    scriptItemASpawnear.itemRecienSpawneado.Empaquetar();
                }

            }
            else //SPAWNEA EL ITEM DE FORMA FORZADA, SIN USAR EL ITEMSPAWNERCONTROLLER
            {
                ItemAtributes item = GameObject.Find(nameItemASpawnear).GetComponent<ItemAtributes>();
                item.active = true;
                item.gameObject.transform.position = puntosSpawn[random].gameObject.transform.position;

                item.idEspecial = idEspecial;

                if (tienePaquete)
                {
                    item.empaquetado = true;
                    item.Empaquetar();
                }
            }
        }

        DesactivarItem();
        //Destroy(gameObject);
        doorController.TocarTimbre();

        //dondeSpawnea.transform.position = posicionOriginal;

        print("EntregarPedido");
    }

    public void GuardarDatos()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetFloat(gameObject.name + "retraso", retraso);
            PlayerPrefs.SetInt(gameObject.name + "active", System.Convert.ToInt32(active));
            PlayerPrefs.SetInt(gameObject.name + "usaName", System.Convert.ToInt32(usaName));
            PlayerPrefs.SetString(gameObject.name + "nameItem", nameItemASpawnear);
        }
    }

    public void GetDatos()
    {
        if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(active)) == 0) { active = false; }
        else { active = true; }

        if (active)
        {
            retraso = PlayerPrefs.GetFloat(gameObject.name + "retraso", retraso);
            nameItemASpawnear = PlayerPrefs.GetString(gameObject.name + "nameItem", nameItemASpawnear);

            if (PlayerPrefs.GetInt(gameObject.name + "usaName", System.Convert.ToInt32(usaName)) == 0) { usaName = false; }
            else { usaName = true; }

            scriptItemASpawnear = GameObject.Find(nameItemASpawnear).GetComponent<ItemSpawnerController>();

            ActivarItem();
        }
        else
        {
            DesactivarItem();
        }

    }

    public void DesactivarItem()
    {
        active = false;
        GuardarDatos();
    }

    public void ActivarItem()
    {
        active = true;
    }
}
