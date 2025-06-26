using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapaBaterController : MonoBehaviour
{
    public Animator tapaBaterAnim;
    public GameObject triggerWC;
    public BaterController baterController;
    public bool open;

    public bool blockPis;

    void Start()
    {
        triggerWC.GetComponent<BoxCollider>().enabled = open;

        if (open)
        {
            open = true;
            blockPis = false;
            tapaBaterAnim.SetBool("open", true);
            baterController.haciendoPis = false;
            baterController.tiempoSinHacerPis = 0;
            triggerWC.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            open = false;
            blockPis = true;
            tapaBaterAnim.SetBool("open", false);
            baterController.haciendoPis = false;
            baterController.tiempoSinHacerPis = 0;
            triggerWC.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void Update()
    {
        
    }

    public void BlockPis()
    {
        blockPis = true;
    }

    public void UnBlockPis()
    {
        blockPis = false;
    }

    public void Open()
    {  
        if (!open) ///ABRIR
        {
            open = true;

            //CancelInvoke(nameof(BlockPis));
            Invoke(nameof(UnBlockPis),0.1f);

            tapaBaterAnim.SetBool("open", true);
            baterController.haciendoPis = false;
            baterController.tiempoSinHacerPis = 0;
            triggerWC.GetComponent<BoxCollider>().enabled = true;
        }
        else ///CERRAR
        {
            open = false;

            CancelInvoke(nameof(UnBlockPis));
            //Invoke(nameof(BlockPis), 1);
            blockPis = true;

            tapaBaterAnim.SetBool("open", false);
            baterController.haciendoPis = false;
            baterController.tiempoSinHacerPis = 0;
            triggerWC.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
