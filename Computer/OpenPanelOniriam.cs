using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPanelOniriam : MonoBehaviour
{
    public ScrollBarScreen scrollBarScreen;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CambiarScrollBar(ScrollRect panel)
    {
        scrollBarScreen.scrollRect = panel;
        scrollBarScreen.scrollRect.normalizedPosition = new Vector2(0, 1);
        //Canvas.ForceUpdateCanvases();
        //LayoutRebuilder.ForceRebuildLayoutImmediate(panel.content);
    }
}
