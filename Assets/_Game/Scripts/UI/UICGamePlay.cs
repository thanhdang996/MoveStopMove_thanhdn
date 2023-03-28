using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICGamePlay : UICanvas
{
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private TextMeshProUGUI textCoint;
    [SerializeField] private TextMeshProUGUI textEnemyRemain;

    public override void Setup()
    {
        base.Setup();
        LevelManager.Instance.CurrentPlayer.PlayerMovement.SetJoystick(joystick);
        SoundManager.Instance.PlayBGSoundMusic();
    }

    public override void Open()
    {
        base.Open();
        UpdateCoinText();
        UpdateEnemeRemainText();
    }

    public void BackToMainMenu()
    {
        UIManager.Instance.OpenUI<UICMainMenu>();
        LevelManager.Instance.OnBackToMainMenu();
        CloseDirectly();
    }

    public void UpdateCoinText()
    {
        textCoint.text = DataManager.Instance.Data.Coin.ToString();
    }
    public void UpdateEnemeRemainText()
    {
        textEnemyRemain.text = LevelManager.Instance.CurrentLevel.EnemyRemain.ToString();
    }

    public void HandleUpdateCoinAndText()
    {
        DataManager.Instance.Data.AddCoinToData();
        DataManager.Instance.SaveData();

        UpdateCoinText();
    }

    public void HandleUpdateTotalEnemyAndText()
    {
        LevelManager.Instance.CurrentLevel.MinusTotalEnemy();
        UpdateEnemeRemainText();
    }

}
