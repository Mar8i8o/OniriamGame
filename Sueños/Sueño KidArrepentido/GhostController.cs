using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float opacity = 0.5f; // La opacidad deseada (entre 0 y 1)
    public Renderer objectRenderer;
    private MaterialPropertyBlock propBlock;

    public GameObject player;

    public float distanciaPlayer;

    public float multiply;

    void Start()
    {
        // Aseg�rate de que el Renderer est� correctamente asignado
        //objectRenderer = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
        player = GameObject.Find("Player");
    }

    void Update()
    {

        distanciaPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanciaPlayer < 100)
        {
            opacity = (distanciaPlayer / multiply) - 0.4f;
            if (opacity < 0) { opacity = 0; Destroy(gameObject); }
            if (opacity > 1) { opacity = 1; }

            LookTo(player.transform);

            // Obt�n la informaci�n actual del material
            objectRenderer.GetPropertyBlock(propBlock);

            // Asigna un nuevo color con la opacidad deseada
            Color baseColor = objectRenderer.sharedMaterial.GetColor("_BaseColor");
            baseColor.a = opacity;  // Ajusta la opacidad deseada (0 a 1)

            // Modifica la propiedad del color base del material en este objeto espec�fico
            propBlock.SetColor("_BaseColor", baseColor);

            // Aplica los cambios
            objectRenderer.SetPropertyBlock(propBlock);
        }
    }

    public void LookTo(Transform target)
    {
        // Obtener la direcci�n hacia el objetivo
        Vector3 targetDirection = target.position - transform.position;

        // Calcular la rotaci�n necesaria para mirar hacia el objetivo solo en el eje X
        Quaternion targetRotationX = Quaternion.LookRotation(targetDirection, Vector3.up);

        // Solo rotar en el eje X manteniendo la rotaci�n actual en los otros ejes
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotationX.eulerAngles.y, targetRotationX.eulerAngles.z);

        // Aplicar la rotaci�n de manera suave solo en el eje X
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
    }
}
