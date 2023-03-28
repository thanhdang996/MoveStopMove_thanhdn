using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICLoseLevel : UICanvas
{
    public void RetryLevel()
    {
        SoundManager.Instance.PlayBGSoundMusic();
        LevelManager.Instance.RevivePlayer();
        CloseDirectly();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
