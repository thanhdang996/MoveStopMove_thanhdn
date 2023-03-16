using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private FixedJoystick joystick;
    public FixedJoystick Joystick => joystick;

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject popUpSetting;

    private void Start()
    {
        UpdateCoinText();
    }


    public void UpdateCoinText()
    {
        coinText.text = GameManager.Instance.Data.Coin.ToString();
    }

    public void ShowPopupSetting()
    {
        popUpSetting.SetActive(true);
    }
}
