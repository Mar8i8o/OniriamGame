using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImpresoraController : MonoBehaviour
{

    public ItemAtributes imagenAImprimir;
    public GameObject impresionPoint;
    public GameObject puntoHojaSalida;

    public bool imprimiendo;
    public float tiempoImprimiendo;

    public float distancia;

    CorreosManager correosManager;
    NoticiasManager noticiasManager;

    void Start()
    {
        correosManager = GameObject.Find("GameManager").GetComponent<CorreosManager>();
        noticiasManager = GameObject.Find("GameManager").GetComponent<NoticiasManager>();
    }

    void Update()
    {
        if (imprimiendo) 
        {
            imagenAImprimir.transform.position = Vector3.MoveTowards(imagenAImprimir.transform.position, puntoHojaSalida.transform.position, 0.0005f);

            tiempoImprimiendo += Time.deltaTime;

            distancia = Vector3.Distance(puntoHojaSalida.transform.position, imagenAImprimir.gameObject.transform.position);

            if (distancia <= 0)
            {
                imagenAImprimir.active = true;
                imagenAImprimir.ActivarItem();
                imprimiendo = false;
            }
        }
    }

    public void Imprimir(Image image)
    {
        //print(image.sprite.name);

        if (!imprimiendo)
        {

            imagenAImprimir = GameObject.Find(image.sprite.name + "_Impress").GetComponent<ItemAtributes>();

            if (!imagenAImprimir.active)
            {
                imagenAImprimir.transform.position = impresionPoint.transform.position;
                imagenAImprimir.transform.rotation = impresionPoint.transform.rotation;
                imagenAImprimir.mesh.SetActive(true);
                imprimiendo = true;

                //correosManager.ImpressNotification();
                ImpressNot();
            }
            else
            {
                correosManager.CantImpress();
                CantImpressNot();
            }
        }
        else
        {
            WaitNot();
        }
        
    }

    /*
    public void ImprimirNoticia(Image image)
    {
        //print(image.sprite.name);
        imagenAImprimir = GameObject.Find(image.sprite.name + "_Impress").GetComponent<ItemAtributes>();

        if (!imagenAImprimir.active)
        {
            imagenAImprimir.transform.position = impresionPoint.transform.position;
            imagenAImprimir.transform.rotation = impresionPoint.transform.rotation;
            imagenAImprimir.mesh.SetActive(true);
            imprimiendo = true;

            //noticiasManager.ImpressNotification();
            ImpressNot();
        }
        else
        {
            //noticiasManager.CantImpress();
            CantImpressNot();
        }

    }
    */

    public GameObject checkPosition;

    public GameObject cantImpressPrefab;
    public GameObject ImpressPrefab;
    public GameObject waitPrefab;
    public void CantImpressNot()
    {
        GameObject instancia = Instantiate(cantImpressPrefab, checkPosition.transform.position, Quaternion.identity);
        instancia.transform.SetParent(checkPosition.transform);
    }

    public void ImpressNot()
    {
        GameObject instancia = Instantiate(ImpressPrefab, checkPosition.transform.position, Quaternion.identity);
        instancia.transform.SetParent(checkPosition.transform);
    }

    public void WaitNot()
    {
        GameObject instancia = Instantiate(waitPrefab, checkPosition.transform.position, Quaternion.identity);
        instancia.transform.SetParent(checkPosition.transform);
    }
}
