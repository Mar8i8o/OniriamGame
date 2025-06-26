using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class DevMenu : MonoBehaviour
{
    public GameObject devMenu;
    public bool devMenuActivo;
    public TextMeshProUGUI consoleText; // Arrastra aquí el TextMeshPro desde el Inspector

    private List<string> logMessages = new List<string>(); // Almacena los logs
    private const int maxLines = 5; // Número máximo de líneas visibles

    GuardarController guardarController;

    public int dia;

    void Start()
    {
        guardarController = GetComponent<GuardarController>();

        // Siempre capturar los logs, aunque el menú esté cerrado
        Application.logMessageReceived += HandleLog;

    }

    void OnDestroy()
    {
        // Eliminar la suscripción al cerrar la escena
        Application.logMessageReceived -= HandleLog;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            if (!devMenuActivo)
            {
                ActivarMenu();
                dia = PlayerPrefs.GetInt("Dia", 1);
            }
            else
            {
                DesactivarMenu();
            }
        }

        if(devMenuActivo)
        {
            diaTXT.text = "Dia: " + dia;
        }

    }

    public TextMeshProUGUI diaTXT;

    void ActivarMenu()
    {
        devMenuActivo = true;
        devMenu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;

        // Mostrar logs guardados
        ActualizarConsola();
    }

    public void DesactivarMenu()
    {
        devMenuActivo = false;
        devMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
    }

    public void Guardar()
    {
        guardarController.Guardar();
        print("Datos guardados");
    }

    public GameObject gameManager;

    public void ResetearDatos()
    {
        PlayerPrefs.DeleteAll();
        print("Datos borrados");

        dia = PlayerPrefs.GetInt("Dia");

    }

    public void SumarDia()
    {
        dia += 1;
    }

    public void RestarDia()
    {
        dia -= 1;
    }

    public void GuardarDia()
    {
       PlayerPrefs.SetInt("Dia", dia);
       print("Dia guardado");
    }

    public void CambiarGameManager()
    {
        gameManager.SetActive(!gameManager.activeSelf);
    }

    public void CargarEscena(string id)
    {
        Time.timeScale = 1.0f;
        LevelLoader.LoadLevel(id);
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Warning) return; // Ignorar warnings

        logMessages.Add(logString);

        if (logMessages.Count > maxLines)
        {
            logMessages.RemoveAt(0);
        }

        if (devMenuActivo)
        {
            ActualizarConsola();
        }
    }

    void ActualizarConsola()
    {
        if (consoleText != null)
        {
            consoleText.text = string.Join("\n", logMessages);
        }
    }
}
