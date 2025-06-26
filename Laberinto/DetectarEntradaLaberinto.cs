using UnityEngine;

public class DetectarEntradaLaberinto : MonoBehaviour
{
    public LaberintoController laberintoController;

    public bool entrada;
    public bool salida;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if(entrada)
            {
                laberintoController.EntrarLaberinto();
            }
            else if(salida)
            {
                laberintoController.SalirLaberinto();
            }
        }
    }
}
