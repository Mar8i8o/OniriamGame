using UnityEngine;

public class LaberintoController : MonoBehaviour
{
    public bool dentroLaberinto;

    public GameObject[] todosLosLaberintos;

    public Transform transformLaberintoInicial;

    public Vector3 posicionLaberintoInicial;
    public Quaternion rotacionLaberintoInicial;
    public Vector3 scalaLaberintoInicial;

    public GameObject laberintoPrefab;

    void Start()
    {
        posicionLaberintoInicial = transformLaberintoInicial.position;
        rotacionLaberintoInicial = transformLaberintoInicial.rotation;
        scalaLaberintoInicial = transformLaberintoInicial.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EntrarLaberinto()
    {

        DestruirLaberinto();

        dentroLaberinto = true;

        GameObject laberintoInstanciado = Instantiate(laberintoPrefab, posicionLaberintoInicial, rotacionLaberintoInicial);
        laberintoInstanciado.transform.SetParent(transform);
        laberintoInstanciado.transform.position = posicionLaberintoInicial;
        laberintoInstanciado.transform.rotation = rotacionLaberintoInicial;
        laberintoInstanciado.transform.localScale = scalaLaberintoInicial;


    }

    public void SalirLaberinto()
    {
        dentroLaberinto = false;


        DestruirLaberinto();

    }

    public void DestruirLaberinto()
    {
        todosLosLaberintos = GameObject.FindGameObjectsWithTag("Laberinto");

        for (int i = 0; todosLosLaberintos.Length > i; i++)
        {
            Destroy(todosLosLaberintos[i]);
        }
    }

}
