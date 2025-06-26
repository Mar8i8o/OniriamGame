using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LookAtObj2 : MonoBehaviour
{

    public Transform player;  // Referencia al jugador
    public float rotationSpeed = 5f;  // Velocidad de rotación

    public float maxRotationAngleYPos = 80f;
    public float maxRotationAngleYMin = 80f;
    public float maxRotationAngleXUp = 45f;
    public float maxRotationAngleXDown = 45f;

    private float initialYRotation;
    private float initialXRotation;

    private float currentYRotation;
    private float currentXRotation;

    public GameObject cabeza;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }

        initialYRotation = transform.eulerAngles.y;
        initialXRotation = transform.eulerAngles.x;

        currentYRotation = initialYRotation;
        currentXRotation = initialXRotation;
    }

    private void OnEnable()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }

        initialYRotation = transform.eulerAngles.y;
        initialXRotation = transform.eulerAngles.x;

        currentYRotation = initialYRotation;
        currentXRotation = initialXRotation;

        print("EnabledLookAt");
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Calcular la dirección hacia el jugador
        Vector3 directionToTarget = player.position - transform.position;

        // Obtener la rotación deseada hacia el jugador
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        float targetYRotation = targetRotation.eulerAngles.y;
        float targetXRotation = targetRotation.eulerAngles.x;

        // Calcular la diferencia angular en el eje Y
        float angleDifferenceY = Mathf.DeltaAngle(initialYRotation, targetYRotation);
        // Limitar la diferencia angular en el eje Y
        float clampedAngleDifferenceY = Mathf.Clamp(angleDifferenceY, -maxRotationAngleYMin, maxRotationAngleYPos);
        float finalYRotation = initialYRotation + clampedAngleDifferenceY;
        currentYRotation = Mathf.LerpAngle(currentYRotation, finalYRotation, rotationSpeed * Time.deltaTime);

        // Calcular la diferencia angular en el eje X
        float angleDifferenceX = Mathf.DeltaAngle(initialXRotation, targetXRotation);
        // Limitar la diferencia angular en el eje X
        float clampedAngleDifferenceX = Mathf.Clamp(angleDifferenceX, -maxRotationAngleXDown, maxRotationAngleXUp);
        float finalXRotation = initialXRotation + clampedAngleDifferenceX;
        currentXRotation = Mathf.LerpAngle(currentXRotation, finalXRotation, rotationSpeed * Time.deltaTime);

        // Aplicar la rotación final limitada en ambos ejes X y Y
        transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);

        cabeza.transform.rotation = transform.rotation;
    }

    #region codigoAntiguo

    /*

    public Transform player;  // Referencia al jugador
    public float rotationSpeed = 5f;  // Velocidad de rotación

    // Límites de rotación en el eje Y (giro horizontal)
    public float maxRotationAngleYPos = 80f;
    public float maxRotationAngleYMin = 80f;

    // Límites de rotación en el eje X (mirar hacia arriba y hacia abajo)
    public float maxRotationAngleXUp = 45f;
    public float maxRotationAngleXDown = 45f;

    private float initialYRotation;  // Rotación inicial de la cabeza en Y
    private float initialXRotation;  // Rotación inicial de la cabeza en X

    private float currentYRotation;  // Rotación actual de la cabeza en Y
    private float currentXRotation;  // Rotación actual de la cabeza en X

    public GameObject cabeza;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }

        // Guardar la rotación inicial en los ejes Y y X
        initialYRotation = transform.eulerAngles.y;
        initialXRotation = transform.eulerAngles.x;

        currentYRotation = initialYRotation;
        currentXRotation = initialXRotation;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Calcular la dirección hacia el jugador
        Vector3 directionToTarget = player.position - transform.position;

        // Obtener la rotación deseada hacia el jugador
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        float targetYRotation = targetRotation.eulerAngles.y;
        float targetXRotation = targetRotation.eulerAngles.x;

        // Calcular la diferencia angular en el eje Y
        float angleDifferenceY = Mathf.DeltaAngle(initialYRotation, targetYRotation);
        // Limitar la diferencia angular en el eje Y
        float clampedAngleDifferenceY = Mathf.Clamp(angleDifferenceY, -maxRotationAngleYMin, maxRotationAngleYPos);
        float finalYRotation = initialYRotation + clampedAngleDifferenceY;
        currentYRotation = Mathf.LerpAngle(currentYRotation, finalYRotation, rotationSpeed * Time.deltaTime);

        // Calcular la diferencia angular en el eje X
        float angleDifferenceX = Mathf.DeltaAngle(initialXRotation, targetXRotation);
        // Limitar la diferencia angular en el eje X
        float clampedAngleDifferenceX = Mathf.Clamp(angleDifferenceX, -maxRotationAngleXDown, maxRotationAngleXUp);
        float finalXRotation = initialXRotation + clampedAngleDifferenceX;
        currentXRotation = Mathf.LerpAngle(currentXRotation, finalXRotation, rotationSpeed * Time.deltaTime);

        // Aplicar la rotación final limitada en ambos ejes X y Y
        transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);

        cabeza.transform.rotation = transform.rotation;
    }

    */

    #endregion

}