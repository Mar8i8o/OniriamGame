using UnityEngine;

public class LookPlayer : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        //ForzarMiradaY(target.transform, 100);
        transform.LookAt(target.transform.position);
    }

    public void ForzarMiradaY(Transform dondeMira, float speed)
    {
        // Obt�n la direcci�n hacia el objetivo
        Vector3 targetDirection2 = dondeMira.transform.position - transform.position;
        targetDirection2.y = 0f; // Aseg�rate de que no haya rotaci�n vertical

        // Calcula la rotaci�n necesaria para mirar hacia el objetivo solo en el eje Y
        Quaternion targetRotation2 = Quaternion.LookRotation(targetDirection2);

        // Ajusta la rotaci�n para que solo afecte el eje Y
        targetRotation2 = Quaternion.Euler(0f, targetRotation2.eulerAngles.y, 0f);

        // Aplica la rotaci�n de forma suavizada
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation2, speed * Time.deltaTime);
    }
}
