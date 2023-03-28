using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICLoseLevel : UICanvas
{

    // Button UI
    public void Button_RetryLevel()
    {
        SoundManager.Instance.PlayBGSoundMusic();
        LevelManager.Instance.RevivePlayer();
        CloseDirectly();
    }

    public void Button_Quit()
    {
        Application.Quit();
    }
}
