using Unity.VisualScripting;
using UnityEngine;
public class FantasmaKidCarta : MonoBehaviour
{
    [Range(0f, 1f)]
    public float opacity = 0.5f; // La opacidad deseada (entre 0 y 1)
    public Renderer objectRenderer;
    private MaterialPropertyBlock propBlock;


    public AudioSource sonido;
    public ItemAtributes itemCarta;
    public bool desactivar;
    void Start()
    {
        propBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {

            if (opacity < 0) { opacity = 0; Destroy(gameObject); }
            if (opacity > 1) { opacity = 1; }

            // Obtén la información actual del material
            objectRenderer.GetPropertyBlock(propBlock);

            // Asigna un nuevo color con la opacidad deseada
            Color baseColor = objectRenderer.sharedMaterial.GetColor("_BaseColor");
            baseColor.a = opacity;  // Ajusta la opacidad deseada (0 a 1)

            // Modifica la propiedad del color base del material en este objeto específico
            propBlock.SetColor("_BaseColor", baseColor);

            // Aplica los cambios
            objectRenderer.SetPropertyBlock(propBlock);

        if(itemCarta.pickUp)
        {
            desactivar = true;
        }
        
        if(desactivar) 
        {
            sonido.volume -= Time.deltaTime;
            opacity -= Time.deltaTime * 5;
        }


    }
}
