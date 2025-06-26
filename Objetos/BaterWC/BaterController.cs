using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaterController : MonoBehaviour
{
    public ParticleSystem particulasPis;
    public Renderer aguaPis;
    PlayerStats playerStats;

    public float tiempoSinHacerPis;

    public bool haciendoPis;

    public float suciedad;

    DreamController dreamController;

    public TapaBaterController tapaBaterController;

    void Start()
    {
        particulasPis = GameObject.Find("ParticulasPis").GetComponent<ParticleSystem>();
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
        dreamController = GameObject.Find("GameManager").GetComponent<DreamController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (haciendoPis)
        {

            if (!dreamController.inDream)
            {
                playerStats.pis -= Time.deltaTime * 15;
                playerStats.agua -= Time.deltaTime * 0.5f;

                if (playerStats.pis <= 0.1)
                {
                    playerStats.pis = -10;
                    haciendoPis = false;
                    tiempoSinHacerPis = 0;
                    particulasPis.Stop();
                }
            }

            tiempoSinHacerPis += Time.deltaTime;

            if (tiempoSinHacerPis > 0.3f)
            {
                haciendoPis = false;
                tiempoSinHacerPis = 0;
                particulasPis.Stop();
            }


        }

        aguaPis.material.color = new Color(aguaPis.material.color.r, aguaPis.material.color.g, aguaPis.material.color.b, alpha);
    }

    public void HacerPis() //SE LLAMA DESDE EL RAYCAST MIENTRAS ESTAS MANTENIENDO, RESETEANDO EL TIEMPO A 0 PARA DETECTAR CUANDO HA SOLTADO EL RATON
    {

        if (tapaBaterController.blockPis) { print("blockPis"); return; }

        if (playerStats.pis > 0 || dreamController.inDream) 
        {
            print("pis");
            if (!particulasPis.isEmitting) { particulasPis.Simulate(0, true, true); }
            particulasPis.Play();
            haciendoPis = true;
            tiempoSinHacerPis = 0;

            if(dreamController.inDream) { PlayerPrefs.SetInt("camaPrincipalSucia", System.Convert.ToInt32(true)); }

        }
        
    }

    public void LimpiarAgua()
    {
        suciedad = 0;
        alpha = 0;
    }

    public float alpha;
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("PisTrigger"))
        {
            if (haciendoPis)
            {
                if (suciedad < 7)
                {
                    suciedad += Time.deltaTime * 2;
                    alpha += Time.deltaTime * 0.2f;
                }
                
            }
        }
    }
}
