using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerCafetera : MonoBehaviour
{
    public bool encendido;
    public bool consumiendoCafe;

    public Animator cafeteraAnim;
    public Animator chorroCafeAnim;

    public GameObject cafe;

    public float cantidadCafe;

    public float offsetY;

    PensamientoControler pensamientoControler;

    public ItemAtributes cafeInterior;

    public bool tieneTaza;

    public GameObject posicionTaza;

    public bool canUse;

    GuardarController guardarController;

    public AudioSource cafeteraSound;
    float startVolume;

    void Start()
    {
        startVolume = cafeteraSound.volume;
        if (PlayerPrefs.GetInt("TieneTaza", System.Convert.ToInt32(tieneTaza)) == 0) { tieneTaza = false; }
        else { tieneTaza = true; }

        ApagarAnim();

        cantidadCafe = PlayerPrefs.GetFloat("CantidadCafeCafetera", 10);

        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        cafeteraAnim.SetBool("Encendido", encendido);
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();

        canUse = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (consumiendoCafe) 
        {
            //cafe.transform.Translate(0, 0, -0.00001f);

            cantidadCafe -= Time.deltaTime/2;
            cafeInterior.cafeController.cantidadCafe += Time.deltaTime;
            cafeInterior.cafeController.caliente = 100;

            if (cafeInterior.cafeController.cantidadCafe >= 10)
            {
                ApagarCafetera();
                canUse = false;
            }
        }

        if (tieneTaza)
        {
            print("Moviendo Taza");
            cafeInterior.gameObject.transform.position = Vector3.MoveTowards(cafeInterior.gameObject.transform.position, posicionTaza.transform.position, 0.09f);
            cafeInterior.gameObject.transform.rotation = posicionTaza.transform.rotation;

            if (cafeInterior.cafeController.cantidadCafe >= 10)
            {
                canUse = false;
            }
        }

        cafe.transform.localPosition = new Vector3(cafe.transform.localPosition.x, (cantidadCafe / 17) - offsetY, cafe.transform.localPosition.z);

    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt("TieneTaza", System.Convert.ToInt32(tieneTaza));
            PlayerPrefs.SetFloat("CantidadCafeCafetera", cantidadCafe);
        }
    }

    public void ApagarCafetera()
    {
        encendido = false;
        consumiendoCafe = false;
        cafeteraAnim.SetBool("Encendido", false);
        chorroCafeAnim.SetBool("Cayendo", false);

        Invoke(nameof(ApagarAnim),2);

        //cafeteraSound.Stop();
        StartCoroutine(FadeOut());
    }

    public void SoltarTaza()
    {
        tieneTaza = false;
        canUse = true;
        ApagarCafetera();
    }

    public void InteractuarCafetera()
    {
        if (tieneTaza)
        {
            if (cantidadCafe > 0)
            {
                if (encendido)
                {
                    ApagarCafetera();

                }
                else if (!encendido)
                {
                    encendido = true;
                    Invoke(nameof(ActivarConsumiendoCafe), 1.5f);
                    EncenderAnim();
                    cafeteraAnim.SetBool("Encendido", true);
                    chorroCafeAnim.SetBool("Cayendo", true);
                    cafeteraSound.volume = startVolume;
                    cafeteraSound.Play();
                }
            }
            else
            {
                pensamientoControler.MostrarPensamiento("No queda cafe", 1);
            }
        }
        else
        {
            pensamientoControler.MostrarPensamiento("Necesito poner una taza antes", 1);
        }
    }

    IEnumerator FadeOut()
    {
        // Obtiene el volumen inicial

        // Reduce gradualmente el volumen a cero durante el tiempo especificado
        while (cafeteraSound.volume > 0)
        {
            cafeteraSound.volume -= startVolume * Time.deltaTime / 0.4f;
            yield return null;
        }

        // Detiene la reproducción después de que el volumen llega a cero
        cafeteraSound.Stop();
    }

    public void ActivarConsumiendoCafe()
    {
        consumiendoCafe = true;
    }

    public void ColocarCafe(ItemAtributes cafe)
    {
        cafeInterior = cafe;
        tieneTaza = true;
    }

    void ApagarAnim()
    {
        cafeteraAnim.enabled = false;
        chorroCafeAnim.enabled = false;
    }

    void EncenderAnim()
    {
        CancelInvoke(nameof(ApagarAnim));
        cafeteraAnim.enabled = true;
        chorroCafeAnim.enabled = true;
    }
}
