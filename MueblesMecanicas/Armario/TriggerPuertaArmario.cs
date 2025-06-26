using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPuertaArmario : MonoBehaviour
{
    public Animator animPuerta;

    public bool puertaAbierta;

    public bool puertaEntreAbierta;

    public Renderer rend;

    public Camera miCamara;

    public GameObject enemy;

    public int aleatorio;

    public GuardarController guardarController;

    public bool isVisible;

    public AudioSource sonidoSusto;

    void Start()
    {

        if (PlayerPrefs.GetInt(gameObject.name + "puertaAbierta", System.Convert.ToInt32(puertaAbierta)) == 0) { puertaAbierta = false; }
        else { puertaAbierta = true; }

        if (PlayerPrefs.GetInt(gameObject.name + "entreAbierta", System.Convert.ToInt32(puertaEntreAbierta)) == 0) { puertaEntreAbierta = false; }
        else { puertaEntreAbierta = true; }

        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();

        animPuerta.SetBool("PuertaEntreAbierta", puertaEntreAbierta);
        animPuerta.SetBool("PuertaAbierta", puertaAbierta);

        miCamara = GameObject.Find("Main Camera").GetComponent<Camera>();
        
    }

    void Update()
    {
        /*
        isVisible = IsVisibleFromCamera(miCamara);

        if (puertaEntreAbierta) 
        {
            //print("Puerta Es Visible:" + isVisible);
            if (Time.frameCount % 10 == 0)
            {
                animPuerta.SetBool("PuertaEntreAbierta", puertaEntreAbierta);
            }

            if (isVisible)
            {
                //print("visto");
                Invoke(nameof(CerrarPuerta), 0.5f);
            }
        }
        */
        
    }

    
    bool IsVisibleFromCamera(Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        if (GeometryUtility.TestPlanesAABB(planes, rend.bounds))
        {
            return true;
        }
        return false;
    }
    
    
    public void EntreAbrirPuerta()
    {
        puertaEntreAbierta = true;
        animPuerta.SetBool("PuertaEntreAbierta", puertaEntreAbierta);
    }

    public void CerrarPuerta()
    {
        CancelInvoke(nameof(CerrarPuerta));
        puertaAbierta = false;
        puertaEntreAbierta = false;
        animPuerta.SetBool("PuertaEntreAbierta", puertaEntreAbierta);
        animPuerta.SetBool("PuertaAbierta", puertaAbierta);

        Invoke(nameof(DesaparecerEnemigo), 0.5f);
    }

    public void DesaparecerEnemigo()
    {
        enemy.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (guardarController.guardando)
        {
            GuardarDatos();
        }
    }

    public void GuardarDatos()
    {
        PlayerPrefs.SetInt(gameObject.name + "entreAbierta", System.Convert.ToInt32(puertaEntreAbierta));
        PlayerPrefs.SetInt(gameObject.name + "puertaAbierta", System.Convert.ToInt32(puertaAbierta));
    }

    public void AbrirPuerta()
    {

        if (puertaAbierta) 
        {
            //animPuerta = GetComponentInParent<Animator>();

            puertaAbierta = false;
            animPuerta.SetBool("PuertaAbierta", false);
        }
        else if (!puertaAbierta || puertaEntreAbierta)
        {
            //animPuerta = GetComponentInParent<Animator>();

            puertaAbierta = true;
            puertaEntreAbierta = false;

            animPuerta.SetBool("PuertaAbierta", true);

            /*
            aleatorio = Random.Range(0, 2);
            
            if (aleatorio == 1)
            {
                print("SustoArmario");
                enemy.SetActive(true);
                sonidoSusto.Play();
                Invoke(nameof(DesaparecerEnemigo), 0.2f);
            }
            */
            
        }

        animPuerta.SetBool("PuertaEntreAbierta", puertaEntreAbierta);
    }
}
