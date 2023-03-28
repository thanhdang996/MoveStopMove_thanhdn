using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICWeapon : UICanvas
{
    [SerializeField] TextMeshProUGUI textCoin;
    public TextMeshProUGUI TextCoin => textCoin;
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform listWeapon;
    [SerializeField] private int indexWeapon;
    public int IndexWeapon => indexWeapon;
    [SerializeField] private Button button;

    private float snapPerItem = 4;
    public int MaxWeaponIndex => listWeapon.childCount - 1;

    public override void Open()
    {
        base.Open();
        CheckWeapon();
        UI_UpdateTextCoin();
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        UIManager.Instance.GetUI<UICMainMenu>().UI_UpdateTextCoin();
    }



    public void UI_UpdateTextCoin()
    {
        textCoin.text = DataManager.Instance.Data.Coin.ToString();
    }
    public void TurnRight()
    {
        indexWeapon++;
        if (indexWeapon > MaxWeaponIndex)
        {
            indexWeapon = MaxWeaponIndex;
            return;
        }
        Vector3 pos = cam.position;
        pos.x += snapPerItem;
        cam.position = pos;
        CheckWeapon();
    }

    public void TurnLeft()
    {
        indexWeapon--;
        if (indexWeapon < 0)
        {
            indexWeapon = 0;
            return;
        }
        Vector3 pos = cam.position;
        pos.x -= snapPerItem;
        cam.position = pos;
        CheckWeapon();
    }

    public void CheckWeapon()
    {
        if(indexWeapon == GetCurrentWeaponIndex())
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Equipped";
            button.GetComponent<ButtonWeapon>().ButtonType = ButtonType.Equipped;
            return;
        }

        if (DataManager.Instance.Data.WeaponOwner.Contains(indexWeapon))
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Select";
            button.GetComponent<ButtonWeapon>().ButtonType = ButtonType.Select;
        }
        else
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Buy " + weaponSO.propWeapons[indexWeapon].price;
            button.GetComponent<ButtonWeapon>().ButtonType = ButtonType.Buy;

        }
    }

    public int GetCurrentWeaponIndex()
    {
        return DataManager.Instance.Data.CurrentWeapon;
    }

    public int GetWeaponPrice()
    {
        return weaponSO.propWeapons[indexWeapon].price;
    }

    //public void HandleOnClick()
    //{
    //    CheckWeapon();
    //    if (buttonType == ButtonType.Equipped)
    //    {
    //        return;
    //    }
    //    if (buttonType == ButtonType.Select)
    //    {
    //        DataManager.Instance.Data.ChangeCurrentWeaponData(uICWeapon.IndexWeapon);
    //        DataManager.Instance.SaveData();
    //        LevelManager.Instance.CurrentPlayer.DeActiveCurrentWeapon();
    //        LevelManager.Instance.CurrentPlayer.ActiveCurrentWeapon();
    //    }
    //}
}
