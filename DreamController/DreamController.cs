using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DreamController : MonoBehaviour
{

    public bool inDream;

    public GameObject negro;

    RelacionesController relacionesController;
    public RedDoorController redDoorController;
    TimeController timeController;

    public string[] dreamsNames;

    public string dreamToday;

    void Start()
    {
        relacionesController = GameObject.Find("GameManager").GetComponent<RelacionesController>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        if (!inDream) {
            timeController = GetComponent<TimeController>();
            print("Calcular sueño " + dreamsNames[timeController.dia]);
            dreamToday = dreamsNames[timeController.dia];
        }

    }
    void Update()
    {
        if (inDream)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                negro.SetActive(true);
                Invoke(nameof(Despertarse), 0.01f);
                //Despertarse();
            }
        }

        if(despertandose)
        {
            mainCamera.fieldOfView += Time.deltaTime * 500;
        }
    }

    bool despertandose;
    Camera mainCamera;
    public void Despertarse()
    {
        despertandose = true;
        if (redDoorController != null) { redDoorController.Guardar(); }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        relacionesController.Guardar();
        //negro.SetActive(true);
        PlayerPrefs.SetInt("SpawnAbrirOjos", 1);

        int dia = PlayerPrefs.GetInt("Dia");

        if (PlayerPrefs.GetInt("DiaDormido", 1) == 1)
        {
            dia = dia + 1;
        }
        PlayerPrefs.SetFloat("TotalSegundos", 32400); // 9 am
        PlayerPrefs.SetInt("Dia", dia);
        PlayerPrefs.SetInt("DiaDormido", 1);

        Invoke(nameof(CambiarEscenaWoke), 0.3f);
    }

    public void ForzarDespertarseApartamento()
    {
        despertandose = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        negro.SetActive(true);
        PlayerPrefs.SetInt("SpawnAbrirOjos", 1);

        Invoke(nameof(CambiarEscenaWoke), 0.3f);
    }

    public void CambiarEscenaWoke()
    {
        //SceneManager.LoadScene("SampleScene");
        negro.SetActive(true);
        LevelLoader.LoadLevel("SampleScene");
    }


    public void Dormirse()
    {
        //SceneManager.LoadScene(dreamToday);
        LevelLoader.LoadLevel(dreamToday);
    }
}
