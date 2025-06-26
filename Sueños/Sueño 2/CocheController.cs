using UnityEngine;

public class CocheController : MonoBehaviour
{
    public bool sentado;

    public GameObject player;
    public CamaraFP scriptPlayer;
    public Transform posicionAsiento;
    public Transform posicionSalirCoche;

    public CapsuleCollider capsuleCollider;
    public CharacterController characterController;

    public bool salirCoche;
    void Start()
    {
        //Invoke(nameof(SalirCoche), 2);
    }

    void Update()
    {
        if(sentado) 
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, posicionAsiento.transform.position, 9 * Time.deltaTime);
        }

        if(salirCoche)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, posicionSalirCoche.position, 3 * Time.deltaTime);
        }

    }

    public void SetSubirCoche()
    {
        scriptPlayer.freeze = true;
        capsuleCollider.enabled = false;
        characterController.enabled = false;

        sentado = true;

        player.transform.position = posicionAsiento.transform.position;
        player.transform.SetParent(gameObject.transform);

    }

    public void SubirCoche() //NO TE "TELETRANSPORTA"
    {
        scriptPlayer.freeze = true;
        capsuleCollider.enabled = false;
        characterController.enabled = false;

        sentado = true;

        player.transform.SetParent(gameObject.transform);

    }

    public void SalirCoche()
    {
        if(sentado) 
        {
            salirCoche = true;
            sentado = false;
            Invoke(nameof(LiberarPlayer), 1f);
        }
    }

    void LiberarPlayer()
    {
        capsuleCollider.enabled = true;
        characterController.enabled = true;
        scriptPlayer.freeze = false;
        salirCoche = false;
        player.transform.SetParent(null);
    }

    public Animator animPuertaDelanteDer;
    public Animator animPuertaDetrasIzq;

    public void AbrirPuerta(string cual)
    {
        if(cual == "delanteDer")
        {
            animPuertaDelanteDer.SetBool("Open", true);
        }
        else if (cual == "detrasIzq")
        {
            animPuertaDetrasIzq.SetBool("Open", true);
        }
        else
        {
            print("ERROR: No se reconoce la id: " + cual);
        }
    }

    public void CerrarPuerta(string cual)
    {
        if (cual == "delanteDer")
        {
            animPuertaDelanteDer.SetBool("Open", false);
        }
        else if (cual == "detrasIzq")
        {
            animPuertaDetrasIzq.SetBool("Open", false);
        }
        else
        {
            print("ERROR: No se reconoce la id: " + cual);
        }
    }
}
