using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

public class Narrativa_s2_alter : MonoBehaviour
{
    public NpcDialogue npcDialogueChica;
    public MoverNPC MoveralterK;
    public Animator alterAnim;

    public GameObject panelDialogos;
    public DialogeController dialogeController;
    public SceneDialogsController sceneDialogsController;

    public bool chicaDead;

    public GameObject dondeMiraApunyalar;
    public Animator chicaAnim;
    public CameraShake cameraShake;
    public ForzarMirada forzarMiradaChica;

    public Transform spawnAlter;
    public Transform destinoAlter1;

    public Material materialChica;
    public Texture texturaChicaHerida;
    public Texture texturaChicaNormal;

    public GameObject lamparas;
    public GameObject fantasmas;


    public Volume volume;
    public FilmGrain grain;

    public GameObject Player;
    public GameObject npcMejorAmigo;

    public DoorController[] puertaDoble;
    public GameObject positionMirarHuida;
    public GameObject luzEntrada;

    public CamaraFP camaraFP;

    public ControlarCaerse2 controlarCaerse;

    private void Awake()
    {
        materialChica.SetTexture("_BaseColorMap", texturaChicaNormal);
        volume.profile.TryGet<FilmGrain>(out grain);
        luzEntrada.SetActive(false);
    }
    void Start()
    {
        //MoveralterK.agent.enabled = false;
        //MoveralterK.gameObject.transform.position = spawnAlter.position;
        //MoveralterK.agent.enabled = true;
        MuerteChica();

        //cameraShake.shake = true;

    }

    void Update()
    {
        /*
        if(npcDialogue.hablado)
        {
            if(!panelDialogos.activeSelf)
            {
                if (!chicaDead)
                {
                    print("MuerteChica");
                    MuerteChica();
                }
            }
        }
        */

        if (persiguiendoPlayer) { PerseguirPlayer(); }

        if(forzandoMirada)
        {
            camaraFP.ForzarMiradaX(positionMirarHuida.transform, 5);
            camaraFP.ForzarMiradaY(positionMirarHuida.transform, 5);
        }


    }

    private void LateUpdate()
    {
        if (yendoAMatarChica)
        {
            MoveralterK.ForzarMiradaY(dondeMiraApunyalar.transform, 5f);
        }
    }

    public bool yendoAMatarChica;

    public void MuerteChica()
    {
        chicaDead = true;

        //dialogeController.blockPasarDialog = true;
        yendoAMatarChica = true;
        MoveralterK.agent.enabled = false;
        MoveralterK.gameObject.transform.position = spawnAlter.position;
        MoveralterK.agent.enabled = true;
        alterAnim.SetBool("Run", true);
        MoveralterK.runing = true;
        MoveralterK.gameObject.SetActive(true);
        MoveralterK.freeze = false;
        npcDialogueChica.canTalk = false;
        //chicaAnim.SetBool("Dead", true);

        Invoke(nameof(AnimacionMuerte1), 1);

        //cameraShake.shake = true;

        grain.intensity.value = 100f;

    }

    public void AnimacionMuerte1()
    {
        alterAnim.SetBool("Run", false);
        MoveralterK.runing = false;
        alterAnim.SetTrigger("Kill");
        print("Matar");
        Invoke(nameof(CambiarSpriteChica), 0.5f);
    }

    public void CambiarSpriteChica()
    {
        materialChica.SetTexture("_BaseColorMap", texturaChicaHerida);
        Invoke(nameof(AnimacionMuerte2), 0.8f);

    }

    public void AnimacionMuerte2()
    {
        chicaAnim.SetBool("Dead", true);
        Invoke(nameof(ApagarLuces), 0.5f);

        sceneDialogsController.ReactivarMovimiento();
        controlarCaerse.enabled = true;
        panelDialogos.SetActive(false);

    }

    public void ApagarLuces()
    {
        lamparas.SetActive(false);
        fantasmas.SetActive(false);
        npcMejorAmigo.SetActive(false);
        Invoke(nameof(EncenderLuces), 0.5f);
        yendoAMatarChica = false;
    }

    public void EncenderLuces()
    {
        lamparas.SetActive(true);
        cameraShake.shake = false;
        luzEntrada.SetActive(true);

        Invoke(nameof(IniciarPerseguirPlayer1), 1);
        Invoke(nameof(IniciarPerseguirPlayer2), 3);
    }

    public bool persiguiendoPlayer;
    bool forzandoMirada;

    public PensamientoControler pensamientoControler;

    public void IniciarPerseguirPlayer1()
    {
        puertaDoble[0].SetPuertaAbierta();
        puertaDoble[1].SetPuertaAbierta();

        pensamientoControler.MostrarPensamiento("Corre", 2);

        forzandoMirada = true;

    }

    public void IniciarPerseguirPlayer2()
    {
        MoveralterK.persiguiendoPlayer = true;

        persiguiendoPlayer = true;
        MoveralterK.agent.speed = 2;
        MoveralterK.agent.stoppingDistance = 1.2f;
        MoveralterK.destino = Player.transform;
        MoveralterK.freeze = false;
        MoveralterK.persiguiendoPlayer = true;
        MoveralterK.sabeDondeEsta = true;
        MoveralterK.freezeRun = true;

        forzandoMirada = false;

        Invoke(nameof(UnFreezeRunEnemy), 8);

    }

    public void UnFreezeRunEnemy()
    {
        MoveralterK.freezeRun = false;
    }

    public void PerseguirPlayer()
    {

    }
}
