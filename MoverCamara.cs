using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverCamara : MonoBehaviour
{
    private Transform camatraTrans;
    public Vector2 sensibility;
    public float speed = 5;
    CharacterController controller;
    public float jumpSpeed = 8.0F;
    public float gravity = 9.87F;
    private Vector3 moveDirection = Vector3.zero;
    public AudioSource audios;

    public static float runSpeed;
    // Start is called before the first frame update
    void Start()
    {
        camatraTrans = transform.Find("Main Camera");
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        audios = GetComponent<AudioSource>();

        runSpeed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        MoverCamaraEjeY();
        MoverCamaraEjeX();
        MovimientoPersonaje();
        //PlaySoud();
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

            float angle = (camatraTrans.localEulerAngles.x - ejeY * sensibility.y + 360) % 360;

            if (angle > 180) { angle -= 360; }
            angle = Mathf.Clamp(angle, -80, 80);

            camatraTrans.localEulerAngles = Vector3.right * angle;
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
                moveDirection.y = jumpSpeed;

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
}