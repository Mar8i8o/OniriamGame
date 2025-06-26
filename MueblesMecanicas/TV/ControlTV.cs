using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class ControlTV : MonoBehaviour
{
    public GameObject pantallaEncendida;
    public GameObject pantallaApagada;

    public bool encendida;
    private bool bloqueadoCambioEstado = false;

    public VideoPlayer videoPlayer;
    public double timeVideo;
    public double lastTimeVideo;

    public VideoClip[] videos;
    public int indiceVideo;

    public VCR_Controller vcrScript;

    GuardarController guardarController;
    ElectricidadController electricidadController;
    PensamientoControler pensamientoControler;

    void Start()
    {
        guardarController = GameObject.Find("GameManager").GetComponent<GuardarController>();
        electricidadController = GameObject.Find("ElectricidadControler").GetComponent<ElectricidadController>();
        pensamientoControler = GameObject.Find("PensamientoController").GetComponent<PensamientoControler>();

        encendida = PlayerPrefs.GetInt("TVEncendida", 0) == 1;
        indiceVideo = PlayerPrefs.GetInt("IndiceVideo", 0);

        if (videos != null && videos.Length > 0 && indiceVideo >= 0 && indiceVideo < videos.Length)
        {
            videoPlayer.clip = videos[indiceVideo];
        }

        if (encendida)
            StartCoroutine(EncenderTV());
        else
            ApagarTVDirecto();

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        if (encendida && videoPlayer.clip != null)
        {
            timeVideo = videoPlayer.time;

            if (!electricidadController.electricidad)
                StartCoroutine(ApagarTV());
        }
    }

    private void LateUpdate()
    {
        if (guardarController.guardando)
        {
            PlayerPrefs.SetInt("TVEncendida", encendida ? 1 : 0);
            PlayerPrefs.SetInt("IndiceVideo", indiceVideo);
        }
    }

    public void InteractuarTV()
    {
        if (bloqueadoCambioEstado) return;

        if (!electricidadController.electricidad)
        {
            pensamientoControler.MostrarPensamiento("No puedo encender la tele sin electricidad", 1);
            return;
        }

        if (encendida)
            StartCoroutine(ApagarTV());
        else
            StartCoroutine(EncenderTV());
    }

    IEnumerator ApagarTV()
    {
        bloqueadoCambioEstado = true;

        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            lastTimeVideo = videoPlayer.time;
            videoPlayer.Pause();
            yield return null;
        }

        pantallaApagada.SetActive(true);
        pantallaEncendida.SetActive(false);

        encendida = false;

        yield return new WaitForSecondsRealtime(0.3f);
        bloqueadoCambioEstado = false;
    }

    void ApagarTVDirecto()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
            videoPlayer.Pause();

        pantallaApagada.SetActive(true);
        pantallaEncendida.SetActive(false);

        encendida = false;
    }

    IEnumerator EncenderTV()
    {
        bloqueadoCambioEstado = true;

        pantallaApagada.SetActive(false);
        pantallaEncendida.SetActive(true);

        encendida = true;

        if (videoPlayer != null && videoPlayer.clip != null)
        {
            videoPlayer.time = lastTimeVideo;
            videoPlayer.Prepare();

            while (!videoPlayer.isPrepared)
                yield return null;

            videoPlayer.Play();
        }

        yield return new WaitForSecondsRealtime(0.3f);
        bloqueadoCambioEstado = false;
    }

    public void ReproducirVHS()
    {
        if (vcrScript.vhsInterior != null && vcrScript.vhsInterior.videoVHS != null)
        {
            videoPlayer.clip = vcrScript.vhsInterior.videoVHS;
            lastTimeVideo = 0;
            videoPlayer.time = 0;
            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("No se puede reproducir VHS: video no asignado.");
        }
    }

    public void QuitarVHS()
    {
        if (videos != null && videos.Length > 0 && indiceVideo >= 0 && indiceVideo < videos.Length)
        {
            videoPlayer.clip = videos[indiceVideo];
            videoPlayer.time = lastTimeVideo;
            videoPlayer.Play();
        }
    }

    void OnVideoEnd(VideoPlayer player)
    {
        if (!encendida || vcrScript.VHSDentro) return;

        indiceVideo++;

        if (indiceVideo >= videos.Length)
            indiceVideo = 0;

        if (videos != null && videos.Length > 0 && indiceVideo >= 0 && indiceVideo < videos.Length)
        {
            videoPlayer.clip = videos[indiceVideo];
            lastTimeVideo = 0;
            videoPlayer.time = 0;
            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("No hay más videos válidos para reproducir.");
        }
    }
}
