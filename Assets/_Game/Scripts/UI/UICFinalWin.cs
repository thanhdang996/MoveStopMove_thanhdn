using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICFinalWin : UICanvas
{
    public override void Open()
    {
        base.Open();
        SoundManager.Instance.PlaySoundSFX2D(SoundType.Win);
        SoundManager.Instance.StopBGSoundMusic();
    }

    // Button UI
    public void Button_Quit()
    {
        Application.Quit();
    }
}
