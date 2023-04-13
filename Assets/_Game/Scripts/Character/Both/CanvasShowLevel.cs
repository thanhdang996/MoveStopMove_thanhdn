using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasShowLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textLevel;

    public void SetTextLevel(int level)
    {
        textLevel.text = level.ToString();
    }
}
