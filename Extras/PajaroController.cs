using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PajaroController : MonoBehaviour
{
    public Animator anim;

    public PajarosManager pajarosManager;

    public bool playerCerca;

    bool volando;

    GameObject objetivo;

    float randomDistancia;

    float speed;

    public bool forzarIdle;

    public float distanciaIrse = 12;

    void Start()
    {
        //pajarosManager = GameObject.Find("PajarosManager").GetComponent<PajarosManager>();

        randomDistancia = Random.Range(0.1f,3.1f);

        if (!forzarIdle)
        {
            int aleatorioAnim = Random.Range(1, 6);
            anim.SetInteger("NumIdle", aleatorioAnim);
        }

        speed = Random.Range(8.1f, 9.8f);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);


    }
    bool setVolar;
    void Update()
    {

        playerCerca = pajarosManager.playerCerca;

        if ((playerCerca && pajarosManager.distanca < distanciaIrse + randomDistancia) || pajarosManager.forzarVolar)
        {
            if (!setVolar)
            {
                setVolar = true;
                //Invoke(nameof(SalirVolando), Random.Range(0.1f, 1.5f));
                pajarosManager.pajaros--;
                SalirVolando();
            }
        }
        if (volando)
        {
            transform.position = Vector3.MoveTowards(transform.position, objetivo.transform.position, speed * Time.deltaTime);
            LookTo(objetivo.transform);
        }

        anim.SetBool("Volando", volando);

    }

    public void SalirVolando()
    {
        int aleatorio = Random.Range(0, pajarosManager.puntosPajaros.Length);
        volando = true;
        objetivo = pajarosManager.puntosPajaros[aleatorio];
        Invoke(nameof(Destruir), 10);
    }

    public void Destruir()
    {
        Destroy(gameObject);
    }

    public void LookTo(Transform target)
    {
        // Obtener la dirección hacia el objetivo
        Vector3 targetDirection = target.position - transform.position;

        // Calcular la rotación necesaria para mirar hacia el objetivo solo en el eje X
        Quaternion targetRotationX = Quaternion.LookRotation(targetDirection, Vector3.up);

        // Solo rotar en el eje X manteniendo la rotación actual en los otros ejes
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotationX.eulerAngles.y, targetRotationX.eulerAngles.z);

        // Aplicar la rotación de manera suave solo en el eje X
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
    }
}
