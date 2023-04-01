using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabItemShop : MonoBehaviour
{
    private int id;
    public int Id => id;

    public void SetId(int id)
    {
        this.id = id;
    }
}
