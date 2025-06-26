using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazoController : MonoBehaviour
{
    public GameObject brazo;
    public GameObject brazoMesh;
    public Rigidbody brazoRigidbody;

    public GameObject posicionBrazoOn;
    public GameObject posicionBrazoOff;

    public bool brazoExtendido;
    public bool palmaOn;

    public Animator brazoAnimator;
    public Transform cameraTransform; // La referencia a la cámara
    public float smoothSpeed = 0.02f; // La velocidad de suavizado
    public Vector3 initialPositionOffset; // El offset inicial del brazo

    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;

    public bool puedeSacarBrazo;

    void Start()
    {
        // Configura la posición inicial del brazo basado en el estado del brazo
        if (brazoExtendido)
        {
            transform.position = posicionBrazoOn.transform.position;
            ActivarBrazo();
        }
        else
        {
            transform.position = posicionBrazoOff.transform.position;
            DesactivarBrazo();
        }
    }

    void Update()
    {

        brazoAnimator.SetBool("PalmaOn", palmaOn);
    }

    private void LateUpdate()
    {
        if (puedeSacarBrazo)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                /*
                if (brazoExtendido)
                {
                    brazoExtendido = false;
                    Invoke(nameof(AbrirPalma), 0.2f);
                    Invoke(nameof(DesactivarBrazo), 0.5f);
                }
                else
                {
                    brazoExtendido = true;
                    ActivarBrazo();
                    CerrarPalma();

                }
                */

                brazoExtendido = true;
                //Invoke(nameof(CerrarPalma), 0.2f);
                CancelInvoke(nameof(AbrirPalma));
                ActivarBrazo();
                CerrarPalma();

            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {

                brazoExtendido = false;
                Invoke(nameof(AbrirPalma), 0.8f);
                Invoke(nameof(DesactivarBrazo), 0.5f);
                //DesactivarBrazo();


            }

        }
        else if (brazoExtendido)
        {
            brazoExtendido = false;
            Invoke(nameof(AbrirPalma), 0.8f);
            Invoke(nameof(DesactivarBrazo), 0.5f);
            //AbrirPalma();
            print("ForzarCerrarBrazo");
        }


        if (brazoExtendido)
        {
            brazo.transform.position = Vector3.Lerp(brazo.transform.position, posicionBrazoOn.transform.position, brazoSpeed * Time.deltaTime);
            if (cameraTransform.eulerAngles.x < 60 || cameraTransform.eulerAngles.x > 90) LookTo(cameraTransform);
            //LookTo(cameraTransform);
        }
        else
        {
            brazo.transform.position = Vector3.MoveTowards(brazo.transform.position, posicionBrazoOff.transform.position, 3 * Time.deltaTime);
        }

        //print(brazo.transform.localEulerAngles);
        if(brazo.transform.localEulerAngles.z > 3 && brazo.transform.localEulerAngles.z < 50)
        {
            brazo.transform.localEulerAngles = new Vector3(brazo.transform.localEulerAngles.x, brazo.transform.localEulerAngles.y, 3);
        }

        //if (cameraTransform.eulerAngles.x < 60 || cameraTransform.eulerAngles.x > 90) LookTo(cameraTransform);

        //brazoRigidbody.velocity = new Vector3 (0, -Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"));

        //brazo.transform.LookAt(cameraTransform.position);



        /*else
        {
            // Calcular la rotación fija con el offset
            Quaternion fixedRotation = posicionBrazoOn.transform.rotation * Quaternion.Euler(rotationOffset);

            // Interpolación suave hacia la rotación fija con el offset
            brazo.transform.rotation = Quaternion.Slerp(brazo.transform.rotation, fixedRotation, 10 * Time.deltaTime);
         }
         */

    }

    public float brazoSpeed = 6;

    public Vector3 rotationOffset;

    public void LookTo(Transform target)
    {
        // Obtener la dirección hacia el objetivo
        Vector3 targetDirection = target.position - brazo.transform.position;

        // Calcular la rotación necesaria para mirar hacia el objetivo en todos los ejes
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        // Aplicar la rotación de manera suave en todos los ejes
        brazo.transform.rotation = Quaternion.Slerp(brazo.transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }
    public void AbrirPalma()
    {
        palmaOn = true;
        //CancelInvoke(nameof(CerrarPalma));
    }

    public void CerrarPalma()
    {
        palmaOn = false;
        //CancelInvoke(nameof(AbrirPalma));
    }

    public void DesactivarBrazo()
    {
        brazoMesh.SetActive(false);
        CancelInvoke(nameof(ActivarBrazo));
        CancelInvoke(nameof(DesactivarBrazo));
    }

    public void ActivarBrazo()
    {
        brazoMesh.SetActive(true);

        // Calcular la rotación fija con el offset
        Quaternion fixedRotation = posicionBrazoOn.transform.rotation * Quaternion.Euler(rotationOffset);

        // Interpolación suave hacia la rotación fija con el offset
        brazo.transform.rotation = Quaternion.Slerp(brazo.transform.rotation, fixedRotation, 100 * Time.deltaTime);

        CancelInvoke(nameof(DesactivarBrazo));
    }
}


