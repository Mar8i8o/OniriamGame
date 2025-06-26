using UnityEngine;

public class ObstaculoNivel : MonoBehaviour
{
    public bool bloqueando;

    public GameObject obstaculoBloqueando;
    public GameObject obstaculoApartado;

    public GameObject obstaculoNavMesh;

    public MoverNPC moverEnemigo;

    public float distanciaEnemigo;

    public AudioSource audioGolpe;

    bool desbloqueado;
    void Start()
    {
        if(bloqueando)
        {
            obstaculoApartado.SetActive(false);
            obstaculoBloqueando.SetActive(true);

            obstaculoNavMesh.SetActive(true);
        }
        else
        {
            obstaculoApartado.SetActive(true);
            obstaculoBloqueando.SetActive(false);

            obstaculoNavMesh.SetActive(false);
        }
    }

    void Update()
    {
        if(moverEnemigo != null)
        {
            if(moverEnemigo.persiguiendoPlayer && bloqueando)
            {
                distanciaEnemigo = Vector3.Distance(moverEnemigo.gameObject.transform.position, transform.position);

                if(moverEnemigo.patrullando)
                {
                    if(distanciaEnemigo < 3 && !desbloqueado)
                    {
                        Invoke(nameof(DesbloquearObstaculo), 0.21f);
                        if(audioGolpe != null)audioGolpe.Play();
                        desbloqueado = true;
                    }
                }
            }
        }
    }

    public void DesbloquearObstaculo()
    {

        bloqueando = false;

        obstaculoApartado.SetActive(true);
        obstaculoBloqueando.SetActive(false);

        obstaculoNavMesh.SetActive(false);
    }
}
