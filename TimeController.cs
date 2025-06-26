using System.Collections;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public GameObject luzSol;
    public Light luzSolTerrado;

    TextMeshProUGUI temporizadorTMP;

    public float totalSegundos = 0f;
    public float porcentajeDia;
    public float timeSpeed = 1f;
    public int delay;
    public int dia;
    public int mes;

    public int hora;
    public int minutes;
    public int diaDormido;

    public CuadriculaController cuadriculaController;
    GuardarController guardarController;
    GameController gameController;

    public string[] diasSemana;
    public bool freezeTime = false;

    private void Awake()
    {
        temporizadorTMP = GameObject.Find("HoraReloj").GetComponent<TextMeshProUGUI>();
        gameController = GameObject.Find("NodeManager")?.GetComponent<GameController>();
        guardarController = GetComponent<GuardarController>();

        totalSegundos = PlayerPrefs.GetFloat("TotalSegundos", totalSegundos);
        dia = PlayerPrefs.GetInt("Dia", dia);
        mes = PlayerPrefs.GetInt("Mes", mes);
        diaDormido = PlayerPrefs.GetInt("DiaDormido", diaDormido);
        diaDormido = 1;

        hora = (int)(totalSegundos / 3600f);
        minutes = (int)((totalSegundos / 60f) % 60f);
        temporizadorTMP.text = string.Format("{0:00}:{1:00}", hora, minutes);
    }

    void Update()
    {
        if (!freezeTime)
            totalSegundos += Time.deltaTime * timeSpeed;

        // Calcular hora y minutos (más fluido que cada 60 frames)
        hora = (int)(totalSegundos / 3600f);
        minutes = (int)((totalSegundos / 60f) % 60f);
        temporizadorTMP.text = string.Format("{0:00}:{1:00}", hora, minutes);

        // Ajuste de día
        if (totalSegundos >= 86400f)
        {
            totalSegundos -= 86400f;
            dia++;
            CambiarDia();
        }
        else if (totalSegundos < 0f)
        {
            totalSegundos += 86399f;
        }

        porcentajeDia = totalSegundos / 86400f;

        /*
        // Rotación fluida del sol
        Quaternion objetivoRotacion = Quaternion.Euler(
            (porcentajeDia * 360f) - delay,
            luzSol.transform.eulerAngles.y,
            luzSol.transform.eulerAngles.z
        );

        luzSol.transform.rotation = Quaternion.RotateTowards(
            luzSol.transform.rotation,
            objetivoRotacion,
            30f * Time.deltaTime
        );
        */

        luzSol.transform.rotation = Quaternion.Euler((porcentajeDia * 360) - delay, luzSol.transform.rotation.y, luzSol.transform.rotation.z).normalized;


        // Intensidad de luz suave
        float targetIntensity = (hora >= 20 || hora < 6) ? 1f : 9f;
        luzSolTerrado.intensity = Mathf.MoveTowards(
            luzSolTerrado.intensity,
            targetIntensity,
            Time.deltaTime * 1f
        );
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetFloat("TotalSegundos", totalSegundos);
            PlayerPrefs.SetInt("Dia", dia);
            PlayerPrefs.SetInt("Mes", mes);
            PlayerPrefs.SetInt("DiaDormido", diaDormido);
        }
    }

    public void CambiarDia()
    {
        diaDormido = 0;
        if (gameController != null)
            gameController.ActualizarDia();
    }

    public float GetHour()
    {
        return (int)(totalSegundos / 3600f);
    }

    public float GetMinutes()
    {
        return (int)((totalSegundos / 60f) % 60f);
    }
}
