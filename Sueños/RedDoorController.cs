using UnityEngine;

public class RedDoorController : MonoBehaviour
{
    public DoorController redDoor;

    public int vecesHablado;
    public bool interactuado;

    void Start()
    {

        vecesHablado = PlayerPrefs.GetInt("RED_DOOR", vecesHablado);

        if (vecesHablado == 0)
        {
            redDoor.idDialogo = "redDoor_1";
        }
        else if(vecesHablado == 1)
        {
            redDoor.idDialogo = "redDoor_2";
        }
        else if (vecesHablado == 2)
        {
            redDoor.idDialogo = "redDoor_3";
        }
    }

    void Update()
    {
        if(!interactuado)
        {
            if(redDoor.hablado)
            {
                interactuado = true;
                vecesHablado++;
            }
        }
    }

    public void Guardar()
    {
        print("GuardarRedDor");
        PlayerPrefs.SetInt("RED_DOOR", vecesHablado);
    }
}
