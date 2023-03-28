using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICMainMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI textCoint;


    public override void Open()
    {
        base.Open();
        UIManager.Instance.IndicatorParent.SetActive(false);
        textCoint.text = DataManager.Instance.Data.Coin.ToString();
    }

    public void OpenSetting()
    {
        UIManager.Instance.OpenUI<UICSetting>();
    }

    public void PlayGame()
    {
        UIManager.Instance.OpenUI<UICGamePlay>();
        UIManager.Instance.IndicatorParent.SetActive(true);
        LevelManager.Instance.OnStartGame();
        CloseDirectly();
    }
}
