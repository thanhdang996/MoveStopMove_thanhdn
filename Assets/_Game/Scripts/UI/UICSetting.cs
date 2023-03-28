using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UICSetting : UICanvas
{
    [SerializeField] private VolumeSetting volumeSetting;

    public override void Open()
    {
        base.Open();
        volumeSetting.LoadValueMusic();
        Time.timeScale = 0;
    }

    public override void CloseDirectly()
    {
        Time.timeScale = 1;
        base.CloseDirectly();
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
}
