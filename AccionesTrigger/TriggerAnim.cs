using UnityEngine;

public class TriggerAnim : MonoBehaviour
{
    public bool activa;
    public float tiempo;

    public Animator animator;

    bool usado;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (!usado)
            {
                if (!activa)
                {
                    Invoke(nameof(DesactivarAnim), tiempo);
                }
                else
                {
                    Invoke(nameof(ActivarAnim), tiempo);
                }

                usado = true;
            }
        }

    }

    public void DesactivarAnim()
    { 
        animator.enabled = false;
    }

    public void ActivarAnim()
    {
        animator.enabled = true;
    }
}
