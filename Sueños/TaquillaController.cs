using UnityEngine;

public class TaquillaController : MonoBehaviour
{
    public Animator anim;
    public bool abierto;
    public bool drawGizmos;
    public bool detectarPlayer;
    public bool oculto;
    public bool playerDentro;

    public DetectarMicrofono detectarMicrofono;

    public MoverNPC enemigo;
    public Transform posicionEspera;
    public Transform posicionDelanteTaquilla;

    public Raycast raycast;

    void Start()
    {
        raycast = GameObject.Find("Main Camera").GetComponent<Raycast>();
        boxRotation = transform.rotation;
        anim.SetBool("Open", abierto);

        if(!abierto)anim.enabled = false;

        detectarMicrofono = GameObject.Find("GameManager").GetComponent<DetectarMicrofono>();

    }

    public Vector3 offsetBox;
    public Vector3 detectionBoxSize;
    Quaternion boxRotation;
    public LayerMask playerLayer; 


    void Update()
    {

        if(!detectarPlayer) { return; }

        playerDentro = Physics.CheckBox(transform.position + offsetBox, detectionBoxSize / 2, boxRotation, playerLayer);

        if(playerDentro && !abierto)
        {
            if (!oculto && !abierto) { //EMPEZAR A ESTAR OCULTO

                detectarMicrofono.ActivarMicrofono();
                print("ActivarMicrofonoTaquilla");

                if (enemigo != null)
                {
                    if (enemigo.persiguiendoPlayer)
                    {
                        enemigo.sabeDondeEsta = enemigo.ComprobarSabeDondeEsta();

                        enemigo.playerOculto = true;
                        enemigo.taquillaController = this;
                        enemigo.posicionEspera = posicionEspera;

                        margenDeteccionVolumen = 0;

                    }
                }
            }
            oculto = true;
        }
        else if (!playerDentro || abierto)
        {
            if(oculto) { //DEJAR DE ESTAR OCULTO

                detectarMicrofono.DesactivarMicrofono();

                if (enemigo != null)
                {
                    if (enemigo.persiguiendoPlayer)
                    {
                        enemigo.playerOculto = false;
                        enemigo.SalirTaquilla();

                        margenDeteccionVolumen = 0;

                    }
                    //enemigo.taquillaController = this;
                }
            }
            oculto = false;

        }

        if(oculto && enemigo != null) 
        {
            if(detectarMicrofono.ComprobarVolumen())
            {
                margenDeteccionVolumen += Time.deltaTime;
            }

            if(margenDeteccionVolumen > 0.3f)
            {
                enemigo.sabeDondeEsta = true;
            }

        }

    }

    public float margenDeteccionVolumen;

    public void Interactuar()
    {

        CancelInvoke(nameof(DesactivarAnim));

        anim.enabled = true;

        print("UsarTaquilla");

        abierto = !abierto;
        anim.SetBool("Open", abierto);

        Invoke(nameof(DesactivarAnim), 2);

    }

    public void SetAbrirTaquilla()
    {
        anim.enabled = true;
        abierto = true;

        anim.SetBool("Open", abierto);
        CancelInvoke(nameof(DesactivarAnim));
        Invoke(nameof(DesactivarAnim), 2);
    }

    public void DesactivarAnim()
    {
        CancelInvoke(nameof(DesactivarAnim));
        anim.enabled = false;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        // Guarda la matriz de transformación original de Gizmos
        Gizmos.matrix = Matrix4x4.TRS(transform.position + offsetBox, transform.rotation, Vector3.one);

        // Establece el color del Gizmo
        Gizmos.color = Color.green;

        // Dibuja el WireCube con la rotación aplicada
        Gizmos.DrawWireCube(Vector3.zero, detectionBoxSize);

        // Restaura la matriz de transformación de Gizmos a su estado original (opcional, pero recomendable)
        Gizmos.matrix = Matrix4x4.identity;
    }

}
