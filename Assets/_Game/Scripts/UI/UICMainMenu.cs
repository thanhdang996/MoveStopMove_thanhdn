using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICMainMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI textCoin;



    public override void Open()
    {
        base.Open();
        UIManager.Instance.IndicatorParent.SetActive(false);
        UI_UpdateTextCoin();
    }

    public void UI_UpdateTextCoin()
    {
        textCoin.text = DataManager.Instance.Data.Coin.ToString();
    }


    // Button UI
    public void Button_Open_UICWeapon()
    {
        UIManager.Instance.OpenUI<UICShopWeapon>();
        CloseDirectly();
    }

    public void Button_Open_UICShopDress()
    {
        UIManager.Instance.OpenUI<UICShopDress>();
        CloseDirectly();
    }

    public void Button_PlayGame()
    {
        UIManager.Instance.OpenUI<UICGamePlay>();
        UIManager.Instance.IndicatorParent.SetActive(true);
        SoundManager.Instance.PlayBGSoundMusic();
        LevelManager.Instance.OnStartGame();
        CloseDirectly();
    }
}
