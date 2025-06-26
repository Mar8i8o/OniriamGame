using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardarController : MonoBehaviour
{
    public bool guardando;
    PensamientoControler pensamientoControler;
    GameController gameController;
    DreamController dreamController;
    void Start()
    {
        dreamController = GameObject.Find("GameManager").GetComponent<DreamController>();
        pensamientoControler = GameObject.Find("GuardandoTXT").GetComponent<PensamientoControler>();
        if (!dreamController.inDream) {

            if (GameObject.Find("NodeManager") != null)
            {
                gameController = GameObject.Find("NodeManager").GetComponent<GameController>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Guardar();
        }
    }

    void LateUpdate()
    {
        if (guardando)
        {
            if (!dreamController.inDream)
            {
                if (gameController != null) { gameController.GuardarEstadoNodos(); }
                //guardando = false; // Resetear el estado de guardando
            }
        }
    }

    public void Guardar()
    {
        if (!guardando)
        {
            guardando = true;
            pensamientoControler.MostrarPensamiento("Guardando", 1f);
            Invoke(nameof(DejarDeGuardar), 0.1f);
        }
    }

    public void DejarDeGuardar()
    {
        guardando = false;
    }

    [ContextMenu(itemName: "Delete_PlayerPrefs")]

    public void deletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All player prefs deleted");
    }
}
