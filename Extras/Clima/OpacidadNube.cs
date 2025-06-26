using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacidadNube : MonoBehaviour
{
    public Renderer objectRenderer;  // El renderer del objeto
    public float opacity = 0.5f;     // El valor de opacidad deseado (0 es completamente transparente, 1 es completamente opaco)

    private MaterialPropertyBlock propBlock;

    void Start()
    {
        // Inicializa el MaterialPropertyBlock
        propBlock = new MaterialPropertyBlock();

        // Asegúrate de que el objeto tenga un Renderer
        if (objectRenderer == null)
        {
            objectRenderer = GetComponent<Renderer>();
        }

        // Obtener las propiedades actuales del material
        objectRenderer.GetPropertyBlock(propBlock);

        // Obtener el color base actual del material
        Color baseColor = objectRenderer.sharedMaterial.GetColor("_BaseColor");

        // Cambiar la opacidad (canal alfa) del color base
        baseColor.a = opacity;

        // Asignar el color modificado al MaterialPropertyBlock
        propBlock.SetColor("_BaseColor", baseColor);

        // Aplicar los cambios al objeto específico
        objectRenderer.SetPropertyBlock(propBlock);
    }

    void Update()
    {
        // En este caso, se actualiza continuamente para mostrar que puedes cambiar el valor en tiempo real
        // Obtener las propiedades actuales del material
        objectRenderer.GetPropertyBlock(propBlock);

        // Obtener el color base actual del material
        Color baseColor = objectRenderer.sharedMaterial.GetColor("_BaseColor");

        // Cambiar la opacidad (canal alfa) del color base
        baseColor.a = opacity;

        // Asignar el color modificado al MaterialPropertyBlock
        propBlock.SetColor("_BaseColor", baseColor);

        // Aplicar los cambios al objeto específico
        objectRenderer.SetPropertyBlock(propBlock);
    }
}
