using UnityEngine;

public class LoopCarretera : MonoBehaviour
{
    public bool avanzando;
    public GameObject carretera1;
    public GameObject carretera2;
    public Vector3 velocidad;

    public GameObject cocheController;

    public int indiceCarretera;

    Vector3 posicionInicialCarretera1;
    Vector3 posicionInicialCarretera2;

    public float maxPoxZ;

    public bool detenerCoche;

    private void Awake()
    {
        carretera1.SetActive(true);
        carretera2.SetActive(true);
    }
    void Start()
    {
        indiceCarretera = 1;

        posicionInicialCarretera1 = carretera1.transform.position;
        posicionInicialCarretera2 = carretera2.transform.position;

    }

    void Update()
    {

        if(acelerando) 
        {
            if(velX < velocidadRequerida.x)
            {
                velX += Time.deltaTime * velocidadAceleracion;
            }
            else if (velY < velocidadRequerida.y)
            {
                velY += Time.deltaTime * velocidadAceleracion;
            }
            else if (velZ < velocidadRequerida.z)
            {
                velZ += Time.deltaTime * velocidadAceleracion;
            }

            velocidad = new Vector3 (velX, velY, velZ);

        }

        if (avanzando)
        {
            carretera1.transform.Translate(velocidad * Time.deltaTime);
            carretera2.transform.Translate(velocidad * Time.deltaTime);

            if (indiceCarretera == 1)
            {
                if(carretera1.transform.position.z > posicionInicialCarretera1.z + maxPoxZ)
                {
                    carretera1.transform.position = posicionInicialCarretera2;
                    indiceCarretera = 2;       
                    
                    if(detenerCoche)
                    {
                        avanzando = false;
                        carretera1.gameObject.SetActive(false);
                    }
                }

            }
            else
            {
                if (carretera2.transform.position.z > posicionInicialCarretera1.z + maxPoxZ)
                {
                    carretera2.transform.position = posicionInicialCarretera2;
                    indiceCarretera = 1;

                    if (detenerCoche)
                    {
                        avanzando = false;
                        carretera2.gameObject.SetActive(false);
                    }
                }
            }


        }
    }

    public void DetenerCarretera()
    {
        detenerCoche = true;
    }


    public bool acelerando;
    public float velocidadAceleracion;

    public Vector3 velocidadRequerida ;


    public float velX;
    public float velY;
    public float velZ;

    public void AcelerarCarretera()
    {

        velocidadRequerida = velocidad;
        velocidad = Vector3.zero;


        acelerando = true;
        avanzando = true;
    }
}
