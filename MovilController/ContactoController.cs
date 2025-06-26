using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ContactoController : MonoBehaviour
{
    public ButtonMovilController buttonMovilControler;

    public string usuario;
    public string numero;
    public bool active;
    public bool contesta;
    public float tiempoEnContestar;
    public string idDialogo;   

    public TextMeshProUGUI contactoTXT;
    public MovilController movilController;

    GuardarController guardarController;

    public bool dialogoUnico;


    private void Awake()
    {
       
    }
    void Start()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        GetDatos();
        if (!active) gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        contactoTXT.text = usuario;
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            //GuardarDatos();
        }
    }

    private void OnEnable()
    {
        if (gameObject.transform.GetSiblingIndex() == 0)
        {
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(gameObject);
            buttonMovilControler.selecionado = true;
        }
    }
    public void GuardarDatos()
    {
        PlayerPrefs.SetInt(gameObject.name + "active", System.Convert.ToInt32(active));
        PlayerPrefs.SetInt(gameObject.name + "contesta", System.Convert.ToInt32(contesta));
    }

    public void GetDatos()
    {
        if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(active)) == 0) { active = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(active)) == 1) { active = true; }

        if (PlayerPrefs.GetInt(gameObject.name + "contesta", System.Convert.ToInt32(contesta)) == 0) { contesta = false; }
        else if (PlayerPrefs.GetInt(gameObject.name + "contesta", System.Convert.ToInt32(contesta)) == 1) { contesta = true; }
    }

    public void AbrirContacto()
    {
        movilController.AbrirContacto(usuario, numero, idDialogo, contesta, tiempoEnContestar, gameObject.GetComponent<ContactoController>());
    }
}
