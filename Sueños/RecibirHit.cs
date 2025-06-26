using UnityEngine;

public class RecibirHit : MonoBehaviour
{

    public bool dealer;
    public NPC_Dealer nPC_Dealer;
    public MoverNPC moverNPC;


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("PickableObject"))
        {
            print("RecibirHit");
            SetRecibirHit();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("PickableObject"))
        {
            print("RecibirHit");
            SetRecibirHit();
        }
    }

    public void SetRecibirHit()
    {
        if (!dealer) { moverNPC.RecibirHit(); }
        else { nPC_Dealer.RecibirHit();}
    }
}
