using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UICShopDress : UICanvas
{
    private enum BuySkinnButtonType { None, Equipped, UnEquipped, Buy }
    [SerializeField] private BuySkinnButtonType currentButtonType;

    [SerializeField] private Button buttonBuy;
    [SerializeField] private TextMeshProUGUI textPrice;

    [SerializeField] private Button buttonEquipped;
    [SerializeField] private TextMeshProUGUI textEquipped;


    [SerializeField] private TextMeshProUGUI textCoin;

    private AbstractTabItem currentTabItem;
    public AbstractTabItem CurrentTabItem => currentTabItem;
    [SerializeField] private List<AbstractTabItem> listTabs;



    [SerializeField] private List<UIItemShop> listUIItemShop;
    public List<UIItemShop> ListUIItemShop => listUIItemShop;

    [SerializeField] private RectTransform contentTF;
    public RectTransform ContentTF => contentTF;

    [SerializeField] private UIItemShop prefabUIItemShop;
    public UIItemShop PrefabUIItemShop => prefabUIItemShop;



    public override void Open()
    {
        base.Open();
        if (LevelManager.Instance.CurrentPlayer.CurrentHairAvaGO != null)
        {
            LevelManager.Instance.CurrentPlayer.CurrentHairAvaGO.SetActive(false);
        }
        UI_UpdateTextCoin();
        OpenTab(tabIndex: 0);

        // check if have hair
        
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        UIManager.Instance.OpenUI<UICMainMenu>();
        UIManager.Instance.GetUI<UICMainMenu>().UI_UpdateTextCoin();

        if (LevelManager.Instance.CurrentPlayer.CurrentHairAvaGO != null)
        {
            LevelManager.Instance.CurrentPlayer.CurrentHairAvaGO.SetActive(true);
        }
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
        if (currentTabItem != null)
        {
            // reset conttent pos to zero x
            Vector2 currentPos = contentTF.anchoredPosition;
            currentPos.x = 0;
            contentTF.anchoredPosition = currentPos;

            currentTabItem.CurrentUIItemShop.Outline.enabled = false;
            currentTabItem.TurnOffOutLine();
            currentTabItem.DeActiveitemOnCurrentPlayer();
        }
        // vi tabIndex bang thu tu cac TabItemHair trong listTabs( luc keo editor)
        currentTabItem = listTabs[tabIndex];
        currentTabItem.ActiveAllUIItemShop();
        currentTabItem.TurnOnOutLine();
    }

    public void ShowButtonEquipped(int idUIITemShop, int currentItemTab)
    {
        if (idUIITemShop == currentItemTab)
        {
            currentButtonType = BuySkinnButtonType.Equipped;
        }
        else
        {
            currentButtonType = BuySkinnButtonType.UnEquipped;
        }
        buttonBuy.gameObject.SetActive(false);
        buttonEquipped.gameObject.SetActive(true);
        textEquipped.text = currentButtonType.ToString();

    }
    public void ShowButtonBuy()
    {
        currentButtonType = BuySkinnButtonType.Buy;
        buttonEquipped.gameObject.SetActive(false);
        buttonBuy.gameObject.SetActive(true);
    }

    public void Button_HandleBuyItem()
    {
        if (CanBuyItem())
        {
            int coinRemain = GetCoinInData() - GetCurrentItemPrice();
            DataManager.Instance.Data.SetCoinToData(coinRemain);

            currentTabItem.GetItemOwnerInData().Add(currentTabItem.CurrentUIItemShop.Id);
            DataManager.Instance.SaveData();

            currentTabItem.CurrentUIItemShop.SetUnlock(true);
            currentTabItem.CurrentUIItemShop.ChangeActiveImageLock(false);
            ShowButtonEquipped(currentTabItem.CurrentUIItemShop.Id, currentTabItem.GetCurrentItemInData());

            UI_UpdateTextCoin();
        }
    }

    public void Button_HandleEquippedItem()
    {
        if (currentButtonType == BuySkinnButtonType.UnEquipped)
        {
            currentButtonType = BuySkinnButtonType.Equipped;
            textEquipped.text = currentButtonType.ToString();

            // thay doi TextEquip item prev
            if (currentTabItem.GetCurrentItemInData() != -1)
            {
                listUIItemShop[currentTabItem.GetCurrentItemInData()].ChangeActiveTextEquip(false);
            }

            currentTabItem.ChangeCurrentItemInData(currentTabItem.CurrentUIItemShop.Id);
            currentTabItem.CurrentUIItemShop.ChangeActiveTextEquip(true);
            currentTabItem.AttachItemToPlayer();
            DataManager.Instance.SaveData();
            return;
        }
        if (currentButtonType == BuySkinnButtonType.Equipped)
        {
            currentButtonType = BuySkinnButtonType.UnEquipped;
            textEquipped.text = currentButtonType.ToString();

            currentTabItem.ChangeCurrentItemInData(-1);
            currentTabItem.DeAttachItemToPlayer();
            currentTabItem.CurrentUIItemShop.ChangeActiveTextEquip(false);

            DataManager.Instance.SaveData();
            return;
        }
    }

    private int GetCoinInData()
    {
        return DataManager.Instance.Data.Coin;
    }
    private int GetCurrentItemPrice()
    {
        return int.Parse(textPrice.text);
    }
    private bool CanBuyItem()
    {
        if (GetCoinInData() < GetCurrentItemPrice()) return false;
        return true;
    }
}
