using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharcosPisController : MonoBehaviour
{
    public bool active;

    public Animator charcoPisAnim;
    public Renderer render;

    public float suciedad;
    public float suciedadScale;
    public GameObject charco;
    public GameObject parent;

    GuardarController guardarController;
    Raycast raycast;

    public MeshCollider colider;
    void Start()
    {
        raycast = GameObject.Find("Main Camera").GetComponent<Raycast>();
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        if (PlayerPrefs.GetInt(gameObject.name + "active", System.Convert.ToInt32(active)) == 0) { active = false; }
        else { active = true; }

        if (active) 
        { 
            GetDatos();
            IniciarAnimacion();
        }

        //PlayerPrefs.DeleteAll();
    }

    void LateUpdate()
    {
        if (guardarController.guardando)
        {
            if (active) { GuardarDatos(); }
            PlayerPrefs.SetInt(gameObject.name + "active", System.Convert.ToInt32(active));
        }

        if (raycast.hasObject) 
        {
            if (raycast.itemAtributes.isFregona) { colider.enabled = true; }
            else { colider.enabled = false; }
        }
        else
        {
            colider.enabled = false;
        }
    }

    float posX;
    float posY;
    float posZ;

    float rotX;
    float rotY;
    float rotZ;
    public void GuardarDatos()
    {
        PlayerPrefs.SetFloat(gameObject.name + "posX", transform.position.x);
        PlayerPrefs.SetFloat(gameObject.name + "posZ", transform.position.z);
        PlayerPrefs.SetFloat(gameObject.name + "posY", transform.position.y);

        PlayerPrefs.SetFloat(gameObject.name + "rotX", transform.eulerAngles.x);
        PlayerPrefs.SetFloat(gameObject.name + "rotZ", transform.eulerAngles.z);
        PlayerPrefs.SetFloat(gameObject.name + "rotY", transform.eulerAngles.y);

        PlayerPrefs.SetFloat(gameObject.name + "suciedad", suciedad);
        PlayerPrefs.SetFloat(gameObject.name + "suciedadScale", suciedadScale);

        PlayerPrefs.SetInt(gameObject.name + "active", System.Convert.ToInt32(active));
    }

    public void GetDatos()
    {
        posX = PlayerPrefs.GetFloat(gameObject.name + "posX", transform.position.x);
        posY = PlayerPrefs.GetFloat(gameObject.name + "posY", transform.position.y);
        posZ = PlayerPrefs.GetFloat(gameObject.name + "posZ", transform.position.z);

        //rotX = PlayerPrefs.GetFloat(gameObject.name + "rotX", transform.eulerAngles.x);
        rotY = PlayerPrefs.GetFloat(gameObject.name + "rotY", transform.eulerAngles.y);
        //rotZ = PlayerPrefs.GetFloat(gameObject.name + "rotZ", transform.eulerAngles.z);

        transform.position = new Vector3(posX, posY, posZ);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotY, transform.eulerAngles.z);

        suciedad = PlayerPrefs.GetFloat(gameObject.name + "suciedad", suciedad);
        suciedadScale = PlayerPrefs.GetFloat(gameObject.name + "suciedadScale", suciedadScale);

    }

    public bool limpiando;
    public float tiempoLimpiando;
    public void Limpiar()
    {
            limpiando = true;
            tiempoLimpiando = 0;
    }

    public void Desactivar()
    {
        active = false;
        transform.position = Vector3.zero;
        charco.GetComponent<MeshRenderer>().enabled = false;
        charco.transform.localScale = new Vector3(1,1,1);
        //gameObject.SetActive(false);
    }

    public void Activar()
    {
        charco.transform.localScale = new Vector3(1, 1, 1);
    }


    public float scaleX;
    public float scaleY;
    public float scaleZ;

    private void Update()
    {

        if(!active) { gameObject.SetActive(false); }

        scaleX = suciedadScale;
        scaleY = suciedadScale;
        scaleZ = suciedadScale;

        if (limpiando)
        {
            tiempoLimpiando += Time.deltaTime;
            suciedad -= Time.deltaTime * 0.2f;
            suciedadScale -= Time.deltaTime * 0.2f;

            if (tiempoLimpiando > 1)
            {
                limpiando = false;
            }
        }

        //if (active)
        //{
            if (suciedad <= 0f) { Desactivar(); }
            render.material.color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, suciedad);
        //}

        charco.transform.localScale = new Vector3 (scaleX, scaleY, scaleZ);

    }
    private void OnEnable()
    {
        //RandomRotation();
        //IniciarAnimacion();
    }

    public void RandomRotation()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0,360), transform.eulerAngles.z);
    }

    public void IniciarAnimacion()
    {
        charcoPisAnim.SetTrigger("Spawn");
        Invoke(nameof(ActivarMesh), 0.1f);
    }

    public void ActivarMesh()
    {
        charco.GetComponent<MeshRenderer>().enabled = true;
    }
}
