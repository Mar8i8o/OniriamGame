using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reloj : MonoBehaviour
{
    [SerializeField]
    public Transform secondsHandle;
    [SerializeField]
    public Transform minutesHandle;
    [SerializeField]
    public Transform hoursHandle;

    public TimeController timeController;

    void Start()
    {

    }

    void Update()
    {
        float inGameSeconds = timeController.totalSegundos;

        // Calcula la rotación para cada manecilla
        float secondsRotation = inGameSeconds * 360 / 60;
        float minutesRotation = timeController.GetMinutes() * 360 / 60;
        float hoursRotation = timeController.GetHour() * 360 / 12; // Suponiendo un reloj de 12 horas

        // Aplica la rotación a las manecillas
        secondsHandle.localRotation = Quaternion.Euler(0, secondsRotation, 0);
        minutesHandle.localRotation = Quaternion.Euler(0, minutesRotation, 0);
        hoursHandle.localRotation = Quaternion.Euler(0, hoursRotation, 0);
    }
}
