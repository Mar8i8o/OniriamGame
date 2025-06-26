using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivarItem : MonoBehaviour
{

    public GameObject objetoActivar;

    public GameObject[] multiplesItems;

    public bool desactiva;

    private void Start()
    {
        if (!desactiva) {

            if (objetoActivar != null) { objetoActivar.SetActive(false); }

            for (int i = 0; i < multiplesItems.Length; i++)
            {
                multiplesItems[i].gameObject.SetActive(false);
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //Destroy(gameObject);
            if(desactiva)
            {
                if (objetoActivar != null) { objetoActivar.SetActive(false); }

                for(int i = 0; i < multiplesItems.Length; i++)
                {
                    multiplesItems[i].gameObject.SetActive(false);
                }

            }
            else
            {
                if (objetoActivar != null) { objetoActivar.SetActive(true); }

                for (int i = 0; i < multiplesItems.Length; i++)
                {
                    multiplesItems[i].gameObject.SetActive(true);
                }

            }
        }
    }
}
