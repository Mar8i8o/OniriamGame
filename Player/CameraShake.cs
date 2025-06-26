using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public Animator anim;

    public bool shake;
    void Start()
    {
        
    }

    void Update()
    {
        anim.SetBool("shake", shake);
    }

    public void CameraShakeMomentaneo(float duracion)
    {
        CancelInvoke(nameof(StopAnim));
        anim.enabled = true;
        shake = true;
        Invoke(nameof(StopShake), duracion);
    }

    public void StopShake()
    {
        shake = false;
        Invoke(nameof(StopAnim), 1);
    }

    public void StopAnim()
    {
        anim.enabled = false;
    }
}
