using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestilloController : MonoBehaviour
{
    public DoorController doorController;
    public Animator anim;
    public bool pestilloAbierto;

    public bool generaPensamiento;
    public string pensamiento;

    GuardarController guardarController;
    void Start()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();

        if (PlayerPrefs.GetInt(gameObject.name + "pestillo", System.Convert.ToInt32(pestilloAbierto)) == 0) { pestilloAbierto = false; }
        else { pestilloAbierto = true; }

        EncenderAnim();

        anim.SetBool("Open", pestilloAbierto);

        Invoke(nameof(ApagarAnim), 2);

        if (!pestilloAbierto && generaPensamiento) 
        {
            doorController.generaPensamiento = true;
            doorController.pensamiento = pensamiento;
        }
    }

    void Update()
    {
        if (!doorController.entreAbierta)
        {
            doorController.sePuedeAbrir = pestilloAbierto;
        }
    }

    private void LateUpdate()
    {
        if(guardarController.guardando)
        {
            GuardarDatos();
        }
    }

    public void GuardarDatos()
    {
        PlayerPrefs.SetInt(gameObject.name + "pestillo", System.Convert.ToInt32(pestilloAbierto));
    }
    public void AbrirPestillo()
    {
        EncenderAnim();

        pestilloAbierto = !pestilloAbierto;
        anim.SetBool("Open", pestilloAbierto);

        if (!doorController.entreAbierta)
        {
            doorController.sePuedeAbrir = pestilloAbierto;
        }

        if (!pestilloAbierto && generaPensamiento)
        {
            doorController.generaPensamiento = true;
            doorController.pensamiento = pensamiento;
        }

        Invoke(nameof(ApagarAnim), 2);
    }

    public void SetAbrirPestillo()
    {

        EncenderAnim();

        pestilloAbierto = true;
        anim.SetBool("Open", true);

        doorController.sePuedeAbrir = true;

        Invoke(nameof(ApagarAnim), 2);
    }

    void ApagarAnim()
    {
        anim.enabled = false;
    }

    void EncenderAnim()
    {
        CancelInvoke(nameof(ApagarAnim));
        anim.enabled = true;
    }
}
