using TMPro;
using UnityEngine;

public class MostrarDinero : MonoBehaviour
{
    public TextMeshProUGUI texto;
    PlayerStats playerStats;
    void Start()
    {
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        texto.text = playerStats.dinero + "$";
    }
}
