using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Consejos : MonoBehaviour
{

    public TextMeshProUGUI txt;

    public string[] consejos;

    void Start()
    {
        int aleatorio = Random.Range(0, consejos.Length);
        txt.text = consejos[aleatorio];
    }

}
