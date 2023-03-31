using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabItem : MonoBehaviour
{
    [SerializeField] private List<ItemShop> listItemHair;


    private void ClickButton()
    {
        listItemHair.Add(listItemHair[0]);
    }
}
