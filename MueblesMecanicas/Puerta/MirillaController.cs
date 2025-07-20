using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirillaController : MonoBehaviour
{
    public GameObject player;
    CamaraFP camaraScript;
    public GameObject camara;

    public Animator mirillaAnim;

    public float radio;
    public bool jugadorCerca;
    public LayerMask capaDelJugador;

    public GameObject mirilla;
    public GameObject posCam;
    public GameObject interfaz;

    Vector3 posCamInicial;

    public bool usandoMirilla;
    public bool mirillaAbiertaPasiva;
    public float tiempoConMirillaAbierto;

    public GameObject camaraMirilla;

    GameObject mainCamera;

    public GameObject posicionMirarMirilla;
    public Animator cameraAnim;

    public GameObject icoSalir;

    private void Awake()
    {
        player = GameObject.Find("Player");
        camara = GameObject.Find("CameraParent");
        mainCamera = GameObject.Find("Main Camera");
        camaraScript = player.GetComponent<CamaraFP>();

        camaraMirilla.SetActive(false);

        ApagarAnim();
    }

    // Update is called once per frame
    void Update()
    {
        //jugadorCerca = Physics.CheckSphere(transform.position, radio, capaDelJugador);

        //camaraMirilla.SetActive(jugadorCerca);

        if (usandoMirilla) 
        {
            camara.transform.position = Vector3.MoveTowards(camara.transform.position, posCam.transform.position, 1f * Time.deltaTime);
            mainCamera.transform.LookAt(posicionMirarMirilla.transform.position);
            //camaraScript.ForzarMiradaX(mirilla.transform, 1);
            //camaraScript.ForzarMiradaY(mirilla.transform, 1);

            if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.C))
            {
                CerarMirilla();
                CerrarTapaMirilla();
            }
        }

        if(mirillaAbiertaPasiva) 
        {
            tiempoConMirillaAbierto += Time.deltaTime;
            if (tiempoConMirillaAbierto > 0.2f) { CerrarTapaMirilla(); }
        }
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }

    public void AbrirTapaMirilla()
    {
        EncenderAnim();

        tiempoConMirillaAbierto = 0;
        mirillaAnim.SetBool("Open", true);
        mirillaAbiertaPasiva = true;

        camaraMirilla.SetActive(true);

        Invoke(nameof(ApagarAnim), 3);
    }

    public void CerrarTapaMirilla()
    {

        EncenderAnim();

        mirillaAnim.SetBool("Open", false);
        mirillaAbiertaPasiva = false;

        camaraMirilla.SetActive(false);

        Invoke(nameof(ApagarAnim), 3);
    }

    public Raycast ray;
    public void AbrirMirilla()
    {

        EncenderAnim();

        mirillaAnim.SetBool("Open", true);
        camaraScript.freeze = true;
        camaraScript.freezeCamera = true;
        //player.GetComponent<CharacterController>().enabled = false;
        //camara.GetComponent<Raycast>().enabled = false;
        ray.enabled = false;
        //camara.transform.SetParent(null);

        interfaz.SetActive(false);

        posCamInicial = camara.transform.position;
        usandoMirilla = true;

        mirillaAbiertaPasiva = false;
        tiempoConMirillaAbierto = 0;

        camaraMirilla.SetActive(true);
        cameraAnim.enabled = false;

        Invoke(nameof(ApagarAnim), 3);

        icoSalir.SetActive(true);

    }

    public void CerarMirilla()
    {
        //mirillaAnim.SetBool("Open", false);
        camaraScript.freeze = false;
        camaraScript.freezeCamera = false;
        //player.GetComponent<CharacterController>().enabled = false;
        //camara.GetComponent<Raycast>().enabled = true;
        ray.enabled = true;
        //camara.transform.SetParent(player.transform);

        interfaz.SetActive(true);

        usandoMirilla = false;

        cameraAnim.enabled = true;

        icoSalir.SetActive(false);

    }

    void ApagarAnim()
    {
        mirillaAnim.enabled = false;
    }

    void EncenderAnim()
    {
        CancelInvoke(nameof(ApagarAnim));
        mirillaAnim.enabled = true;
    }
}
