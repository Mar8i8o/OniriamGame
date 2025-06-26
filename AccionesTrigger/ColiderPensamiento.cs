using UnityEngine;

public class ColiderPensamiento : MonoBehaviour
{

    PensamientoControler pensamientoControler;
    public string pensamiento;

    float tiempoEntrePensamiento;

    private void Awake()
    {
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            pensamientoControler.MostrarPensamiento(pensamiento, 2f);
            print("TriggerMostrarPensamiento");
            Destroy(gameObject);
        }
    }
}
