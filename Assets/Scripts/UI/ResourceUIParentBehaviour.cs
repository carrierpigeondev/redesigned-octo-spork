using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUIParentBehaviour : MonoBehaviour
{
    public TMP_Text[] ResourceTexts;

    public bool IsOpen = false;

    private RectTransform r;

    private void Start()
    {
        r = GetComponent<RectTransform>();
    }

    public void MoveOut()
    {
        r.anchoredPosition = new Vector3(700f, 0f, 0f);
    }

    public void MoveIn()
    {
        r.anchoredPosition = new Vector3(0f, 0f, 0f);
    }
}
