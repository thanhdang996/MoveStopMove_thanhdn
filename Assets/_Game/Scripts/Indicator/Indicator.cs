using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : GameUnit
{
    [SerializeField] private RectTransform rtf;
    public RectTransform RTF => rtf;

    [SerializeField] Image image;

    public void ShowIndicator()
    {
        image.enabled = true;
        //gameObject.SetActive(true);
    }
    public void HideIndicator()
    {
        image.enabled = false;
        //gameObject.SetActive(false);
    }
}
