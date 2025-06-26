using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DetectarCampoVision : MonoBehaviour
{
    public float distancia;

    public LayerMask mask;

    public bool viendoPlayer;

    public GameObject player;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(player.transform.position);

        Debug.DrawRay(transform.position, transform.forward * distancia, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distancia, mask))
        {
            if (hit.collider.tag == "Player")
            {
                viendoPlayer = true;
            }
            else
            {
                viendoPlayer = false;
            }

        }
        else
        {
            viendoPlayer = false;
        }
    }
}
