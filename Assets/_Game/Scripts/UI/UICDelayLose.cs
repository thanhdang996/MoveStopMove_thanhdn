using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UICDelayLose : UICanvas
{
    [SerializeField] private RectTransform circleImageRTF;
    [SerializeField] private TextMeshProUGUI textSecond;
    private float maxTime = 5;
    private float timer;

    private void OnEnable()
    {
        timer = maxTime;
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        while (timer > 0)
        {
            SoundManager.Instance.PlaySoundSFX2D(SoundType.Count);
            textSecond.text = Mathf.FloorToInt(timer % 60).ToString();
            yield return new WaitForSeconds(1);
            timer--;
        }

        UIManager.Instance.OpenUI<UICLoseLevel>();
        CloseDirectly();
    }

    private void Update()
    {
        circleImageRTF.Rotate(-200 * Time.deltaTime * Vector3.forward);
    }

    // Button UI
    public void Button_Revive()
    {
        SoundManager.Instance.PlayBGSoundMusic();
        LevelManager.Instance.RevivePlayer();
        CloseDirectly();
    }
}
