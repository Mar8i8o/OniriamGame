using UnityEngine;

public class FantasmaBaile : MonoBehaviour
{
    public Animator anim;
    public FantasmaBaile fantasmaBaile;

    public float offset;
    void Awake()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        offset = Random.Range(0, 3);
        //Invoke(nameof(ActivarAnimacion), offset);
        ActivarAnimacion();
    }

    public void ActivarAnimacion()
    {
        //anim.enabled = true;
        int aleatorio = Random.Range(1, 3);
        anim.SetInteger("numBaile", aleatorio);

        if (aleatorio == 1)
        {
            anim.CrossFade("baile", 0f);
        }
        else
        {
            anim.CrossFade("baileManosArriba", 0f);
        }

        fantasmaBaile.enabled = false;

    }

}
