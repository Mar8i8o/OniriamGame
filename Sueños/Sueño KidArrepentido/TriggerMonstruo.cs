using UnityEngine;

public class TriggerMonstruo : MonoBehaviour
{
    public MonstruoS1 scriptMonstruo;
    public DoorController doorController;

    public bool salida;

    public bool golpeandoPuerta;

    public AudioSource sonidoGolpearPuerta;

    public GameObject miniMonstruos;

    bool usado;

    void Start()
    {
        if(miniMonstruos != null)miniMonstruos.SetActive(false);
    }

    void Update()
    {
        if(golpeandoPuerta)
        {
            scriptMonstruo.agent.SetDestination(sonidoGolpearPuerta.gameObject.transform.position);
            scriptMonstruo.MoverSonido();
            if(scriptMonstruo.sonidoNormal.volume > 0)scriptMonstruo.sonidoNormal.volume -= Time.deltaTime * 0.1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (!usado)
            {
                if (!salida)
                {
                    scriptMonstruo.IniciarPersecucion();
                    doorController.sonidoCerrarPuerta.enabled = false;
                    doorController.SetCerrarPuerta();
                    doorController.sePuedeAbrir = false;
                    doorController.generaPensamiento = true;
                    doorController.pensamiento = "Esta cerrada, deberia buscar otro camino";
                    scriptMonstruo.particulas.Play();
                    usado = true;
                    gameObject.SetActive(false);
                }
                else
                {
                    scriptMonstruo.DesactivarPersecucion();
                    doorController.SetCerrarPuerta();
                    scriptMonstruo.particulas.Stop();
                    doorController.sePuedeAbrir = false;
                    doorController.generaPensamiento = true;
                    doorController.pensamiento = "No pienso volver a entrar ahi";
                    miniMonstruos.SetActive(true);

                    Invoke(nameof(EmpezarGolpearPuerta), 2);

                    golpeandoPuerta = true;
                    usado = true;
                }
            }

        }
    }

    public void EmpezarGolpearPuerta()
    {
        sonidoGolpearPuerta.Play();
        Invoke(nameof(DejarDeGolpearPuerta), 20);
    }

    public void DejarDeGolpearPuerta()
    {
        sonidoGolpearPuerta.Stop();
        scriptMonstruo.sonidoNormal.Stop();
        golpeandoPuerta = false;
    }

}
