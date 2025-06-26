using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComprobarVisibilidad : MonoBehaviour
{
    public Renderer rend;
    public Camera miCamara;

    void Start()
    {
        // Asigna la c�mara deseada desde el Editor de Unity o encuentra la c�mara de alguna manera.
        //miCamara = Camera.main;
    }
    void Update()
    {
        // Verifica si el objeto est� dentro del frustum de la c�mara
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
