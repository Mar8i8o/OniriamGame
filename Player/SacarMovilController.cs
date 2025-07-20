using UnityEngine;
using UnityEngine.EventSystems;

public class SacarMovilController : MonoBehaviour
{
    //public ItemAtributes itemAtributesMovil;
    public GameObject movil;
    public GameObject movilMesh;
    public GameObject movilPosition;
    public GameObject usingMovilPosition;
    public GameObject spawnMovilPosition;
    public MovilController movilController;
    LlamadasController llamadasController;

    public GameObject player;
    public GameObject mainCamera;

    public float tiempoConObjeto;

    public Raycast raycast;

    public bool movilSacado;
    public bool guardandoMovil;

    public GameObject icoGuardarMovil;
    public GameObject icoUsarMovil;
    public GameObject icoDejarDeUsarMovil;
    public GameObject icoLinterna;

    void Start()
    {
        llamadasController = GameObject.Find("GameManager").GetComponent<LlamadasController>();
        movil.gameObject.transform.SetParent(mainCamera.transform, true);
        movil.gameObject.transform.position = spawnMovilPosition.transform.position;
        movilMesh.SetActive(false);


        movilController.camaraMovil.SetActive(false);

        icoGuardarMovil.SetActive(false);
        icoUsarMovil.SetActive(false);
        icoDejarDeUsarMovil.SetActive(false);
    }

    void Update()
    {

        if (movilSacado)
        {
            if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (!guardandoMovil && !llamadasController.esperandoLlamada)
                {
                    if (!raycast.usingMovil)
                    {
                        GuardarMovil();
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.L))
            {
                movilController.EncenderLinterna();
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (!guardandoMovil)
                {
                    SacarMovil();
                }
            }
        }



        if(movilSacado)
        {
            UsarMovilController();
            tiempoConObjeto += Time.deltaTime;
            if (!raycast.usingMovil)
            {
                if (tiempoConObjeto > 0.4f)
                {
                    movil.transform.position = Vector3.Lerp(movil.transform.position, movilPosition.transform.position, 3 * Time.deltaTime);
                }
                else
                {
                    movil.transform.position = Vector3.Lerp(movil.transform.position, movilPosition.transform.position, 6 * Time.deltaTime);
                }
            }
            else
            {
                movil.transform.position = Vector3.Lerp(movil.transform.position, usingMovilPosition.transform.position, 3 * Time.deltaTime);
            }
        }
        else if(guardandoMovil)
        {
            movil.transform.Translate(Vector3.down * 1.5f * Time.deltaTime);
        }
    }

    public void SacarMovil()
    {
        movilSacado = true;
        movilMesh.SetActive(true);
        movil.gameObject.transform.SetParent(mainCamera.transform, true);
        movil.gameObject.transform.position = spawnMovilPosition.transform.position;
        movil.transform.rotation = movilPosition.transform.rotation;
        raycast.movilController = movilController;
        guardandoMovil = false;
        movilController.camaraMovil.SetActive(true);

        //raycast.CogerObjeto(itemAtributesMovil.gameObject);
        //raycast.usingMovil = true;
        tiempoConObjeto = 0;

        raycast.canPickUp = false;
        if (raycast.hasObject) { raycast.GuardarItem(); }

        icoGuardarMovil.SetActive(true);
        icoUsarMovil.SetActive(true);
        icoDejarDeUsarMovil.SetActive(false);
        icoLinterna.SetActive(true);

    }

    public void GuardarMovil()
    {
        movilSacado = false;
        guardandoMovil = true;
        //raycast.ForzarSoltarObjeto();
        raycast.usingMovil = false;

        //movil.gameObject.transform.SetParent(player.transform, true);
        Invoke(nameof(DejarDeGuardarMovil), 0.8f);
        tiempoConObjeto = 0;

        raycast.canPickUp = true;

        //player.GetComponent<CamaraFP>().freeze = false;
        raycast.puntero.enabled = true;

        icoGuardarMovil.SetActive(false);
        icoUsarMovil.SetActive(false);
        icoDejarDeUsarMovil.SetActive(false);
        icoLinterna.SetActive(false);

    }

    public void DejarDeGuardarMovil()
    {
        //movil.gameObject.transform.SetParent(player.transform, true);
        movilMesh.SetActive(false);
        guardandoMovil = false;
        movilController.camaraMovil.SetActive(false);

        if (!movilController.llamadasController.esperandoLlamada)
        {
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(null);
        }
    }

    public void UsarMovilController()
    {
        if ((Input.GetKeyDown(KeyCode.Mouse0) && tiempoConObjeto > 0.2f))
        {
            if (!raycast.usingMovil && !raycast.seleccionando)
            {
                //player.GetComponent<CamaraFP>().freezeCamera = true;
                player.GetComponent<CamaraFP>().freeze = true;
                movilController.AbrirMovil();
                movilController.EncenderPantalla();
                movilController.encendido = true;
                raycast.puntero.enabled = false;
                raycast.usingMovil = true;
                icoDejarDeUsarMovil.SetActive(true);
                icoUsarMovil.SetActive(false);
                icoGuardarMovil.SetActive(false);
            }
        }
    }
}
