using UnityEngine;

public class BordeJuego : MonoBehaviour
{
    public GameObject centroMirar;

    CamaraFP camaraFP;

    PensamientoControler pensamientoControler;


    void Start()
    {
        camaraFP = GameObject.Find("Player").GetComponent<CamaraFP>();
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            pensamientoControler.MostrarPensamiento("No deberia ir por ahi", 2);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            print("DentroDelBorde");

            camaraFP.ForzarMiradaX(centroMirar.transform, 1);
            camaraFP.ForzarMiradaY(centroMirar.transform, 1);
            //camaraFP.freezeCamera = true;

            if(Input.GetAxis("Vertical") < 0)
            {
                camaraFP.freeze = true;
            }
            else
            {
                camaraFP.freeze = false;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {

            camaraFP.freeze = false;
            camaraFP.freezeCamera = false;

        }
    }
}
