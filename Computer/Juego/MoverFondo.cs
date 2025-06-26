using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoverFondo : MonoBehaviour
{
    public RawImage rawImage;
    public Vector2 scrollSpeed = new Vector2(0.1f, 0);

    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + scrollSpeed * Time.deltaTime, rawImage.uvRect.size);
    }
}
