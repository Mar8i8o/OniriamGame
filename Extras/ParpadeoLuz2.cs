using UnityEngine;
using System.Collections;

public class ParpadeoLuz2 : MonoBehaviour
{
    public GameObject luz;
    public bool encendido;
    public float MaxTiempoEncendido;
    public float MaxTiempoApagado;

    private void Start()
    {
        StartCoroutine(ControlarParpadeo());
    }

    private IEnumerator ControlarParpadeo()
    {
        while (true)
        {
            if (encendido)
            {
                luz.SetActive(true);
                yield return new WaitForSeconds(MaxTiempoEncendido);
                encendido = false;
            }
            else
            {
                luz.SetActive(false);
                yield return new WaitForSeconds(MaxTiempoApagado);
                encendido = true;
            }
        }
    }
}
