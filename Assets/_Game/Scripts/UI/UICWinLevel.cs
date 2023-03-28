using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICWinLevel : UICanvas
{
    public override void Open()
    {
        base.Open();
        SoundManager.Instance.PlaySoundSFX2D(SoundType.Win);
        SoundManager.Instance.StopBGSoundMusic();
    }


    // Button UI
    public void Button_NextLevel()
    {
        SoundManager.Instance.PlayBGSoundMusic();
        LevelManager.Instance.OnLoadNextLevel();
        CloseDirectly();
    }


    public void Button_Quit()
    {
        Application.Quit();
    }
}
