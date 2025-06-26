using UnityEngine;
using System;

public class SegmentController : MonoBehaviour
{
    [Header("Spawn de los próximos caminos")]
    public Transform spawnLeft;
    public Transform spawnRight;

    [Header("Prefab del segmento")]
    public GameObject segmentPrefab;

    [Header("Colliders de elección")]
    public Collider exitLeftCollider;
    public Collider exitRightCollider;

    [Header("Control interno")]
    public bool isInstantiatedManually = false; // True cuando el segmento ha sido creado por otro segmento

    private bool hasChosen = false;
    private GameObject leftSegment;
    private GameObject rightSegment;

    public GameObject laberintoParent;

    public GameObject culoRight;
    public GameObject culoLeft;

    public Light luzRight;
    public Light luzLeft;

    void Start()
    {

        laberintoParent = GameObject.Find("LaberintoParent");
        transform.SetParent(laberintoParent.transform);

        // Si es el primer segmento, instancia los caminos iniciales
        if (!isInstantiatedManually)
        {
            leftSegment = Instantiate(segmentPrefab, spawnLeft.position, spawnLeft.rotation);
            leftSegment.transform.SetParent(laberintoParent.transform, true);
            leftSegment.transform.position = spawnLeft.position;
            leftSegment.transform.localScale = transform.localScale;

            rightSegment = Instantiate(segmentPrefab, spawnRight.position, spawnRight.rotation);
            rightSegment.transform.SetParent(laberintoParent.transform, true);
            rightSegment.transform.position = spawnRight.position;
            rightSegment.transform.localScale = transform.localScale;


            leftSegment.GetComponent<SegmentController>().isInstantiatedManually = true;
            rightSegment.GetComponent<SegmentController>().isInstantiatedManually = true;
        }

        // Añadir triggers dinámicamente
        var triggerLeft = exitLeftCollider.gameObject.AddComponent<ExitTrigger>();
        triggerLeft.OnPlayerEnter += () => ChoosePath(true);

        var triggerRight = exitRightCollider.gameObject.AddComponent<ExitTrigger>();
        triggerRight.OnPlayerEnter += () => ChoosePath(false);
    }

    void ChoosePath(bool leftChosen)
    {
        if (hasChosen) return;
        hasChosen = true;

        if (leftChosen)
        {
            // Instanciar los siguientes caminos del camino izquierdo
            var next = leftSegment.GetComponent<SegmentController>();
            next.SpawnNextSegments();

            // Destruir el otro
            Destroy(rightSegment);
            culoRight.SetActive(true);
            luzRight.color = Color.red;
        }
        else
        {
            var next = rightSegment.GetComponent<SegmentController>();
            next.SpawnNextSegments();

            Destroy(leftSegment);
            culoLeft.SetActive(true);
            luzLeft.color = Color.red;
        }
    }

    // Método que instancia los dos próximos caminos
    public void SpawnNextSegments()
    {
        if (leftSegment != null || rightSegment != null) return;

        leftSegment = Instantiate(segmentPrefab, spawnLeft.position, spawnLeft.rotation);
        leftSegment.transform.SetParent(laberintoParent.transform, true);
        leftSegment.transform.position = spawnLeft.position;
        leftSegment.transform.localScale = transform.localScale;

        rightSegment = Instantiate(segmentPrefab, spawnRight.position, spawnRight.rotation);
        rightSegment.transform.SetParent(laberintoParent.transform, true);
        rightSegment.transform.position = spawnRight.position;
        rightSegment.transform.localScale = transform.localScale;


        leftSegment.GetComponent<SegmentController>().isInstantiatedManually = true;
        rightSegment.GetComponent<SegmentController>().isInstantiatedManually = true;
    }
}
