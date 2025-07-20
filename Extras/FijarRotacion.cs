using UnityEngine;

public class FijarRotacion : MonoBehaviour
{
    public HingeJoint jointOriginal;
    private JointLimits limitesIniciales;

    void Start()
    {
        //jointOriginal = GetComponent<HingeJoint>();
        limitesIniciales = jointOriginal.limits; // Guarda una copia de los l�mites
    }

    void OnEnable()
    {
        if (jointOriginal != null)
        {
            // Reasigna los l�mites para que se recalculen en la nueva rotaci�n
            jointOriginal.limits = limitesIniciales;
        }
    }
}
