using UnityEngine;

public class DelayAnimation : MonoBehaviour
{
    public Animator anim;

    public float offset;
    void Awake()
    {
        offset = Random.Range(0, 1.1f);
        Invoke(nameof(ActivarAnimacion), offset);
    }

    public void ActivarAnimacion()
    {
        anim.enabled = true;
    }
}
