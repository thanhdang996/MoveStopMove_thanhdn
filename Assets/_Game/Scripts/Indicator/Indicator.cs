using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Indicator : GameUnit
{
    [SerializeField] private RectTransform rtf;
    public RectTransform RTF => rtf;

    [SerializeField] private GameObject avatar;
    public GameObject Avatar => avatar;
    [SerializeField] private TextMeshProUGUI textLevel;

    public void ShowIndicator()
    {
        avatar.SetActive(true);
        textLevel.enabled = true;

        //image.enabled = true;
        //gameObject.SetActive(true);
    }
    public void HideIndicator()
    {
        avatar.SetActive(false);
        textLevel.enabled = false;

        //image.enabled = false;
        //gameObject.SetActive(false);
    }

    public void SetTextLevel(int level)
    {
        textLevel.text = level.ToString();
    }
    public void SetColor(Color color)
    {
        avatar.GetComponent<Image>().color = color;
    }
}
