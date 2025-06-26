using UnityEngine;

public class TriggerControlarCaerse : MonoBehaviour
{
    
    ControlarCaerse2 caerseController;

    void Awake()
    {
        caerseController = GameObject.Find("Player").GetComponent<ControlarCaerse2>();  
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if(!caerseController.estadoCaidaLevantarse && !caerseController.cayendose)
            {
                caerseController.enabled = false;
            }
        }
    }
}
