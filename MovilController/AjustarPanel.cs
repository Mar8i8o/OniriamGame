using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UI;

public class AjustarPanel : MonoBehaviour
{
    public GameObject[] contactos;
    public float offsetPanel;
    public float offsetPos;

    public float multiplyPanel;
    public float multiplyPos;

    public GameObject panel;
    public GameObject layoutGroup;

    public string thisTag;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (panel.activeSelf)
        {
            contactos = GameObject.FindGameObjectsWithTag(thisTag);
            if (contactos.Length != 0)
            {
                if (contactos.Length <= 4)
                {
                    offsetPanel = 1;
                    panel.GetComponent<RectTransform>().anchorMax = new Vector3(transform.localScale.x, offsetPanel, transform.localScale.z);

                }
                else
                {
                    offsetPanel = 1 + (contactos.Length - 4) * multiplyPanel;
                    offsetPos = (contactos.Length - 4) * multiplyPos;
                    panel.GetComponent<RectTransform>().anchorMax = new Vector3(transform.localScale.x, offsetPanel, transform.localScale.z);
                    layoutGroup.GetComponent<RectTransform>().pivot = new Vector3(0, -offsetPos, 0);
                    //verLayoutGroup.padding.top = -offsetPos;
                    //layoutGroup.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + offsetPos, transform.localPosition.z);
                }
            }
        }
    }
}
