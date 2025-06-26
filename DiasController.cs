using UnityEngine;

public class DiasController : MonoBehaviour
{
    TimeController timeController;

    public GameObject[] dias;

    public bool test;

    private void Awake()
    {
        timeController = GetComponent<TimeController>();
        
        for (int i = 0; i < dias.Length; i++)
        {
            dias[i].SetActive(false);
        }
    }
    void Start()
    {
        if (!test)
        {
            for (int i = 0; i < dias.Length; i++)
            {
                if (i == timeController.dia)
                {
                    dias[i].SetActive(true);
                }
                else
                {
                    dias[i].SetActive(false);
                }
            }
        }
    }

}
