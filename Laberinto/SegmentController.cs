using UnityEngine;
using System;

public class SegmentController : MonoBehaviour
{
    [Header("Spawn de los pr�ximos caminos")]
    public Transform spawnLeft;
    public Transform spawnRight;

    [Header("Prefab del segmento")]
    public GameObject segmentPrefab;

    [Header("Colliders de elecci�n")]
    public Collider exitLeftCollider;
    public Collider exitRightCollider;

    [Header("Control interno")]
    public bool isInstantiatedManually = false; // True cuando el segmento ha sido creado por otro segmento

    private bool hasChosen = false;
    private GameObject leftSegment;
    private GameObject rightSegment;

    public GameObject laberintoParent;
    public LaberintoController laberintoController;

    public GameObject culoRight;
    public GameObject culoLeft;

    public Light luzRight;
    public Light luzLeft;

    public GameObject guardianLaberinto;
    public GameObject spawnGuardianLaberinto;

    public bool caminoErroneo;
    bool lucesRojas;
    public Light[] luces;

    void Start()
    {

        laberintoParent = GameObject.Find("LaberintoParent");
        laberintoController = laberintoParent.GetComponent<LaberintoController>();
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

        // A�adir triggers din�micamente
        var triggerLeft = exitLeftCollider.gameObject.AddComponent<ExitTrigger>();
        triggerLeft.OnPlayerEnter += () => ChoosePath(true);

        var triggerRight = exitRightCollider.gameObject.AddComponent<ExitTrigger>();
        triggerRight.OnPlayerEnter += () => ChoosePath(false);
    }

    private void Update()
    {
        if(laberintoController.caminoErroneo && !lucesRojas)
        {
            CambiarColorLuces();
        }
    }

    void ChoosePath(bool leftChosen)
    {
        if (hasChosen) return;
        hasChosen = true;

        if (leftChosen)
        {

            if (laberintoController.caminoCorrecto[laberintoController.indiceCamino] == "Left")
            {
                print("CaminoCorrecto");
            }
            else
            {
                print("CaminoErroneo");
                caminoErroneo = true;
                laberintoController.EnfadarGuardianes();
                laberintoController.caminoErroneo = true;
                CambiarColorLuces();
            }

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

            if (laberintoController.caminoCorrecto[laberintoController.indiceCamino] == "Right")
            {
                print("CaminoCorrecto");
            }
            else
            {
                print("CaminoErroneo");
                caminoErroneo = true;
                laberintoController.EnfadarGuardianes();
                laberintoController.caminoErroneo = true;
                CambiarColorLuces();
            }

            var next = rightSegment.GetComponent<SegmentController>();
            next.SpawnNextSegments();

            Destroy(leftSegment);
            culoLeft.SetActive(true);
            luzLeft.color = Color.red;
        }

        if (laberintoController.caminoCorrecto.Length > laberintoController.indiceCamino)
        {
            laberintoController.indiceCamino++;
        }

        Instantiate(guardianLaberinto, spawnGuardianLaberinto.transform.position, Quaternion.identity);
    }

    // M�todo que instancia los dos pr�ximos caminos
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

    public void CambiarColorLuces()
    {
        for (int i = 0; i < luces.Length; i++) 
        {
            luces[i].color = Color.red;
        }
        lucesRojas = true;
    }
}
