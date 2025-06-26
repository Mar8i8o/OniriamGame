using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesaparecerAlSerVisto : MonoBehaviour
{
    public Renderer render;
    public Camera miCamara;

    public AudioSource sonidoScreamer;

    public bool isVisible;
    public float tiempoSiendoVisible;

    public LayerMask layerMask;

    void Start()
    {
        miCamara = GameObject.Find("Main Camera").GetComponent<Camera>();
        sonidoScreamer = GameObject.Find("AudioSusto").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        isVisible = IsVisibleFromCamera(miCamara);

        if (isVisible) 
        {
            //sonidoScreamer.Play();
            //Invoke(nameof(Desaparecer), 0.5f);
            tiempoSiendoVisible += Time.deltaTime;
        }
        else
        {
            tiempoSiendoVisible = 0;
        }
    }

    bool IsVisibleFromCamera(Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        // Verifica si el objeto está en el campo de visión de la cámara
        if (GeometryUtility.TestPlanesAABB(planes, render.bounds))
        {
            // Realiza un raycast desde la cámara hacia el objeto para verificar si está obstruido
            Vector3 directionToTarget = render.bounds.center - camera.transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            // Verifica si hay una obstrucción
            if (Physics.Raycast(camera.transform.position, directionToTarget.normalized, out RaycastHit hit, 99, layerMask))
            {
                // Retorna true solo si el objeto golpeado es el mismo objeto que queremos ver
                if (hit.collider.gameObject == render.gameObject)
                {
                    return true;
                }
            }
        }

        // Si no está en el campo de visión o está bloqueado, retorna false
        return false;
    }

    public void Desaparecer()
    {
        Destroy(gameObject);
    }
}
