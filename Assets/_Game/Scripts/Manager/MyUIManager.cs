using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MyUIManager : Singleton<MyUIManager>
{
    public event Action OnRetryButton;
    public event Action OnNextButton;

    [SerializeField] private FloatingJoystick joystick;
    public FloatingJoystick Joystick => joystick;

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI enemyRemainText;

    [SerializeField] private GameObject popUpSetting;
    [SerializeField] private GameObject panelWin;
    [SerializeField] private GameObject panelLose;


    public void OnInitLoadUI()
    {
        UpdateCoinText();
        UpdateEnemeRemainText();
    }

    public void UpdateCoinText()
    {
        coinText.text = DataManager.Instance.Data.Coin.ToString();
    }
    public void UpdateEnemeRemainText()
    {
        enemyRemainText.text = LevelManager.Instance.CurrentLevel.TotalEnemy.ToString();
    }

    public void ShowPopupSetting()
    {
        popUpSetting.SetActive(true);
    }

    public void TurnOnSetting()
    {
        popUpSetting.SetActive(true);
        Time.timeScale = 0;
    }
    public void TurnOffSetting()
    {
        popUpSetting.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowPanelWin()
    {
        panelWin.SetActive(true);
    }
    public void HidePanelWin()
    {
        panelWin.SetActive(false);
    }

    public void ShowPanelLose()
    {
        panelLose.SetActive(true);
    }
    public void HidePanelLose()
    {
        panelLose.SetActive(false);
    }

    public void HandUpdateCoinAndText()
    {
        DataManager.Instance.Data.AddCoinToData();
        DataManager.Instance.SaveData();

        UpdateCoinText();
    }

    public void UpdateTotalEnemyAndText()
    {
        LevelManager.Instance.CurrentLevel.MinusTotalEnemy();
        UpdateEnemeRemainText();
    }

    public void Retry()
    {
        HidePanelLose();
        SoundManager.Instance.PlayBGSoundMusic();
        OnRetryButton?.Invoke();
    }

    public void Next()
    {
        HidePanelWin();
        LevelManager.Instance.RemoveLastMap();
        DataManager.Instance.Data.AddLevelToData();
        DataManager.Instance.SaveData();
        SoundManager.Instance.PlayBGSoundMusic();

        OnNextButton?.Invoke();
    }

    public void BuyWeapon(int indexWeaponOnShop)
    {
        DataManager.Instance.Data.AddNewItemToData(indexWeaponOnShop);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
