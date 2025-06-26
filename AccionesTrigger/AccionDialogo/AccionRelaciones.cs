using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionRelaciones : MonoBehaviour
{
    public RelacionesController relacionesController;
    public DreamController dreamController;

    public float cuantoSuma;

    public bool kid;
    public bool killer;

    public bool seDespierta;

    void Start()
    {
        if(kid)
        {
            relacionesController.relacionAlterKid += cuantoSuma;
        }
        if (killer)
        {
            relacionesController.relacionAlterKiller += cuantoSuma;
        }

        if(seDespierta)
        {
            Invoke(nameof(Despertarse), 0.2f);
        }
    }

    public void Despertarse()
    {
        dreamController.Despertarse();
    }

}
