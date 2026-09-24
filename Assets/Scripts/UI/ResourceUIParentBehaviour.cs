using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUIParentBehaviour : MonoBehaviour
{
    public TMP_Text[] ResourceTexts;
    [SerializeField] private RawImage background;

    private void Start()
    {
        Texture2D tex = background.texture as Texture2D;
        tex.Apply();

        Color newC = new Color(0f, 0f, 0f, 0f);
        tex.SetPixel(10, 10, newC);
        tex.Apply();
    }
}
