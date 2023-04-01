using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICShopDress : UICanvas
{

    [SerializeField] private TextMeshProUGUI textCoin;
    [SerializeField] private TextMeshProUGUI textPrice;

    private TabItemController currentTabItemController;
    public TabItemController CurrentTabItemController => currentTabItemController;
    [SerializeField] private List<TabItemController> listTabs;



    public override void Open()
    {
        base.Open();
        UI_UpdateTextCoin();
        OpenTab(tabIndex: 0);
    }

    public void UI_UpdateTextCoin()
    {
        textCoin.text = DataManager.Instance.Data.Coin.ToString();
    }
    public void UI_SetTextPrice(int price)
    {
        textPrice.text = price.ToString();
    }

    public void OpenTab(int tabIndex)
    {
        if(currentTabItemController != null)
        {
            currentTabItemController.Outline.enabled = false;
            currentTabItemController.TurnOnOutLineCurrentButton(0);
            currentTabItemController.DeActiveAllUIItemShop();
            currentTabItemController.DeActivePrefabCurrentPlayer();
        }
        // vi tabIndex bang thu tu cac TabItem trong listTabs( luc keo editor)
        currentTabItemController = listTabs[tabIndex]; 
        currentTabItemController.ActiveAllUIItemShop();
        currentTabItemController.Outline.enabled = true;
    }
}
