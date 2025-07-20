using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaController : MonoBehaviour
{
    public GameObject posiconSofa;
    public GameObject posiconSofaLevantarse;
    public bool sentadoSofa;
    bool recienLevantado;

    public GameObject player;
    public CharacterController characterController;
    public CamaraFP camaraFP;
    public Raycast ray;

    public Collider colider;

    public float speed = 5;


    GameObject pisPosition;
    public Vector3 offsetPis;
    Vector3 pisPositionPosicionInicial;

    public GameObject icoLevantarse;

    public GameObject panelDialogos;
    public GameObject panelLlamadas;

    void Start()
    {
        ray = GameObject.Find("Main Camera").GetComponent<Raycast>();
        player = GameObject.Find("Player");
        camaraFP = player.GetComponent<CamaraFP>();
        characterController = player.GetComponent<CharacterController>();

        pisPosition = GameObject.Find("PisPosition");
    }

    // Update is called once per frame
    void Update()
    {
        if (sentadoSofa) 
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, posiconSofa.transform.position, speed * Time.deltaTime);
            pisPosition.transform.localPosition = offsetPis;

            if (ray.tiempoSinObjeto>0.1f)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.C))
                {


                    if (panelDialogos != null) { if (panelDialogos.activeSelf) { return; } }
                    if (panelLlamadas != null) { if (panelLlamadas.activeSelf) { return; } }

                    LevantarseSofa();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.C))
                {

                    if(panelDialogos != null) { if (panelDialogos.activeSelf) { return; } }
                    if(panelLlamadas != null) { if (panelLlamadas.activeSelf) { return; } }

                    LevantarseSofa();
                }
            }
        }

        if(recienLevantado)
        {
            if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.C))
            {
                ray.sentado = false;
            }
        }
    }

    public void Usar()
    {
        if (!sentadoSofa) 
        {

            SentarseSofa();
        }
    }

    public void SentarseSofa()
    {
        sentadoSofa = true;
        camaraFP.freeze = true;
        colider.enabled = false;
        ray.sentado = true;

        pisPositionPosicionInicial = pisPosition.transform.localPosition;

        if(icoLevantarse != null)icoLevantarse.SetActive(true);

    }

    public void LevantarseSofa()
    {
        sentadoSofa = false;
        camaraFP.freeze = false;
        colider.enabled = true;
        characterController.enabled = false;
        player.transform.position = posiconSofaLevantarse.transform.position;
        characterController.enabled = true;
        recienLevantado = true;

        pisPosition.transform.localPosition = pisPositionPosicionInicial;

        if (icoLevantarse != null)icoLevantarse.SetActive(false);

    }
}
