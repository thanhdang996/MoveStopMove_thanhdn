using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICShopDress : UICanvas
{

    [SerializeField] private TextMeshProUGUI textCoin;
    [SerializeField] private TextMeshProUGUI textPrice;

    private ITabItem currentTabItem;
    public ITabItem CurrentTabItem => currentTabItem;
    [SerializeField] private List<ITabItem> listTabs;



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
        if(currentTabItem != null)
        {
            currentTabItem.TurnOffOutLine();

            // khi chuyen tab, tab cu set button select luon la  vi tri 0 la nut dau tien ( do trong HandleOutLineButton co SetCurrentUIItemShop)
            currentTabItem.HandleOutLineButton(0); 
            currentTabItem.DeActiveAllUIItemShop();
            currentTabItem.DeActiveitemOnCurrentPlayer();
        }
        // vi tabIndex bang thu tu cac TabItemHair trong listTabs( luc keo editor)
        currentTabItem = listTabs[tabIndex]; 
        currentTabItem.ActiveAllUIItemShop();
        currentTabItem.TurnOnOutLine();
    }
}
