using UnityEngine;

public class TriggerLuzEnemigo : MonoBehaviour
{
    public GameObject enemigo;
    public GameObject luz;

    public bool usado;
    void Start()
    {
        enemigo.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if(!usado)
            {
                usado = true;
                Invoke(nameof(ApagarLuz1), 1);
            }
        }
    }

    public void ApagarLuz1() 
    {
        luz.SetActive(false);
        enemigo.SetActive(true);

        Invoke(nameof(EncenderLuz1), 0.1f);

    }

    public void EncenderLuz1()
    {
        luz.SetActive(true);
        enemigo.SetActive(true);

        Invoke(nameof(ApagarLuz2), 5);

    }

    public void ApagarLuz2()
    {
        luz.SetActive(false);
        enemigo.SetActive(false);

        Invoke(nameof(EncenderLuz2), 0.1f);

    }

    public void EncenderLuz2()
    {
        luz.SetActive(true);
        enemigo.SetActive(false);
    }
}
