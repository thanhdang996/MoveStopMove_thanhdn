using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PropsColor
{
    public Material mat;
}

[CreateAssetMenu(menuName = "Data/Color")]
public class ColorSO : ScriptableObject
{
    public PropsColor[] propsColors;
}

