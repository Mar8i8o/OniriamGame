using UnityEngine;

public class AccionSubirCoche : MonoBehaviour
{
    public CocheController cocheController;

    public bool cierraPuerta;
    public string quePuerta;

    void Start()
    {
        cocheController.SubirCoche();

        if(cierraPuerta)
        {
            Invoke(nameof(CerrarPuerta), 0.5f);
        }

    }

    public void CerrarPuerta()
    {
        cocheController.CerrarPuerta(quePuerta);
    }

}
