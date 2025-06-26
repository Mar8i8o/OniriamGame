using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

public class PlayerVida : MonoBehaviour
{
    public int vidas = 3;
    CameraShake cameraShake;
    CamaraFP camaraFP;


    public Volume volume;
    Vignette vigrette;
    public float intensidadVigrete;
    public Animator sangrePantalla;
    public bool proteccion;
    bool manteniendoDanyo;

    public ParticleSystem particulasSangre;

    IndicadorAtrapasuenos indicadorAtrapasuenos;

    void Start()
    {

        indicadorAtrapasuenos = GameObject.Find("Main Camera").GetComponent<IndicadorAtrapasuenos>();

        cameraShake = GetComponent<CameraShake>();
        volume.profile.TryGet<Vignette>(out vigrette);

        camaraFP = GetComponent<CamaraFP>();

        vidas = 3 + indicadorAtrapasuenos.atrapasuenosVida;
    }

    void Update()
    {
        if(vidas < 3)
        {
            ControlarVolume();
            if (particulasSangre != null)
            {
                if (!camaraFP.freeze)
                {
                    if (!particulasSangre.isEmitting) { particulasSangre.Play(); }
                    print("MoverSangre");
                    particulasSangre.gameObject.transform.position = new Vector3(transform.position.x, particulasSangre.gameObject.transform.position.y, transform.position.z);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.F2)) { RecibirDanyo(); }
    }

    public void ControlarVolume()
    {
        if (vidas == 2) { intensidadVigrete = 0.2f; }
        else if (vidas == 1) { intensidadVigrete = 0.5f; }
        else if (vidas <= 0) { intensidadVigrete = 0.8f; }

        if(vigrette.intensity.value > intensidadVigrete) { vigrette.intensity.value -= Time.deltaTime; }
        else if(vigrette.intensity.value < intensidadVigrete) { vigrette.intensity.value += Time.deltaTime; }

    }

    public void RecibirDanyo()
    {

        if (proteccion) { return; }

        proteccion = true;

        Invoke(nameof(QuitarProteccion), 1);

        cameraShake.CameraShakeMomentaneo(1);
        sangrePantalla.SetBool("sangrePantalla", true);
        vidas--;

        CancelInvoke(nameof(QuitarSangre));
        if (vidas == 2) { Invoke(nameof(QuitarSangre), 0.1f); }
        else if (vidas == 1) { Invoke(nameof(QuitarSangre), 0.4f); }
        else if (vidas <= 0) { Invoke(nameof(QuitarSangre), 0.7f); }
    }

    public void MantenerDanyo()
    {
        cameraShake.shake = true;
        sangrePantalla.SetBool("sangrePantallaLento", true);
        vigrette.intensity.value += Time.deltaTime * 0.2f;
        manteniendoDanyo = true;
    }

    public void SoltarDanyo()
    {
        cameraShake.shake = false;
        sangrePantalla.SetBool("sangrePantallaLento", false);
        if (vidas == 3) { vigrette.intensity.value = 0; }
        manteniendoDanyo = false;
    }

    public void QuitarSangre()
    {
        sangrePantalla.SetBool("sangrePantalla", false);
    }

    public void QuitarProteccion()
    {
        proteccion = false;
    }
}
