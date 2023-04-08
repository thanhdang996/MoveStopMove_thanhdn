using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UICShopWeapon : UICanvas
{
    private enum BuyWeaponButtonType { None, Equipped, Select, Buy }
    private BuyWeaponButtonType currentButtonType;
    private WeaponType currentWeaponTypeInShop;

    [SerializeField] TextMeshProUGUI textCoin;
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private Transform camTF;
    [SerializeField] private Transform listWeaponTF;

    [SerializeField] private Button buttonBuy;
    [SerializeField] private TextMeshProUGUI textButton;

    private float snapPerItem = 5;
    public int MaxWeaponIndex => listWeaponTF.childCount - 1;

    public override void Open()
    {
        base.Open();
        HandleCamToShowWeaponCurrent();
        UI_UpdateTextCoin();
        ChangeStateButtonBaseIndexWeapon();
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        UIManager.Instance.OpenUI<UICMainMenu>();
        UIManager.Instance.GetUI<UICMainMenu>().UI_UpdateTextCoin(); // vi MainMenu van o duoi UICShopWeapon
    }

    private void HandleCamToShowWeaponCurrent()
    {
        currentWeaponTypeInShop = GetCurrentWeaponIndexInData();
        Vector3 pos = camTF.localPosition;
        pos.x = snapPerItem * ((int)currentWeaponTypeInShop);
        camTF.localPosition = pos;
    }


    private void UI_UpdateTextCoin()
    {
        textCoin.text = DataManager.Instance.Data.Coin.ToString();
    }

    // Button UI
    public void Button_Right()
    {
        currentWeaponTypeInShop++;
        if ((int)currentWeaponTypeInShop > MaxWeaponIndex)
        {
            currentWeaponTypeInShop = (WeaponType)MaxWeaponIndex;
            return;
        }
        Vector3 pos = camTF.localPosition;
        pos.x += snapPerItem;
        camTF.localPosition = pos;
        ChangeStateButtonBaseIndexWeapon();
    }

    public void Button_Left()
    {
        currentWeaponTypeInShop--;
        if (currentWeaponTypeInShop < 0)
        {
            currentWeaponTypeInShop = 0;
            return;
        }
        Vector3 pos = camTF.localPosition;
        pos.x -= snapPerItem;
        camTF.localPosition = pos;
        ChangeStateButtonBaseIndexWeapon();
    }
    public void Button_HandleBuyWeapon()
    {
        if (currentButtonType == BuyWeaponButtonType.Equipped)
        {
            UIManager.Instance.CloseUI<UICShopWeapon>();
            return;
        }
        if (currentButtonType == BuyWeaponButtonType.Select)
        {
            DataManager.Instance.Data.ChangeCurrentWeaponData(currentWeaponTypeInShop);
            DataManager.Instance.SaveData();

            LevelManager.Instance.CurrentPlayer.ChangeWeaponOnHand();
            ChangeStateButtonBaseIndexWeapon();
            return;
        }
        if (currentButtonType == BuyWeaponButtonType.Buy)
        {
            if (CanBuyWeapon())
            {
                int coinRemain = GetCoinInData() - GetCurrentWeaponPrice();
                DataManager.Instance.Data.SetCoinToData(coinRemain);

                DataManager.Instance.Data.ListWeaponOwner.Add(currentWeaponTypeInShop);
                DataManager.Instance.Data.ChangeCurrentWeaponData(currentWeaponTypeInShop);

                DataManager.Instance.SaveData();

                UI_UpdateTextCoin();
                LevelManager.Instance.CurrentPlayer.AddNewWeapon(currentWeaponTypeInShop);
                LevelManager.Instance.CurrentPlayer.ChangeWeaponOnHand();

                foreach (var bot in LevelManager.Instance.ListBotCurrent)
                {
                    bot.CreateNewWeaponBasePlayerJustAdd(currentWeaponTypeInShop);
                }
                ChangeStateButtonBaseIndexWeapon();
            }
            else
            {
                Debug.Log("Not Enough money to buy");
            }
        }
    }


    private void ChangeStateButtonBaseIndexWeapon()
    {

        if ((currentWeaponTypeInShop == GetCurrentWeaponIndexInData()))
        {
            buttonBuy.interactable = true;
            currentButtonType = BuyWeaponButtonType.Equipped;
            textButton.text = currentButtonType.ToString();
            return;
        }

        if (GetListWeaponOwnerInData().Contains(currentWeaponTypeInShop))
        {
            buttonBuy.interactable = true;
            currentButtonType = BuyWeaponButtonType.Select;
            textButton.text = currentButtonType.ToString();
            return;
        }

        if (!GetListWeaponOwnerInData().Contains(currentWeaponTypeInShop))
        {
            buttonBuy.interactable = CanBuyWeapon();

            currentButtonType = BuyWeaponButtonType.Buy;
            textButton.text = $"{currentButtonType} {GetCurrentWeaponPrice()}";
            return;
        }
    }

    private WeaponType GetCurrentWeaponIndexInData()
    {
        return DataManager.Instance.Data.CurrentWeapon;
    }

    private List<WeaponType> GetListWeaponOwnerInData()
    {
        return DataManager.Instance.Data.ListWeaponOwner;
    }

    private int GetCoinInData()
    {
        return DataManager.Instance.Data.Coin;
    }
    private int GetCurrentWeaponPrice()
    {
        return weaponSO.propWeapons[(int)currentWeaponTypeInShop].price;
    }


    private bool CanBuyWeapon()
    {
        if (GetCoinInData() < GetCurrentWeaponPrice()) return false;
        return true;
    }
}
