using UnityEngine;

public class ForzarMirada : MonoBehaviour
{

    public bool mirarAlHablar;
    public NpcDialogue npcDialogue;

    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void Update()
    {
        if(mirarAlHablar)
        {
            if(npcDialogue.dialogoActivo)
            {
                ForzarMiradaY(player.transform, 5);
            }
        }
    }

    public void ForzarMiradaY(Transform dondeMira, float speed)
    {
        // Obtén la dirección hacia el objetivo
        Vector3 targetDirection2 = dondeMira.transform.position - transform.position;
        targetDirection2.y = 0f; // Asegúrate de que no haya rotación vertical

        // Calcula la rotación necesaria para mirar hacia el objetivo solo en el eje Y
        Quaternion targetRotation2 = Quaternion.LookRotation(targetDirection2);

        // Ajusta la rotación para que solo afecte el eje Y
        targetRotation2 = Quaternion.Euler(0f, targetRotation2.eulerAngles.y, 0f);

        // Aplica la rotación de forma suavizada
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation2, speed * Time.deltaTime);
    }
}
