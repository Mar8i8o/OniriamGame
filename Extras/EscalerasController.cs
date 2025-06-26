using UnityEngine;

public class EscalerasController : MonoBehaviour
{
    public bool dentroEscaleras;

    public DoorController puertaEscaleras;
    public PensamientoControler pensamientoControler;

    public DesaparecerAlSerVisto enemyEscaleras;
    public Animator enemyAnim;

    GameObject player;

    public float distancia;

    public Transform[] posicionesEnemy;

    public int indicePosiciones;

    public GameObject luzDelSol;
    public GameObject luzTerrado;
    public GameObject vacio;

    void Start()
    {
        player = GameObject.Find("Player");
        enemyEscaleras.gameObject.SetActive(false);
    }


    void Update()
    {
        if(dentroEscaleras)
        {
            distancia = Vector3.Distance(player.transform.position, enemyEscaleras.gameObject.transform.position);


            if (distancia > 25 && !enemyEscaleras.isVisible)
            {
                if (indicePosiciones +1 < posicionesEnemy.Length) { indicePosiciones++; }
                enemyEscaleras.gameObject.transform.position = posicionesEnemy[indicePosiciones].position + new Vector3(0,0.5f,0);
                enemyAnim.SetBool("MirandoAbajo", true);
            }

            if (distancia > 9 ) 
            {
                if(!enemyEscaleras.gameObject.activeSelf)
                {
                    enemyEscaleras.gameObject.SetActive(true);
                }
                else if (enemyEscaleras.isVisible)
                {
                    if (enemyEscaleras.tiempoSiendoVisible > 0.35f)
                    {
                        enemyEscaleras.gameObject.transform.Translate(-enemyEscaleras.gameObject.transform.forward * 2 * Time.deltaTime, Space.World);
                        enemyAnim.SetBool("MirandoAbajo", false);
                    }
                }

            }
            else
            {
                if (enemyEscaleras.gameObject.activeSelf)
                {
                    if (enemyEscaleras.isVisible) { enemyEscaleras.isVisible = false; enemyEscaleras.tiempoSiendoVisible = 0; }
                    enemyEscaleras.gameObject.SetActive(false);
                }
            }
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (!dentroEscaleras)
            {
                pensamientoControler.MostrarPensamiento("Juraria que la puerta estaba cerrada", 1);

                dentroEscaleras = true;
                puertaEscaleras.SetCerrarPuerta();
                puertaEscaleras.sePuedeAbrir = false;
                puertaEscaleras.generaPensamiento = true;
                puertaEscaleras.pensamiento = "La estan bloqueando por detrás";

                luzDelSol.SetActive(false);
                luzTerrado.SetActive(false);
                vacio.SetActive(false);

                //Invoke(nameof(DesactivarEscenario), 1);
            }

        }
    }

}
