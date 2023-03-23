using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : GameUnit
{
    public void ShowIndicator()
    {
        gameObject.SetActive(true);
    }
    public void HideIndicator()
    {
        gameObject.SetActive(false);
    }
}
