using UnityEngine;

public class OffsetMaterial : MonoBehaviour
{
    public Vector2 velocidadOffset = new Vector2(0.1f, 0f);
    private Material material;
    private Vector2 offsetActual;
    private Vector2 tilingOriginal;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;

        // Leer tiling original del material HDRP
        Vector4 st = material.GetVector("_BaseColorMap_ST");
        tilingOriginal = new Vector2(st.x, st.y); // st = (tilingX, tilingY, offsetX, offsetY)

        offsetActual = new Vector2(st.z, st.w); // por si tiene offset ya asignado
    }

    void Update()
    {
        offsetActual += velocidadOffset * Time.deltaTime;

        // Conservar tiling original, cambiar solo offset
        material.SetVector("_BaseColorMap_ST", new Vector4(tilingOriginal.x, tilingOriginal.y, offsetActual.x, offsetActual.y));
    }
}
