using UnityEngine;

public class DetectarIsVisible : MonoBehaviour
{
    public Camera mainCamera;
    public Renderer render;

    public bool isVisible;

    public bool activaItem;
    public GameObject queItem;

    [Tooltip("Capas consideradas para bloqueo de visión")]
    public LayerMask layerMask = ~0;  // Por defecto todas las capas

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        isVisible = IsTrulyVisibleFromCamera(mainCamera);

        if (activaItem && !isVisible && !queItem.activeSelf)
        {
            queItem.SetActive(true);
        }
    }

    public bool IsTrulyVisibleFromCamera(Camera camera)
    {
        if (camera == null || render == null)
            return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        // Comprobar si está dentro del frustum
        if (!GeometryUtility.TestPlanesAABB(planes, render.bounds))
            return false;

        Vector3 origen = camera.transform.position;

        // Chequear primero centro del bounds (como antes)
        if (IsPointVisible(origen, render.bounds.center))
            return true;

        // Si centro no es visible, chequear esquinas del bounds (mejora)
        Vector3[] puntos = GetBoundsPoints(render.bounds);
        for (int i = 1; i < puntos.Length; i++)  // Empieza en 1 porque 0 es el centro ya probado
        {
            if (IsPointVisible(origen, puntos[i]))
                return true;
        }

        // Ningún punto visible sin bloqueo
        return false;
    }

    // Método auxiliar para comprobar visibilidad de un punto desde origen
    private bool IsPointVisible(Vector3 origen, Vector3 punto)
    {
        Vector3 dir = punto - origen;
        float distancia = dir.magnitude;
        dir /= distancia;

        // Lanzamos raycast a todos los objetos entre cámara y punto
        RaycastHit[] hits = Physics.RaycastAll(origen, dir, distancia, layerMask);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject == gameObject)
                return true;  // Este objeto está visible sin bloqueos delante

            if (!hit.collider.isTrigger)
                return false; // Otro objeto bloquea visión
        }

        // Sin impactos, asumimos visible
        return true;
    }

    // Devuelve puntos clave (centro + 8 esquinas) del bounds
    Vector3[] GetBoundsPoints(Bounds bounds)
    {
        Vector3 c = bounds.center;
        Vector3 e = bounds.extents;

        return new Vector3[]
        {
            c, // centro
            c + new Vector3(e.x, e.y, e.z),
            c + new Vector3(e.x, e.y, -e.z),
            c + new Vector3(e.x, -e.y, e.z),
            c + new Vector3(e.x, -e.y, -e.z),
            c + new Vector3(-e.x, e.y, e.z),
            c + new Vector3(-e.x, e.y, -e.z),
            c + new Vector3(-e.x, -e.y, e.z),
            c + new Vector3(-e.x, -e.y, -e.z)
        };
    }
}
