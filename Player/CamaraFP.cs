using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraFP : MonoBehaviour
{
    private Transform camaraTrans;
    public Vector2 sensibility;
    public float speed;
    CharacterController controller;
    public float jumpSpeed = 8.0F;
    public float gravity = 9.87F;
    private Vector3 moveDirection = Vector3.zero;
    public AudioSource audios;
    GameObject player;

    public bool freeze;
    public bool freezeCamera;

    public bool freezeRun;
    public float runSpeed;
    public bool runing;

    [HideInInspector] public float initialSpeed;

    DreamController dreamController;
    GuardarController guardarController;
    ParpadeoController parpadeoController;

    public float tiempoCorriendo;

    // Start is called before the first frame update
    void Start()
    {
        initialSpeed = speed;
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        parpadeoController = GameObject.Find("GameManager").GetComponent<ParpadeoController>();
        dreamController = GameObject.Find("GameManager").GetComponent<DreamController>();
        respiracionController = GameObject.Find("GameManager").GetComponent<RespiracionController>();
        pasosController = GameObject.Find("PasosController").GetComponent<PasosController>();
        player = GameObject.Find("Player");
        
        //PlayerPrefs.DeleteAll();

        camaraTrans = GameObject.Find("Main Camera").GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        audios = GetComponent<AudioSource>();

        if (!dreamController.inDream) { GetPosition(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (!freezeCamera)
        {
            MoverCamaraEjeY();
            MoverCamaraEjeX();
        }

        if(!freeze) MovimientoPersonaje();
        //PlaySoud();

        if (!freezeRun) { RunController(); }
    }

    public void ForzarMirada(Transform dondeMira, float speedMirar) ////NO FUNCIONA BIEN 
    {
        // Calcula la rotación hacia el objetivo
        Vector3 targetDirection = dondeMira.transform.position - camaraTrans.transform.position;
        targetDirection.y = 0f; // Asegúrate de que no haya rotación vertical

        // Calcula la rotación necesaria solo en los ejes X y Z
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Mantén constante la rotación en el eje Y
        Quaternion currentRotation = transform.rotation;
        targetRotation.eulerAngles = new Vector3(targetRotation.eulerAngles.x, currentRotation.eulerAngles.y, targetRotation.eulerAngles.z);

        // Aplica la rotación de forma suavizada
        camaraTrans.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, speedMirar * Time.deltaTime);

        //////////////////////////

        // Obtén la dirección hacia el objetivo
        Vector3 targetDirection2 = dondeMira.transform.position - player.transform.position;
        targetDirection2.y = 0f; // Asegúrate de que no haya rotación vertical

        // Calcula la rotación necesaria para mirar hacia el objetivo solo en el eje Y
        Quaternion targetRotation2 = Quaternion.LookRotation(targetDirection2);

        // Ajusta la rotación para que solo afecte el eje Y
        targetRotation2 = Quaternion.Euler(0f, targetRotation2.eulerAngles.y, 0f);

        // Aplica la rotación de forma suavizada
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation2, speedMirar * Time.deltaTime);

    }

    public void ForzarMiradaX(Transform dondeMira, float speed)
    {
        // Calcular la dirección hacia el objetivo
        Vector3 direccionObjetivo = dondeMira.position - camaraTrans.position;

        // Calcula la rotación objetivo para mirar hacia el punto objetivo, pero solo en el eje X
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionObjetivo);

        // Mantener constante la rotación en los ejes Y y Z
        Quaternion rotacionActual = camaraTrans.rotation;
        rotacionObjetivo.eulerAngles = new Vector3(rotacionObjetivo.eulerAngles.x, rotacionActual.eulerAngles.y, rotacionActual.eulerAngles.z);

        // Rotar suavemente hacia la rotación objetivo usando Slerp
        camaraTrans.rotation = Quaternion.Slerp(rotacionActual, rotacionObjetivo, Time.deltaTime * speed);
    }

    public void ForzarMiradaY(Transform dondeMira, float speed)
    {
        // Obtén la dirección hacia el objetivo
        Vector3 targetDirection2 = dondeMira.transform.position - player.transform.position;
        targetDirection2.y = 0f; // Asegúrate de que no haya rotación vertical

        // Calcula la rotación necesaria para mirar hacia el objetivo solo en el eje Y
        Quaternion targetRotation2 = Quaternion.LookRotation(targetDirection2);

        // Ajusta la rotación para que solo afecte el eje Y
        targetRotation2 = Quaternion.Euler(0f, targetRotation2.eulerAngles.y, 0f);

        // Aplica la rotación de forma suavizada
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation2, speed * Time.deltaTime);
    }

    public PasosController pasosController;
    RespiracionController respiracionController;

    public bool cansado;

    public void RunController() 
    {

        if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            if (!cansado)
            {
                EmpezarACorrer();
            }
        }

        if (Input.GetKeyUp(KeyCode.RightShift) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            DejarDeCorrer();
        }

        
        
        

    }

    private void FixedUpdate()
    {
        if (runing)
        {
            tiempoCorriendo += Time.deltaTime;
            if (tiempoCorriendo > 10)
            {
                DejarDeCorrer();
                cansado = true;
                respiracionController.seEscucha = true;
                StartCoroutine(DejarDeEstarCansadoCoroutine());
            }
        }
        else if (tiempoCorriendo > 0)
        {
            tiempoCorriendo -= Time.deltaTime;
        }
    }

    public void EmpezarACorrer()
    {
        runSpeed = 2;
        //print("correr");
        runing = true;
        pasosController.EmpezarACorrer();
    }

    public void DejarDeCorrer()
    {
        runSpeed = 1;
        //print("dejarDeCorrer");
        runing = false;
        pasosController.DejarDeCorrer();
    }

    private IEnumerator DejarDeEstarCansadoCoroutine()
    {
        yield return new WaitForSeconds(5);
        DejarDeEstarCansado();
    }

    public void DejarDeEstarCansado()
    {
        respiracionController.seEscucha = false;
        cansado = false;
    }

    public void MoverCamaraEjeY()
    {
        float ejeX = Input.GetAxis("Mouse X");
        if (ejeX != 0)
        {
            transform.Rotate(Vector3.up * ejeX * sensibility.x);
        }
    }
    public void MoverCamaraEjeX()
    {
        float ejeY = Input.GetAxis("Mouse Y");
        if (ejeY != 0)
        {
            //camera.Rotate(Vector3.right * -ejeY * sensibility.y);

            float angle = (camaraTrans.localEulerAngles.x - ejeY * sensibility.y + 360) % 360;

            if (angle > 180) { angle -= 360; }
            angle = Mathf.Clamp(angle, -80, 80);

            camaraTrans.localEulerAngles = Vector3.right * angle;
        }
    }

    public void MovimientoPersonaje()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed * runSpeed;

            if (Input.GetButton("Jump"))
            {
                //moveDirection.y = jumpSpeed;
            }

        }
        else
        {
            moveDirection.x = Input.GetAxis("Horizontal") * speed * runSpeed;
            moveDirection.z = Input.GetAxis("Vertical") * speed * runSpeed;
            moveDirection = transform.TransformDirection(moveDirection);
        }
        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);
    }
    public void PlaySoud()
    {
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
        {
            audios.Play();
        }
        else if (!Input.GetButton("Horizontal") && !Input.GetButton("Vertical") && GetComponent<AudioSource>().isPlaying)
        {
            audios.Stop();
        }

    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            if (!parpadeoController.beingKO)
            {
                if (!dreamController.inDream)
                {
                    GuardarController();
                }
            }
        }
    }

    public void GuardarController()
    {
        PlayerPrefs.SetFloat(gameObject.name + "posX", transform.position.x);
        PlayerPrefs.SetFloat(gameObject.name + "posZ", transform.position.z);
        PlayerPrefs.SetFloat(gameObject.name + "posY", transform.position.y);

        PlayerPrefs.SetFloat(gameObject.name + "rotX", transform.eulerAngles.x);
        PlayerPrefs.SetFloat(gameObject.name + "rotZ", transform.eulerAngles.z);
        PlayerPrefs.SetFloat(gameObject.name + "rotY", transform.eulerAngles.y);
    }

    float posX;
    float posY;
    float posZ;

    float rotX;
    float rotY;
    float rotZ;
    public void GetPosition()
    {
        posX = PlayerPrefs.GetFloat(gameObject.name + "posX", transform.position.x);
        posY = PlayerPrefs.GetFloat(gameObject.name + "posY", transform.position.y);
        posZ = PlayerPrefs.GetFloat(gameObject.name + "posZ", transform.position.z);

        rotX = PlayerPrefs.GetFloat(gameObject.name + "rotX", transform.eulerAngles.x);
        rotY = PlayerPrefs.GetFloat(gameObject.name + "rotY", transform.eulerAngles.y);
        rotZ = PlayerPrefs.GetFloat(gameObject.name + "rotZ", transform.eulerAngles.z);

        //transform.position = new Vector3(posX, posY, posZ);
        //transform.eulerAngles = new Vector3(rotX, rotY, rotZ);
    }
    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Silla"))
        {
            print("ActivarColisiones");
            other.gameObject.GetComponent<ChairController>().ActivarColisiones();
        }
    }
    */
}
