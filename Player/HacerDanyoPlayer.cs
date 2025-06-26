using UnityEngine;

public class HacerDanyoPlayer : MonoBehaviour
{
    PlayerVida playerVida;
    void Start()
    {
        playerVida = GameObject.Find("Player").GetComponent<PlayerVida>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playerVida.RecibirDanyo();
        }
    }
}
