using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCR_Controller : MonoBehaviour
{
    public float tiempoSinApuntar;

    public bool tapaAbierta;

    public Animator VCRAnim;
    public Animator VHSPositionAnim;

    public bool VHSDentro;

    public ItemAtributes vhsInterior;
    //public string nombreVHSInterior;

    public ControlTV controlTV;

    GuardarController guardarController;

    public GameObject botonOn;
    public GameObject botonOff;

    public GameObject botonSinLuz;
    public GameObject botonConLuz;

    ElectricidadController electricidadController;

    PensamientoControler pensamientoControler;

    void Start()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();

        electricidadController = GameObject.Find("ElectricidadControler").GetComponent<ElectricidadController>();
        if (PlayerPrefs.GetInt("VHS_Dentro", System.Convert.ToInt32(VHSDentro)) == 0) { VHSDentro = false; }
        else { VHSDentro = true; MeterVHS(); controlTV.ReproducirVHS(); }

        VCRAnim.enabled = false;
        VHSPositionAnim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
       if (tapaAbierta)
       {
            tiempoSinApuntar += Time.deltaTime;

            if (tiempoSinApuntar > 0.5f ) 
            {
                CerrarTapa();
            }
       }

       botonOff.SetActive(!VHSDentro);
       botonOn.SetActive(VHSDentro);
       
        if (electricidadController.electricidad)
        {
            botonConLuz.SetActive(true);
            botonSinLuz.SetActive(false);
        }
        else
        {
            botonConLuz.SetActive(false);
            botonSinLuz.SetActive(true);
        }

    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt( "VHS_Dentro", System.Convert.ToInt32(VHSDentro));
        }
    }
    
    public void SoltarVHS()
    {
        Invoke(nameof(DesactivarAnimVHS), 2);
        VHSPositionAnim.SetBool("Dentro", false);
        controlTV.QuitarVHS();
        Invoke(nameof(TirarCinta),0.3f);
    }

    public void TirarCinta()
    {
        vhsInterior.inVCR = false;
        vhsInterior.SoltarVHS();
        VHSDentro = false;
    }

    public void MeterVHS()
    {
        VHSDentro = true;
        VHSPositionAnim.enabled = true;
        CancelInvoke(nameof(DesactivarAnimVHS));
        VHSPositionAnim.SetBool("Dentro", true);
        Invoke(nameof(ReproducirVHS),0.4f);
    }

    public void ReproducirVHS()
    {
        controlTV.ReproducirVHS();
    }

    public void ApuntandoTapa() //DESDE EL RAYCAST CUANDO APUNTAS 
    {
        if (VHSDentro)
        {
            tiempoSinApuntar = 0;
            tapaAbierta = true;
            AbrirTapa();
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && !pensamientoControler.mostrandoPensamiento)
        {
            pensamientoControler.MostrarPensamiento("Podria meter un VHS aquí", 1);
        }

    }
    public void AbrirTapa()
    {
        VCRAnim.enabled = true;
        CancelInvoke(nameof(DesactivarAnimVCR));
        VCRAnim.SetBool("Open", true);
    }

    public void CerrarTapa()
    {
        VCRAnim.SetBool("Open", false);
        Invoke(nameof(DesactivarAnimVCR), 2);
    }

    public void DesactivarAnimVCR()
    {
        VCRAnim.enabled = false;
    }

    public void DesactivarAnimVHS()
    {
        VHSPositionAnim.enabled = false;
    }
}
