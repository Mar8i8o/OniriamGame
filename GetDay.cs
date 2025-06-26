using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetDay : MonoBehaviour
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
        if (timeController.dia >= 10)
        {
            text.text = (timeController.dia) + "/"+timeController.mes+"/10";
        }
        else
        {
            text.text = "0" + (timeController.dia) + "/" + timeController.mes + "/10";
        }
    }
}
