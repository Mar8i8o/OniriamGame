using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetHour : MonoBehaviour
{
    TextMeshProUGUI text;
    TimeController timeController;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        timeController = GameObject.Find("GameManager").GetComponent<TimeController>();
    }

    void Update()
    {
        float totalSegundos = timeController.totalSegundos;

        float hora = Mathf.FloorToInt(totalSegundos / 3600);
        float minutes = Mathf.FloorToInt(totalSegundos / 60);
        minutes = Mathf.FloorToInt(minutes % 60);
        float seconds = Mathf.FloorToInt(totalSegundos % 60);

        //temporizadorTMP.text = string.Format("{0:00}:{1:00}:{2:00}", hora, minutes, seconds);
        text.text = string.Format("{0:00}:{1:00}", hora, minutes);
    }
}
