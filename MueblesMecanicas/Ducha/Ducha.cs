using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ducha : MonoBehaviour
{
    public bool isPuerta;
    public bool seRellena;

    public bool puertaAbierta;
    public bool palancaAbierta;

    public Animator anim;

    public ParticleSystem particulasDucha;

    GuardarController guardarController;

    public Collider triggerAgua;
    public TriggerBeberAgua scriptAgua;

    public AudioSource sonido;

    void Start()
    {

        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();

        if (isPuerta)
        {
            if (PlayerPrefs.GetInt(gameObject.name + "puertaAbierta", System.Convert.ToInt32(puertaAbierta)) == 0) { puertaAbierta = false; }
            else if (PlayerPrefs.GetInt(gameObject.name + "puertaAbierta", System.Convert.ToInt32(puertaAbierta)) == 1) { puertaAbierta = true; }
        }

        if (isPuerta)
        {
            if (!puertaAbierta)
            {
                ApagarAnim();

                anim.SetBool("Open", false);

            }
            else
            {
                EncenderAnim();

                anim.SetBool("Open", true);

                Invoke(nameof(ApagarAnim), 3);
            }
        }
        else
        {
            if (!palancaAbierta)
            {
                ApagarAnim();

                anim.SetBool("Open", false);
                if(seRellena)triggerAgua.enabled = false;
                particulasDucha.Stop();

            }
            else
            {
                EncenderAnim();

                anim.SetBool("Open", true);
                if(seRellena)triggerAgua.enabled = true;
                particulasDucha.Play();

                Invoke(nameof(ApagarAnim), 3);
            }
        }
    }


    private void LateUpdate()
    {
        if (isPuerta)
        {
            if (guardarController.guardando)
            {
                PlayerPrefs.SetInt(gameObject.name + "puertaAbierta", System.Convert.ToInt32(puertaAbierta));
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Usar()
    {
        if (isPuerta) 
        {
            if (puertaAbierta)
            {
                EncenderAnim();

                anim.SetBool("Open", false);
                puertaAbierta = false;

                Invoke(nameof(ApagarAnim), 3);
            }
            else
            {
                EncenderAnim();

                anim.SetBool("Open", true);
                puertaAbierta = true;

                Invoke(nameof(ApagarAnim), 3);
            }
        }
        else
        {
            if (palancaAbierta)
            {
                if (scriptAgua != null)
                {
                    if (!scriptAgua.llenandoBotella)
                    {
                        EncenderAnim();

                        anim.SetBool("Open", false);
                        palancaAbierta = false;
                        if (seRellena) triggerAgua.enabled = false;
                        particulasDucha.Stop();
                        if (sonido != null) sonido.Stop();

                        Invoke(nameof(ApagarAnim), 3);

                    }
                }
                else
                {

                    EncenderAnim();

                    anim.SetBool("Open", false);
                    palancaAbierta = false;
                    if (seRellena) triggerAgua.enabled = false;
                    particulasDucha.Stop();
                    if (sonido != null) sonido.Stop();

                    Invoke(nameof(ApagarAnim), 3);

                }
            }
            else
            {

                EncenderAnim();

                anim.SetBool("Open", true);
                palancaAbierta = true;
                if(seRellena)triggerAgua.enabled = true;
                particulasDucha.Play();
                if(sonido != null )sonido.Play();

                Invoke(nameof(ApagarAnim), 3);

            }
        }

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
