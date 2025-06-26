using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class GrainIntensityController : MonoBehaviour
{
    public GameObject player;

    public float distancia;
    public float multiplicador = 1f; // velocidad de cambio
    public float maxDistancia = 55f; // rango para activar el efecto

    public Volume volume;
    private FilmGrain grain;

    private float intensidadActual = 0f;

    // Para controlar el cambio de tipo solo una vez
    private bool dentroRango = false;

    void Start()
    {
        if (volume == null)
        {
            Debug.LogWarning("Asigna el Volume desde el inspector");
            return;
        }

        if (!volume.profile.TryGet<FilmGrain>(out grain))
        {
            Debug.LogWarning("No se encontró FilmGrain en el Volume Profile.");
        }
    }

    void Update()
    {
        if (player == null || grain == null) return;

        distancia = Vector3.Distance(player.transform.position, transform.position);

        float intensidadObjetivo = 0f;

        if (distancia < maxDistancia)
        {
            intensidadObjetivo = Mathf.Clamp01(1f - (distancia / maxDistancia));

            if (!dentroRango)
            {
                // Entramos en rango, cambiamos el tipo de ruido a Large02
                grain.type.value = FilmGrainLookup.Large02;
                dentroRango = true;
            }

            intensidadActual = Mathf.MoveTowards(intensidadActual, intensidadObjetivo, multiplicador * Time.deltaTime);
            grain.intensity.value = intensidadActual;
        }
        else
        {

            if (dentroRango)
            {
                // Salimos del rango, volvemos al tipo Thin1
                grain.type.value = FilmGrainLookup.Medium1;
                dentroRango = false;
            }
        }


    }
}
