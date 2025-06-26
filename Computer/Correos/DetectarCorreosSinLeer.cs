using TMPro;
using UnityEngine;

public class DetectarCorreosSinLeer : MonoBehaviour
{
    public EventosCorreos eventosCorreos;
    public TextMeshProUGUI textoCorreosSinLeer;

    void Update()
    {
        textoCorreosSinLeer.text = eventosCorreos.correosSinLeer + "";   
    }

    private void OnEnable()
    {
        eventosCorreos.ContarCorreosSinLeer();
    }
}
