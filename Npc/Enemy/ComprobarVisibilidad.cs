using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComprobarVisibilidad : MonoBehaviour
{
    public Renderer rend;
    public Camera miCamara;

    void Start()
    {
        // Asigna la cámara deseada desde el Editor de Unity o encuentra la cámara de alguna manera.
        //miCamara = Camera.main;
    }
    void Update()
    {
        // Verifica si el objeto está dentro del frustum de la cámara
        bool isVisible = IsVisibleFromCamera(miCamara);

        if (isVisible)
        {
            Debug.Log("El objeto es visible.");
        }
        else
        {
            Debug.Log("El objeto no es visible.");
        }
    }

    bool IsVisibleFromCamera(Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        if (GeometryUtility.TestPlanesAABB(planes, rend.bounds))
        {
            return true;
        }
        return false;
    }

}
