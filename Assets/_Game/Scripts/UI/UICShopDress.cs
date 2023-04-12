using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UICShopDress : UICanvas
{
    private enum BuySkinnButtonType { None, Select, UnEquip, Buy }
    [SerializeField] private BuySkinnButtonType currentButtonType;

    [SerializeField] private Button buttonBuy;
    [SerializeField] private TextMeshProUGUI textPrice;

    [SerializeField] private Button buttonDressing;
    [SerializeField] private TextMeshProUGUI textDressing;

    [SerializeField] private Color selectColor;
    [SerializeField] private Color unEquipColor;
    [SerializeField] private Sprite selectImage;
    [SerializeField] private Sprite unEquipImage;

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
        UI_UpdateTextCoin();
        OpenTab(tabIndex: 0);

        LevelManager.Instance.CurrentPlayer.IsInShopDress = true;
        StartCoroutine(LevelManager.Instance.CurrentPlayer.Cam.AnimatCamPullDown());
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        UIManager.Instance.OpenUI<UICMainMenu>();
        UIManager.Instance.GetUI<UICMainMenu>().UI_UpdateTextCoin();

        LevelManager.Instance.CurrentPlayer.IsInShopDress = false;
        LevelManager.Instance.CurrentPlayer.Cam.StartAnimatCamPullUp();
    }

    public void UI_UpdateTextCoin()
    {
        textCoin.text = DataManager.Instance.Data.Coin.ToString();
    }
    public void UI_SetTextPrice(int price)
    {
        textPrice.text = price.ToString();
    }

    private void HandleBeforeOpenTab()
    {
        if (currentTabItem != null)
        {
            // reset conttent pos to zero x
            Vector2 currentPos = contentTF.anchoredPosition;
            currentPos.x = 0;
            contentTF.anchoredPosition = currentPos;

            currentTabItem.CurrentUIItemShop.Outline.enabled = false;
            currentTabItem.TurnOffOutLine();
            currentTabItem.HidePreviewItem();
        }
    }

    public void OpenTab(int tabIndex)
    {
        HandleBeforeOpenTab();

        // vi tabIndex bang thu tu cac TabItemHat trong listTabs( luc keo editor)
        currentTabItem = listTabs[tabIndex];
        currentTabItem.ActiveAllUIItemShop();
        currentTabItem.TurnOnOutLine();
    }

    public void ShowButtonDressing(int idUIITemShop, int currentItemInDataAtTab)
    {
        if (idUIITemShop == currentItemInDataAtTab)
        {
            SetPropButtonUnEquip();
        }
        else
        {
            SetPropButtonSelect();
        }
        buttonBuy.gameObject.SetActive(false);
        buttonDressing.gameObject.SetActive(true);
    }
    public void ShowButtonBuy()
    {
        currentButtonType = BuySkinnButtonType.Buy;
        buttonDressing.gameObject.SetActive(false);
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
            ShowButtonDressing(currentTabItem.CurrentUIItemShop.Id, currentTabItem.GetCurrentItemInData()); // luc nao cung se la nut Select

            UI_UpdateTextCoin();
        }
    }

    public void Button_HandleDressingItem()
    {
        if (currentButtonType == BuySkinnButtonType.Select)
        {
            SetPropButtonUnEquip();

            // logic only pick one item in one tab
            int numTab = currentTabItem.TabIndex;
            for (int i = 0; i < listTabs.Count; i++)
            {
                if (i == numTab) continue;
                listTabs[i].ChangeCurrentItemInData(-1);
                listTabs[i].DeAttachItemToPlayer();
            }

            // thay doi TextEquip item previous
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
        if (currentButtonType == BuySkinnButtonType.UnEquip)
        {
            SetPropButtonSelect();

            currentTabItem.ChangeCurrentItemInData(-1);
            currentTabItem.DeAttachItemToPlayer();
            currentTabItem.CurrentUIItemShop.ChangeActiveTextEquip(false);

            DataManager.Instance.SaveData();
            return;
        }
    }

    public void SetPropButtonSelect()
    {
        currentButtonType = BuySkinnButtonType.Select;
        buttonDressing.GetComponent<Image>().sprite = selectImage;
        buttonDressing.GetComponentInChildren<TextMeshProUGUI>().color = selectColor;
        textDressing.text = Constant.BUTTON_TEXT_SELECT;
    }

    public void SetPropButtonUnEquip()
    {
        currentButtonType = BuySkinnButtonType.UnEquip;
        buttonDressing.GetComponent<Image>().sprite = unEquipImage;
        buttonDressing.GetComponentInChildren<TextMeshProUGUI>().color = unEquipColor;
        textDressing.text = Constant.BUTTON_TEXT_UNEQUIP;
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
