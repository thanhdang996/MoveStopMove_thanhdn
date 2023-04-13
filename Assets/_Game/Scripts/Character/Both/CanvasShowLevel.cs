using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasShowLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textLevel;
    [SerializeField] private Image circleImage;

    public void SetTextLevel(int level)
    {
        textLevel.text = level.ToString();
    }

    public void SetColor(Color color)
    {
        circleImage.color = color;
    }
}
