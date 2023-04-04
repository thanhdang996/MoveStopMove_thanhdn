using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeMatchUI : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler;
    private void Start()
    {
        float ratio = Screen.height / Screen.width;
        canvasScaler.matchWidthOrHeight = ratio < 1.778 ? 1 : 0;
    }
}