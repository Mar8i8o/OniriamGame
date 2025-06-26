using UnityEngine;

public class TPController : MonoBehaviour
{
    public Transform[] teleportPoint;

    public int spawnPoint;

    public GameObject player;
    public CharacterController characterController;
    public CamaraFP camaraFP;

    void Start()
    {
        camaraFP.enabled = false;
        characterController.enabled = false;
        player.transform.position = teleportPoint[spawnPoint].position;
        camaraFP.enabled = true;
        characterController.enabled = true;
    }

}
