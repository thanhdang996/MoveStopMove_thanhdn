using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICWinLevel : UICanvas
{
    public void NextLevel()
    {
        SoundManager.Instance.PlayBGSoundMusic();
        LevelManager.Instance.OnLoadNextLevel();
        CloseDirectly();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
