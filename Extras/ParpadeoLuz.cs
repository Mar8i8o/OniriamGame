using UnityEngine;

public class ParpadeoLuz : MonoBehaviour
{
    public Light targetLight; // La luz que va a parpadear
    public Renderer targetRenderer; // El renderer del objeto que queremos modificar
    public float minValue = 0.0f; // Valor mínimo para el parpadeo
    public float maxValue = 1.0f; // Valor máximo para el parpadeo
    public float flickerSpeed = 0.1f; // Velocidad de parpadeo
    public float lightMultiplier = 1.0f; // Multiplicador para la luz
    public float opacityMultiplier = 1.0f; // Multiplicador para la opacidad
    public Color baseColor = Color.white; // Color base del objeto

    private MaterialPropertyBlock propertyBlock;

    public float valorIntensidad;

    public bool encendido;
    public bool apagado;

    bool encendiendose;
    bool apagandose;

    public float maxTiempoEncendido;
    public float maxTiempoApagado;

    public float tiempoEncendido;
    public float tiempoApagado;

    private void Start()
    {
        // Inicializa el bloque de propiedades
        //propertyBlock = new MaterialPropertyBlock();

        encendido = true;
        valorIntensidad = 1;
    }

    private void Update()
    {

        if(encendido)
        {
            tiempoEncendido += Time.deltaTime;

            if(tiempoEncendido > maxTiempoEncendido) 
            {
                tiempoEncendido = 0;
                tiempoApagado = 0;
                apagandose = true;
                encendiendose = false;
                encendido = false;
                apagado = false;
            }
        }
        else if (apagado)
        {
            tiempoApagado += Time.deltaTime;

            if (tiempoApagado > maxTiempoApagado)
            {
                tiempoEncendido = 0;
                tiempoApagado = 0;
                apagandose = false;
                encendiendose = true;
                encendido = false;
                apagado = false;
            }
        }
        else if (encendiendose)
        {
            valorIntensidad += Time.deltaTime * flickerSpeed;

            if (valorIntensidad >= 1)
            {
                encendido = true;
                encendiendose = false;
                apagandose = false;
                apagado = false;
                tiempoEncendido = 0;
                tiempoApagado = 0;
            }
        }
        else if (apagandose)
        {
            valorIntensidad -= Time.deltaTime * flickerSpeed;

            if (valorIntensidad <= 0)
            {
                encendido = false;
                encendiendose = false;
                apagandose = false;
                apagado = true;
                tiempoEncendido = 0;
                tiempoApagado = 0;
            }
        }

        //AjustarMateriales();
    }

    public void AjustarMateriales()
    {
        // Usamos un solo Mathf.PerlinNoise para obtener el valor fluctuante
        //float syncValue = Mathf.PerlinNoise(Time.time * flickerSpeed, 0); // Un valor fluctuante sincronizado

        // Ajustamos los min y max con los multiplicadores
        float minLight = minValue * lightMultiplier;  // Ajusta el mínimo de la luz
        float maxLight = maxValue * lightMultiplier;  // Ajusta el máximo de la luz
        float minOpacity = minValue * opacityMultiplier;  // Ajusta el mínimo de la opacidad
        float maxOpacity = maxValue * opacityMultiplier;  // Ajusta el máximo de la opacidad

        // Aplicamos el valor fluctuante y multiplicadores para controlar luz y opacidad
        float lightIntensity = Mathf.Lerp(minLight, maxLight, valorIntensidad); // Controlamos la luz
        float opacity = Mathf.Lerp(minOpacity, maxOpacity, valorIntensidad); // Controlamos la opacidad

        // Actualiza la intensidad de la luz
        if (targetLight != null)
        {
            targetLight.intensity = lightIntensity;
        }

        // Actualiza la opacidad del material si ha cambiado
        if (targetRenderer != null)
        {
            // Obtiene el bloque de propiedades del material
            targetRenderer.GetPropertyBlock(propertyBlock);

            // Calcula el color con la nueva opacidad (alpha)
            Color finalColor = baseColor;
            finalColor.a = opacity; // Asigna el valor de opacidad calculado

            // Establece el color base con la opacidad
            propertyBlock.SetColor("_BaseColor", finalColor);

            // Aplica el bloque de propiedades al renderer
            targetRenderer.SetPropertyBlock(propertyBlock);

            // Asegúrate de que el material sea compatible con transparencia
            if (targetRenderer.material.HasProperty("_Surface"))
            {
                targetRenderer.material.SetFloat("_Surface", 1);  // 1: Transparent, 0: Opaque
                targetRenderer.material.SetFloat("_BlendMode", 0); // BlendMode: 0 es transparente, 1 es opaco
            }
        }
    }
}
