using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajaController : MonoBehaviour
{
    public Animator cajaAnim;
    public bool abierto;
    public ItemAtributes cajaItem;
    public GameObject contenidoCajon;
    public GameObject objetosCajon;
    public float tiempoCajaAbierta;
    public GameObject puntoSpawnItemsCaja;

    public ContenidoCajonController contenidoCajonController;
    public Collider contenidoCajonCol;

    public GameObject cajasObjectParent;

    GuardarController guardarController;

    BoxCollider boxCollider;

    public DetectarPlayer detectarPlayer;

    void Awake()
    {
        contenidoCajon.name = "ContenidoCajon_" + cajaItem.name;
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();

        boxCollider = cajaItem.GetComponent<BoxCollider>();

        detectarPlayer = GameObject.Find("TrigerDetectarPlayerFuera").GetComponent<DetectarPlayer>();

    }

    private void Start()
    {
        if (abierto)
        {
            abierto = true;
            //objetosCajon.SetActive(true);
        }
        else
        {
            abierto = false;
            //objetosCajon.SetActive(false);
        }
    }

    void Update()
    {
        cajaAnim.SetBool("Open", abierto);

        if (abierto)
        {
            cajaItem.rb.isKinematic = true;
            cajaItem.col.enabled = false;
            if (cajaItem.pickUp)
            {
                abierto = false;
            }

            tiempoCajaAbierta += 0;
        }
        else if(!cajaItem.pickUp)
        {
            cajaItem.col.enabled = true;
            cajaItem.rb.isKinematic = false;
        }
        /*
        if (cajaItem.pickUp)
        {
            objetosCajon.SetActive(false);
        }
        */

        if (guardarController.guardando && !detectarPlayer.playerDentro)
        {
            if (BoxColliderDentroDeZona(boxCollider,zonaPos, zonaTamaño) && contenidoCajonController.objetos == 0)
            {
                cajaItem.active = false;
                cajaItem.DesactivarItem();
                cajaItem.GuardarPosicion();
                cajaItem.gameObject.SetActive(false);
            }
        }

        //estaDentro = BoxColliderDentroDeZona(boxCollider,zonaPos, zonaTamaño);
    }

    public Collider[] collidersHijos;

    public void ActivarColiders()
    {
        collidersHijos = cajasObjectParent.GetComponentsInChildren<Collider>();

        for (int i = 0; i < collidersHijos.Length; i++)
        {
            collidersHijos[i].enabled = true;
        }
    }

    public void DesactivarColiders()
    {
        collidersHijos = cajasObjectParent.GetComponentsInChildren<Collider>();

        for (int i = 0; i < collidersHijos.Length; i++)
        {
            collidersHijos[i].enabled = false;
        }

        print("DesactivarColiders");

    }

    public void Interactuar() //SE LLAMA DESDE TAPA_CAJA
    {
        if(abierto) 
        {
            abierto = false;

            //cajasObjectParent.SetActive(false);
            //objetosCajon.SetActive(false);

            DesactivarColiders();


            Rigidbody[] rigidbodies = objetosCajon.GetComponentsInChildren<Rigidbody>();
            Collider[] coliders = objetosCajon.GetComponentsInChildren<Collider>();

            for (int i = 0; i < rigidbodies.Length; i++)
            {
                rigidbodies[i].isKinematic = true;
            }
            for (int i = 0; i < coliders.Length; i++)
            {
                coliders[i].isTrigger = true;
            }
            tiempoCajaAbierta = 0;

        }
        else
        {
            abierto = true;
            //cajasObjectParent.SetActive(true);
            tiempoCajaAbierta = 0;

            ActivarColiders();

        }
    }

    public void PickUp()
    {
        Rigidbody[] rigidbodies = objetosCajon.GetComponentsInChildren<Rigidbody>();
        Collider[] coliders = objetosCajon.GetComponentsInChildren<Collider>();

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = true;
        }
        for (int i = 0; i < coliders.Length; i++)
        {
            coliders[i].isTrigger = true;
        }
    }

    public Vector3 zonaPos = Vector3.zero;
    public Vector3 zonaTamaño = new Vector3(5, 3, 5);

    public bool estaDentro;

    public bool draw;

    private void OnDrawGizmos()
    {
        if (!draw) return;
        Gizmos.color = new Color(0, 1, 0, 0.25f); // Verde translúcido
        Gizmos.DrawCube(zonaPos, zonaTamaño);

        Gizmos.color = Color.green; // Borde sólido
        Gizmos.DrawWireCube(zonaPos, zonaTamaño);

        // Dibujar BoxCollider
        if (boxCollider != null)
        {
            Vector3[] corners = ObtenerVerticesBoxCollider(boxCollider);
            Gizmos.color = Color.red;
            foreach (var c in corners)
                Gizmos.DrawSphere(c, 0.05f);
        }
    }

    public bool BoxColliderDentroDeZona(BoxCollider boxCollider, Vector3 center, Vector3 size)
    {
        // Asegurar que areaMin tenga los valores más bajos y areaMax los más altos
        Vector3 aMin = center - size / 2f;
        Vector3 aMax = center + size / 2f;
        Vector3 areaMin = Vector3.Min(aMin, aMax);
        Vector3 areaMax = Vector3.Max(aMin, aMax);

        Vector3[] corners = ObtenerVerticesBoxCollider(boxCollider);
        bool completamenteDentro = true;

        for (int i = 0; i < corners.Length; i++)
        {
            Vector3 corner = corners[i];
            if (corner.x < areaMin.x || corner.x > areaMax.x ||
                corner.y < areaMin.y || corner.y > areaMax.y ||
                corner.z < areaMin.z || corner.z > areaMax.z)
            {
                //Debug.LogWarning($"❌ Vértice {i} fuera del área: {corner}");
                completamenteDentro = false;
            }
            else
            {
                //Debug.Log($"✅ Vértice {i} dentro: {corner}");
            }
        }

        /*
        if (!completamenteDentro)
        {
            Debug.Log($"⛔ Collider fuera del área. Área min: {areaMin}, max: {areaMax}");
        }
        else
        {
            Debug.Log($"✅ Collider COMPLETAMENTE dentro del área.");
        }
        */

        return completamenteDentro;
    }


    Vector3[] ObtenerVerticesBoxCollider(BoxCollider box)
    {
        Vector3[] corners = new Vector3[8];
        Transform t = box.transform;
        Vector3 c = t.TransformPoint(box.center);
        Vector3 s = Vector3.Scale(box.size, t.lossyScale) / 2f;

        // 8 vértices
        corners[0] = c + t.rotation * new Vector3(-s.x, -s.y, -s.z);
        corners[1] = c + t.rotation * new Vector3(s.x, -s.y, -s.z);
        corners[2] = c + t.rotation * new Vector3(-s.x, s.y, -s.z);
        corners[3] = c + t.rotation * new Vector3(s.x, s.y, -s.z);
        corners[4] = c + t.rotation * new Vector3(-s.x, -s.y, s.z);
        corners[5] = c + t.rotation * new Vector3(s.x, -s.y, s.z);
        corners[6] = c + t.rotation * new Vector3(-s.x, s.y, s.z);
        corners[7] = c + t.rotation * new Vector3(s.x, s.y, s.z);

        return corners;
    }




}
