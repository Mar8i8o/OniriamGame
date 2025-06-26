using TMPro;
using UnityEngine;

public class MostrarTitular : MonoBehaviour
{
    public NoticiasManager noticiasManager;
    public TextMeshProUGUI text;
    void Start()
    {
        text.text = noticiasManager.noticiaTitular;
    }

    private void OnEnable()
    {
        text.text = noticiasManager.noticiaTitular;
    }
}
