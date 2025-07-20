using UnityEngine;

public class FijarRotacion : MonoBehaviour
{
    public HingeJoint jointOriginal;
    private JointLimits limitesIniciales;

    void Start()
    {
        //jointOriginal = GetComponent<HingeJoint>();
        limitesIniciales = jointOriginal.limits; // Guarda una copia de los límites
    }

    void OnEnable()
    {
        if (jointOriginal != null)
        {
            // Reasigna los límites para que se recalculen en la nueva rotación
            jointOriginal.limits = limitesIniciales;
        }
    }
}
