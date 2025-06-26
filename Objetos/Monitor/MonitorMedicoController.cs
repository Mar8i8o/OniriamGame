using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonitorMedicoController : MonoBehaviour
{
    public TextMeshProUGUI pulsacionesTXT;

    PlayerStats playerStats;
    void Start()
    {
        playerStats = GameObject.Find("GameManager").gameObject.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        pulsacionesTXT.text = "" + playerStats.pulsaciones + " BPM";
        //pulsacionesTXT.text = "sex";
    }
}
