using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType { None, Equipped, Select, Buy}
public class ButtonWeapon : MonoBehaviour
{
    [SerializeField] private UICWeapon uICWeapon;
    [SerializeField] private ButtonType buttonType;
    public ButtonType ButtonType { get => buttonType; set => buttonType = value; }

    public void HandleOnClick()
    {
        uICWeapon.CheckWeapon();
        if (buttonType == ButtonType.Equipped)
        {
            UIManager.Instance.CloseUI<UICWeapon>();
            return;
        }
        if(buttonType == ButtonType.Select)
        {
            {
                DataManager.Instance.Data.ChangeCurrentWeaponData(uICWeapon.IndexWeapon);
                DataManager.Instance.SaveData();
                LevelManager.Instance.CurrentPlayer.ChangeWeaponOnHand();
                uICWeapon.CheckWeapon();
                return;
            }
        }
        if(buttonType == ButtonType.Buy)
        {
            if (IsCanBuy())
            {
                uICWeapon.CheckWeapon();
                buttonType = ButtonType.Select;
                DataManager.Instance.Data.ChangeCurrentWeaponData(uICWeapon.IndexWeapon);
                DataManager.Instance.SaveData();
                LevelManager.Instance.CurrentPlayer.ChangeWeaponOnHand();
                uICWeapon.CheckWeapon();
            }

        }
    }
    public bool IsCanBuy()
    {
        int coinCurrent = DataManager.Instance.Data.Coin;
        int currentWeaponPrice = uICWeapon.GetWeaponPrice();
        if (coinCurrent < currentWeaponPrice) return false;

        int cointRemain = coinCurrent - currentWeaponPrice;
        DataManager.Instance.Data.SetCoinToData(cointRemain);
        DataManager.Instance.SaveData();
        uICWeapon.TextCoin.text = DataManager.Instance.Data.Coin.ToString();
        AddNewItemToData(uICWeapon.IndexWeapon);
        return true;
    }


    public void AddNewItemToData(int indexWeaponOnShop)
    {
        DataManager.Instance.Data.WeaponOwner.Add(indexWeaponOnShop);
        DataManager.Instance.SaveData();

        LevelManager.Instance.CurrentPlayer.AddNewWeapon(indexWeaponOnShop);
    }
}
