using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICGamePlay : UICanvas
{
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private TextMeshProUGUI textEnemyRemain;

    public override void Setup()
    {
        base.Setup();
        LevelManager.Instance.CurrentPlayer.PlayerMovement.SetJoystick(joystick);
       
    }
    public override void Open()
    {
        base.Open();
        UI_UpdateEnemeRemainText();
    }
    public void UI_UpdateEnemeRemainText()
    {
        textEnemyRemain.text = LevelManager.Instance.CurrentLevel.EnemyRemain.ToString();
    }


    // Button UI
    public void Button_OpenSetting()
    {
        UIManager.Instance.OpenUI<UICSetting>();
    }

}
