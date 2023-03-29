using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UICWeapon : UICanvas
{
    private enum BuyWeaponButtonType { None, Equipped, Select, Buy }
    private BuyWeaponButtonType currentButtonType;
    private int currentIndexWeaponInShop;

    [SerializeField] TextMeshProUGUI textCoin;
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private Transform camTF;
    [SerializeField] private Transform listWeaponTF;

    [SerializeField] private Button buttonBuy;
    [SerializeField] private TextMeshProUGUI textButton;

    private float snapPerItem = 4;
    public int MaxWeaponIndex => listWeaponTF.childCount - 1;

    public override void Open()
    {
        base.Open();
        UI_UpdateTextCoin();
        ChangeStateButtonBaseIndexWeapon();
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        UIManager.Instance.GetUI<UICMainMenu>().UI_UpdateTextCoin(); // vi MainMenu van o duoi UICWeapon
    }



    public void UI_UpdateTextCoin()
    {
        textCoin.text = DataManager.Instance.Data.Coin.ToString();
    }

    // Button UI
    public void Button_Right()
    {
        currentIndexWeaponInShop++;
        if (currentIndexWeaponInShop > MaxWeaponIndex)
        {
            currentIndexWeaponInShop = MaxWeaponIndex;
            return;
        }
        Vector3 pos = camTF.position;
        pos.x += snapPerItem;
        camTF.position = pos;
        ChangeStateButtonBaseIndexWeapon();
    }

    public void Button_Left()
    {
        currentIndexWeaponInShop--;
        if (currentIndexWeaponInShop < 0)
        {
            currentIndexWeaponInShop = 0;
            return;
        }
        Vector3 pos = camTF.position;
        pos.x -= snapPerItem;
        camTF.position = pos;
        ChangeStateButtonBaseIndexWeapon();
    }
    public void Button_HandleBuyWeapon()
    {
        if (currentButtonType == BuyWeaponButtonType.Equipped)
        {
            UIManager.Instance.CloseUI<UICWeapon>();
            return;
        }
        if (currentButtonType == BuyWeaponButtonType.Select)
        {
            DataManager.Instance.Data.ChangeCurrentWeaponData(currentIndexWeaponInShop);
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

                DataManager.Instance.Data.WeaponOwner.Add(currentIndexWeaponInShop);
                DataManager.Instance.Data.ChangeCurrentWeaponData(currentIndexWeaponInShop);

                DataManager.Instance.SaveData();

                UI_UpdateTextCoin();
                LevelManager.Instance.CurrentPlayer.AddNewWeapon(currentIndexWeaponInShop);
                LevelManager.Instance.CurrentPlayer.ChangeWeaponOnHand();

                foreach (var bot in LevelManager.Instance.ListBotCurrent)
                {
                    bot.CreateNewWeaponBasePlayerJustAdd(currentIndexWeaponInShop);
                }
                ChangeStateButtonBaseIndexWeapon();
            }
            else
            {
                Debug.Log("Not Enough money to buy");
            }
        }
    }


    public void ChangeStateButtonBaseIndexWeapon()
    {

        if (currentIndexWeaponInShop == GetCurrentWeaponIndexInData())
        {
            buttonBuy.interactable = true;
            currentButtonType = BuyWeaponButtonType.Equipped;
            textButton.text = currentButtonType.ToString();
            return;
        }

        if(GetListWeaponOwnerInData().Contains(currentIndexWeaponInShop))
        {
            buttonBuy.interactable = true;
            currentButtonType = BuyWeaponButtonType.Select;
            textButton.text = currentButtonType.ToString();
            return;
        }

        if (!GetListWeaponOwnerInData().Contains(currentIndexWeaponInShop))
        {
            buttonBuy.interactable = CanBuyWeapon();

            currentButtonType = BuyWeaponButtonType.Buy;
            textButton.text = $"{currentButtonType} {GetCurrentWeaponPrice()}";
            return;
        }
    }

    public int GetCurrentWeaponIndexInData()
    {
        return DataManager.Instance.Data.CurrentWeapon;
    }

    public List<int> GetListWeaponOwnerInData()
    {
        return DataManager.Instance.Data.WeaponOwner;
    }

    public int GetCoinInData()
    {
        return DataManager.Instance.Data.Coin;
    }
    public int GetCurrentWeaponPrice()
    {
        return weaponSO.propWeapons[currentIndexWeaponInShop].price;
    }


    private bool CanBuyWeapon()
    {
        if (GetCoinInData() < GetCurrentWeaponPrice()) return false;
        return true;
    }
}
