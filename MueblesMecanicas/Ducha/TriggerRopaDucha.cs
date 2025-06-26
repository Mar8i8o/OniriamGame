using UnityEngine;

public class TriggerRopaDucha : MonoBehaviour
{
    public GameObject ropaDucha;

    Vector3 posicionInicial;
    Vector3 rotacionInicial;

    private void Awake()
    {
        posicionInicial = ropaDucha.transform.position;
        rotacionInicial = ropaDucha.transform.eulerAngles;
        ropaDucha.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            ropaDucha.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            ropaDucha.SetActive(false);
            ropaDucha.transform.position = posicionInicial;
            ropaDucha.transform.eulerAngles = rotacionInicial;
        }

    }
}
