using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerColocarCafe : MonoBehaviour
{

    public TriggerCafetera triggerCafetera;

    public ItemAtributes cafeInterior;

    public Collider col;

    private void Update()
    {
        if (triggerCafetera.tieneTaza)
        {
            col.enabled = false;
        }
        else
        {
            col.enabled = true;
        }
    }
    public void ColocarCafe()
    {
        triggerCafetera.ColocarCafe(cafeInterior);
    }
}
