using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LookAtObj : MonoBehaviour
{
    public GameObject player;
    public GameObject lookAt;

    public float rotationSpeed;

    public float maxRotationAnglePos;
    public float maxRotationAngleMin;

    public float targetAngleY;
    public float currentAngleY;
    public float clampedAngleY;

    public float offset;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void LateUpdate()
    {
        /*
        // Calcula la rotación hacia el objetivo
        Vector3 directionToTarget = player.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Limita el ángulo de rotación en el eje Y
        targetAngleY = targetRotation.eulerAngles.y - offset;
        currentAngleY = transform.localEulerAngles.y;

        clampedAngleY = Mathf.Clamp(targetAngleY, currentAngleY - (maxRotationAngleMin) , currentAngleY + maxRotationAnglePos);

        // Aplica la rotación a la cabeza
        lookAt.transform.rotation = Quaternion.Slerp(lookAt.transform.rotation, Quaternion.Euler(0f, clampedAngleY, 0f), rotationSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(transform.localEulerAngles.z, lookAt.transform.localEulerAngles.y, transform.localEulerAngles.z);

        //print(lookAt.transform.eulerAngles.y);
        if (lookAt.transform.eulerAngles.y < 310 && lookAt.transform.eulerAngles.y > 160)
        {
            //print("transformRotation");
            
            //transform.localEulerAngles = new Vector3(transform.localEulerAngles.z, lookAt.transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        */

        // Calcula la rotación hacia el objetivo
        Vector3 directionToTarget = player.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Calcula el ángulo de rotación deseado en el eje Y
        float targetAngleY = targetRotation.eulerAngles.y;
        float currentAngleY = transform.rotation.eulerAngles.y;

        // Calcula la diferencia angular entre los ángulos actual y deseado
        float angleDifference = Mathf.DeltaAngle(currentAngleY, targetAngleY);

        // Limita el ángulo de rotación en el eje Y considerando la diferencia angular
        float clampedAngleY = Mathf.Clamp(angleDifference, -maxRotationAngleMin, maxRotationAnglePos);

        // Calcula el nuevo ángulo objetivo teniendo en cuenta la limitación
        float newTargetAngleY = currentAngleY + clampedAngleY;

        // Aplica la rotación a la cabeza de manera suave
        Quaternion targetRotationY = Quaternion.Euler(0f, newTargetAngleY, 0f);
        lookAt.transform.rotation = Quaternion.RotateTowards(lookAt.transform.rotation, targetRotationY, rotationSpeed * Time.deltaTime);

        // Establece la rotación de la cabeza
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookAt.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);



    }
}
