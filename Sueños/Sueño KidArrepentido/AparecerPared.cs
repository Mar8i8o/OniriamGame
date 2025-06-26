using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.Port;

public class AparecerPared : MonoBehaviour
{
    public Renderer render;
    Camera camara;
    public bool isVisible;
    public float opacity = 0.5f; // La opacidad deseada (entre 0 y 1)
    private MaterialPropertyBlock propBlock;
    Collider colider;
    public bool activa;

    public LayerMask playerLayer;

    Quaternion boxRotation;

    public bool tieneExtra;
    public AparecerPared extra;

    public bool desactivaObj;
    public GameObject OBJdesactiva;

    public bool activaObj;
    public GameObject OBJactiva;

    void Start()
    {
        camara = GameObject.Find("Main Camera").GetComponent<Camera>();
        colider = gameObject.GetComponent<Collider>();
        propBlock = new MaterialPropertyBlock();

        activa = false;

        if(activaObj)OBJactiva.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        boxRotation = transform.rotation;


        isVisible = IsVisibleFromCamera(camara);

        playerDentro = Physics.CheckBox(transform.position + offsetBox, detectionBoxSize / 2, boxRotation, playerLayer);

        // Obtén la información actual del material
        render.GetPropertyBlock(propBlock);

        // Asigna un nuevo color con la opacidad deseada
        Color baseColor = render.sharedMaterial.GetColor("_BaseColor");
        baseColor.a = opacity;

        // Modifica la propiedad del color base del material en este objeto específico
        propBlock.SetColor("_BaseColor", baseColor);

        // Aplica los cambios
        render.SetPropertyBlock(propBlock);

        //////////////////////////////////////////////////////////////////

        if(playerDentro && !isVisible && !activa)
        {
            activa = true;

            if(tieneExtra)
            {
                extra.activa = true;
            }

            if(desactivaObj)
            {
                OBJdesactiva.SetActive(false);
                print("Objeto desactivado");
            }

            if(activaObj)
            {
                OBJactiva.SetActive(true);
            }
        }



        if(activa)
        {
            colider.isTrigger = false;
            opacity = 1;
        }
        else
        {
            colider.isTrigger = true;
            opacity = 0;
        }
    }

    bool IsVisibleFromCamera(Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        if (GeometryUtility.TestPlanesAABB(planes, render.bounds))
        {
            return true;
        }
        return false;
    }

    public bool playerDentro;

    public Vector3 offsetBox;
    public Vector3 detectionBoxSize;

    public bool drawGizmos;
    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        // Guarda la matriz de transformación original de Gizmos
        Gizmos.matrix = Matrix4x4.TRS(transform.position + offsetBox, transform.rotation, Vector3.one);

        // Establece el color del Gizmo
        Gizmos.color = Color.green;

        // Dibuja el WireCube con la rotación aplicada
        Gizmos.DrawWireCube(Vector3.zero, detectionBoxSize);

        // Restaura la matriz de transformación de Gizmos a su estado original (opcional, pero recomendable)
        Gizmos.matrix = Matrix4x4.identity;
    }
}
