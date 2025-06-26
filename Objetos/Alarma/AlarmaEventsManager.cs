using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmaEventsManager : MonoBehaviour
{

    public bool alarmaActiva;
    public TimeController timeController;

    public float totalSegundosAlarma;
    public float totalSegundos;

    public bool dream;
    public AlarmController alarmController;

    [HideInInspector] public bool blockComprobarAlarma;

    private void Start()
    {
        timeController = GameObject.Find("GameManager").GetComponent<TimeController>();

        if (alarmaActiva) { alarmController.SonarAlarma(); }

    }
    void Update()
    {
        totalSegundos = timeController.totalSegundos;
        //if (alarmaActiva) ComprobarAlarma();
    }

    public void ComprobarAlarma()
    {

        float hora = Mathf.FloorToInt(totalSegundos / 3600);
        float minutes = Mathf.FloorToInt(totalSegundos / 60);
        minutes = Mathf.FloorToInt(minutes % 60);
        float seconds = Mathf.FloorToInt(totalSegundos % 60);

        float horaAlarma = Mathf.FloorToInt(totalSegundosAlarma / 3600);
        float minutesAlarma = Mathf.FloorToInt(totalSegundosAlarma / 60);
        minutesAlarma = Mathf.FloorToInt(minutesAlarma % 60);
        float secondsAlarma = Mathf.FloorToInt(totalSegundosAlarma % 60);

        //print("hora alarma" + horaAlarma + " minuto alarma" + minutesAlarma + " hora normal" + hora + " minutos normal" + minutes);

        if (minutesAlarma == minutes && horaAlarma == hora)
        {
            print("SONAR ALARMA");
            if (!dream && !blockComprobarAlarma) 
            {
                if (!alarmController.usandoAlarma && !alarmController.sonandoAlarma)
                {
                    alarmController.SonarAlarma();
                }
            }
        }

    }
}
