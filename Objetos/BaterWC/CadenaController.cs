using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CadenaController : MonoBehaviour
{
    public Animator cadenaAnim;
    public Animator aguaAnim;
    public BaterController baterController;

    public GameObject remolino;
    public Renderer rendRemolino;
    public float opacidadRemolino;
    public bool tirandoCadena;
    public Vector3 velocidad;

    public AudioSource audioCadena;
    void Start()
    {
        remolino.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(tirandoCadena) 
        {
            remolino.transform.Rotate(velocidad);
        }
    }

    public void TirarCadena()
    {
        cadenaAnim.SetTrigger("Flush");
        if (!tirandoCadena)
        {
            aguaAnim.SetTrigger("Flush");
            Invoke(nameof(LimpiarAgua), 0.5f);
            Invoke(nameof(FinalizarTirarCadena), 2f);
            tirandoCadena = true;
            remolino.SetActive(true);
            audioCadena.Play();
        }
    }

    public void LimpiarAgua()
    {
        baterController.LimpiarAgua();
        remolino.SetActive(false);
    }

    public void FinalizarTirarCadena()
    {
        tirandoCadena = false;
    }
}
