using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UICSetting : UICanvas
{
    [SerializeField] private VolumeSetting volumeSetting;

    public override void Setup()
    {
        base.Setup();
        //SoundManager.Instance.SetVolumeSetting(volumeSetting);
    }

    public override void Open()
    {
        volumeSetting.LoadValueMusic();
        Time.timeScale = 0;
        base.Open();
    }

    public override void CloseDirectly()
    {
        Time.timeScale = 1;
        base.CloseDirectly();
    }
}
