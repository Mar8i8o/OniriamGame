using UnityEngine;

public class GramofonoController : MonoBehaviour
{
    public AudioSource sonidoGramola;

    public Animator animator;

    public float duracion;
    public float tiempoSonando;

    public bool sonando;
    void Start()
    {
        
    }

    void Update()
    {
        if(sonando)
        {
            tiempoSonando += Time.deltaTime;

            if(tiempoSonando > duracion) 
            {
                DejarDeSonar();
            }

        }
    }

    public void Interactuar()
    {
        animator.SetBool("On", true);
        if(!sonando)sonidoGramola.Play();
        sonando = true;

        tiempoSonando = 0;

    }

    public void DejarDeSonar()
    {
        sonando = false;
        sonidoGramola.Stop();
        animator.SetBool("On", false);

        tiempoSonando = 0;

    }

}
