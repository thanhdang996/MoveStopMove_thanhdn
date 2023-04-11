using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Set")]
public class SetSO : ScriptableObject
{
    public PropsSet[] propsSets;

    public bool HasHat(int index)
    {
        return propsSets[index].hatPrefab;
    }
    public bool HasWing(int index)
    {
        return propsSets[index].wingPrefab;
    }
    public bool HasTail(int index)
    {
        return propsSets[index].tailPrefab;
    }
}