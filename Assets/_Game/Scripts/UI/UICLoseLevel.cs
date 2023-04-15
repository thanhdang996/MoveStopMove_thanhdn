using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICLoseLevel : UICanvas
{

    public override void Open()
    {
        base.Open();
        SoundManager.Instance.PlaySoundSFX2D(SoundType.Lose);
    }

    // Button UI
    public void Button_BackTo_UICMainMenu()
    {
        UIManager.Instance.OpenUI<UICMainMenu>();
        UIManager.Instance.CloseUI<UICGamePlay>();
        SoundManager.Instance.StopBGSoundMusic();
        LevelManager.Instance.OnBackToMainMenu();
        CloseDirectly();
    }

    public void Button_Quit()
    {
        Application.Quit();
    }
}
