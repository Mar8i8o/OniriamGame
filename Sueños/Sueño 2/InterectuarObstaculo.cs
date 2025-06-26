using UnityEngine;
using UnityEngine.UI;

public class InterectuarObstaculo : MonoBehaviour
{
    public Transform obstaculoHijo; // Referencia al hijo que queremos mover
    public Vector3 posicionLocalFinal; // Posici�n final local
    public Vector3 rotacionLocalFinal; // Rotaci�n final local
    public float velocidad = 1f; // Velocidad de interacci�n
    public float velocidadCaida = 2f; // Velocidad de "ca�da" al soltar

    private Vector3 posicionLocalInicial;
    private Quaternion rotacionLocalInicial;
    private float progresoAccion = 0f;
    private bool interactuando = false;
    private bool cayendo = false;

    GameObject barraProgreso;
    Image barraProgresoImage;

    private void Awake()
    {
        barraProgreso = GameObject.Find("barraProgreso");
        barraProgresoImage = barraProgreso.GetComponent<Image>();
    }

    void Start()
    {

        // Guardar posici�n y rotaci�n locales iniciales del hijo
        if (obstaculoHijo != null)
        {
            posicionLocalInicial = obstaculoHijo.localPosition;
            rotacionLocalInicial = obstaculoHijo.localRotation;
        }
        if (navMeshObstacle != null) { navMeshObstacle.SetActive(true); }

        barraProgreso.SetActive(false);
    }

    void Update()
    {
        if (interactuando && obstaculoHijo != null)
        {
            // Incrementar el progreso mientras se mantiene clic
            progresoAccion += Time.deltaTime * velocidad;

            // Limitar el progreso a 100
            progresoAccion = Mathf.Clamp(progresoAccion, 0f, 100f);

            // Interpolar entre localPosition y localRotation iniciales y finales
            obstaculoHijo.localPosition = Vector3.Lerp(posicionLocalInicial, posicionLocalFinal, progresoAccion / 100f);
            obstaculoHijo.localRotation = Quaternion.Lerp(rotacionLocalInicial, Quaternion.Euler(rotacionLocalFinal), progresoAccion / 100f);

            // Comprobar si se alcanz� el 100% para finalizar
            if (progresoAccion >= 100f)
            {
                interactuando = false;
                cayendo = false; // Aseguramos que no est� cayendo
                obstaculoMovido = true;
                print("Obst�culo movido completamente.");
                gameObject.layer = 2;
                if (navMeshObstacle != null) { navMeshObstacle.SetActive(false); }
                barraProgreso.SetActive(false);
                if(avisaAlEnemigo) 
                { 
                    moverNPC.sabeDondeEsta = true;
                    moverNPC.patrullando = false;
                } 
            }

            tiempoInteractuando += Time.deltaTime;
            if (tiempoInteractuando > 0.1) { DejarDeInteractuar(); }

            barraProgresoImage.fillAmount = progresoAccion / 100;

        }
        else if (cayendo && obstaculoHijo != null)
        {
            // Incrementar progresivamente la velocidad de ca�da
            velocidadActualCaida += Time.deltaTime * velocidadCaida;

            // Mover la posici�n hacia el objetivo de forma suave y din�mica
            obstaculoHijo.localPosition = Vector3.MoveTowards(obstaculoHijo.localPosition, posicionLocalInicial, velocidadActualCaida * Time.deltaTime);
            obstaculoHijo.localRotation = Quaternion.RotateTowards(obstaculoHijo.localRotation, rotacionLocalInicial, velocidadActualCaida * Time.deltaTime * 50f);

            // Comprobar si se ha alcanzado la posici�n inicial
            if (obstaculoHijo.localPosition == posicionLocalInicial && obstaculoHijo.localRotation == rotacionLocalInicial)
            {
                cayendo = false;
                velocidadActualCaida = 0f; // Reiniciar la velocidad
                print("Obst�culo ha regresado a su posici�n inicial.");
            }
        }
    }

    public bool obstaculoMovido;

    public float tiempoInteractuando;
    public float velocidadActualCaida;
    public AnimationCurve curvaMovimiento;

    public GameObject navMeshObstacle;

    public bool avisaAlEnemigo;
    public MoverNPC moverNPC;

    public void Interactuar()
    {
        // Solo reiniciar si no se estaba ya interactuando

        if(obstaculoMovido) { return; }

        if (!interactuando && !cayendo)
        {
            interactuando = true;
            cayendo = false;
            progresoAccion = 0f; // Reiniciar el progreso

            // Resetear la posici�n y rotaci�n iniciales del obst�culo
            if (obstaculoHijo != null)
            {
                //obstaculoHijo.localPosition = posicionLocalInicial;
                //obstaculoHijo.localRotation = rotacionLocalInicial;
            }

            print("Interactuando con el obst�culo desde el inicio.");
            barraProgresoImage.fillAmount = progresoAccion / 100;
            barraProgreso.SetActive(true);
        }

        tiempoInteractuando = 0;
        velocidadActualCaida = 0f;
    }

    public void DejarDeInteractuar()
    {
        if (progresoAccion < 100f && obstaculoHijo != null)
        {
            // Activar la simulaci�n de ca�da si no se complet� la interacci�n
            interactuando = false;
            barraProgreso.SetActive(false);
            cayendo = true;
            print("Progreso perdido, el obst�culo est� cayendo.");
        }
    }
}
