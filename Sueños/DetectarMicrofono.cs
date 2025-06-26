using UnityEngine;
using UnityEngine.UI; // Para trabajar con imágenes UI en Unity
using TMPro; // Si usas TMPro para el Dropdown

public class DetectarMicrofono : MonoBehaviour
{
    public TMP_Dropdown micDropdown; // Dropdown para seleccionar el micrófono
    public Image volumeBar; // Barra de volumen (fillAmount)
    public string micDevice; // Nombre del micrófono seleccionado
    private AudioClip micClip;
    private bool micInitialized;

    public float detectionThreshold = 0.05f; // Umbral de detección para acción
    public float maxVolume = 0.2f; // Volumen máximo para escalar la barra
    public float checkInterval = 0.1f; // Intervalo de chequeo en segundos
    private float nextCheckTime;

    public float volume;
    public float volumeDelay;

    public bool detectando;
    public GameObject panelMicrofono;

    void Start()
    {
        // Rellenar el Dropdown con los dispositivos de micrófono
        micDropdown.ClearOptions();
        micDropdown.AddOptions(new System.Collections.Generic.List<string>(Microphone.devices));

        if (Microphone.devices.Length > 0)
        {
            micDevice = Microphone.devices[0]; // Seleccionar el primer micrófono por defecto
            StartMicrophone(micDevice);
        }
        else
        {
            Debug.LogError("No se encontró ningún micrófono");
        }

        micDropdown.onValueChanged.AddListener(delegate { OnMicDropdownChanged(); });

        DesactivarMicrofono();

    }

    void Update()
    {

        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;

        if (!micInitialized || Time.time < nextCheckTime) return;

        nextCheckTime = Time.time + checkInterval;

        volume = GetMicVolume();
        UpdateVolumeBar(volume); // Actualizar la barra de volumen

        ComprobarVolumen();

        /*
        if(detectando)
        {
            if (volumeDelay < volume) { volumeDelay += Time.deltaTime * 0.1f; }
            else if (volumeDelay > volume) { volumeDelay -= Time.deltaTime * 0.1f; }
            UpdateVolumeBar(volumeDelay);
        }
        */
    }

    public bool ComprobarVolumen()
    {
        if (volume > detectionThreshold)
        {
            Debug.Log("¡Volumen superado!");
            OnVolumeExceeded(); // Ejecutar la función si el volumen supera el umbral
            volumeBar.color = Color.red;
            return true;
        }
        else
        {
            volumeBar.color = Color.white;
            return false;
        }
    }

    void StartMicrophone(string device)
    {
        micClip = Microphone.Start(device, true, 1, 44100);
        micInitialized = true;
    }

    public void StopMicrophone(string device)
    {
        Microphone.End(micDevice);
    }

    float GetMicVolume()
    {
        if (micClip == null) return 0f;

        float[] samples = new float[256];
        int micPosition = Microphone.GetPosition(micDevice) - samples.Length;
        if (micPosition < 0) return 0f;

        micClip.GetData(samples, micPosition);
        float sum = 0f;

        foreach (float sample in samples)
        {
            sum += sample * sample;
        }

        return Mathf.Sqrt(sum / samples.Length); // Calcular RMS
    }

    void OnMicDropdownChanged()
    {
        Microphone.End(micDevice); // Detener el micrófono actual
        micDevice = micDropdown.options[micDropdown.value].text;
        StartMicrophone(micDevice); // Empezar con el nuevo micrófono
        Debug.Log("Micrófono seleccionado: " + micDevice);
    }

    void UpdateVolumeBar(float currentVolume)
    {
        // Normalizar el volumen para ajustarlo a fillAmount (0 a 1)
        float normalizedVolume = Mathf.Clamp01(currentVolume / maxVolume * 2);
        volumeBar.fillAmount = normalizedVolume; // Actualizar fillAmount de la barra
    }



    void OnVolumeExceeded()
    {
        // Aquí puedes poner la lógica cuando el volumen supere el umbral
        Debug.Log("¡Ejecutando acción por volumen excesivo!");
    }

    public void ActivarMicrofono()
    {
        detectando = true;
        panelMicrofono.SetActive(true);

        micDropdown.ClearOptions();
        micDropdown.AddOptions(new System.Collections.Generic.List<string>(Microphone.devices));

        if (Microphone.devices.Length > 0)
        {
            micDevice = Microphone.devices[0]; // Seleccionar el primer micrófono por defecto
            StartMicrophone(micDevice);
        }
        else
        {
            Debug.LogError("No se encontró ningún micrófono");
        }

        micDropdown.onValueChanged.AddListener(delegate { OnMicDropdownChanged(); });

    }

    public void DesactivarMicrofono()
    {
        detectando = false;
        panelMicrofono.SetActive(false);

        micDropdown.ClearOptions();
        micDropdown.AddOptions(new System.Collections.Generic.List<string>(Microphone.devices));
        StopMicrophone(micDevice);

        if (Microphone.devices.Length > 0)
        {
            micDevice = Microphone.devices[0]; // Seleccionar el primer micrófono por defecto

        }
        else
        {
            Debug.LogError("No se encontró ningún micrófono");
        }

        micDropdown.onValueChanged.AddListener(delegate { OnMicDropdownChanged(); });

    }
}
