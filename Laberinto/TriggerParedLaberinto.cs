using UnityEngine;

public class TriggerParedLaberinto : MonoBehaviour
{
    public RepetirLaberinto laberintoSiguiente;
    public RepetirLaberinto laberintoOpuesto;

    public bool izquierda;
    public bool derecha;

    public GameObject laberintoPrefab;

    public GameObject instanciaLaberintoIzq;
    public GameObject instanciaLaberintoDer;

    public bool usado;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //camaraFP.runSpeed = setspeed;
            if (!usado)
            {
                usado = true;

                print("Instanciar laberinto");

                laberintoOpuesto.gameObject.SetActive(false);

                instanciaLaberintoIzq = Instantiate(laberintoPrefab, laberintoSiguiente.laberintoIzquierda.transform, laberintoSiguiente.laberintoIzquierda);
                instanciaLaberintoDer = Instantiate(laberintoPrefab, laberintoSiguiente.laberintoDerecha.transform, laberintoSiguiente.laberintoDerecha);

                instanciaLaberintoIzq.transform.position = laberintoSiguiente.laberintoIzquierda.transform.position;
                instanciaLaberintoIzq.transform.localScale = laberintoSiguiente.laberintoIzquierda.transform.localScale;
                instanciaLaberintoIzq.transform.rotation = laberintoSiguiente.laberintoIzquierda.transform.rotation;

                instanciaLaberintoDer.transform.position = laberintoSiguiente.laberintoDerecha.transform.position;
                instanciaLaberintoDer.transform.localScale = laberintoSiguiente.laberintoDerecha.transform.localScale;
                instanciaLaberintoDer.transform.rotation = laberintoSiguiente.laberintoDerecha.transform.rotation;

                //AÑADE LOS SIGUIENTES LABERINTOS AL LABERINTO DE DELANTE

                laberintoSiguiente.triggerRepetirIzquierda.laberintoSiguiente = instanciaLaberintoIzq.GetComponent<RepetirLaberinto>().laberintoIzquierda.GetComponent<RepetirLaberinto>();
                laberintoSiguiente.triggerRepetirIzquierda.laberintoOpuesto = instanciaLaberintoDer.GetComponent<RepetirLaberinto>().laberintoDerecha.GetComponent<RepetirLaberinto>();

                laberintoSiguiente.triggerRepetirDerecha.laberintoSiguiente = instanciaLaberintoDer.GetComponent<RepetirLaberinto>().laberintoDerecha.GetComponent<RepetirLaberinto>();
                laberintoSiguiente.triggerRepetirDerecha.laberintoOpuesto = instanciaLaberintoIzq.GetComponent<RepetirLaberinto>().laberintoIzquierda.GetComponent<RepetirLaberinto>();


            }
        }
    }
}
