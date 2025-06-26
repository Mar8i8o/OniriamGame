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

    public GameObject player;
    public GameObject mainCamera;

    public float tiempoConObjeto;

    public Raycast raycast;

    public bool movilSacado;
    public bool guardandoMovil;

    void Start()
    {
        movil.gameObject.transform.SetParent(mainCamera.transform, true);
        movil.gameObject.transform.position = spawnMovilPosition.transform.position;
        movilMesh.SetActive(false);


        movilController.camaraMovil.SetActive(false);
    }

    void Update()
    {

        if (movilSacado)
        {
            if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (!guardandoMovil)
                {
                    if (!raycast.usingMovil)
                    {
                        GuardarMovil();
                    }
                }
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
        if (raycast.hasObject) { raycast.ForzarSoltarObjeto(); }

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
            }
        }
    }
}
