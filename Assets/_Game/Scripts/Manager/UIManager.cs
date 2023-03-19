using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : Singleton<UIManager>
{
    public event Action OnRetryButton;
    public event Action OnNextButton;

    [SerializeField] private FixedJoystick joystick;
    public FixedJoystick Joystick => joystick;

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
        coinText.text = GameManager.Instance.Data.Coin.ToString();
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
        GameManager.Instance.AddCoin();
        GameManager.Instance.SaveData();

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
        OnRetryButton?.Invoke();
    }

    public void Next()
    {
        HidePanelWin();
        LevelManager.Instance.RemoveLastMap();
        GameManager.Instance.AddLevel();
        GameManager.Instance.SaveData();
        //ObjectPooling.Instance.ReturnGameObject(GameManager.Instance.CurrentPlayer.gameObject, PoolType.Player);
        OnNextButton?.Invoke();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
